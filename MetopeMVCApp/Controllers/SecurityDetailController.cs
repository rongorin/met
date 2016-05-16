using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;

namespace MetopeMVCApp.Controllers
{
    public class SecurityDetailController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();

        // GET: /SecurityDetail/
        public ActionResult Index()
        { 

            var security_detail = db.Security_Detail.Include(s => s.Country).Include(s => s.Country1).Include(s => s.Currency).Include(s => s.Currency1).Include(s => s.Currency2).Include(s => s.Currency3).Include(s => s.Currency_Pair).Include(s => s.Entity);
            return View(security_detail.ToList());
        }

        // GET: /SecurityDetail/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_Detail security_detail = db.Security_Detail.Find(id);
            if (security_detail == null)
            {
                return HttpNotFound();
            }
            return View(security_detail);
        }

        // GET: /SecurityDetail/Create
        public ActionResult Create()
        {
            ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name");
            ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name");
            //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");

            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Currency_Pair_Code");
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");
            return View();
        }

        // POST: /SecurityDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Security_ID,Entity_ID,Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Benchmark,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,System_Locked,Last_Update_User,Last_Update_Date")] Security_Detail security_detail)
        {
            if (ModelState.IsValid)
            {
                db.Security_Detail.Add(security_detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
            ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
            //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
            ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
            ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Base_Currency_Code", security_detail.Currency_Pair_Code);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);
            return View(security_detail);
        }

        // GET: /SecurityDetail/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_Detail security_detail = db.Security_Detail.Find(id);
            if (security_detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
            ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
            //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
            ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
            ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Currency_Pair_Code", security_detail.Currency_Pair_Code);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);
            return View(security_detail);
        }

        // POST: /SecurityDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Security_ID,Entity_ID,Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Benchmark,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,System_Locked,Last_Update_User,Last_Update_Date")] Security_Detail security_detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(security_detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
            ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
            //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
            ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
            ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Base_Currency_Code", security_detail.Currency_Pair_Code);
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);
            return View(security_detail);
        }

        // GET: /SecurityDetail/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Security_Detail security_detail = db.Security_Detail.Find(id);
            if (security_detail == null)
            {
                return HttpNotFound();
            }
            return View(security_detail);
        }

        // POST: /SecurityDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Security_Detail security_detail = db.Security_Detail.Find(id);
            db.Security_Detail.Remove(security_detail);
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
