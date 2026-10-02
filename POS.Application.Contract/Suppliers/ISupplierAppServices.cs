using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Contract.Suppliers
{
    public interface ISupplierAppServices : IApplicationService
    {
        Task<Result<SupplierDto>> CreateAsync(CreateSupplierDto dto);
        Task<PagedResultDTo<SupplierWithOrdersDto>> GetListAsync(SupplierInput input);
        Task<Result<SupplierDto>> DeleteAsync(Guid id);
        Task<SupplierDto> GetAsync(Guid id);
        Task<Result<SupplierDto>> UpdateAsync(Guid id, CreateSupplierDto dto);

        Task<PagedResultDTo<SharedLookUp>> GetSupplierLookupAsync();
    }
}
