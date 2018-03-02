using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Metope.DAL;
using MetopeMVCApp.Data;
using MetopeMVCApp.Models;
using MetopeMVCApp.Filters;

namespace MetopeMVCApp.Controllers
{

    [SetAllowedEntityIdAttribute]
    public class OrderAllocationController : Controller
    { 
        private readonly IOrderAllocationRepository db11;
    

        public OrderAllocationController(IOrderAllocationRepository iDb   )
        {
            db11 = iDb; 
        }

        //  for the 'JonHattan'server side version, see commit on 2018-02-04. (see IndexOld.asp) this serverside models version  which 
        //  was based off http://johnatten.com/2014/01/05/asp-net-mvc-display-an-html-table-with-checkboxes-to-select-row-items/
        //  is not being used. Rather use this jscrip-Ajax version. 
        //
        public ActionResult Index()
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            var vwm = db11.GetAll().Include(a => a.Order_Detail).
                                    Include(a => a.Order_Detail.Security_Detail).Where(a => a.Entity_ID == EntityID)
                                 .Take(300)
                .Select(g => new SelectOrderAllocEditorViewModel
                    { 
                          Selected = true,
                        Entity_ID = g.Entity_ID,
                          Allocation_ID = g.Allocation_ID,
                        Order_ID = g.Entity_ID,
                        Ticker = g.Order_Detail.Security_Detail.Ticker,
                        Transaction_Type = g.Order_Detail.Transaction_Type,
                        Trade_Date = g.Order_Detail.Trade_Date,
                        Execution_AllIn_Price = g.Order_Detail.Execution_AllIn_Price,

                        Portfolio_Code = g.Portfolio_Code, 
                        Execution_Quantity = g.Execution_Quantity,
                        Execution_Net_Amount_TradeCur = g.Execution_Net_Amount_TradeCur  
                    }
                )
                 .OrderByDescending(r => r.Trade_Date).ThenBy(n => n.Order_ID).ThenBy(n => n.Portfolio_Code)
                .ToList();
            // var order_Allocation = db.Order_Allocation.Include(o => o.Entity).Include(o => o.Order_Detail).Include(o => o.Portfolio).Include(o => o.User);
            return View(vwm);
        }
        [CustomEntityAuthoriseFilter]
        public ActionResult BulkDelete(string myArray)
        {
            decimal [] myArraydecimals  ;

            try
            {
                myArraydecimals = myArray.Split(',').Select(x => decimal.Parse(x)).ToArray(); 
            }
            catch
            {  
                return Json(new { success = false, responseText = "nothing was selected!" }, JsonRequestBehavior.AllowGet); 
            }
            try{
                db11.DeleteBulk(myArraydecimals);
                db11.Save();
            }
            catch (DataException ex)
            {
                return Json(new { success = false, responseText = "An error occurred on attempting to delete!"  }, JsonRequestBehavior.AllowGet); 
                
            }
            TempData["ResultMessage"] = "Order Allocations " + myArray + " deleted successfully!";

            if (HttpContext.Request.IsAjaxRequest())
                return  Json(new { success = true, responseText = "Index" }, JsonRequestBehavior.AllowGet);
                   // return new HttpStatusCodeResult( HttpStatusCode.BadRequest,"bbbaaa"); 
            else
                return RedirectToAction("Index");  

        } 
        // GET: Order_Allocation/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           //    Order_Allocation orderAllocation = db11.FindBy(r => r.Allocation_ID == id).Include(r => r.Order_Detail).FirstOrDefault();
            Order_Allocation orderAllocation = db11.Get( id) ;

            if (orderAllocation == null)
            {
                return HttpNotFound();
            }
            return View(orderAllocation);
        }  
    

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {           
                db11.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
