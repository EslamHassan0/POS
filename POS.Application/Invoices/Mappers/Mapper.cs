using AutoMapper;
using POS.Application.Contract.InvoiceItems;
using POS.Application.Contract.Invoices;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Invoices.Mappers
{
    public class Mapper : Profile 
    {
        public Mapper()
        {
            CreateMap<CreateInvoiceItemDto, InvoiceItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            CreateMap<InvoiceItem, CreateInvoiceItemDto>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceWithDetails,InvoiceWithDetailsDto>();


        }
    }
}
