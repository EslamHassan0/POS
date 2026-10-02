using POS.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Suppliers
{
    public class SupplierManager : DomainServices
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierManager(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Supplier> CreateAsync(string name, string phone, string address, string email)
        {
            var supplier = new Supplier(Guid.NewGuid(), name, phone, address, email);

           return await _supplierRepository.AddAsync(supplier);
        }

        public async Task<Result<Supplier>> UpdateAsync(Guid id,string name, string phone, string address, string email)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
                return Result<Supplier>.Failure("Not Found Supplier by this id");

            supplier.Phone = phone;
            supplier.Name = name;
            supplier.Address = address;
            supplier.Email = email;

            return Result<Supplier>.Success(await _supplierRepository.UpdateAsync(supplier));
        }
    }
}
