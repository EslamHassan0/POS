using MediatR;
using POS.Application.Invoices.Events.Notifications;
using POS.Domain.Entity.StockTransactions;
using POS.Domain.Shared.StockTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Invoices.Events.CreateStock
{
    public class InvoiceCancelEventHandler : INotificationHandler<InvoiceCancelNotification>
    {
        private readonly IStockTransactionRepository _stockRepo;

        public InvoiceCancelEventHandler(IStockTransactionRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task Handle(InvoiceCancelNotification notification, CancellationToken cancellationToken)
        {
            foreach (var item in notification.Items)
            {
                await _stockRepo.AddAsync(new StockTransaction(Guid.NewGuid(), item.ItemId, item.Qty, TransactionType.In, notification.InvoiceId, DateTime.Now));
            }
        }
    }
}
