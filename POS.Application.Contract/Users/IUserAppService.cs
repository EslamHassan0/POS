using POS.Application.Contract.Authentications;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<AuthModel> RegisterAsync(RegisterDto dto);
        Task<AuthModel> LoginAsync(LoginDto dto);
        Task<string> AddRoleAsync(RolesDto dto);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<Result<AuthModel>> UpdateUserAsync(Guid id, EditUserViewModel model);

        Task<Result<UserDto>> GetCurrentUser();
        Task<Result<string>> CreateUserByAdminAsync(CreateUserByAdminDto dto);
        Task<PagedResultDTo<UsersWithRolesDto>> GetAllUsersAsync(UserInput input);
        Task<Result<bool>> ToggleUserStatusAsync(Guid userId, bool activate);
        Task<Result<string>> DeleteAsync(Guid userId);
        Task<Result<string>> ChangeUserRoleAsync(Guid userId, string newRole);
    }
}
