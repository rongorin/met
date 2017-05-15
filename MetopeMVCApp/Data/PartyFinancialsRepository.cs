using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data
{
    public class PartyFinancialsRepository : GenericRepository<MetopeDbEntities, Party_Financials>,
                                        IPartyFinancialsRepository
    {
    }
}
 