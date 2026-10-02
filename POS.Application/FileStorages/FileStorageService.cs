using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using POS.Application.Contract.FileStorages;
using POS.Domain.Shared.FileStorages;
using POS.Domain.Shared.Results;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;

namespace POS.Application.FileStorages;

public class FileStorageService : ApplicationService, IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IOptionsMonitor<FileUploadOptions> _options;
    private readonly IOptionsMonitor<FileStorage> _fileStorage;
    public FileStorageService(IMapper mapper, IWebHostEnvironment environment, IOptionsMonitor<FileUploadOptions> options, IOptionsMonitor<FileStorage> fileStorage) : base(mapper)
    {
        _environment = environment;
        _options = options;
        _fileStorage = fileStorage;
    }
    public string? GetFileUrl(
        FileStorageType type,
        string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return null;

        var requestPath =
            _fileStorage.CurrentValue.RequestPath.TrimEnd('/');

        var folder = type switch
        {
            FileStorageType.Item =>
                _fileStorage.CurrentValue.ItemFolder,

            FileStorageType.Purchase =>
                _fileStorage.CurrentValue.PurchaseFolder,

            FileStorageType.Invoice =>
                _fileStorage.CurrentValue.POSInvoiceFolder,

            FileStorageType.Setting =>
                _fileStorage.CurrentValue.SettingFolder,

            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        var relativeFolder = folder
            .Replace('\\', '/')
            .Trim('/');

        var file = fileName
            .TrimStart('/')
            .Replace('\\', '/');

        return $"{requestPath}/{relativeFolder}/{file}";
    }

    public async Task<Result<string>> UploadAsync(FileStorageType type, IFormFile imageFile)
    {
        if (imageFile == null)
        {
            return Result<string>.Failure("You cannot upload an empty file.");
        }

        var contentPath = _environment.ContentRootPath;


        var folder = type switch
        {
            FileStorageType.Item =>
                _fileStorage.CurrentValue.ItemFolder,

            FileStorageType.Purchase =>
                _fileStorage.CurrentValue.PurchaseFolder,

            FileStorageType.Invoice =>
                _fileStorage.CurrentValue.POSInvoiceFolder,

            FileStorageType.Setting =>
                _fileStorage.CurrentValue.SettingFolder,

            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
        var uploadPath = _fileStorage.CurrentValue.UploadsFolder;
        var path = Path.Combine(contentPath, uploadPath, folder);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Check the allowed extenstions
        var currentValueAllowed = _options
                                    .CurrentValue
                                    .AllowedFileExtensions
                                    .ToHashSet();

        var ext = Path.GetExtension(imageFile.FileName);
        if (!currentValueAllowed.Contains(ext))
        {
            return Result<string>.Failure($"Only {string.Join(",", currentValueAllowed)} are allowed.");
            //throw new ArgumentException($"Only {string.Join(",", currentValueAllowed)} are allowed.");
        }

        // generate a unique filename
        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
       
        var fileNameWithPath = Path.Combine(path, fileName);
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        return Result<string>.Success(fileName);
    }


    public async Task DeleteAsync(FileStorageType type, string fileNameWithExtension)
    {
        if (string.IsNullOrEmpty(fileNameWithExtension))
        {
            throw new ArgumentNullException(nameof(fileNameWithExtension));
        }
        var contentPath = _environment.ContentRootPath;

        var folder = type switch
        {
            FileStorageType.Item =>
                _fileStorage.CurrentValue.ItemFolder,

            FileStorageType.Purchase =>
                _fileStorage.CurrentValue.PurchaseFolder,

            FileStorageType.Invoice =>
                _fileStorage.CurrentValue.POSInvoiceFolder,

            FileStorageType.Setting =>
                _fileStorage.CurrentValue.SettingFolder,

            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
        var fileUpload = _fileStorage.CurrentValue.UploadsFolder;
        //var path = Path.Combine(fileUpload, folder, fileNameWithExtension);
        var path = Path.Combine(contentPath, fileUpload, folder, fileNameWithExtension);


        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Invalid file path");
        }
        File.Delete(path);
    }
    public string GenerateAndSaveBarcodeImage(string itemName)
    {
        var prefix = new string(itemName
            .ToUpper()
            .Where(char.IsLetterOrDigit)
            .Take(3)
            .ToArray());

        // بدل الـ ID استخدم Guid
        var unique = Guid.NewGuid().ToString("N")[..6].ToUpper();

        var barcodeValue = $"{prefix}-{unique}";
        // هيطلع: BOT-A3F9B2

        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Height = 80,
                Width = 250,
                Margin = 10,
                PureBarcode = false
            }
        };

        using var bitmap = writer.Write(barcodeValue);

        var fileName = $"barcode_{unique}.png";
        var uploadsFolder = _fileStorage.CurrentValue.UploadsFolder;
        var path = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, fileName);

        using var ms = new MemoryStream();
        bitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
        File.WriteAllBytes(path, ms.ToArray());

        return barcodeValue;
    }

}
