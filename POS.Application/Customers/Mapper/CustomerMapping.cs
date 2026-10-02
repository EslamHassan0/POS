using AutoMapper;
using POS.Application.Contract.Customers;
using POS.Domain.Entity.Customers;

namespace POS.Application.Customers.Mapper
{
    public class CustomerMapping : Profile
    {
        public CustomerMapping()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}
