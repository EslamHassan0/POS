using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.Warehouses;
using POS.Application.Warehouses;
using POS.Domain.Entity.Warehouses;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.API.Controllers.Warehouses
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase, IWarehouseAppServices
    {
        private readonly IWarehouseAppServices _warehouseAppServices;

        public WarehousesController(IWarehouseAppServices warehouseAppServices)
        {
            _warehouseAppServices = warehouseAppServices;
        }
        [HttpPost]
        public Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseDto dto)
        {
            return _warehouseAppServices.CreateAsync(dto);
        }
        [HttpDelete("{id}")]
        public Task<Result<string>> DeleteAsync(Guid id)
        {
            return _warehouseAppServices.DeleteAsync(id);
        }
        [HttpGet("{id}")]
        public Task<WarehouseDto> GetAsync(Guid id)
        {
            return _warehouseAppServices.GetAsync(id);
        }
        [HttpGet]
        public Task<PagedResultDTo<WarehouseDto>> GetListAsync()
        {
            return _warehouseAppServices.GetListAsync();
        }
        [HttpGet("GetWarehouseLookup")]
        public Task<PagedResultDTo<SharedLookUp>> GetWarehouseLookupAsync()
        {
            return _warehouseAppServices.GetWarehouseLookupAsync();
        }

        [HttpPut("{id}")]
        public Task<Result<WarehouseDto>> UpdateAsync(Guid id,UpdateWarehouseDto dto)
        {
            return _warehouseAppServices.UpdateAsync(id,dto);
        }
    }
}
