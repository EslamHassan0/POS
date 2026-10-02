using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using POS.Application.Contract.Customers;
using POS.Application.Contract.ItemCategories;
using POS.Domain.Entity.Customers;
using POS.Domain.Entity.Categories;

namespace POS.Application.ItemCategories.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap();

         
        }
    }
}
