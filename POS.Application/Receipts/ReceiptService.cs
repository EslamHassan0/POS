using AutoMapper;
using Microsoft.Extensions.Options;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using POS.Application.Contract.FileStorages;
using POS.Application.Contract.Receipts;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.POSSettings;
using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.Shared.Exceptions;
using Document = MigraDoc.DocumentObjectModel.Document;

namespace POS.Application.Receipts;

public class ReceiptService : ApplicationService, IReceiptService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IOptionsMonitor<FileStorage> _fileStorage;
    private readonly IPOSSettingRepository _pOSSettingRepository;
    public ReceiptService(
        IInvoiceRepository invoiceRepository,
        IMapper mapper,
        IOptionsMonitor<FileStorage> fileStorage,
        IPurchaseOrderRepository purchaseOrderRepository,
        IPOSSettingRepository pOSSettingRepository) : base(mapper)
    {
        _invoiceRepository = invoiceRepository;
        _fileStorage = fileStorage;
        _purchaseOrderRepository = purchaseOrderRepository;
        _pOSSettingRepository = pOSSettingRepository;
    }
    private Task<POSSetting> GetSetting()
    {
        return _pOSSettingRepository.GetSettingInfoAsync();
    }
    public async Task<string> GeneratePurchaseReceiptAsync(Guid purchaseOrderId)
    {
        var folderPath = _fileStorage.CurrentValue.PurchaseFolder;
        //var setting = GetSetting();
        var filePath = Path.Combine(folderPath, $"{purchaseOrderId}.pdf");

        if (File.Exists(filePath))
            return filePath;

        var purchase = await _purchaseOrderRepository.GetAsync(purchaseOrderId);

        if (purchase is null)
            throw new NotFoundException($"purchase {purchase} Not found");

        var document = new Document();

        document.Info.Title = $"Purchase Number {purchase.PurchaseOrderNo}";

        var section = document.AddSection();

        section.PageSetup.PageFormat = PageFormat.A6;
        section.PageSetup.TopMargin = "0.6cm";
        section.PageSetup.BottomMargin = "0.6cm";
        section.PageSetup.LeftMargin = "0.6cm";
        section.PageSetup.RightMargin = "0.6cm";

        var normalStyle = document.Styles["Normal"];
        normalStyle.Font.Name = "Arial";
        normalStyle.Font.Size = 8;

        var title = section.AddParagraph("POS");

        title.Format.Alignment = ParagraphAlignment.Center;
        title.Format.SpaceAfter = "0.05cm";
        title.Format.Font.Bold = true;
        title.Format.Font.Size = 18;

        var subtitle = section.AddParagraph("SALES INVOICE");

        subtitle.Format.Alignment = ParagraphAlignment.Center;
        subtitle.Format.SpaceAfter = "0.2cm";
        subtitle.Format.Font.Bold = true;
        subtitle.Format.Font.Size = 9;

        var invoiceInfo = section.AddTable();

        invoiceInfo.Borders.Width = 0;

        invoiceInfo.AddColumn("50%");
        invoiceInfo.AddColumn("50%");

        var infoRow1 = invoiceInfo.AddRow();

        infoRow1.Cells[0]
            .AddParagraph($"Purchase Number #: {purchase.PurchaseOrderNo}");

        infoRow1.Cells[1]
            .AddParagraph($"Date: {purchase.Date:yyyy-MM-dd}")
            .Format.Alignment = ParagraphAlignment.Right;

        var infoRow2 = invoiceInfo.AddRow();

        infoRow2.Cells[0]
            .AddParagraph($"Time: {purchase.Date:HH:mm}");

        infoRow2.Cells[1]
            .AddParagraph("Payment: Cash")
            .Format.Alignment = ParagraphAlignment.Right;

        var separator = section.AddParagraph();

        separator.Format.Alignment = ParagraphAlignment.Center;
        separator.Format.SpaceBefore = "0.15cm";
        separator.Format.SpaceAfter = "0.15cm";
        separator.Format.Font.Size = 7;

        separator.AddText("----------------------------------------");

        var table = section.AddTable();

        table.Borders.Width = 0.5;
        table.Borders.Color = Colors.Gray;
        table.Format.Font.Size = 7.5;

        table.AddColumn("2.5cm");
        table.AddColumn("0.8cm");
        table.AddColumn("1.3cm");
        table.AddColumn("1.5cm");

        var headerRow = table.AddRow();

        headerRow.Shading.Color = Colors.LightGray;
        headerRow.Format.Font.Bold = true;
        headerRow.Format.Font.Size = 7.5;

        headerRow.Cells[0].AddParagraph("ITEM");
        headerRow.Cells[1].AddParagraph("QTY");
        headerRow.Cells[2].AddParagraph("PRICE");
        headerRow.Cells[3].AddParagraph("TOTAL");

        headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
        headerRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;
        headerRow.Cells[3].Format.Alignment = ParagraphAlignment.Right;

        foreach (var item in purchase.PurchaseOrderItems)
        {
            var row = table.AddRow();

            row.Cells[0].AddParagraph(item.ItemName);

            row.Cells[1]
                .AddParagraph(item.Qty.ToString())
                .Format.Alignment = ParagraphAlignment.Center;

            row.Cells[2]
                .AddParagraph(item.UnitPrice.ToString("0.00"))
                .Format.Alignment = ParagraphAlignment.Right;

            row.Cells[3]
                .AddParagraph(item.TotalLine.ToString("0.00"))
                .Format.Alignment = ParagraphAlignment.Right;
        }

        var spacing = section.AddParagraph();
        spacing.Format.SpaceAfter = "0.1cm";

        var totals = section.AddTable();

        totals.Borders.Width = 0;

        totals.AddColumn("4cm");
        totals.AddColumn("2.1cm");

        var subtotalRow = totals.AddRow();

        subtotalRow.Cells[0]
            .AddParagraph("Subtotal");

        subtotalRow.Cells[1]
            .AddParagraph(purchase.TotalAmount.ToString("0.00"))
            .Format.Alignment = ParagraphAlignment.Right;

        var discountRow = totals.AddRow();

        var totalRow = totals.AddRow();

        totalRow.Shading.Color = Colors.LightGray;
        totalRow.Format.Font.Bold = true;
        totalRow.Format.Font.Size = 10;


        var finalTotal = section.AddParagraph(
            $"TOTAL: {purchase.TotalAmount:0.00}"
        );

        finalTotal.Format.Alignment = ParagraphAlignment.Center;
        finalTotal.Format.SpaceBefore = "0.25cm";
        finalTotal.Format.SpaceAfter = "0.15cm";
        finalTotal.Format.Font.Bold = true;
        finalTotal.Format.Font.Size = 13;

        var footerSeparator = section.AddParagraph();

        footerSeparator.Format.Alignment = ParagraphAlignment.Center;
        footerSeparator.Format.SpaceBefore = "0.1cm";
        footerSeparator.Format.SpaceAfter = "0.1cm";
        footerSeparator.Format.Font.Size = 7;

        footerSeparator.AddText("----------------------------------------");

        var thankYou = section.AddParagraph(
            "Thank you for shopping with us!"
        );

        thankYou.Format.Alignment = ParagraphAlignment.Center;
        thankYou.Format.SpaceBefore = "0.1cm";
        thankYou.Format.Font.Bold = true;
        thankYou.Format.Font.Size = 8;

        var footer = section.AddParagraph(
            $"Purchase  #{purchase.PurchaseOrderNo}"
        );

        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.Format.SpaceBefore = "0.1cm";
        footer.Format.Font.Size = 7;

        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        Directory.CreateDirectory(folderPath);

        filePath = Path.Combine(folderPath, $"{purchase.Id}.pdf");

        renderer.PdfDocument.Save(filePath);

        return filePath;
    }
    public async Task<string> GenerateInvoiceAsync(Guid invoiceId)
    {
        var folderPath = _fileStorage.CurrentValue.POSInvoiceFolder;
        var filePath = Path.Combine(folderPath, $"{invoiceId}.pdf");

        if (File.Exists(filePath))
            return filePath;

        var invoice = await _invoiceRepository.GetInvoiceReport(invoiceId);

        if (invoice is null)
            throw new NotFoundException($"Invoice {invoiceId} Not found");

        var document = new Document();

        document.Info.Title = $"Invoice {invoice.InvoiceNo}";

        var section = document.AddSection();

        section.PageSetup.PageFormat = PageFormat.A6;
        section.PageSetup.TopMargin = "0.6cm";
        section.PageSetup.BottomMargin = "0.6cm";
        section.PageSetup.LeftMargin = "0.6cm";
        section.PageSetup.RightMargin = "0.6cm";

        var normalStyle = document.Styles["Normal"];
        normalStyle.Font.Name = "Arial";
        normalStyle.Font.Size = 8;

        var title = section.AddParagraph("POS");

        title.Format.Alignment = ParagraphAlignment.Center;
        title.Format.SpaceAfter = "0.05cm";
        title.Format.Font.Bold = true;
        title.Format.Font.Size = 18;

        var subtitle = section.AddParagraph("SALES INVOICE");

        subtitle.Format.Alignment = ParagraphAlignment.Center;
        subtitle.Format.SpaceAfter = "0.2cm";
        subtitle.Format.Font.Bold = true;
        subtitle.Format.Font.Size = 9;

        var invoiceInfo = section.AddTable();

        invoiceInfo.Borders.Width = 0;

        invoiceInfo.AddColumn("50");
        invoiceInfo.AddColumn("50");

        var infoRow1 = invoiceInfo.AddRow();

        infoRow1.Cells[0]
            .AddParagraph($"Invoice #: {invoice.InvoiceNo}");

        infoRow1.Cells[1]
            .AddParagraph($"Date: {invoice.InvoiceDate:yyyy-MM-dd}")
            .Format.Alignment = ParagraphAlignment.Right;

        var infoRow2 = invoiceInfo.AddRow();

        infoRow2.Cells[0]
            .AddParagraph($"Time: {invoice.InvoiceDate:HH:mm}");

        infoRow2.Cells[1]
            .AddParagraph("Payment: Cash")
            .Format.Alignment = ParagraphAlignment.Right;

        var separator = section.AddParagraph();

        separator.Format.Alignment = ParagraphAlignment.Center;
        separator.Format.SpaceBefore = "0.15cm";
        separator.Format.SpaceAfter = "0.15cm";
        separator.Format.Font.Size = 7;

        separator.AddText("----------------------------------------");

        var table = section.AddTable();

        table.Borders.Width = 0.5;
        table.Borders.Color = Colors.Gray;
        table.Format.Font.Size = 7.5;

        table.AddColumn("2.5cm");
        table.AddColumn("0.8cm");
        table.AddColumn("1.3cm");
        table.AddColumn("1.5cm");

        var headerRow = table.AddRow();

        headerRow.Shading.Color = Colors.LightGray;
        headerRow.Format.Font.Bold = true;
        headerRow.Format.Font.Size = 7.5;

        headerRow.Cells[0].AddParagraph("ITEM");
        headerRow.Cells[1].AddParagraph("QTY");
        headerRow.Cells[2].AddParagraph("PRICE");
        headerRow.Cells[3].AddParagraph("TOTAL");

        headerRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
        headerRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;
        headerRow.Cells[3].Format.Alignment = ParagraphAlignment.Right;

        foreach (var item in invoice.InvoiceItems)
        {
            var row = table.AddRow();

            row.Cells[0].AddParagraph(item.ItemName);

            row.Cells[1]
                .AddParagraph(item.Qty.ToString())
                .Format.Alignment = ParagraphAlignment.Center;

            row.Cells[2]
                .AddParagraph(item.Price.ToString("0.00"))
                .Format.Alignment = ParagraphAlignment.Right;

            row.Cells[3]
                .AddParagraph(item.LineTotal.ToString("0.00"))
                .Format.Alignment = ParagraphAlignment.Right;
        }

        var spacing = section.AddParagraph();
        spacing.Format.SpaceAfter = "0.1cm";

        var totals = section.AddTable();

        totals.Borders.Width = 0;

        totals.AddColumn("4cm");
        totals.AddColumn("2.1cm");

        var subtotalRow = totals.AddRow();

        subtotalRow.Cells[0]
            .AddParagraph("Subtotal");

        subtotalRow.Cells[1]
            .AddParagraph(invoice.TotalBeforeTax.ToString("0.00"))
            .Format.Alignment = ParagraphAlignment.Right;

        var discountRow = totals.AddRow();

        discountRow.Cells[0]
            .AddParagraph("Discount");

        discountRow.Cells[1]
            .AddParagraph(invoice.Discount.ToString("0.00"))
            .Format.Alignment = ParagraphAlignment.Right;

        var totalRow = totals.AddRow();

        totalRow.Shading.Color = Colors.LightGray;
        totalRow.Format.Font.Bold = true;
        totalRow.Format.Font.Size = 10;

        totalRow.Cells[0]
            .AddParagraph("NET TOTAL");

        totalRow.Cells[1]
            .AddParagraph(invoice.TotalAfterDiscount.ToString("0.00"))
            .Format.Alignment = ParagraphAlignment.Right;

        var finalTotal = section.AddParagraph(
            $"TOTAL: {invoice.TotalAfterDiscount:0.00}"
        );

        finalTotal.Format.Alignment = ParagraphAlignment.Center;
        finalTotal.Format.SpaceBefore = "0.25cm";
        finalTotal.Format.SpaceAfter = "0.15cm";
        finalTotal.Format.Font.Bold = true;
        finalTotal.Format.Font.Size = 13;

        var footerSeparator = section.AddParagraph();

        footerSeparator.Format.Alignment = ParagraphAlignment.Center;
        footerSeparator.Format.SpaceBefore = "0.1cm";
        footerSeparator.Format.SpaceAfter = "0.1cm";
        footerSeparator.Format.Font.Size = 7;

        footerSeparator.AddText("----------------------------------------");

        var thankYou = section.AddParagraph(
            "Thank you for shopping with us!"
        );

        thankYou.Format.Alignment = ParagraphAlignment.Center;
        thankYou.Format.SpaceBefore = "0.1cm";
        thankYou.Format.Font.Bold = true;
        thankYou.Format.Font.Size = 8;

        var footer = section.AddParagraph(
            $"Invoice #{invoice.InvoiceNo}"
        );

        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.Format.SpaceBefore = "0.1cm";
        footer.Format.Font.Size = 7;

        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        Directory.CreateDirectory(folderPath);

        filePath = Path.Combine(folderPath, $"{invoice.Id}.pdf");

        renderer.PdfDocument.Save(filePath);

        return filePath;
    }
}