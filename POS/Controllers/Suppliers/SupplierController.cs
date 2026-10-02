using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.Suppliers;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.API.Controllers.Suppliers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase, ISupplierAppServices
    {
        private readonly ISupplierAppServices _supplierAppServices;

        public SupplierController(ISupplierAppServices supplierAppServices)
        {
            _supplierAppServices = supplierAppServices;
        }
        [HttpPost]
        public Task<Result<SupplierDto>> CreateAsync(CreateSupplierDto dto)
        {
           return _supplierAppServices.CreateAsync(dto);
        }
        [HttpDelete("{id}")]
        public Task<Result<SupplierDto>> DeleteAsync(Guid id)
        {
            return _supplierAppServices.DeleteAsync(id);
        }
        [HttpGet("{id}")]
        public Task<SupplierDto> GetAsync(Guid id)
        {
            return _supplierAppServices.GetAsync(id);
        }
        [HttpGet]
        public Task<PagedResultDTo<SupplierWithOrdersDto>> GetListAsync([FromQuery]SupplierInput input)
        {
            return _supplierAppServices.GetListAsync(input);
        }
        [HttpGet("GetSupplierLookup")]
        public Task<PagedResultDTo<SharedLookUp>> GetSupplierLookupAsync()
        {
            return _supplierAppServices.GetSupplierLookupAsync();
        }

        [HttpPut("{id}")]
        public Task<Result<SupplierDto>> UpdateAsync(Guid id, CreateSupplierDto dto)
        {
            return _supplierAppServices.UpdateAsync(id, dto);
        }
    }
}
