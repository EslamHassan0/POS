using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.ItemCategories
{
    public class CreateCategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
