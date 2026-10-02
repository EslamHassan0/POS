using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using POS.Application.Contract.Warehouses;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.Entity.Warehouses;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.PurchaseOrderCheckTypes;
using POS.Domain.Shared.Results;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Warehouses
{
    public class WarehouseAppServices : ApplicationService, IWarehouseAppServices
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly WarehouseManager _warehouseManager;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        public WarehouseAppServices(IWarehouseRepository warehouseRepository,
            WarehouseManager warehouseManager,
            IMapper mapper,
            IPurchaseOrderRepository purchaseOrderRepository) : base(mapper)
        {
            _warehouseRepository = warehouseRepository;
            _warehouseManager = warehouseManager;
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseDto dto)
        {
            var warehouse = await _warehouseManager.CreateAsync(dto.Name, dto.Location, dto.IsDefault, dto.IsActive);
            if (!warehouse.IsSuccess)
                return Result<WarehouseDto>.Failure(warehouse.Error);

            return Result<WarehouseDto>.Success(_mapper.Map<Warehouse, WarehouseDto>(warehouse.Value));
        }
        public async Task<Result<WarehouseDto>> UpdateAsync(Guid id, UpdateWarehouseDto dto)
        {
            var warehouse = await _warehouseManager.UpdateAsync(id, dto.Name, dto.Location, dto.IsDefault, dto.IsActive);
            if (!warehouse.IsSuccess)
                return Result<WarehouseDto>.Failure(warehouse.Error);

            return Result<WarehouseDto>.Success(_mapper.Map<Warehouse, WarehouseDto>(warehouse.Value));
        }
        public async Task<Result<string>> DeleteAsync(Guid id)
        {
            if(await _purchaseOrderRepository.HasPurchaseOrderId(id, PurchaseOrderCheckType.Warehouse))
            {
                return Result<string>.Failure("The warehouse cannot be deleted because it has existing purchase orders");
            }
            var warehouse = await _warehouseRepository.GetByIdAsync(id);
            if (warehouse != null)
            {
                _warehouseRepository.Delete(warehouse);
                await _warehouseRepository.SaveChangesAsync();

                return Result<string>.Success("");
            }
            return Result<string>.Success("");
        }

        public async Task<WarehouseDto> GetAsync(Guid id)
        {
            return _mapper.Map<Warehouse, WarehouseDto>(await _warehouseRepository.GetByIdAsync(id));
        }

        public async Task<PagedResultDTo<WarehouseDto>> GetListAsync()
        {
            var items = _warehouseRepository.GetQueryable();


            return new PagedResultDTo<WarehouseDto>(_mapper.Map<List<Warehouse>, List<WarehouseDto>>(items.ToList()), items.LongCount());

        }

        public async Task<PagedResultDTo<SharedLookUp>> GetWarehouseLookupAsync()
        {
            var items = await _warehouseRepository.GetWarehouseLookupAsync();
            return new PagedResultDTo<SharedLookUp>(items, (long)items.Count);
        }
    }
}
