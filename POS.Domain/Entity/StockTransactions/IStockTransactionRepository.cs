using POS.Domain.Entity.Invoices;
using POS.Domain.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.StockTransactions
{
    public interface IStockTransactionRepository : IGenericRepository<StockTransaction, Guid>
    {
         

        decimal GetAvailableStock(Guid itemId);
    }
}
