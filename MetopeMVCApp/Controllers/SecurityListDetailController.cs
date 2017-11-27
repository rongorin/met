using System;
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

//Customised to take a ViewModel with a custom CheckBoxList to use checkboxes.
//see this concept described at https://www.exceptionnotfound.net/simple-checkboxlist-in-asp-net-mvc/
namespace MetopeMVCApp.Controllers
{

    [SetAllowedEntityIdAttribute]
    public class SecurityListDetailController : Controller
    {
        private MetopeDbEntities db = new MetopeDbEntities();
        private readonly ISecurityListDetailRepository  db11;
        private readonly ISecurityListRepository db2;
        private readonly ISecurityDetailRepository db3;

        public SecurityListDetailController(ISecurityListDetailRepository iDb, ISecurityListRepository iDb2, ISecurityDetailRepository iDb3)
        {
            db11 = iDb;
            db2 = iDb2;
            db3 = iDb3; 
        }
        // GET 
        public ActionResult Index()
        {
            decimal EntityID = (decimal)ViewBag.EntityId; 
            var sldVm = db11.GetAll()
                    .MatchCriteria(c => c.Entity_ID == EntityID) 
                    .Select(g => new SecurityListDetailIndexVM
                    {
                        Security_List_Code = g.Security_List_Code,
                        Entity_ID = g.Entity_ID,
                        Description = g.Description,
                        Security_List_Name = g.Security_List_Name  
                        // SecurityList = new List<Security_List>
                        //{
                        //    Security_ID = 333,
                        //    Entity_ID = 2
                        //}  
                    })
                    .OrderBy(s => s.Security_List_Code).ToList();

                //to populate the SecLists for this Detail:
            PopulateTheSecList (ref sldVm); 
              
            return View(sldVm );
        }
      
        // GET: SecurityListDetail/Create
        public ActionResult Create()
        {
            decimal EntityID = (decimal)ViewBag.EntityId;
            AddSecurityListDetailVM model = new AddSecurityListDetailVM();
            model.Entity_ID = EntityID;

            var checkBoxListItems = new List<CheckBoxListItem>();

            //1. get all the Securities
            var allSecs = db3.GetAll()
                    .MatchCriteria(c => c.Entity_ID == EntityID) .ToList();

            //2. and load them in a CheckBoxListItem object :
            foreach (var sec in allSecs)
            {
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = sec.Security_ID,
                    Display = sec.Security_Name,
                    IsChecked = false
                });
            }

            //3. and bind to the model:
            model.Secs = checkBoxListItems;
            return View(model); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Customised to take a ViewModel
        //see this concept at https://www.exceptionnotfound.net/simple-checkboxlist-in-asp-net-mvc/

        public ActionResult Create(AddSecurityListDetailVM securityListDetailVm)
        {                           //public ActionResult Create([Bind(Include = "Entity_ID,Security_List_Code,Security_List_Name,Description,System_Locked")] Security_List_Detail security_List_Detail)
        
            decimal EntityID = (decimal)ViewBag.EntityId;

            // Gets a List of only the secID's :
            var selectedSecs = securityListDetailVm.Secs.Where(x => x.IsChecked).Select(x => x.ID).ToList(); 

            if (ModelState.IsValid)
            {
                db11.Add(EntityID, securityListDetailVm.Security_List_Code, securityListDetailVm.Security_List_Name,
                                securityListDetailVm.Description, securityListDetailVm.System_Locked, selectedSecs);
        
                db11.Save();
                TempData["ResultMessage"] = "List Detail " + securityListDetailVm.Security_List_Name + " Added successfully!";

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "An error occurred trying to create the Securities List");
             
            return View(securityListDetailVm);
        }

        // GET: SecurityListDetail/Edit/5
        [CustomEntityAuthoriseFilter]
        public ActionResult Edit(string securityListCode)
        {
            var EntityID = (decimal)ViewBag.EntityId;

            Security_List_Detail sld = db11.FindBy(r => r.Security_List_Code == securityListCode && r.Entity_ID == EntityID)
                                        //.MatchCriteria(c => c.Entity_ID == EntityID)
                                        .FirstOrDefault(); 
            if (sld == null)    
            {
                return HttpNotFound();
            }
            var vmodel = new AddSecurityListDetailVM()
            {
                Entity_ID = sld.Entity_ID,
                Security_List_Code = sld.Security_List_Code,
                Security_List_Name = sld.Security_List_Name,
                Description = sld.Description,
                System_Locked = sld.System_Locked,  
            }; 
             // Gets a list of ALL the securities:    //1. get all the Securities
             var allSecs = db3.GetAll()
                    .MatchCriteria(c => c.Entity_ID == EntityID).ToList();

            // Gets a List of only the secID's  :
             var secsInList = db2.GetAll().MatchCriteria(s2 => s2.Entity_ID == EntityID &&
                                                            s2.Security_List_Code == securityListCode)
                                                        .Include(s2 => s2.Security_Detail) .ToList();
             
             //2.now load them in a CheckBoxListItem object :
             var checkBoxListItems = new List<CheckBoxListItem>();
             foreach (var sec in allSecs)
             {
                 checkBoxListItems.Add(new CheckBoxListItem()
                 {
                     ID = sec.Security_ID,
                     Display = sec.Security_Name,
                     IsChecked = secsInList.Where(x => x.Security_ID == sec.Security_ID).Any()
                 });
             } 

             //3. and bind to the model:
             vmodel.Secs = checkBoxListItems;
             
            return View(vmodel);
        }


        //Customised to take a ViewModel
        //see this concept at https://www.exceptionnotfound.net/simple-checkboxlist-in-asp-net-mvc/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomEntityAuthoriseFilter]
        public ActionResult Edit(AddSecurityListDetailVM sldViewModel)
                          // Edit([Bind(Include = "Entity_ID,Security_List_Code,Security_List_Name,Description,System_Locked")] Security_List_Detail security_List_Detail)
        { 
            decimal EntityID = (decimal)ViewBag.EntityId; 

            // 1. get the Model as we used a ViewModel in this edit.
            Security_List_Detail sld = db11.FindBy(r => r.Security_List_Code == sldViewModel.Security_List_Code
                                                && r.Entity_ID == EntityID).FirstOrDefault();
            if (sld == null)
            {
                return HttpNotFound();
            } 

            if (ModelState.IsValid)
            {
        
                    //2. Gets a List of only the secID's that user selected
                var selectedSecs = sldViewModel.Secs.Where(x => x.IsChecked).Select(x => x.ID).ToList();

                try
                {
                    db11.Edit(EntityID, sldViewModel.Security_List_Code, sldViewModel.Security_List_Name,
                                    sldViewModel.Description, sldViewModel.System_Locked, selectedSecs);
                    db11.Save();
                  
                    TempData["ResultMessage"] = "Security List " + sldViewModel.Security_List_Name + " edited successfully!";
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    TempData["FailMsg"] = "Failure saving List Code " + sldViewModel.Security_List_Code + ". something went wrong.";
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Securities List");

            return View( sldViewModel); 
        }

        // GET: SecurityListDetail/Delete/5
        [CustomEntityAuthoriseFilter]
        public ActionResult Delete(string SecurityListCode)
        {
            decimal EntityID = (decimal)ViewBag.EntityId;

            if (SecurityListCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Security_List_Detail sld = db11.FindBy(r => r.Entity_ID == EntityID && r.Security_List_Code == SecurityListCode)
                                                .FirstOrDefault();
            if (sld == null)
            {
                return HttpNotFound();
            }

            return View(sld);
        }

        // POST: SecurityListDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public ActionResult DeleteConfirmed( string SecurityListCode )
        {  
            decimal EntityID = (decimal)ViewBag.EntityId;

            Security_List_Detail security_List_Detail = db11.FindBy(r => r.Entity_ID == EntityID && r.Security_List_Code == SecurityListCode)
                                .FirstOrDefault(); 
             
            db11.Delete(security_List_Detail);
            db11.Save();
            TempData.Add("ResultMessage", "Security List " + security_List_Detail.Security_List_Code + "\" deleted successfully!");

            return RedirectToAction("Index");

        }
        private void PopulateTheSecList(ref List<SecurityListDetailIndexVM> vm)
        {
            foreach (var s in vm)
            {
                //get all the Sec Lists for the ListDetail record:
               
                var sls = db2.GetAll().MatchCriteria(s2 => s2.Entity_ID == s.Entity_ID &&
                                                            s2.Security_List_Code == s.Security_List_Code)
                                            .Include(s2 => s2.Security_Detail).ToList();  
                foreach (var secList in sls)
                {
                    s.SecurityList.Add(new SecurityListVM()
                    {
                        Security_ID = secList.Security_ID,
                        Security_Name = secList.Security_Detail.Security_Name
                    });

                    //var obj = new SecurityListVM()
                    //{
                    //    Security_ID = 887,
                    //    Security_Name = "s33sssss"
                    //};
                    //s.SecurityList.Add(obj);
                }
            }
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
