using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.StockTransactions;
using POS.Domain.Shared.StockTransactions;
using POS.EFCore.POSContexts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POS.EFCore.GenericRepository.StockTransactions
{
    public class StockTransactionRepository : GenericRepository<StockTransaction, Guid>, IStockTransactionRepository , IRepositoryMarker
    {

        public StockTransactionRepository(POSContext context) : base(context)
        {

        }

        

        public decimal GetAvailableStock(Guid itemId)
        {
            var _dbSet = GetDbSet();
            return _dbSet
                     .Where(a => a.ItemId == itemId)
                     .Sum(a => a.QtyChange);
        }
    }
}
