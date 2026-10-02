using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.ItemCategories;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.API.Controllers.ItemCategories
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCategoriesController : ControllerBase, ICategoryAppService
    {
        private readonly ICategoryAppService _itemCategorieAppService;

        public ItemCategoriesController(ICategoryAppService itemCategorieAppService)
        {
            _itemCategorieAppService = itemCategorieAppService;
        }
        [HttpPost]
        public Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
           return _itemCategorieAppService.CreateAsync(dto);
        }
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _itemCategorieAppService.DeleteAsync(id);
        }
        [HttpGet("{id}")]
        public Task<CategoryDto> GetAsync(Guid id)
        {
            return _itemCategorieAppService.GetAsync(id);
        }
        [HttpGet("GetCategorieLookup")]
        public Task<List<SharedLookUp>> GetCategorieLookupAsync()
        {
            return _itemCategorieAppService.GetCategorieLookupAsync();
        }
        [HttpGet("GetList")]
        public Task<PagedResultDTo<CategoryDto>> GetListAsync([FromQuery]CategoryInput input)
        {
            return _itemCategorieAppService.GetListAsync(input);
        }
        [HttpPut("{id}")]
        public Task<CategoryDto> UpdateAsync(Guid id, CreateCategoryDto updateCustomerDto)
        {
            return _itemCategorieAppService.UpdateAsync(id, updateCustomerDto);
        }
    }
}
