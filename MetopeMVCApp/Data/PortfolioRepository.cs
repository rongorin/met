using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Data.Entity ;
namespace MetopeMVCApp.Data
{
    public class PortfolioRepository : IPortfolioRepository
    {
        MetopeDbEntities _ctx;
         
        public PortfolioRepository(MetopeDbEntities contxt) //created new consturctor
        { 
            _ctx = contxt;  //_ctx = new MetopeDbEntities();
        } 
        public IQueryable<Portfolio> GetPortfolios(decimal iUserId, string searchTerm = null)
        {
          return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId) 
                  .Where(r => searchTerm == null || r.Portfolio_Name.StartsWith(searchTerm))
                  .Include(p => p.Entity)

                  .Include(p => p.User);
              //return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId) ; 


            //return _ctx.Replies.Where(r => r.TopicId == topicId);
        }

        public IQueryable<User> GetUsers(decimal iEntityId)
        {
            return _ctx.Users.Where(r => r.Entity_ID == iEntityId);
            //return _ctx.Portfolios.Where(c => c.Entity_ID == iUserId) ; 


            //return _ctx.Replies.Where(r => r.TopicId == topicId);
        }
        public Portfolio GetPortfolioById(decimal EntityId, string PortfolioCode)
        {
            return _ctx.Portfolios.Find(EntityId, PortfolioCode);
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
    }
}