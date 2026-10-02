using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.StockTransactions;
using POS.Domain.Shared.Item;

namespace POS.Domain.Entity.Items
{
    public class ItemManager : DomainServices
    {
        private readonly IItemRepository _itemRepository;
        //private readonly IStockTransactionRepository _stockTransactionRepository;
        public ItemManager(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        public async Task<Item> CreateAsync(string name, string code, string barcode, decimal price, decimal cost,
                    decimal vatPercent, bool isVATEligible, Guid categoryId, string? image, ItemUnit unit)
        {
            var item = new Item(Guid.NewGuid(),name ,code , barcode , price , cost,vatPercent,isVATEligible,true, categoryId, image,unit);


            return await _itemRepository.AddAsync(item);
        }
        public async Task<Item> UpdateAsync(Guid id,string name, string code, decimal price, decimal cost,
                    decimal vatPercent, bool isVATEligible, Guid categoryId, string? image, ItemUnit unit)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            item.Name = name;
            item.Code = code;
            item.Price = price;
            item.Cost = cost;
            item.VatPercent = vatPercent;
            item.IsVATEligible = isVATEligible;
            item.CategoryId = categoryId;
            item.Image = image;
            item.Unit = unit;
            


            return await _itemRepository.UpdateAsync(item);
        }
    }
}
