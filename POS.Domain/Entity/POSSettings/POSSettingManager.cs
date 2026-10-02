using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.POSSettings
{
    public class POSSettingManager : DomainServices
    {
        private readonly IPOSSettingRepository _pOSSettingRepository;

        public POSSettingManager(IPOSSettingRepository pOSSettingRepository)
        {
            _pOSSettingRepository = pOSSettingRepository;
        }

        public async Task<POSSetting> CreateAsync(string businessName, string address, string phone, decimal defaultVAT,
            string invoicePrefix, int startingNumber, decimal visaFee,
            bool allowNegativeStock, int lowStockAlert, string? logoPath)
        {
            var pos = new POSSetting(Guid.NewGuid(), businessName, address, phone, defaultVAT, invoicePrefix, startingNumber, visaFee, allowNegativeStock, lowStockAlert, logoPath);

            return await _pOSSettingRepository.AddAsync(pos);
        }
        public async Task<POSSetting> UpdateAsync(Guid id ,string businessName, string address, string phone, decimal defaultVAT,
            string invoicePrefix, int startingNumber, decimal visaFee,
            bool allowNegativeStock, int lowStockAlert, string? logoPath)
        {
            var last = await _pOSSettingRepository.FindAsync(id);
            if(last is not null)
            {

            last.InvoicePrefix = invoicePrefix;
            last.BusinessName = businessName;
            last.Address = address;
            last.Phone = phone;
            last.DefaultVAT = defaultVAT;
            last.AllowNegativeStock = allowNegativeStock;
            last.LowStockAlert = lowStockAlert;
            //last.LogoPath = logoPath;
            last.VisaFee = visaFee;
            last.StartingNumber = startingNumber;
            last.LogoPath = logoPath;
            }

       
            return await  _pOSSettingRepository.UpdateAsync(last);
        }
    }
}
