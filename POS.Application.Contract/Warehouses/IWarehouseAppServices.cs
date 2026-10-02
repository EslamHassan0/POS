using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Application.Contract;
using POS.Application.Contract.Warehouses;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Warehouses
{
    public interface IWarehouseAppServices : IApplicationService
    {

        Task<PagedResultDTo<WarehouseDto>> GetListAsync();
        Task<WarehouseDto> GetAsync(Guid id);
        Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseDto dto);
        Task<Result<WarehouseDto>> UpdateAsync(Guid id , UpdateWarehouseDto dto);
        Task<Result<string>>  DeleteAsync(Guid id);

        Task<PagedResultDTo<SharedLookUp>> GetWarehouseLookupAsync();

    


    }
}
