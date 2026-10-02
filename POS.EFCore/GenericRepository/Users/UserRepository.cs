using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Domain.Entity.Users;
using POS.Domain.Identity;
using POS.Domain.Shared.Extensions;
using POS.EFCore.POSContexts;

namespace POS.EFCore.GenericRepository.Users
{
    public class UserRepository : GenericRepository<IdentityApplicationUser, Guid>, IUserRepository, IRepositoryMarker
    {
        public UserRepository(POSContext context) : base(context)
        {
        }

        public async Task<IdentityApplicationUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await (GetDbSet().OrderBy(x => x.Id)).FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public async Task<List<UsersWithRoles>> GetListAsync(
          string sorting = null,
          int maxResultCount = int.MaxValue,
          int skipCount = 0,
          string filter = null,
          CancellationToken cancellationToken = default)
        {

            var _dbContext = await GetDbContextAsync();
            var query =  (from user in _dbContext.Users
                               join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                               join role in _dbContext.Roles on userRole.RoleId equals role.Id
                               where user.IsActive == true
                               select new UsersWithRoles
                               {
                                   Id = user.Id,
                                   FirstName = user.FirstName,
                                   IsActive = user.IsActive,
                                   LastName = user.LastName,
                                   UserName = user.UserName,
                                   PhoneNumber = user.PhoneNumber,
                                   LastLoginTime = user.LastLoginTime,
                                   Email = user.Email,
                                   Role = role.Name
                               });
                    


            return await query.OrderBy(a => a.UserName)
                    .PageBy(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<IdentityApplicationUser>> SearchAsync(
           string sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           string filter = null,
           CancellationToken cancellationToken = default)
        {
            return await (GetDbSet())
                .WhereIf(!filter.IsNullOrEmpty(),
                    a => a.UserName.Contains(filter) ||
                    (a.Email != null && a.Email.Contains(filter)) ||
                    (a.FirstName != null && a.FirstName.Contains(filter)) ||
                    (a.LastName != null && a.LastName.Contains(filter))
                    )
                 .OrderBy(a => a.UserName)
                 .PageBy(skipCount, maxResultCount)
                 .ToListAsync(cancellationToken);

        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await (GetDbSet()).LongCountAsync(cancellationToken);
        }
    }
}
