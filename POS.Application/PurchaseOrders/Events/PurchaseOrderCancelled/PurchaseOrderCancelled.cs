using MediatR;
using POS.Application.PurchaseOrders.Events.Notifications.PurchaseOrderCancelled;
using POS.Domain.Entity.StockTransactions;
using POS.Domain.Events.PurchaseOrder;
using POS.Domain.Shared.StockTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.PurchaseOrders.Events.PurchaseOrderCancelled
{
    public class PurchaseOrderCancelled : INotificationHandler<PurchaseOrderCancelledNotification>
    {
        private readonly IStockTransactionRepository _stockRepo;

        public PurchaseOrderCancelled(IStockTransactionRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }
        public async Task Handle(PurchaseOrderCancelledNotification notification, CancellationToken cancellationToken)
        {
            foreach (var item in notification.Items)
            {
                await _stockRepo.AddAsync(new StockTransaction(Guid.NewGuid(), item.ItemId, item.Qty, TransactionType.Out, notification.PurchaseId, DateTime.Now));
                 
            }
        }
    }
}
    