using POS.Application.Contract.Invoices;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Items
{
    public interface IItemAppService : IApplicationService
    {
        Task<PagedResultDTo<ItemWithDetailDto>> GetListAsync(ItemInput input);
        Task<Result<ItemDto>> CreateAsync(CreateItemDto itemDto);
        Task<Result<ItemDto>> UpdateAsync(Guid id, CreateItemDto itemDto);
        Task DeleteAsync(Guid id);
        Task<ItemDto> GetAsync(Guid id);
        //public void DeleteFile(string fileNameWithExtension);
    }
}
