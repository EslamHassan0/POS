using POS.Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Suppliers
{
    public class Supplier : BaseEntity<Guid>
    {
        
        public Supplier(Guid id,string name, string phone, string address, string email)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Address = address;
            Email = email;
        }
        protected Supplier () { }
        public string  Name { get; set; }
        public string  Phone { get; set; }
        public string  Address { get; set; }
        public string  Email { get; set; }
    }
}
