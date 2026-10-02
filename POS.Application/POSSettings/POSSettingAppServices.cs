using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using POS.Application.Contract.FileStorages;
using POS.Application.Contract.Items;
using POS.Application.Contract.POSSettings;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.POSSettings;
using POS.Domain.Shared.FileStorages;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace POS.Application.POSSettings
{
    public class POSSettingAppServices : ApplicationService, IPOSSettingAppServices
    {
        private readonly IPOSSettingRepository _pOSSettingRepository;
        private readonly POSSettingManager _pOSSettingManager;
        private readonly IFileStorageService _fileStorageService;
        public POSSettingAppServices(
            IPOSSettingRepository pOSSettingRepository,
            IMapper mapper,
            POSSettingManager pOSSettingManager,
            IFileStorageService fileStorageService) : base(mapper)
        {
            _pOSSettingRepository = pOSSettingRepository;
            _pOSSettingManager = pOSSettingManager;
            _fileStorageService = fileStorageService;
        }

        public async Task<POSSettingDto> GetSettingInfoAsync()
        {
           var setting = await _pOSSettingRepository.GetSettingInfoAsync();
           var settingDto = _mapper.Map<POSSetting,POSSettingDto>(setting);
            settingDto.LogoBase64 = _fileStorageService.GetFileUrl(FileStorageType.Setting, setting.LogoPath ?? " ");
            return settingDto;
        }

        public async Task<Result<POSSettingDto>> CreateAsync(CreatePOSSetting input)
        {
            string? logoPath = null;
            
            if (input.Logo != null && input.Logo.Length > 0)
            {
                var resultImg = await _fileStorageService.UploadAsync(FileStorageType.Setting, input.Logo);
                if (resultImg.IsSuccess)
                    logoPath = resultImg.Value;
                else
                    return Result< POSSettingDto >.Failure(resultImg.Error);

            }
 


            var pos = await _pOSSettingManager.CreateAsync(input.BusinessName,input.Address , input.BusinessName ,
                input.DefaultVAT , input.InvoicePrefix , input.StartingNumber , input.VisaFee , input.AllowNegativeStock , input.LowStockAlert, logoPath);

            var dto = _mapper.Map<POSSetting, POSSettingDto>(pos);
            dto.LogoBase64 = _fileStorageService.GetFileUrl(FileStorageType.Setting, pos.LogoPath ?? " ");

            return Result < POSSettingDto >.Success(dto);
        }
        public async Task<Result<POSSettingDto>> UpdateAsync(Guid id, UpdatePOSSetting input)
        {
            var existing = await _pOSSettingRepository.GetByIdAsync(id);
            string? logoPath = existing.LogoPath;
            if (input.Logo != null && input.Logo.Length > 0)
            {
               
                var newLogoPath = await _fileStorageService.UploadAsync(FileStorageType.Setting, input.Logo);

                if (!string.IsNullOrEmpty(existing.LogoPath))
                {
                    await _fileStorageService.DeleteAsync(FileStorageType.Setting, existing.LogoPath);
                }
                if(newLogoPath.IsSuccess)
                    logoPath = newLogoPath.Value;
                else
                    return Result<POSSettingDto>.Failure(newLogoPath.Error);
            }
            else if (input.RemoveLogo)
            {
                if (!string.IsNullOrEmpty(existing.LogoPath))
                {
                    await _fileStorageService.DeleteAsync(FileStorageType.Setting, existing.LogoPath);
                }

                logoPath = null;
            }


            var pos = await _pOSSettingManager.UpdateAsync(id,input.BusinessName, input.Address, input.Phone,
            input.DefaultVAT, input.InvoicePrefix, input.StartingNumber, input.VisaFee, input.AllowNegativeStock, input.LowStockAlert, logoPath);

            var dto = _mapper.Map<POSSetting, POSSettingDto>(pos);
            dto.LogoBase64 = _fileStorageService.GetFileUrl(FileStorageType.Setting, pos.LogoPath ?? " ");

            return Result<POSSettingDto>.Success(dto);
        }

    }
}
