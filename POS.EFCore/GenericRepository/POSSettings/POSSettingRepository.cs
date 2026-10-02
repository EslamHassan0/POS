using POS.Domain.Entity.POSSettings;
using POS.Domain.Entity.PurchaseOrders;
using POS.EFCore.POSContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.EFCore.GenericRepository.POSSettings
{
    public class POSSettingRepository : GenericRepository<POSSetting, Guid>, IPOSSettingRepository, IRepositoryMarker
    {
        public POSSettingRepository(POSContext context) : base(context)
        {
        }

        public async Task<POSSetting> GetSettingInfoAsync()
        {
            var _dbSet = GetDbSet();
            var result = _dbSet
               .FirstOrDefault();

            return result;
        }
    }
}
