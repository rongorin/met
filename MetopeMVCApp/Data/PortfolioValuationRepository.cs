using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data.Repositories
{ 
        public class PortfolioValuationRepository : GenericRepository<MetopeDbEntities, Portfolio_Valuation>, 
                                        IPortfolioValuationRepository
    {

    }
}