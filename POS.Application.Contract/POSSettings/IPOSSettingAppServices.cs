using Microsoft.AspNetCore.Http;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.POSSettings
{
    public interface IPOSSettingAppServices : IApplicationService
    {
        Task<POSSettingDto> GetSettingInfoAsync();
        Task<Result<POSSettingDto>> CreateAsync(CreatePOSSetting input);
        Task<Result<POSSettingDto>> UpdateAsync(Guid id, UpdatePOSSetting input);
    }
}
