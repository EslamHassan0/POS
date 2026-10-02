using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Shared.PurchaseOrderStatus
{
    public class PurchaseOrderConsts
    {
        private const string DefaultSorting = "{0}OrderNo asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "PurchaseOrder." : string.Empty);
        }
    }
}
