using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data
{
    public class DebtExpiryProfileRepository : GenericRepository<MetopeDbEntities, Debt_Expiry_Profile>,
                                        IDebtExpiryProfileRepository
    {

        public IQueryable<Debt_Expiry_Profile> GetAllDebtExpiryValues(decimal iEntity, decimal iGenericEntityId)
        {
            return GetAll().Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity);

        }
    }
}