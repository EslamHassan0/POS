using POS.Domain.Entity.PurchaseOrders;
using POS.Domain.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Entity.POSSettings
{
    public interface IPOSSettingRepository : IGenericRepository<POSSetting, Guid>
    {
        Task<POSSetting> GetSettingInfoAsync();

    }
}
