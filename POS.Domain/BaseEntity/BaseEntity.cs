using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.BaseEntity
{
    public class BaseEntity<T>
    {
        public  T Id { get; set; }
        public T ModifiedBy { get; set; }
        public T CreateBy { get; set; }

        public DateTime? CreationTime { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } 
    }
}
