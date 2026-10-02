using POS.Domain.BaseEntity;
using POS.Domain.Entity.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.Categories
{
    public class Category : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public Category(Guid id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = true;
        }
        protected Category()
        {
            
        }
    }
}
