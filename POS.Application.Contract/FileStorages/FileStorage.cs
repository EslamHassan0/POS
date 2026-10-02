using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.FileStorages;

public class FileStorage
{
    public string UploadsFolder { get; set; } = string.Empty;
    public string PurchaseFolder { get; set; } = string.Empty;
    public string ItemFolder { get; set; } = string.Empty;
    public string POSInvoiceFolder { get; set; } = string.Empty;
    public string SettingFolder { get; set; } = string.Empty;
    public string RequestPath { get; set; } = string.Empty;
}
