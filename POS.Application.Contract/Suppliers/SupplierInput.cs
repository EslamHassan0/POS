using POS.Domain.Shared.PagedResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Suppliers
{
    public class SupplierInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }
        
    }
}
