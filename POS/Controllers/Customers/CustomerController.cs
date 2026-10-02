using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.Customers;
using POS.Application.Contract.Invoices;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.API.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase, ICustomerAppService
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }
        [HttpPost]
        public Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto)
        {
            return _customerAppService.CreateAsync(createCustomerDto);
        }
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _customerAppService.DeleteAsync(id);
        }

        [HttpGet("{id}")]
        public Task<CustomerDto> GetAsync(Guid id)
        {
            return _customerAppService.GetAsync(id);
        }
        [HttpGet("GetCustomerLookup")]
        public Task<PagedResultDTo<SharedLookUp>> GetCustomerLookupAsync()
        {
            return _customerAppService.GetCustomerLookupAsync();
        }

        [HttpGet]
        public Task<PagedResultDTo<CustomerDto>> GetListAsync([FromQuery] CustomerInput input)
        {
            return _customerAppService.GetListAsync(input);
        }
        [HttpPut("{id}")]
        public Task<CustomerDto> UpdateAsync(Guid id,UpdateCustomersDto updateCustomerDto)
        {
            return _customerAppService.UpdateAsync(id,updateCustomerDto);
        }
    }
}
