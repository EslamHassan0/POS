using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using POS.Application.Contract.PurchaseOrders;
using POS.Application.Contract.Suppliers;
using POS.Application.PurchaseOrders;
using POS.Domain.Entity.Items;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.Entity.Suppliers;
using POS.Domain.Entity.Warehouses;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.PurchaseOrderCheckTypes;
using POS.Domain.Shared.Results;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Suppliers
{
    public class SupplierAppServices : ApplicationService, ISupplierAppServices
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly SupplierManager _supplierManager;
        
        public SupplierAppServices(ISupplierRepository supplierRepository,
            SupplierManager supplierManager,
            IMapper mapper,
            IPurchaseOrderRepository purchaseOrderRepository) : base(mapper)
        {
            _supplierRepository = supplierRepository;
            _supplierManager = supplierManager;
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<Result<SupplierDto>> CreateAsync(CreateSupplierDto dto)
        {
            var supplier = await _supplierManager.CreateAsync(dto.Name,dto.Phone,dto.Address,dto.Email);
            return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(supplier));
        }

        public async Task<Result<SupplierDto>> UpdateAsync(Guid id, CreateSupplierDto dto)
        {
            var supplier = await _supplierManager.UpdateAsync(id, dto.Name, dto.Phone, dto.Address, dto.Email);
            if (!supplier.IsSuccess)
                return Result<SupplierDto>.Failure(supplier?.Error);

            return Result<SupplierDto>.Success(_mapper.Map<Supplier,SupplierDto>(supplier.Value));
        }
        public async Task<PagedResultDTo<SupplierWithOrdersDto>> GetListAsync(SupplierInput input)
        {
            var items = await _supplierRepository.GetListAsync(input.FilterText, input.Sorting, input.MaxResultCount);
            var totalCount = await _supplierRepository.GetCountAsync(input.FilterText);


            var order = _mapper.Map<List<SupplierWithOrders>, List<SupplierWithOrdersDto>>(items);

            return new PagedResultDTo<SupplierWithOrdersDto>(order, totalCount);
        }
        public async Task<SupplierDto> GetAsync(Guid id)
            => _mapper.Map<Supplier,SupplierDto>(await _supplierRepository.GetByIdAsync(id));

        public async Task<PagedResultDTo<SharedLookUp>> GetSupplierLookupAsync()
        {
            var items = await _supplierRepository.GetSupplierLookupAsync();
            return new PagedResultDTo<SharedLookUp>(items, (long)items.Count);
        }
        public async Task<Result<SupplierDto>> DeleteAsync(Guid id)
        {
            if (await _purchaseOrderRepository.HasPurchaseOrderId(id, PurchaseOrderCheckType.Supplier))
            {
                return Result<SupplierDto>.Failure("The supplier cannot be deleted because it has existing purchase orders");
            }
            var supplier = await _supplierRepository.GetByIdAsync(id);

            if(supplier != null)
            {
                _supplierRepository.Delete(supplier);
                await _supplierRepository.SaveChangesAsync();
            }
            else
            {
                return Result<SupplierDto>.Failure("Not Found Supplier by this id");
            }
            return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(supplier));

        }
            
    }
}
