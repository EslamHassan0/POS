using Microsoft.AspNetCore.Http;
using POS.Domain.Shared.FileStorages;
using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.FileStorages;

public interface IFileStorageService : IApplicationService
{
    Task<Result<string>> UploadAsync(FileStorageType type, IFormFile imageFile);
    Task DeleteAsync(FileStorageType type, string fileUrl);
    string? GetFileUrl(FileStorageType type,
        string? fileName);
    string GenerateAndSaveBarcodeImage(string itemName);
}
