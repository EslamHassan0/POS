using Microsoft.EntityFrameworkCore;

using POS.Domain.Entity.Items;
using POS.Domain.Shared.Extensions;
using POS.Domain.Shared.Item;
using POS.Domain.Shared.StockTransactions;
using POS.EFCore.POSContexts;
 
using System.Linq.Dynamic.Core;
 

namespace POS.EFCore.GenericRepository.Items
{
    public class ItemRepository : GenericRepository<Item, Guid>, IItemRepository , IRepositoryMarker
    {
        public ItemRepository(POSContext context) : base(context)
        {

        }

        public async Task<IReadOnlyList<ItemPrice>> GetItemsByIds(List<Guid> itemsIds)
        {
            if (itemsIds == null)
                throw new ArgumentNullException("itemsIds");

            var _dbSet = GetDbSet();
            var query = await _dbSet
                 .Where(a => itemsIds.Contains(a.Id) && a.IsActive)
                 .Select(a => new ItemPrice
                 {
                     Id = a.Id,
                     Price = a.Price,
                     Cost = a.Cost,
                     VatPercent = a.VatPercent,
                 })
                 .AsNoTracking()
                 .ToListAsync();

            return  query;
        }
        protected async Task<IQueryable<ItemWithDetail>> ApplyFilter(string? filterText = null)
        {
            var _dbContext = await GetDbContextAsync();
            var nurmalizedFilter = filterText?.ToLower();
            var query = (from item in _dbContext.Items
                         where item.IsActive && (nurmalizedFilter == null ||  item.Name.ToLower().Contains(nurmalizedFilter))
                         join st in _dbContext.StockTransactions on item.Id equals st.ItemId into stockGroup
                         select new ItemWithDetail
                         {
                             Id = item.Id,
                             Price = item.Price,
                             Cost = item.Cost,
                             VatPercent = item.VatPercent,
                             Barcode = item.Barcode,
                             Name = item.Name,
                             IsActive = item.IsActive,
                             CategoryName = item.Category.Name,
                             Image = item.Image,
                             StockQty = stockGroup.Sum(a=>a.TransactionType == TransactionType.In ? a.QtyChange : -a.QtyChange),
                             Code = item.Code
                         });
            return  query;
        }
        public async Task<List<ItemWithDetail>> GetListAsync(
            string filterText = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(filterText);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
           string filterText = null,
           string sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(filterText);
            return await query.LongCountAsync(cancellationToken);
        }
    }
}
