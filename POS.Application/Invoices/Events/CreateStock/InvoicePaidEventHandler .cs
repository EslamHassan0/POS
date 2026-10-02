using MediatR;
using POS.Application.Invoices.Events.Notifications;
using POS.Domain.Entity.StockTransactions;
using POS.Domain.Events;
using POS.Domain.GenericRepository;
using POS.Domain.Shared.StockTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Invoices.Events.CreateStock
{
    public class InvoicePaidEventHandler : INotificationHandler<InvoicePaidNotification>
    {
        private readonly IStockTransactionRepository _stockRepo;

        public InvoicePaidEventHandler(IStockTransactionRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task Handle(InvoicePaidNotification notification, CancellationToken cancellationToken)
        {
            foreach (var item in notification.Items)
            {
                await _stockRepo.AddAsync(new StockTransaction(Guid.NewGuid(), item.ItemId, item.Qty, TransactionType.Out, notification.InvoiceId, DateTime.Now));
            }
        }
    }
}
