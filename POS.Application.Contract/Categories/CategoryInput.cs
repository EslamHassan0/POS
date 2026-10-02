using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Shared.PagedResults;

namespace POS.Application.Contract.ItemCategories
{
    public class CategoryInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? Name { get; set; }
    }
}
