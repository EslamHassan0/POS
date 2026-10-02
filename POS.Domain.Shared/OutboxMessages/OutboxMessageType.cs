namespace POS.Domain.Shared.OutboxMessages;

public enum OutboxMessageType
{
    InvoiceCreated = 1,
    InvoicePaid = 2,
    InvoiceCancelled = 3,
    GenerateReceipt = 4,
    SendEmail = 5,
    PurchaseOrderReceived = 6,

}