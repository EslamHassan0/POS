using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Entity.Invoices;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Domain.Entity.Customers
{
    public interface ICustomerRepository : IGenericRepository<Customer, Guid>
    {

        Task<List<Customer>> GetListAsync(string? filterText = null, string? customerName = null, string sorting = null,
              int maxResultCount = int.MaxValue,
              int skipCount = 0,
              CancellationToken cancellationToken = default);

        Task<List<SharedLookUp>> GetCustomerLookupAsync();


    }
}
