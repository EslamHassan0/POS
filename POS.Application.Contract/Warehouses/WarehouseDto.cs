using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Warehouses
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
    }
}
