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

        //public IList<Party> GetPartyValues(int iEntity , string iType)
        //{

        //    return _ctx.Parties.Where(c => c.Party_Type == iType)
        //            .Where(r => r.Entity_ID == 1 || r.Entity_ID == iEntity).ToList();
                    
        //}

    }
}