using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.BaseEntity;

namespace POS.Domain.Entity.Warehouses
{
    public class Warehouse : BaseEntity<Guid>
    {
        public Warehouse(Guid id,string name, string? location, bool isDefault, bool isActive)
        {
            Id = id;
            Name = name;
            Location = location;
            IsDefault = isDefault;
            IsActive = isActive;
        }
        protected Warehouse()
        {
            
        }

        public string Name { get; set; }
        public string? Location { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
