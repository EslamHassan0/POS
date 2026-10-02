using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Entity.Suppliers;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Domain.Entity.Warehouses
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse, Guid>
    {
        Task<List<Warehouse>> GetListAsync(
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

        Task<List<SharedLookUp>> GetWarehouseLookupAsync();
    }
}
