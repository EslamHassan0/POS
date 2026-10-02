using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using POS.Application.Contract.Authentications;
using POS.Application.Contract.Users;
using POS.Domain.Entity.Users;
using POS.Domain.Identity;
using POS.Domain.Shared.Helpers;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ZXing;

namespace POS.Application.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly UserManager<IdentityApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly OptionsUserRole _roleOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public UserAppService(
            UserManager<IdentityApplicationUser> userManager,
            IOptions<JWT> jwt,
            RoleManager<IdentityRole<Guid>> roleManager,
            IOptions<OptionsUserRole> roleOptions,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _roleOptions = roleOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<Result<string>> CreateUserByAdminAsync(CreateUserByAdminDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return Result<string>.Failure("Email is already registered");
            if (!await _roleManager.RoleExistsAsync(dto.RoleName))
                return Result<string>.Failure("Invalid role");

            var user = new IdentityApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = dto.Password,
                IsActive = true,
                PhoneNumber = dto.PhoneNumber
            };


            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return Result<string>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, dto.RoleName);

            return Result<string>.Success("Createad Success");
        }
        public async Task<Result<string>> ChangeUserRoleAsync(Guid userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result<string>.Failure("Not Found User");
            var currentRole = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRole);
            await _userManager.AddToRoleAsync(user, newRole);


            return Result<string>.Success("Success");
        }

        public async Task<string> AddRoleAsync(RolesDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null || !await _roleManager.RoleExistsAsync(dto.RoleName))
                return "Invalid user Id or Roles";

            if (await _userManager.IsInRoleAsync(user, dto.RoleName))
                return "User Aready assinged to this role";

            var result = await _userManager.AddToRoleAsync(user, dto.RoleName);


            return result.Succeeded ? string.Empty : "Sonething went wrong";

        }



        public async Task<AuthModel> LoginAsync(LoginDto dto)
        {
            var auth = new AuthModel();

            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new AuthModel { Massage = "Email or Password Is incorrect!" };
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new AuthModel { Massage = "This account has been disabled" };
            }
            var jwtSecurityTokin = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);


            auth.IsAuthenticated = true;
            auth.Email = user.Email;
            auth.UserName = user.UserName;
            auth.LastLoginTime = DateTime.UtcNow;
            user.LastLoginTime = DateTime.UtcNow;
            //auth.ExpiresOn = jwtSecurityTokin.ValidTo;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityTokin);
            auth.Roles = rolesList.ToList();


            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                auth.RefreshToken = activeRefreshToken.Token;
                auth.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                auth.RefreshToken = refreshToken.Token;
                auth.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return auth;
        }

        public async Task<AuthModel> RegisterAsync(RegisterDto dto)
        {

            try
            {
                if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                    return new AuthModel { Massage = "Email is already registered! " };

                if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                    return new AuthModel { Massage = "UserName is already registered! " };


                var user = new IdentityApplicationUser
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    Password = dto.Password,
                    LastName = dto.LastName,

                };
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {

                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description},";
                    }
                    return new AuthModel { Massage = errors };

                }
                string defaultUserRole = _roleOptions.DefaultUserRole;
                await _userManager.AddToRoleAsync(user, defaultUserRole);

                var jwtSecurityTokin = await CreateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);

                return new AuthModel
                {
                    Email = user.Email,
                    //ExpiresOn = jwtSecurityTokin.ValidTo,
                    IsAuthenticated = true,
                    Roles = new List<string> { "User" },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityTokin),
                    UserName = user.UserName,
                    LastLoginTime = DateTime.Now,
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.ExpiresOn

                };
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };

        }

        private async Task<JwtSecurityToken> CreateJwtToken(IdentityApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaimsList = new List<Claim>();

            foreach (var roleName in roles)
            {
                roleClaimsList.Add(new Claim("roles", roleName));

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    roleClaimsList.AddRange(roleClaims.Where(c => c.Type == "permission"));
                }
            }

            var finalClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaimsList);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: finalClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        //private async Task<JwtSecurityToken> CreateJwtToken(IdentityApplicationUser user)
        //{
        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var roleClaims = new List<Claim>();

        //    foreach (var role in roles)
        //        roleClaims.Add(new Claim("roles", role));

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //        new Claim("uid", user.Id.ToString())
        //    }
        //    .Union(userClaims)
        //    .Union(roleClaims);


        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //    var jwtSecurityToken = new JwtSecurityToken(
        //    issuer: _jwt.Issuer,
        //        audience: _jwt.Audience,
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(_jwt.DurationInMinutes),
        //        signingCredentials: signingCredentials);

        //    return jwtSecurityToken;
        //}

        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var authModel = new AuthModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {

                authModel.Massage = "Invalid token";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(a => a.Token == token);
            if (!refreshToken.IsActive)
            {

                authModel.Massage = "Inactive token";
                return authModel;
            }
            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            return authModel;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(a => a.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;

        }
        public async Task<Result<AuthModel>> UpdateUserAsync(Guid id, EditUserViewModel model)
        {

            //get User by id 
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return Result<AuthModel>.Failure("User not found");

            var existEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existEmail != null && existEmail.Id != user.Id)
                return Result<AuthModel>.Failure("This Email is already taken by another user.");

            var existUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existUserName != null && existUserName.Id != user.Id)
                return Result<AuthModel>.Failure("This UserName is already taken.");

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.UserName = model.UserName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                //foreach (var error in result.Errors)
                //{
                //    errors += $"{error.Description},";
                //}
                errors = string.Join(", ", result.Errors.Select(a => a.Description));
                return Result<AuthModel>.Failure(errors);
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            var authModel = new AuthModel
            {
                IsAuthenticated = true,
                Email = user.Email,
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles = rolesList.ToList()
            };


            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if (activeRefreshToken is not null)
            {
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }


            return Result<AuthModel>.Success(authModel);

        }
        public async Task<Result<UserDto>> GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("uid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Result<UserDto>.Failure("Unauthorized: User ID not found in token");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<UserDto>.Failure("User not found");

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                LastLoginTime = user.LastLoginTime
            };

            return Result<UserDto>.Success(userDto);
        }
        public async Task<PagedResultDTo<UsersWithRolesDto>> GetAllUsersAsync(UserInput input)
        {
            var users = await _userRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
            var userMapper = _mapper.Map<List<UsersWithRoles>, List<UsersWithRolesDto>>(users);
            var totalCount = users.LongCount();
            return new PagedResultDTo<UsersWithRolesDto>(userMapper, totalCount);
        }
        public async Task<Result<bool>> ToggleUserStatusAsync(Guid userId, bool activate)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result<bool>.Failure("User not found");

            if (activate)
                await _userManager.SetLockoutEndDateAsync(user, null); // active
            else
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue); // unActive

            return Result<bool>.Success(true);

        }

        public async Task<Result<string>> DeleteAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Result<string>.Failure("Not found User");
           
            await _userManager.DeleteAsync(user);

            return Result<string>.Success("Delete sucssefly");
        }
    }
}
