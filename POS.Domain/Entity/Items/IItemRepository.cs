using POS.Domain.Entity.StockTransactions;
using POS.Domain.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Items
{
    public interface IItemRepository : IGenericRepository<Item, Guid>
    {
        Task<IReadOnlyList<ItemPrice>> GetItemsByIds(List<Guid> itemsIds);
        Task<List<ItemWithDetail>> GetListAsync(string? filterText = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);


        Task<long> GetCountAsync(
          string? filterText = null,
          string? sorting = null,
          int maxResultCount = int.MaxValue,
          int skipCount = 0,
          CancellationToken cancellationToken = default);


    }
}
