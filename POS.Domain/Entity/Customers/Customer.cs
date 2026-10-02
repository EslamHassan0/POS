using POS.Domain.BaseEntity;
using POS.Domain.Entity.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Customers
{
    public class Customer : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public ICollection<Invoice> Invoices { get; }

        public Customer()
        {
            
        }
        public Customer(Guid id ,  string name, string phone, string? address)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Address = address;
            Invoices =  new List<Invoice>();
        }
    }
}
