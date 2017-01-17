using MetopeMVCApp.Data;
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
        private MetopeDbEntities db = new MetopeDbEntities();
 

        //
        // GET: /GridSecurityDetail/
        public ActionResult Index()
        {
            IEnumerable<Security_Detail> security_detail = db.Security_Detail.ToList();

            return View(security_detail);

        } 
         
    }
}
