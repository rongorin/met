using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
 

namespace MetopeMVCApp.Data
{
    public class PartyRepository 
    {
        MetopeDbEntities _ctx;

        public PartyRepository(MetopeDbEntities contxt) //created new consturctor
        { 
            _ctx = contxt;  //_ctx = new MetopeDbEntities();
        }
 
        public IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId)
        {
            return _ctx.Parties.Where(c => c.Party_Type == iType)
                    .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity);
             
            
        }
 
    }
}