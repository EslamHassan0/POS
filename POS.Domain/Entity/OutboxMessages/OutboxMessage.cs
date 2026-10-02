using POS.Domain.BaseEntity;
using POS.Domain.Shared.OutboxMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.OutboxMessages;

public class OutboxMessage : BaseEntity<Guid>
{
    //InvoicePaid, InvoiceCancelled
    public OutboxMessageType Type { get; set; } = default!;

    //  Aggregate (InvoiceId)
    public Guid AggregateId { get; set; }

    public string Payload { get; set; } = default!;

    public OutboxStatus Status { get; set; }

    // عدد مرات إعادة المحاولة
    public int Attempts { get; set; }

    // إمتى الرسالة مسموح تتنفذ
    public DateTime AvailableAt { get; set; }

    // وقت إنشاء الرسالة
    public DateTime CreatedAt { get; set; }
    public DateTime? LockedAt { get; set; }

    // وقت انتهاء التنفيذ بنجاح
    public DateTime? ProcessedAt { get; set; }

    // آخر خطأ حصل
    public string? LastError { get; set; }


}
