using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web; 

namespace MetopeMVCApp.Data
{
    public class PartyFinancialsHistoryRepository : GenericRepository<MetopeDbEntities, Party_Financials_History >,
                                         IPartyFinancialsHistoryRepository
    {
    }
}