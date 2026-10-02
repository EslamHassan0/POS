using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Application.Contract.Customers;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Contract.ItemCategories
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<CategoryDto> CreateAsync(CreateCategoryDto  dto);
        Task<CategoryDto> UpdateAsync(Guid id, CreateCategoryDto updateCustomerDto);
        Task<PagedResultDTo<CategoryDto>> GetListAsync(CategoryInput input);

        Task<CategoryDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<List<SharedLookUp>> GetCategorieLookupAsync();
    }

}
