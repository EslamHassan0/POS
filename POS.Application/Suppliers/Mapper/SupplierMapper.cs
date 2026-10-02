using AutoMapper;
using POS.Application.Contract.Suppliers;
using POS.Domain.Entity.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Suppliers.Mapper
{
    public class SupplierMapper : Profile
    {
        public SupplierMapper()
        {
            CreateMap<SupplierWithOrders, SupplierWithOrdersDto>();
            CreateMap<Supplier, SupplierDto>();
        }
    }
}
