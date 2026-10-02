using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Contract.POSSettings;
using POS.Domain.Shared.Results;

namespace POS.API.Controllers.POSSettings
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSSettingController : ControllerBase, IPOSSettingAppServices
    {
        private readonly IPOSSettingAppServices _pOSSettingAppServices;
        public POSSettingController(IPOSSettingAppServices pOSSettingAppServices)
        {
            _pOSSettingAppServices = pOSSettingAppServices;
        }

        [HttpGet]
        public Task<POSSettingDto> GetSettingInfoAsync()
        {
            return _pOSSettingAppServices.GetSettingInfoAsync();
        }

        [HttpPost]
        public Task<Result<POSSettingDto>> CreateAsync([FromForm] CreatePOSSetting input)
        {

            return _pOSSettingAppServices.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<Result<POSSettingDto>> UpdateAsync(Guid id, [FromForm] UpdatePOSSetting input)
        {
            return _pOSSettingAppServices.UpdateAsync(id, input);
        }
    }
}
