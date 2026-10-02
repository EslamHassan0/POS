using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Categories;
using POS.Domain.Entity.Items;
using POS.Domain.Shared.Extensions;
using POS.Domain.Shared.Item;
using POS.Domain.Shared.ItemCategories;
using POS.Domain.Shared.SharedLookupDtos;
using POS.EFCore.POSContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.Categories
{
    public class CategoriesRepository : GenericRepository<Category, Guid>, ICategoryRepository, IRepositoryMarker
    {
        public CategoriesRepository(POSContext context) : base(context)
        {
        }

        public async Task<long> GetCountAsync(string filterText = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter((GetDbSet()),filterText);
            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<List<Category>> GetListAsync(string filterText = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter((GetDbSet()),filterText);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemCategorieConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }
        protected async Task<IQueryable<Category>> ApplyFilter(
            IQueryable<Category> query,
            string? filterText = null)
        {
            return query.WhereIf(!string.IsNullOrEmpty(filterText), a => a.Name.Contains(filterText));
        }

        public async Task<List<SharedLookUp>> GetCategorieLookupAsync()
        {
            var _dbContext = await GetDbContextAsync();
            var items = _dbContext.Categories
                        .AsNoTracking()
                        .Select(a => new SharedLookUp
                        {
                            Id = a.Id,
                            Name = a.Name
                        })
                        .ToList();

            return items;
        }
    }
}
