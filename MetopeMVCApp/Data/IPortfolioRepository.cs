using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetopeMVCApp.Models;
using PagedList;
namespace MetopeMVCApp.Data
{
    public interface IPortfolioRepository
    {
        //IQueryable<Portfolio> GetPortfolios(decimal iEntityId, string iSearchTerm=null);
        //IQueryable<Portfolio> GetPortfolios(decimal iEntityId, string iSearchTerm = null);
        IPagedList<Portfolio> GetPortfolios(decimal iEntityId, int  page=1, string iSearchTerm = null);
        IQueryable<User> GetUsers(decimal iEntityId);
        Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode);
        void DeletePortfolio(decimal EntityId, string PortfolioCode);
        void UpdatePortfolio(Portfolio portfolio);  
        void CreatePortfolio(Portfolio portfolio);
        void Save();

        //dropDown data:
        IQueryable<Code_Miscellaneous> GetCodeMiscVals(string iCodeType);
        IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId);


        //IQueryable<Portfolio> GetTopicsIncludingReplies();

        //IQueryable<Reply> GetRepliesByTopic(int topicId);

        //bool Save();

        //bool AddTopic(Topic newTopic);
        //bool AddReply(Reply newReply);
    }
}
 
 