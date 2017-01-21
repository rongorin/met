using MetopeMVCApp.Data;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MetopeMVCApp.Controllers
{
    public class GridSecurityDetailController : Controller
    {
        //private MetopeDbEntities db = new MetopeDbEntities();
        private readonly ISecurityDetailRepository db11 ;
 
       public GridSecurityDetailController(ISecurityDetailRepository iDb  )
        {
            db11 = iDb; 
        }
        //
 
            //IEnumerable<Security_Detail> security_detail = db.Security_Detail.ToList();
        [LogAttribuite]
        public ActionResult Index(int page=1, string searchTerm=null)  
        {
           // var security_detail = db.Security_Detail.Include(s => s.Country).Include(s => s.Country1).Include(s => s.Currency).Include(s => s.Currency1).Include(s => s.Currency2).Include(s => s.Currency3).Include(s => s.Currency_Pair).Include(s => s.Entity).Include(s => s.Exchange).Include(s => s.Exchange1).Include(s => s.Security_Type);
            //var security_detail = db.Security_Detail;

            //var security_detail = db11.Query<Security_Detail>() 


            //var security_detailx = db11.GetAll(r => r.Call_Account_Flag == true) ;
            var security_detail = db11.GetAll ()
                     //.SearchSecName(searchTerm)  
                     //////.Include(s => s.Country).Include(s => s.Country1)  
                     .OrderBy(s => s.Security_Name)
                     
                     .Select(g => new SecurityDetailIndexModel
                     { 
                         Security_ID = g.Security_ID,
                         Entity_ID = g.Entity_ID,
                         Security_Type_Code = g.Security_Type_Code,
                         Security_Name = g.Security_Name,
                         Current_Market_Price = g.Current_Market_Price,
                         Ticker = g.Ticker,
                         Primary_Exch = g.Primary_Exch ,
                         Maturity_Date = g.Maturity_Date  ,
                         Active_Flag = g.Active_Flag    
                     }).    
                     ToList();  

  
            return View(security_detail);
        }

        // GET: /GridSecurityDetail/
        //public ActionResult Index()
        //{
        //    return View(security_detail);

        //} 
         
    }
}
