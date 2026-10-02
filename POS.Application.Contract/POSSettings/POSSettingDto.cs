using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.POSSettings
{
    public class POSSettingDto
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal DefaultVAT { get; set; }
        public string InvoicePrefix { get; set; }
        public int StartingNumber { get; set; }
        public decimal VisaFee { get; set; }
        public bool AllowNegativeStock { get; set; }
        public int LowStockAlert { get; set; }
        public string? LogoBase64 { get; set; }
    }
}
