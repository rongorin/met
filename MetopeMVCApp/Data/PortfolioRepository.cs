using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Data.Entity ;
using PagedList;
namespace MetopeMVCApp.Data
{
    public class PortfolioRepository : IPortfolioRepository, IDisposable
    {
        MetopeDbEntities _ctx;
         
        public PortfolioRepository(MetopeDbEntities contxt) //created new consturctor
        { 
            _ctx = contxt;  //_ctx = new MetopeDbEntities();
        }
        public IList<Portfolio> GetPortfolios(decimal iUserId )
        {
          return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId) 
                  .ToList(); 
        }
        public IPagedList<Portfolio> GetPortfolios(decimal iUserId, int page = 1, string searchTerm = null)
        {
            return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId)
                    .SearchName(searchTerm)
                    //.Where(r => searchTerm == null || r.Portfolio_Name.Contains(searchTerm))
                    .Include(p => p.Entity)

                    .Include(p => p.User)
                    .OrderBy(s => s.Portfolio_Name)
                    .ToPagedList(page, 10);
            //return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId) ; 

            //return _ctx.Replies.Where(r => r.TopicId == topicId);
        }

        public IQueryable<User> GetUsers(decimal iEntityId)
        {
            return _ctx.Users.Where(r => r.Entity_ID == iEntityId);
            //return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId) ; 

                
            //return _ctx.Replies.Where(r => r.TopicId == topicId);
        }

        public IQueryable<Code_Miscellaneous> GetCodeMiscVals(string iCodeType)
        {
            return _ctx.Code_Miscellaneous.Where(r => r.MisCode_Type == iCodeType); 
        }

        public IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId)
        { 
            return _ctx.Parties.Where(c => c.Party_Type == iType)
                    .Where(r => r.Entity_ID == iGenericEntityId  || r.Entity_ID == iEntity ); 
        }
        public Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode )
        {
            return _ctx.Portfolios.Find(EntityId, PortfolioCode);     
        }
        public Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode, bool IncludeUser)
        {
            Portfolio myPrt = _ctx.Portfolios.Find(EntityId, PortfolioCode); 
           _ctx.Entry(myPrt).Reference(p => p.User).Load();
            return myPrt;
        }
       
        public void DeletePortfolio(decimal EntityId, string PortfolioCode) 
        {
            Portfolio port = _ctx.Portfolios.Find( EntityId,   PortfolioCode);
            _ctx.Portfolios.Remove(port);
        }

        public void UpdatePortfolio(Portfolio portf )
        {
            _ctx.Entry(portf).State = EntityState.Modified;
        }
        public void  CreatePortfolio(Portfolio portfolio)
        {
            _ctx.Portfolios.Add(portfolio);
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            if (!disposed)
            {
                _ctx.Dispose();
                disposed = true;
            }
        }
    }
}