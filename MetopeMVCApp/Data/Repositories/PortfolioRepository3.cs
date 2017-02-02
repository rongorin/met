using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PagedList;
using MetopeMVCApp.Data.Repositories;
using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
// for this using of generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Controllers
{ 
    public class PortfolioRepository3 : GenericRepository<MetopeDbEntities, Portfolio>, 
                                        IPortfolioRepository3, IDisposable
    {
        // MetopeDbEntities _ctx; 
        //public PortfolioRepository2(MetopeDbEntities contxt) //created new consturctor
        //{       
        //    _ctx = contxt;  //_ctx = new MetopeDbEntities(); 
        //} 

        public IList<Portfolio> GetPortfolios(decimal iUserId)
        {
            List<Portfolio> portfs = null; 
            using (Context)
            { 
                portfs = GetAll().ToList().Where(c => c.Entity_ID == iUserId).ToList();
            }

            return portfs; 
           
        } 
        public IPagedList<Portfolio> GetPortfolios(decimal iUserId, int page = 1, string searchTerm = null)
        {
            return GetAll().Where(c => c.Entity_ID == iUserId)
                    .SearchPortfName(searchTerm)
                //.Where(r => searchTerm == null || r.Portfolio_Name.Contains(searchTerm))
                    .Include(p => p.Entity)
                    .Include(p => p.User)
                    .OrderBy(s => s.Portfolio_Name)
                    .ToPagedList(page, 10);
            //return DataContext.Portfolios.Where(c => c.Entity_ID == iUserId) ; 

            //return DataContext.Replies.Where(r => r.TopicId == topicId);
        }

        public IQueryable<User> GetUsers(decimal iEntityId)
        {
            return Context. Users.Where(r => r.Entity_ID == iEntityId); 
             
            //return DataContext.Portfolios.Where(c => c.Entity_ID == iUserId) ;   
            //return DataContext.Replies.Where(r => r.TopicId == topicId);
        }

        public IQueryable<Code_Miscellaneous> GetCodeMiscVals(string iCodeType)
        {
            return Context.Code_Miscellaneous.Where(r => r.MisCode_Type == iCodeType);
        }

        public IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId)
        {
            return Context.Parties.Where(c => c.Party_Type == iType)
                    .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity);
        }
        public Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode)
        {
            return Context.Portfolios.Find(EntityId,PortfolioCode);
        }

        public Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode, bool IncludeUser)
        {
            Portfolio myPrt = Context.Portfolios.Find(EntityId, PortfolioCode);
            Context.Entry(myPrt).Reference(p => p.User).Load();
            return myPrt;
        }

        public void DeletePortfolio(decimal EntityId, string PortfolioCode)
        {
            Portfolio port = Context.Portfolios.Find(EntityId, PortfolioCode);
            Context.Portfolios.Remove(port);
        }

        public void UpdatePortfolio(Portfolio portf)
        {
            Context.Entry(portf).State = EntityState.Modified;
        }
        public void CreatePortfolio(Portfolio portfolio)
        {
            Context.Portfolios.Add(portfolio);
        }

     


       

    }
}