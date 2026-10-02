using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.Items;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;

namespace POS.API.Controllers.Items
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase, IItemAppService
    {
        private readonly IItemAppService _itemAppService;

        public ItemController(IItemAppService itemAppService)
        {
            _itemAppService = itemAppService;
        }
        [HttpPost]
        public Task<Result<ItemDto>> CreateAsync([FromForm]  CreateItemDto itemDto)
        {
            return _itemAppService.CreateAsync(itemDto);
        }
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _itemAppService.DeleteAsync(id);
        }
        [HttpGet("{id}")]
        public Task<ItemDto> GetAsync(Guid id)
        {
            return _itemAppService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDTo<ItemWithDetailDto>> GetListAsync([FromQuery]ItemInput input)
        {
            return  _itemAppService.GetListAsync(input);
        }
        [HttpPut("{id}")]
        public Task<Result<ItemDto>> UpdateAsync(Guid id, [FromForm]  CreateItemDto itemDto)
        {
            return _itemAppService.UpdateAsync(id, itemDto);
        }
    }
}
