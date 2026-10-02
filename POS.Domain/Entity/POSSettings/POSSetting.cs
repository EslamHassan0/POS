using POS.Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.POSSettings
{
    public class POSSetting : BaseEntity<Guid>
    {
        public POSSetting(
         Guid id, string businessName, string address, string phone, decimal defaultVAT,
            string invoicePrefix, int startingNumber, decimal visaFee, 
            bool allowNegativeStock, int lowStockAlert, string? logoPath)
        {
            Id = id;
            BusinessName = businessName;
            Address = address;
            Phone = phone;
            DefaultVAT = defaultVAT;
            InvoicePrefix = invoicePrefix;
            StartingNumber = startingNumber;
            VisaFee = visaFee;
            AllowNegativeStock = allowNegativeStock;
            LowStockAlert = lowStockAlert;
            LogoPath = logoPath;
        }

        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal DefaultVAT { get; set; }
        public string InvoicePrefix { get; set; }
        public int StartingNumber { get; set; }
        public decimal VisaFee { get; set; }
        public bool AllowNegativeStock { get; set; }
        public int LowStockAlert { get; set; }
        public string? LogoPath { get; set; }  

    }
}
