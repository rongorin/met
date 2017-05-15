using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data
{
    public class PartyDebtAnalysisRepository : GenericRepository<MetopeDbEntities, Party_Debt_Analysis>,
                                        IPartyDebtAnalysisRepository
    {
    }
}