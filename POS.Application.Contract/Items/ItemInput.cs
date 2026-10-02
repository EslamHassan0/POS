using POS.Domain.Shared.PagedResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Items
{
    public class ItemInput : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }
       
    }
}
