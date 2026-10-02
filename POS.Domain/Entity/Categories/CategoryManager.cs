using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Entity.Items;

namespace POS.Domain.Entity.Categories
{
    public class CategoryManager : DomainServices
    {
        private readonly ICategoryRepository _itemCategories;

        public CategoryManager(ICategoryRepository itemCategories)
        {
            _itemCategories = itemCategories;
        }

        public async Task<Category> CreateAsync(string name, string? description)
        {
            var item = new Category(Guid.NewGuid(), name,description);


            return await _itemCategories.AddAsync(item);
        }

        public async Task<Category> UpdateAsync(Guid id,string name, string? description)
        {
            var category =  await _itemCategories.GetByIdAsync(id);

            category.Name = name;          
            category.Description = description; 
            return await _itemCategories.UpdateAsync(category);
        }
    }
}
