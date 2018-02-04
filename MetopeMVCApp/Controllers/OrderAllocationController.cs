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

namespace MetopeMVCApp.Controllers
{

    public class OrderAllocationController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly IOrderAllocationRepository db11; 

        public OrderAllocationController(IOrderAllocationRepository iDb   )
        {
            db11 = iDb; 
        } 
         
        // GET: Order_Allocation
        public ActionResult Index()
        {
            var model = new OrderAllocSelectionViewModel();



            var view = db11.GetAll().Include(a => a.Order_Detail).Include(a => a.Order_Detail.Security_Detail)
                .Select(g => new SelectOrderAllocEditorViewModel
                    { 
                          Selected = true,
                        Entity_ID = g.Entity_ID,
                        Allocation_ID = g.Entity_ID,
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
                .ToList();
            foreach (var oa in view.ToList() )
            {
                var e = new SelectOrderAllocEditorViewModel()
                {
                    Portfolio_Code = oa.Portfolio_Code
                };
                model.OrderAllocations.Add(e);
            }

           // var order_Allocation = db.Order_Allocation.Include(o => o.Entity).Include(o => o.Order_Detail).Include(o => o.Portfolio).Include(o => o.User);
            return View(model);
        }
        public ActionResult BulkDelete(string myArray)
        {
            var myArrayInt = myArray.Split(',').Select(x => Int32.Parse(x)).ToArray();
            return RedirectToAction("Index");


        }
        // GET: Order_Allocation/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Allocation order_Allocation = db.Order_Allocation.Find(id);
            if (order_Allocation == null)
            {
                return HttpNotFound();
            }
            return View(order_Allocation);
        }

        // GET: Order_Allocation/Create
        public ActionResult Create()
        {
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            ViewBag.Order_ID = new SelectList(db.Order_Detail, "Order_ID", "Transaction_Type");
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name");
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name");
            return View();
        }

        // POST: Order_Allocation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Allocation_ID,Entity_ID,Order_ID,Portfolio_Code,Target_Quantity,Target_Clean_Amount_TradeCur,Target_Income_Amount_TradeCur,Target_AllIn_Amount_TradeCur,Target_Clean_Amount_BaseCur,Target_Income_Amount_BaseCur,Target_AllIn_Amount_BaseCur,Execution_Quantity,Place_Quantity,Execution_Clean_Amount_TradeCur,Execution_Income_Amount_TradeCur,Execution_AllIn_Amount_TradeCur,Execution_Clean_Amount_BaseCur,Execution_Income_Amount_BaseCur,Execution_AllIn_Amount_BaseCur,Commission_Rate,Commission_Type,Commission_Amount_TradeCur,Commission_Amount_BaseCur,Execution_Gross_Amount_TradeCur,Execution_Gross_Amount_BaseCur,Execution_Net_Amount_TradeCur,Execution_Net_Amount_BaseCur,Buy_Currency_Target_Amount_TradeCur,Sell_Currency_Target_Amount_TradeCur,Buy_Currency_Target_Amount_BaseCur,Sell_Currency_Target_Amount_BaseCur,Buy_Currency_Execution_Amount_TradeCur,Sell_Currency_Execution_Amount_TradeCur,Buy_Currency_Execution_Amount_BaseCur,Sell_Currency_Execution_Amount_BaseCur,Fee_Amount1_TradeCur,Fee_Amount2_TradeCur,Fee_Amount3_TradeCur,Fee_Amount4_TradeCur,Fee_Amount5_TradeCur,Fee_Amount6_TradeCur,Tax_Amount_TradeCur,Fee_Amount1_BaseCur,Fee_Amount2_BaseCur,Fee_Amount3_BaseCur,Fee_Amount4_BaseCur,Fee_Amount5_BaseCur,Fee_Amount6_BaseCur,Tax_Amount_BaseCur,Trade_Base_FX_Rate,Export_Reference,Export_Status,Allocation_Ack_Nack_Status,Operations_Status,Last_Update_Date,Last_Update_User")] Order_Allocation order_Allocation)
        {
            if (ModelState.IsValid)
            {
                db.Order_Allocation.Add(order_Allocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", order_Allocation.Entity_ID);
            ViewBag.Order_ID = new SelectList(db.Order_Detail, "Order_ID", "Transaction_Type", order_Allocation.Order_ID);
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", order_Allocation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", order_Allocation.Entity_ID);
            return View(order_Allocation);
        }

        // GET: Order_Allocation/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Allocation order_Allocation = db.Order_Allocation.Find(id);
            if (order_Allocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", order_Allocation.Entity_ID);
            ViewBag.Order_ID = new SelectList(db.Order_Detail, "Order_ID", "Transaction_Type", order_Allocation.Order_ID);
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", order_Allocation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", order_Allocation.Entity_ID);
            return View(order_Allocation);
        }

        // POST: Order_Allocation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Allocation_ID,Entity_ID,Order_ID,Portfolio_Code,Target_Quantity,Target_Clean_Amount_TradeCur,Target_Income_Amount_TradeCur,Target_AllIn_Amount_TradeCur,Target_Clean_Amount_BaseCur,Target_Income_Amount_BaseCur,Target_AllIn_Amount_BaseCur,Execution_Quantity,Place_Quantity,Execution_Clean_Amount_TradeCur,Execution_Income_Amount_TradeCur,Execution_AllIn_Amount_TradeCur,Execution_Clean_Amount_BaseCur,Execution_Income_Amount_BaseCur,Execution_AllIn_Amount_BaseCur,Commission_Rate,Commission_Type,Commission_Amount_TradeCur,Commission_Amount_BaseCur,Execution_Gross_Amount_TradeCur,Execution_Gross_Amount_BaseCur,Execution_Net_Amount_TradeCur,Execution_Net_Amount_BaseCur,Buy_Currency_Target_Amount_TradeCur,Sell_Currency_Target_Amount_TradeCur,Buy_Currency_Target_Amount_BaseCur,Sell_Currency_Target_Amount_BaseCur,Buy_Currency_Execution_Amount_TradeCur,Sell_Currency_Execution_Amount_TradeCur,Buy_Currency_Execution_Amount_BaseCur,Sell_Currency_Execution_Amount_BaseCur,Fee_Amount1_TradeCur,Fee_Amount2_TradeCur,Fee_Amount3_TradeCur,Fee_Amount4_TradeCur,Fee_Amount5_TradeCur,Fee_Amount6_TradeCur,Tax_Amount_TradeCur,Fee_Amount1_BaseCur,Fee_Amount2_BaseCur,Fee_Amount3_BaseCur,Fee_Amount4_BaseCur,Fee_Amount5_BaseCur,Fee_Amount6_BaseCur,Tax_Amount_BaseCur,Trade_Base_FX_Rate,Export_Reference,Export_Status,Allocation_Ack_Nack_Status,Operations_Status,Last_Update_Date,Last_Update_User")] Order_Allocation order_Allocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_Allocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", order_Allocation.Entity_ID);
            ViewBag.Order_ID = new SelectList(db.Order_Detail, "Order_ID", "Transaction_Type", order_Allocation.Order_ID);
            ViewBag.Entity_ID = new SelectList(db.Portfolios, "Entity_ID", "Portfolio_Name", order_Allocation.Entity_ID);
            ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", order_Allocation.Entity_ID);
            return View(order_Allocation);
        }

        // GET: Order_Allocation/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Allocation order_Allocation = db.Order_Allocation.Find(id);
            if (order_Allocation == null)
            {
                return HttpNotFound();
            }
            return View(order_Allocation);
        }

        // POST: Order_Allocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Order_Allocation order_Allocation = db.Order_Allocation.Find(id);
            db.Order_Allocation.Remove(order_Allocation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
