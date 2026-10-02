using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using POS.Application.Contract.Warehouses;
using POS.Domain.Entity.Warehouses;

namespace POS.Application.Warehouses.Mapper
{
    public class warehouseMapper : Profile
    {
        public warehouseMapper()
        {
            CreateMap<Warehouse, WarehouseDto>()
                                   .ReverseMap();
        }
    }
}
