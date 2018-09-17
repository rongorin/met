using Metope.DAL;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// for this using of generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data   
{
    public class PartyDebtAnalysisRepository : GenericRepository<MetopeDbEntities, Party_Debt_Analysis>,
                                        IPartyDebtAnalysisRepository
    {
    }
}