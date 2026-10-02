using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Entity.Items;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Domain.Entity.Suppliers
{
    public interface ISupplierRepository : IGenericRepository<Supplier, Guid>
    {
        Task<List<SupplierWithOrders>> GetListAsync(
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

        Task<List<SharedLookUp>> GetSupplierLookupAsync();
    }
}
