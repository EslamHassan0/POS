using AutoMapper;
using POS.Application.Contract.Invoices;
using POS.Application.Contract.Items;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Items.Mapper
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemWithDetail, ItemWithDetailDto>();
            CreateMap<InvoiceWithDetails,InvoiceWithDetailsDto>();
            CreateMap<ItemWithDetail, ItemWithDetailDto>();



        }
    }
}
