using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.PagedResults
{
    public class PagedAndSortedResultRequestDto
    {
        public string Sorting { get; set; } = "Id DESC";
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; } = 0;


        public int PageNumber
        {
            get => (SkipCount / MaxResultCount) + 1;
            set => SkipCount = (value - 1) * MaxResultCount;
        }
    }
}
