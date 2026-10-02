using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using POS.Domain.Entity.Suppliers;
using POS.Domain.GenericRepository;
using POS.Domain.Identity;

namespace POS.Domain.Entity.Users
{
    public interface IUserRepository : IGenericRepository<IdentityApplicationUser, Guid>
    {
        Task<IdentityApplicationUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        Task<List<UsersWithRoles>> GetListAsync(string sorting = null,
          int maxResultCount = int.MaxValue,
          int skipCount = 0,
          string filter = null,
          CancellationToken cancellationToken = default);


        Task<List<IdentityApplicationUser>> SearchAsync(
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        CancellationToken cancellationToken = default
    );

        Task<long> GetCountAsync(
            CancellationToken cancellationToken = default);
    }
}
