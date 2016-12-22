using MetopeMVCApp.Data;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetopeMVCApp.Controllers
{
    public class PortfolioGridController :    IPortfolioRepository, IDisposable
    {
        MetopeDbEntities _ctx;

        public PortfolioGridController(MetopeDbEntities contxt) //created new consturctor       
        { 
            _ctx = contxt;  //_ctx = new MetopeDbEntities();
        }

        //
        public IEnumerable<Portfolio> GetPortfolios(decimal iUserId, int page = 1, string searchTerm = null)
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

	}
}