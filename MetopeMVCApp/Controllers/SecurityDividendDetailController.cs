﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Data;

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class SecurityDividendDetailController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly ISecurityDividendDetailRepository db11;
        private readonly ISecurityDetailRepository db2;

        public SecurityDividendDetailController(ISecurityDividendDetailRepository iDb,ISecurityDetailRepository iDb2)
        {
            db11 = iDb;
            db2 = iDb2; 
        }
        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            };

        // GET: SecurityDividendDetail
        public ActionResult Index(int SecurityId,int? numberOfRows, int page = 1, string searchTerm = null)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            var viewModel = new SecurityDividendDetailViewModel(); 

            if (numberOfRows == null)
                numberOfRows = 20; 
            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);
  
            viewModel.SecurityDetails = db2.FindBy(r => r.Security_ID == SecurityId)
                                                .FirstOrDefault();   
            viewModel.SecurityDividendDetail = db11.GetAll().AsNoTracking()
                       .MatchCriteria(c => c.Entity_ID == EntityID)
                       .MatchCriteria(c => c.Security_ID == SecurityId)
                     .OrderByDescending(r => r.Dividend_Seq_Number).ThenBy(n => n.Dividend_Annual_Number);

            //if (PartyCode != "")
            //    ViewBag.PartyCode = PartyCode;

            //ViewBag.Nav = Nav;

            return View(viewModel ); 
             
        }
         
        // GET: SecurityDividendDetail/Create
        [AllSecuritiesFilter]
        [CurrencyFilter]
        public ActionResult Create(decimal SecurityId)
        {
            var EntityID = (decimal)ViewBag.EntityId; 
        
            var securityDetail = db.Security_Detail
                 .Where(c => c.Security_ID == SecurityId && c.Entity_ID == EntityID).FirstOrDefault<Security_Detail>();
             
            if (securityDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.SecuritiesAll = securityDetail.Security_ID;

            ViewBag.SecurityID = securityDetail.Security_ID;
            ViewBag.SecurityName = securityDetail.Security_Name;

            return View();  
        }

        // POST: SecurityDividendDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllSecuritiesFilter]
        [CurrencyFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Entity_ID,Security_ID,Dividend_Seq_Number,Dividend_Annual_Number,Forecast_Dividend_Payment_Date,Dividend_Currency_Code,Actual_Dividend_Payment_Date,Actual_Last_Date_To_Register,Actual_Ex_Dividend_Date,Dividend_Split,Forecast_Dividend,Actual_Dividend,Dividend_Type,Forecast_Last_Date_to_Register,Forecast_Ex_Dividend_Date,Last_Update_Date,Last_Update_User,Financial_Year,Actual_FX_Rate,Lock_Flag,Actual_NonRecurring_Dividend")] Security_Dividend_Detail sdd)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            /*------------------------------------------ 
             first check if this party code is already used ! 
             ----------------------------------------*/
            Security_Dividend_Detail check = db11.FindBy(r => r.Security_ID == sdd.Security_ID &&
                                                        r.Dividend_Seq_Number == sdd.Dividend_Seq_Number) 
                                                 .MatchCriteria(c => c.Entity_ID == EntityID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Dividend  number " + sdd.Dividend_Seq_Number.ToString() + "  Already exists!");
                }
                else
                {
                    sdd.Entity_ID = EntityID;
                    sdd.Last_Update_Date = DateTime.Now;
                    sdd.Last_Update_User = User.Identity.Name;

                    db11.Add(sdd);
                    db11.Save();
                    TempData.Add("ResultMessage", "new Dividend number " + sdd.Dividend_Seq_Number.ToString() + " created successfully!");

                    return RedirectToAction("Index", new { SecurityId = sdd.Security_ID });
                }
            }

            //ViewBag.Dividend_Currency_Code = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_Dividend_Detail.Dividend_Currency_Code);
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Dividend_Detail.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Dividend_Detail.Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Dividend_Detail.Entity_ID);
            ViewBag.EntityId = sdd.Entity_ID;
            return View(sdd);
        }

        // GET: SecurityDividendDetail/Edit/5
        [TrueFalseFilter]
        [CurrencyFilter]
        public ActionResult Edit(decimal SecurityId, decimal DivSeqNo)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Security_Dividend_Detail sdd = db11.FindBy(r => r.Security_ID == SecurityId &&
                                                        r.Dividend_Seq_Number == DivSeqNo).Include(s => s.Security_Detail)
                                                 .MatchCriteria(c => c.Entity_ID == EntityID).FirstOrDefault();
            if (sdd == null)
            {
                return HttpNotFound();
            }
            ViewBag.DividendCurrencyCode = sdd.Dividend_Currency_Code;
            ViewBag.MySysLockedList = sdd.Lock_Flag;
            ViewBag.DividendSplit = sdd.Dividend_Split;

            //ViewBag.Dividend_Currency_Code = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_Dividend_Detail.Dividend_Currency_Code);
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Dividend_Detail.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Dividend_Detail.Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Dividend_Detail.Entity_ID);
            return View(sdd);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [TrueFalseFilter]
        [CurrencyFilter]
        public ActionResult Edit([Bind(Include = "Entity_ID,Security_ID,Dividend_Seq_Number,Dividend_Annual_Number,Forecast_Dividend_Payment_Date,Dividend_Currency_Code,Actual_Dividend_Payment_Date,Actual_Last_Date_To_Register,Actual_Ex_Dividend_Date,Dividend_Split,Forecast_Dividend,Actual_Dividend,Dividend_Type,Forecast_Last_Date_to_Register,Forecast_Ex_Dividend_Date,Last_Update_Date,Last_Update_User,Financial_Year,Actual_FX_Rate,Lock_Flag,Actual_NonRecurring_Dividend")] Security_Dividend_Detail security_Dividend_Detail)
        {
            var EntityID = (decimal)ViewBag.EntityId;   
             
            if (ModelState.IsValid)
            { 
                db11.Update(security_Dividend_Detail); //sets the modified status 
                security_Dividend_Detail.Last_Update_Date = DateTime.Now;
                security_Dividend_Detail.Last_Update_User = User.Identity.Name;
                db11.Save();
                TempData["ResultMessage"] = "Dividend number " + security_Dividend_Detail.Dividend_Seq_Number.ToString() + " edited successfully!";
                return RedirectToAction("Index", new { SecurityId = security_Dividend_Detail.Security_ID });     
            }
            ViewBag.DividendCurrencyCode = security_Dividend_Detail.Dividend_Currency_Code;
            ViewBag.MySysLockedList = security_Dividend_Detail.Lock_Flag;
            //ViewBag.DividendSplit = security_Dividend_Detail.Dividend_Split;

                
            //ViewBag.Dividend_Currency_Code = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_Dividend_Detail.Dividend_Currency_Code);
            //ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_Dividend_Detail.Entity_ID);
            //ViewBag.Security_ID = new SelectList(db.Security_Detail, "Security_ID", "Security_Name", security_Dividend_Detail.Security_ID);
            //ViewBag.Entity_ID = new SelectList(db.Users, "Entity_ID", "User_Name", security_Dividend_Detail.Entity_ID);
            return View(security_Dividend_Detail);
        }

        // GET: SecurityDividendDetail/Delete/5
        public ActionResult Delete(decimal SecurityId, decimal DivSeqNo)
        {
            Security_Dividend_Detail sdd = db11.FindBy(r => r.Security_ID == SecurityId && r.Dividend_Seq_Number == DivSeqNo  
                   ).FirstOrDefault();

            if (sdd == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Nav = Nav;

            return View(sdd); 
             
        }

        // POST: SecurityDividendDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal SecurityId, decimal DivSeqNo, string navIndicator = "")
        {
            Security_Dividend_Detail sdd = db11.FindBy(r => r.Security_ID == SecurityId &&
                                                            r.Dividend_Seq_Number == DivSeqNo ).FirstOrDefault();
            var Securityname = sdd.Security_Detail.Security_Name; 
            db11.Delete(sdd);
            db11.Save();
            TempData.Add("ResultMessage", "Dividend number " + DivSeqNo.ToString() + " for \"" + Securityname + "\" deleted successfully!");

            if (navIndicator == "")
                return RedirectToAction("Index", null, new { SecurityId = sdd.Security_ID });
            else
                return RedirectToAction("Index", null, new { SecurityId = sdd.Security_ID });

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
