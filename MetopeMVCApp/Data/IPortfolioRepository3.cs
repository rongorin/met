using MetopeMVCApp.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// for this generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data.GenericRepository
{
    public interface IPortfolioRepository3 : IGenericRepository<Portfolio>
    {
        //IQueryable<Portfolio> GetPortfolios(decimal iEntityId, string iSearchTerm=null);
        //IQueryable<Portfolio> GetPortfolios(decimal iEntityId, string iSearchTerm = null);
        IPagedList<Portfolio> GetPortfolios(decimal iEntityId, int page = 1, string iSearchTerm = null);
        IList<Portfolio> GetPortfolios(decimal iEntityId);
        IQueryable<User> GetUsers(decimal iEntityId);
        Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode);
        Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode, bool IncludeUser);
        void DeletePortfolio(decimal EntityId, string PortfolioCode);
        void UpdatePortfolio(Portfolio portfolio);
        void CreatePortfolio(Portfolio portfolio);
        

        //dropDown data:
        IQueryable<Code_Miscellaneous> GetCodeMiscVals(string iCodeType);
        IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId);

    }
}