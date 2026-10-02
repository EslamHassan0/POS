using AutoMapper;
using POS.Application.Contract.FileStorages;
using POS.Application.Contract.Items; 
using POS.Domain.Entity.Items;
using POS.Domain.Entity.POSSettings;
using POS.Domain.Shared.FileStorages;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.Results;


namespace POS.Application.Items
{
    public class ItemAppService  : ApplicationService, IItemAppService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ItemManager _invoiceManager;
        private readonly IFileStorageService _fileStorageService;
        public ItemAppService(
            IMapper mapper,
            IItemRepository itemRepository,
            ItemManager invoiceManager,
            IFileStorageService fileStorageService) : base(mapper)
        {
            _itemRepository = itemRepository;
            _invoiceManager = invoiceManager;
            _fileStorageService = fileStorageService;
        }


        public async Task<ItemDto> GetAsync(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            var itemDto=  _mapper.Map<Item, ItemDto>(item!);
            itemDto.ImageUrl = _fileStorageService.GetFileUrl(FileStorageType.Item, item.Image ?? " ");
            return itemDto;
        }
        public async Task<Result<ItemDto>> CreateAsync(CreateItemDto itemDto)
        {
            string? itemImg = null;
            var result = (await _fileStorageService.UploadAsync(FileStorageType.Item,itemDto.Image));
            if (itemDto.Image != null && result.IsSuccess)
                itemImg = result.Value;
            else
                return Result<ItemDto>.Failure(result.Error);

            string barcode = _fileStorageService.GenerateAndSaveBarcodeImage(itemDto.Name);

            var item = await _invoiceManager.CreateAsync(itemDto.Name, itemDto.Code, barcode, itemDto.Price, itemDto.Cost,
                    itemDto.VatPercent, itemDto.IsVATEligible, itemDto.CategoryId, itemImg, itemDto.Unit);
            var mapperItem =  _mapper.Map<Item, ItemDto>(item);

            
            mapperItem.ImageUrl = _fileStorageService.GetFileUrl(FileStorageType.Item, item.Image ?? " ");

            return Result<ItemDto>.Success(mapperItem);
        }
        public async Task<Result<ItemDto>> UpdateAsync(Guid id,CreateItemDto itemDto)
        {
            string? itemImg = null;
            var result = (await _fileStorageService.UploadAsync(FileStorageType.Item,itemDto.Image));
            if (itemDto.Image != null && result.IsSuccess)
                itemImg = result.Value;
            else
                return Result<ItemDto>.Failure(result.Error);


            


            var item = await _invoiceManager.UpdateAsync(id, itemDto.Name, itemDto.Code, itemDto.Price, itemDto.Cost,
                    itemDto.VatPercent, itemDto.IsVATEligible, itemDto.CategoryId, itemImg, itemDto.Unit);

            var mapperItem = _mapper.Map<Item, ItemDto>(item);


            mapperItem.ImageUrl =  _fileStorageService.GetFileUrl(FileStorageType.Item,item.Image ?? " ");

            
            return Result<ItemDto>.Success(mapperItem);
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            _itemRepository.Delete(item!);
            await _itemRepository.SaveChangesAsync();
        }
        public async Task<PagedResultDTo<ItemWithDetailDto>> GetListAsync(ItemInput input)
        {
            
            //var baseUrl = await _fileStorageService.GetBase64Async();

            var items = await _itemRepository.GetListAsync(input.FilterText, input.Sorting, input.MaxResultCount);
            var totalCount = await _itemRepository.GetCountAsync(input.FilterText, input.Sorting, input.MaxResultCount);

            var itemMapp = _mapper.Map<List<ItemWithDetail>, List<ItemWithDetailDto>>(items);
            //item = item.Select(item => new ItemWithDetailDto
            //{
            //    Id = item.Id,
            //    Price = item.Price,
            //    Cost = item.Cost,
            //    VatPercent = item.VatPercent,
            //    Barcode = item.Barcode,
            //    Name = item.Name,
            //    IsActive = item.IsActive,
            //    CategoryName = item.CategoryName,
            //    Image = item.Image,
            //    Unit = item.Unit,
            //    Code = item.Code,
            //    StockQty = item.StockQty,
            //    //ImageUrl = await _fileStorageService.GetBase64Async(item.ImageUrl)

            //}).ToList();


            foreach (var item in itemMapp)
            {
                if (!string.IsNullOrEmpty(item.Image))
                {
                    item.ImageUrl =
                        _fileStorageService.GetFileUrl(FileStorageType.Item, item.Image);
                }
            }


            return new PagedResultDTo<ItemWithDetailDto>(itemMapp, totalCount);
        }

        

    }
}
