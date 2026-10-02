using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.Invoices
{
    public class InvoiceConsts
    {

        private const string DefaultSorting = "{0}InvoiceNo asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Invoice." : string.Empty);
        }
    }
}
