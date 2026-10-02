using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entity.InvoiceItems;
using POS.Domain.Entity.Invoices;
using POS.Domain.Entity.Items;
using POS.Domain.GenericRepository;
//using System.Linq.Expressions;
using POS.Domain.Shared.Extensions;
using POS.Domain.Shared.Invoices;
using POS.EFCore.POSContexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.Invoices
{
    public class InvoiceRepository : GenericRepository<Invoice, Guid>, IInvoiceRepository , IRepositoryMarker
    {
        public InvoiceRepository(POSContext context) : base(context)
        {
        }

        public async Task<List<InvoiceWithDetails>> GetListAsync(
            string filterText = null, 
            string invoiceNo = null, 
            string sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(filterText, invoiceNo);
            query =  query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? InvoiceConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }
        public async Task<long> GetCountAsync(
            string filterText = null,
            string invoiceNo = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilter(filterText, invoiceNo);
            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<string> GetNextInvoice()
        {
            var _dbContext = await GetDbContextAsync();

            var resultParam = new SqlParameter
            {
                ParameterName = "@NextId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "SET @NextId = NEXT VALUE FOR InvoiceNumberSequence",
                resultParam
            );
            int nextId = (int)resultParam.Value;

            return nextId.ToString();
        }

        //public async Task<> GetByIdWithItemsAsync()
        //{

        //}

        public  Task<Invoice> GetWithDetailsAsync(Guid id)
        {
            return GetDbSet()
                .Include(x => x.Customer)
                .Include(x => x.InvoiceItems)
                //.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<InvoiceWithDetails> GetInvoiceReport(Guid id)
        {
            var query = ApplyFilter(id);
            return query;
        }

        protected async Task<InvoiceWithDetails> ApplyFilter(
       Guid? invoiceId = null)
        {
            var _dbContext = await GetDbContextAsync();
            var query = await (from invoices in _dbContext.Invoices
                             //join invoiceItems in _dbContext.InvoiceItems on invoices.Id equals invoiceItems.InvoiceId
                             //join item in _dbContext.Items on invoiceItems.ItemId equals item.Id
                         join customers in _dbContext.Customers on invoices.CustomerId equals customers.Id
                         where (invoiceId == null || invoices.Id == invoiceId)
                         select new InvoiceWithDetails
                         {
                             Id = invoices.Id,
                             CustomerName = customers.Name,
                             InvoiceNo = invoices.InvoiceNo,
                             Discount = invoices.Discount,
                             GrandTotal = invoices.GrandTotal,
                             InvoiceDate = invoices.InvoiceDate,
                             InvoiceStatus = invoices.InvoiceStatus,
                             PaymentFee = invoices.PaymentFee,
                             PaymentType = invoices.PaymentType,
                             TotalAfterDiscount = invoices.TotalAfterDiscount,
                             TotalAfterTax = invoices.TotalAfterTax,
                             TotalBeforeTax = invoices.TotalBeforeTax,
                             VatAmount = invoices.VatAmount,

                             InvoiceItems = invoices.InvoiceItems
                             .Select(a => new InvoiceItemWithDetails
                             {
                                 ItemName = a.Item.Name,
                                 Price = a.Price,
                                 Qty = a.Qty,
                                 LineTotal = a.TotalLine
                             })
                             .ToList()
                         })
                         .FirstOrDefaultAsync();
            return query;


        }
        protected async Task<IQueryable<InvoiceWithDetails>> ApplyFilter(
               string filterText = null,
               string invoiceNo = null,
               Guid? invoiceId = null)
        {
            var _dbContext =await GetDbContextAsync();
            var query = (from invoices in _dbContext.Invoices
                         //join invoiceItems in _dbContext.InvoiceItems on invoices.Id equals invoiceItems.InvoiceId
                         //join item in _dbContext.Items on invoiceItems.ItemId equals item.Id
                         join customers in _dbContext.Customers on invoices.CustomerId equals customers.Id
                         where
                        (string.IsNullOrEmpty(filterText) || invoices.Customer.Name.ToLower().Contains(filterText.ToLower()))
                        && (invoiceId == null || invoices.Id == invoiceId)
                        && (string.IsNullOrEmpty(invoiceNo) || invoices.InvoiceNo.Contains(invoiceNo))
                         select new InvoiceWithDetails
                         {
                             Id = invoices.Id,
                             CustomerName = customers.Name,
                             InvoiceNo = invoices.InvoiceNo,
                             Discount = invoices.Discount,
                             GrandTotal = invoices.GrandTotal,
                             InvoiceDate = invoices.InvoiceDate,
                             InvoiceStatus = invoices.InvoiceStatus,
                             PaymentFee = invoices.PaymentFee,
                             PaymentType = invoices.PaymentType,
                             TotalAfterDiscount = invoices.TotalAfterDiscount,
                             TotalAfterTax = invoices.TotalAfterTax,
                             TotalBeforeTax = invoices.TotalBeforeTax,
                             VatAmount = invoices.VatAmount,

                             InvoiceItems = invoices.InvoiceItems
                             .Select(a => new InvoiceItemWithDetails
                             {
                                 ItemName = a.Item.Name,
                                 Price = a.Price,
                                 Qty = a.Qty,
                                 LineTotal = a.TotalLine
                             })
                             .ToList()
                         });
            return query;
                 

        }
    }
}
