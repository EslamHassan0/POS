using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Domain.Shared.StockTransactions;

namespace POS.Domain.Entity.StockTransactions
{
    public class StockWithItem
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        //public TransactionType TransactionType { get; set; }

        //public DateTime Date { get; set; }
    }
}
