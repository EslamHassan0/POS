using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Authentications
{
    public interface IAuthrAppServices : IApplicationService
    {
        //Task<AuthModel> RegisterAsync(RegisterDto dto);
        //Task<AuthModel> LoginAsync(LoginDto dto);
        //Task<string> AddRoleAsync(RolesDto dto);

        //Task<AuthModel> RefreshTokenAsync(string token);
        //Task<bool> RevokeTokenAsync(string token);
        //ApplicationUserDo FindUser(Func<ApplicationUserDo, bool> predicate);


        Task<AuthModel> RegisterAsync(RegisterDto dto);
        Task<AuthModel> LoginAsync(LoginDto dto);
        Task<string> AddRoleAsync(RolesDto dto);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);


        //Task<UserDto> FindByEmailAsync(string email);
        //Task<UserDto> FindByUsernameAsync(string username);
    }
}
