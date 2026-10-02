using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Application.Contract.Invoices;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Contract.Customers
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<CustomerDto> CreateAsync(CreateCustomerDto  createCustomerDto);
        Task<CustomerDto> UpdateAsync(Guid id,UpdateCustomersDto updateCustomerDto);
        Task<PagedResultDTo<CustomerDto>> GetListAsync(CustomerInput input);

        Task<CustomerDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PagedResultDTo<SharedLookUp>> GetCustomerLookupAsync();
    }
}
