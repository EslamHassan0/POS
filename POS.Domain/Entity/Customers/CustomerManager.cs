using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Customers
{
    public class CustomerManager : DomainServices
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Customer> CreateAsync(string name, string phone, string address)
        {
            var customer = new Customer(Guid.NewGuid(), name, phone, address);

            return await _customerRepository.AddAsync(customer);
        }

        public async Task<Customer> UpdateAsync(Guid id,string name, string phone, string address)
        {
            var customre = await _customerRepository.FindAsync(id);
            if (customre != null) 
            {
                customre.Address = address;
                customre.Name = name;
                customre.Phone = phone;
                customre.ModifiedDate = DateTime.Now;

            }

            return  await _customerRepository.UpdateAsync(customre);
        }

    }
}
