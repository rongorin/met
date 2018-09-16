using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MetopeMVCApp.Data.Repositories
{
    public class PartyRepository : GenericRepository<MetopeDbEntities, Party>, MetopeMVCApp.Data.
                                        
    {
        public IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId)
        {
            //return GetAll().Where(c => c.Party_Type == iType)
            //                    .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity);

        } 
    }
}