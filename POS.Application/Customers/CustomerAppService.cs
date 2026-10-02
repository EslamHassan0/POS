using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using POS.Application.Contract.Customers;
using POS.Domain.Entity.Customers;
using POS.Domain.Shared.PagedResults;
using POS.Domain.Shared.SharedLookupDtos;

namespace POS.Application.Customers
{
    public class CustomerAppService : ApplicationService, ICustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerManager _customerManager;
        public CustomerAppService(ICustomerRepository customerRepository,
            CustomerManager customerManager,
            IMapper mapper):base(mapper)
        {
            _customerRepository = customerRepository;
            _customerManager = customerManager;
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = await _customerManager.CreateAsync(createCustomerDto.Name, createCustomerDto.Phone, createCustomerDto.Address);
            return _mapper.Map<Customer, CustomerDto>(customer);
            
        }


        public async Task<CustomerDto> UpdateAsync(Guid id,UpdateCustomersDto updateCustomerDto)
        {
            var customer = await _customerManager.UpdateAsync(id, updateCustomerDto.Name, updateCustomerDto.Phone, updateCustomerDto.Address);
            return _mapper.Map<Customer, CustomerDto>(customer);

        }

        public async Task<CustomerDto> GetAsync(Guid id)
        {
           return _mapper.Map<Customer, CustomerDto>(await _customerRepository.GetByIdAsync(id));
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null) 
            {
                 _customerRepository.Delete(customer);
                await _customerRepository.SaveChangesAsync();
            }
        }

        public async Task<PagedResultDTo<CustomerDto>> GetListAsync(CustomerInput input)
        {
            
            var items = await _customerRepository.GetListAsync(input.FilterText,input.CustomerName,input.Sorting , input.MaxResultCount,input.SkipCount);
            var itemMapper = _mapper.Map<List<Customer>, List<CustomerDto>>(items);
            return new PagedResultDTo<CustomerDto>(itemMapper, (long)itemMapper.Count);
        }

        public async Task<PagedResultDTo<SharedLookUp>> GetCustomerLookupAsync()
        {
            var items = await _customerRepository.GetCustomerLookupAsync();
            return new PagedResultDTo<SharedLookUp>(items, (long)items.Count);
        }
    }
}
