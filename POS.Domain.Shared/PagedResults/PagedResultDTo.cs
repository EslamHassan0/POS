using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.PagedResults
{
    public class PagedResultDTo<T>
    {
        public PagedResultDTo(List<T> items, long totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public List<T> Items { get; set; } = new List<T>();
        public long TotalCount { get; set; }     // العدد الكلي للعناصر
        //public int PageNumber { get; set; }     // الصفحة الحالية
        //public int PageSize { get; set; }       // عدد العناصر في الصفحة

        //public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
