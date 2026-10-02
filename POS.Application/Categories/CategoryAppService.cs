using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using POS.Application.Contract.ItemCategories;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.Categories;
using POS.Domain.Entity.Items;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Categories
{
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly ICategoryRepository _itemCategoriesRepository;
        private readonly CategoryManager _categorieManager;
        public CategoryAppService(
            ICategoryRepository itemCategoriesRepository,
            CategoryManager categorieManager,
            IMapper mapper) :base(mapper)
        {
            _itemCategoriesRepository = itemCategoriesRepository;
            _categorieManager = categorieManager;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = await _categorieManager.CreateAsync(dto.Name, dto.Description);

            return _mapper.Map<Category, CategoryDto>(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _itemCategoriesRepository.GetByIdAsync(id);
             _itemCategoriesRepository.Delete(category);
            await _itemCategoriesRepository.SaveChangesAsync();
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            return _mapper.Map<Category, CategoryDto>(await _itemCategoriesRepository.GetByIdAsync(id));
        }

        public async Task<List<SharedLookUp>> GetCategorieLookupAsync()
        {
            var items = await _itemCategoriesRepository.GetCategorieLookupAsync();
            return items;
        }

        public async Task<PagedResultDTo<CategoryDto>> GetListAsync(CategoryInput input)
        {
            var items = await _itemCategoriesRepository.GetListAsync(input.FilterText, input.Sorting, input.MaxResultCount);
            var totalCount = await _itemCategoriesRepository.GetCountAsync(input.FilterText, input.Sorting, input.MaxResultCount);

            var itemMapper =  _mapper.Map<List<Category>, List<CategoryDto>>(items);
            return new PagedResultDTo<CategoryDto>(itemMapper, totalCount);
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, CreateCategoryDto updateCustomerDto)
        {
            var category = await _categorieManager.UpdateAsync(id, updateCustomerDto.Name,updateCustomerDto.Description);

            return _mapper.Map<Category, CategoryDto>(category);
        }
    }
}
