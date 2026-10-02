using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Entity.Invoices;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Domain.Entity.Categories
{
    public interface ICategoryRepository : IGenericRepository<Category, Guid>
    {
        Task<List<Category>> GetListAsync(
       string filterText = null,
       string sorting = null,
       int maxResultCount = int.MaxValue,
       int skipCount = 0,
       CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            string filterText = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);

        Task<List<SharedLookUp>> GetCategorieLookupAsync();
    }
}
