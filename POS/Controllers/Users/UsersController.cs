using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.Authentications;
using POS.Application.Contract.Users;
using POS.Domain.Shared.Results;

namespace POS.API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAppService.RegisterAsync(registerDto);

            if (!result.IsAuthenticated)
                return BadRequest(new { message=result.Massage });


            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new
            {
                Email = result.Email,
                UserName = result.UserName,
                LastLoginTime = result.LastLoginTime,
                roles = result.Roles,

                token = result.Token


            });


        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAppService.LoginAsync(dto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Massage);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new
            {
                Email = result.Email,
                UserName = result.UserName,
                LastLoginTime = result.LastLoginTime,
                roles = result.Roles,

                token = result.Token
            });


        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RolesDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAppService.AddRoleAsync(dto);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(dto);
        }
        [HttpPost("GetRefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Refresh token is missing.");

            var result = await _userAppService.RefreshTokenAsync(refreshToken);
            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new
            {
                Email = result.Email,
                UserName = result.UserName,
                roles = result.Roles,
                token = result.Token
            });
        }

        [HttpPost("RevokeToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RevokeToken([FromBody] string  revokeTokenDto)
        {
            var token = revokeTokenDto ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("token is required!");

            var result = await _userAppService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("token is invalid");

            return Ok();
        }
        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptiones = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                //SameSite = SameSiteMode.Strict, // CSRF
                SameSite = SameSiteMode.None, // CSRF

            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptiones);

        }
 
        [HttpPut("update-profile/{id}")]
        public async Task<IActionResult> UpdateUserAsync( Guid id,  EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAppService.UpdateUserAsync(id,model);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return Ok(new
            {
                Email = result.Value.Email,
                UserName = result.Value.UserName,
                LastLoginTime = result.Value.LastLoginTime,
                roles = result.Value.Roles,
                token = result.Value.Token
            });
        }

        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
             
            var result = await _userAppService.GetCurrentUser();
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Error });

            return Ok(new
            {   
                Id = result.Value.Id,
                Email = result.Value.Email,
                UserName = result.Value.UserName,
                LastName = result.Value.LastName,
                FirstName = result.Value.FirstName,
                LastLoginTime = result.Value.LastLoginTime,
            });
        }
        [HttpGet("get-All-user")]
        public  async Task<IActionResult> GetAllUsersAsync([FromQuery] UserInput input)
        {
            var result = await _userAppService.GetAllUsersAsync(input);
            return Ok(result);
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
            => Ok(await _userAppService.DeleteAsync(userId));

        [HttpPut("{userId}/status")]
        public async Task<IActionResult> ToggleStatus(Guid userId, [FromQuery] bool activate)
            => Ok(await _userAppService.ToggleUserStatusAsync(userId, activate));

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> ChangeRole(Guid userId, [FromBody] string role)
             => Ok(await _userAppService.ChangeUserRoleAsync(userId, role));

        [HttpPost("CreateUserByAdmin")]
        public async Task<IActionResult> CreateUser(CreateUserByAdminDto dto)
            => Ok(await _userAppService.CreateUserByAdminAsync(dto));

    }
}
