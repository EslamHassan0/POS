using System.Reflection.Emit;
using System.Security.Cryptography;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.Results;

namespace POS.Domain.Entity.Warehouses
{
    public class WarehouseManager : DomainServices
    {
        private readonly IGenericRepository<Warehouse, Guid> _warehouseRepository;
        public WarehouseManager(IGenericRepository<Warehouse, Guid> warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Result<Warehouse>> CreateAsync(string name, string? location, bool isDefault, bool isActive)
        {
            var resultCheck =  IsExistsBefore(name, location);
            if (!resultCheck.IsSuccess)
                return Result<Warehouse>.Failure(resultCheck.Error);

            var warehouse = new Warehouse(Guid.NewGuid(), name, location, isDefault, isActive);
            return Result<Warehouse>.Success(await _warehouseRepository.AddAsync(warehouse));
        }
        public async Task<Result<Warehouse>> UpdateAsync(Guid id, string name, string? location, bool isDefault, bool isActive)
        {
            var resultCheck = IsExistsBefore(name, location,id);
            if (!resultCheck.IsSuccess)
                return Result<Warehouse>.Failure(resultCheck.Error);

            var warehouse = await _warehouseRepository.GetByIdAsync(id);
            if (warehouse != null)
            {
                warehouse.Name = name;
                warehouse.Location = location;
                warehouse.IsDefault = isDefault;
                warehouse.IsActive = isActive;

            }
            return Result<Warehouse>.Success(await _warehouseRepository.UpdateAsync(warehouse));
        }

        public Result<Warehouse> IsExistsBefore(string name, string? location, Guid? id= null)
        {
            var warehoues = (_warehouseRepository.GetQueryable());

            var result = warehoues.FirstOrDefault(a => a.Name == name && a.Location == location);
            if (result !=  null) 
            {
                if (result.Id != id)
                    return Result<Warehouse>.Failure($"That warehouse already exists Name {name} or Location {location} ");
            }
            return Result<Warehouse>.Success(result);
        }

    }
}
