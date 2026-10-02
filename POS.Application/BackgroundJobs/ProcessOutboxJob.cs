using Polly.CircuitBreaker;
using POS.Application.Contract.Receipts;
using POS.Domain.Entity.OutboxMessages;
using POS.Domain.Entity.OutboxMessages.Events;
using POS.Domain.Shared.OutboxMessages;
using Polly;
using System.Text.Json;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.PurchaseOrders;

namespace POS.Application.BackgroundJobs;

public class ProcessOutboxJob
{
    private readonly IOutboxMessageRepository _outboxRepository;
    private readonly IReceiptService _receiptService;
    private readonly InvoiceManager _invoiceManager;
    private readonly PurchaseOrderManager _purchaseOrderManager;

    private static readonly Dictionary<OutboxMessageType, AsyncCircuitBreakerPolicy> _circuitBreakers = new();
    private static readonly object _lock = new();
    public ProcessOutboxJob(
         IOutboxMessageRepository outboxRepository,
         IReceiptService receiptService,
         InvoiceManager invoiceManager,
         PurchaseOrderManager purchaseOrderManager)
    {
        _outboxRepository = outboxRepository;
        _receiptService = receiptService;
        _invoiceManager = invoiceManager;
        _purchaseOrderManager = purchaseOrderManager;
    }


    private static AsyncCircuitBreakerPolicy GetCircuitBreaker(OutboxMessageType type)
    {
        lock (_lock)
        {
            if (!_circuitBreakers.TryGetValue(type, out var policy))
            {
                policy = Policy
                    .Handle<Exception>(ex => RetryPolicy.IsRetryable(ex))
                    .CircuitBreakerAsync(
                        exceptionsAllowedBeforeBreaking: 5,
                        durationOfBreak: TimeSpan.FromMinutes(3));

                _circuitBreakers[type] = policy;
            }
            return policy;
        }
    }
    public async Task Process(Guid messageId)
    {
        var message = await _outboxRepository.GetByIdAsync(messageId);
        if (message == null)
            return;


        var circuit = GetCircuitBreaker(message.Type);
        if (circuit.CircuitState == CircuitState.Open)
        {
            message.Status = OutboxStatus.Pending;
            message.AvailableAt = DateTime.UtcNow.AddSeconds(30);
            message.LockedAt = null;
            await _outboxRepository.UpdateAsync(message);
            return;
        }


        try
        {
            message.Status = OutboxStatus.Processing;
            message.LockedAt = DateTime.UtcNow;
            await _outboxRepository.UpdateAsync(message);

            switch (message.Type)
            {
                case OutboxMessageType.InvoicePaid:

                    var data =JsonSerializer.Deserialize<GenerateReceiptRequestedEvent>(message.Payload);
                    var path = await _receiptService.GenerateInvoiceAsync(data.InvoiceId);

                   var  attachResult =  await _invoiceManager.AttachReceiptAsync(data.InvoiceId,path);
                    if (!attachResult.IsSuccess)
                        throw new Exception($"Failed to attach receipt: {attachResult.Error}");

                    break;
                case OutboxMessageType.PurchaseOrderReceived:
                    {
                        var purchaseData =
                               JsonSerializer.Deserialize<
                                   GeneratePurchaseReceiptRequestedEvent>(
                                       message.Payload);
                        if (purchaseData is null)
                            throw new Exception(
                                "Invalid purchase receipt payload");

                        var pathPurchas = await _receiptService.GeneratePurchaseReceiptAsync(purchaseData.PurchaseOrderId);
                        var attachResultPurchas = await _purchaseOrderManager.AttachReceiptAsync(purchaseData.PurchaseOrderId, pathPurchas);
                        if (!attachResultPurchas.IsSuccess)
                            throw new Exception($"Failed to attach receipt: {attachResultPurchas.Error}");

                        break;
                    }
            }

            message.Status = OutboxStatus.Completed;
            message.LockedAt = null;
            await _outboxRepository.UpdateAsync(message);
        }
        catch (Exception ex)
        {
            message.Attempts++;
            message.LastError = ex.Message;
            message.LockedAt = null;

            var isRetryable = RetryPolicy.IsRetryable(ex);
            var exceededMaxAttempts = message.Attempts >= RetryPolicy.MaxAttempts;

            if(!isRetryable || exceededMaxAttempts)
            {
                message.Status = OutboxStatus.DeadLetter;
                await _outboxRepository.UpdateAsync(message);
                //alert here 

                return;
            }

            message.Status = OutboxStatus.Pending;
            message.AvailableAt = DateTime.UtcNow.Add(RetryPolicy.GetNextDelay(message.Attempts));

            await _outboxRepository.UpdateAsync(message);


            throw;
        }
    }

}
