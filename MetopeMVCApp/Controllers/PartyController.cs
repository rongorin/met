using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using MetopeMVCApp.Data;
using System.Configuration;
using MetopeMVCApp.Filters;
using NLog;
using Metope.DAL;

namespace MetopeMVCApp.Controllers
{
    [SetAllowedEntityIdAttribute]
    public class PartyController : Controller
    { 

        private readonly IPartyRepository db11; 
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private List<SelectListItem> numOfRows = new List<SelectListItem> {
						new SelectListItem { Text = "10", Value = "10" },
						new SelectListItem { Text = "20", Value = "20" },
						new SelectListItem { Text = "50", Value = "50" },
						new SelectListItem { Text = "100", Value = "100" }
			            }; 
        public PartyController(IPartyRepository iDb)
        {
            db11 = iDb; 
        }
         
        public ActionResult Index(int? numberOfRows)  
        {
             

            var currentUser = manager.FindById(User.Identity.GetUserId()); 
            ViewBag.EntityId = currentUser.EntityIdScope;
            if (numberOfRows == null)
                numberOfRows = 20;

            ViewBag.RowsPerPage = new SelectList(numOfRows, "Value", "Text", numberOfRows);

            var parties = db11.GetAllPartyValues( currentUser.EntityIdScope, (decimal)ViewBag.genericEntity).
                                                            Include(p => p.Country).Include(p => p.Entity); 
            return View(parties.ToList());
        } 

        // GET: Party/Create
        [CountryFilter]
        public ActionResult Create()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());   
            return View(); 
        }

        // POST: Party/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CountryFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Entity_ID,Party_Code,Party_Name,Party_Type,Financial_Year_End,Country_Code,System_Locked,SWIFT_ID,BIC_Code")] Party party)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            party.Entity_ID = currentUser.EntityIdScope;

            /*------------------------------------------ 
            first check if this party code is already used ! 
            ----------------------------------------*/
            Party check = db11.FindBy(r => r.Party_Code == party.Party_Code).FirstOrDefault();
                     //   Party check = db11.Get(party.Party_Code); 
            if (ModelState.IsValid)
            {
                if (check != null)
                {
                    ModelState.AddModelError("Name", "FAILED to create Party \"" + party.Party_Name + "\" code:\"" + party.Party_Code + "\". Already exists!");
                }
                else
                {
                    db11.Add(party);
                    db11.Save();
                    TempData.Add("ResultMessage", "new Party \"" + party.Party_Name + "\" created successfully!");
                    return RedirectToAction("Index");
                }
             }
                //ViewBag.Country_Code = new SelectList(db.Countries, "Country_Code", "Country_Name", party.Country_Code); 
            return View(party);
        }

        // GET: Party/Edit/5  
        [CustomEntityAuthoriseFilter]
        [CountryFilter]
        public ActionResult Edit(string PartyCode, decimal EntityId)
        {   
            Party party = db11.FindBy(r => r.Party_Code == PartyCode)
                                    .MatchCriteria(c => c.Entity_ID == EntityId )
                                    .Include(p => p.Country).FirstOrDefault();
            if (party == null)
            {
                return HttpNotFound();
            }
 
            ViewBag.RecordCountryOfDomicile = party.Country_Code;

            //if (Request.IsAjaxRequest())
            //{
            //    return View(party);
            //}
            return View(party);
        }
 
        [HttpPost] 
        [CountryFilter]  
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Entity_ID,Party_Code,Party_Name,Party_Type,Financial_Year_End,Country_Code,System_Locked,SWIFT_ID,BIC_Code")] Party party)
        { 
            var currentUser = manager.FindById(User.Identity.GetUserId());
            try
            {
                if (ModelState.IsValid)
                {
                    //int bb = 11 / aa;
                    if (currentUser.EntityIdScope != party.Entity_ID && (decimal)ViewBag.genericEntity != party.Entity_ID)
                    {
                        ModelState.AddModelError("Error", "An error occurred trying to edit. Party isnt in scope");
                    }
                    else
                    {
                        db11.Update(party);

                        db11.Save();
                        TempData.Add("ResultMessage", "Party \"" + party.Party_Name + "\" editied successfully!");
                        return RedirectToAction("Index");
                    }
                }

                return View(party);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("error in the follow: {0}","throwing err div zero!"),
                                    ex);
            }
        } 
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(string PartyCode, decimal EntityId)
        { 
            Party party = db11.FindBy(r => r.Party_Code == PartyCode)
                       .MatchCriteria(c => c.Entity_ID == EntityId)
                       .FirstOrDefault(); 

            if (party == null)
            {
                return HttpNotFound();
            }
            return View(party);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult DeleteConfirmed(string PartyCode, decimal EntityId)
        {  
            Party party = db11.FindBy(r => r.Party_Code == PartyCode).FirstOrDefault();
            db11.Delete(party);
            db11.Save();
            TempData.Add("ResultMessage", "Party \"" + party.Party_Name + "\" deleted successfully!");
            return RedirectToAction("Index");
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
