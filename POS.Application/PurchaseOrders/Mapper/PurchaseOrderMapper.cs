using AutoMapper;
using POS.Application.Contract.PurchaseOrders;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.Entity.PurchaseOrders.PurchaseOrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PurchaseOrders.Mapper
{
    public class PurchaseOrderMapper : Profile
    {
        public PurchaseOrderMapper()
        {

            //PurchaseOrderWithDetails, PurchaseOrderWithDetailsDto
            CreateMap<PurchaseOrderWithDetails, PurchaseOrderWithDetailsDto>();
            CreateMap<PurchaseOrderItemWithDetails, PurchaseOrderItemWithDetailsDto>();
            CreateMap<PurchaseOrderWithDetails,PurchaseOrderDto>();

            CreateMap<PurchaseOrderDto, PurchaseOrder>();
            CreateMap<PurchaseOrder, PurchaseOrderDto>();
 
        }
    }
}
