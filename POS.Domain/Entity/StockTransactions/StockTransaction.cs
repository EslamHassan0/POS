using POS.Domain.BaseEntity;
using POS.Domain.Shared.StockTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.StockTransactions
{
    public class StockTransaction : BaseEntity<Guid>
    {
        public StockTransaction(Guid id,Guid itemId, decimal qtyChange, TransactionType transactionType, Guid referenceId, DateTime date)
        {
            Id = id;
            ItemId = itemId;
            QtyChange = qtyChange;
            TransactionType = transactionType;
            ReferenceId = referenceId;
            Date = date;
        }
        protected StockTransaction()
        {
            
        }
        public Guid ItemId { get; set; }
        public decimal QtyChange { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid ReferenceId { get; set; }
        public DateTime Date { get; set; }
    }
}
