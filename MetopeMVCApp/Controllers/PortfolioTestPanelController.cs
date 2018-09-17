using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity.EntityFramework;

using MetopeMVCApp.Data;
using PagedList;
using MetopeMVCApp.Filters;
using MetopeMVCApp.Models;
using System.Configuration;
using System.Net;


namespace MetopeMVCApp.Controllers
{
    public class PortfolioTestPanelController : Controller
    {
        //private PortfolioRepository _repo = new PortfolioRepository( );
        private readonly IPortfolioRepository _repo;

        private MetopeDbEntities db = new MetopeDbEntities(); //REMOVE this when done doing repository
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        // need parameterless constructor. This is poor mans constructor Dependency Injection (DI)/ 
        // so this just passes in an implementation of the IPortfolioRep interface (which is the parm required by the constructor)
        // The MVC runtime can't instantiate your controller as it can't provide an implementation of IAccommodationService. You either need to do poor man's constructor injection like this:
        //  public FeaturedAccommodationController()
        //     : this(new AccommodationService())
        //  {
        //  }
        //  public FeaturedAccommodationController(IAccommodationService accommodationService)
        //  { 
        //  }

        // Inversion of Control (IoC) like ninject or similar. http://stackoverflow.com/questions/12605445/mvc-no-parameterless-constructor-defined-for-this-object
        public PortfolioTestPanelController()
            : this(new PortfolioRepository(new MetopeDbEntities()))
        {
        }

        //public PortfolioTestPanelController() 
        //{
        //    this._repo = new PortfolioRepository(new MetopeDbEntities());
        //}

        //use this when doing the proper DI :
        public PortfolioTestPanelController(IPortfolioRepository repo)
        {
            _repo = repo;
        }
        // GET: /PortfolioTestPanel/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /PortfolioTestPanel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PortfolioTestPanel/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PortfolioTestPanel/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Portfolio/Edit/5
        public ActionResult Edit(decimal EntityId, string PortfolioCode)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());


            if (EntityId == null || PortfolioCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (currentUser.EntityIdScope != EntityId)
            {
                throw new Exception("Forbidden");
                //throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "Forbidden");
                //return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!
            }
            Portfolio portfolio = _repo.GetPortfolioById(EntityId, PortfolioCode);

            if (portfolio == null)
            {
                return HttpNotFound();
            }

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.PortfolioBaseCurrency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Base_Currency);
            ViewBag.Portfolio_Report_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Report_Currency);
            ViewBag.PortfolIo_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", portfolio.PortfolIo_Domicile);
            ViewBag.Portfolio_Types = new SelectList(GetCodeMiscellVals("PORTTYP"), "MisCode", "MisCode", portfolio.Portfolio_Type);
            ViewBag.PortfolioStatus = new SelectList(GetCodeMiscellVals("PFSTATUS"), "MisCode", "MisCode", portfolio.Portfolio_Status);
            ViewBag.Custodians = new SelectList(GetPartyValues(currentUser.EntityIdScope), "Party_Code", "Party_Name", portfolio.Custodian_Code);


            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
            ViewBag.MyActiveFlagList = new SelectList(selectListItems, "Value", "Text", portfolio.Active_Flag);
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", portfolio.System_Locked);

            ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name", portfolio.Manager);

            return View(portfolio);
        }

        // POST: /Portfolio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Entity_ID,Portfolio_Code,Portfolio_Name,Manager,Portfolio_Type,Portfolio_Base_Currency,PortfolIo_Domicile,Portfolio_Report_Currency,Inception_Date,Financial_Year_End, Portfolio_Status ,Custodian_Code,Active_Flag,System_Locked")] 
                                    Portfolio portfolio)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                if (currentUser.EntityIdScope != portfolio.Entity_ID)
                {
                    ModelState.AddModelError("Error", "An error occurred trying to edit. Portfolio isnt in scope");
                    //return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable); //user manipulated querystring!
                }
                else
                {
                    _repo.UpdatePortfolio(portfolio); // this is  EntityState.Modified;
                    _repo.Save();
                    TempData.Add("ResultMessage", "Portfolio \"" + portfolio.Portfolio_Name + "\" editied successfully!");
                    return RedirectToAction("Index");

                }
            }
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", portfolio.Entity_ID);
            ViewBag.PortfolioBaseCurrency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Base_Currency);
            ViewBag.Portfolio_Report_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", portfolio.Portfolio_Report_Currency);
            ViewBag.PortfolIo_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", portfolio.PortfolIo_Domicile);
            ViewBag.Portfolio_Types = new SelectList(GetCodeMiscellVals("PORTTYP"), "MisCode", "MisCode", portfolio.Portfolio_Type);
            ViewBag.PortfolioStatus = new SelectList(GetCodeMiscellVals("PFSTATUS"), "MisCode", "MisCode", portfolio.Portfolio_Status);
            ViewBag.Custodians = new SelectList(GetPartyValues(currentUser.EntityIdScope), "Party_Code", "Party_Name", portfolio.Custodian_Code);

            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
            ViewBag.MyActiveFlagList = new SelectList(selectListItems, "Value", "Text", portfolio.Active_Flag);
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", portfolio.System_Locked);

            ViewBag.managers = new SelectList(LoadManagers(currentUser.EntityIdScope), "User_Code", "User_Name", portfolio.Manager);
            return View(portfolio);
        }
         
        //For DropDowns populating
        public IQueryable<User> LoadManagers(decimal iEntityId)
        {
            //return db.Users.Where(r => r.Entity_ID == iEntityId);
            return _repo.GetUsers(iEntityId);

        } 
        //
        // GET: /PortfolioTestPanel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PortfolioTestPanel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public IQueryable<Code_Miscellaneous> GetCodeMiscellVals(string iSettings)
        {
            //return db.Users.Where(r => r.Entity_ID == iEntityId);
            return _repo.GetCodeMiscVals(iSettings);

        }
        public IQueryable<Party> GetPartyValues(decimal iEntityId)
        {
            decimal refGenericEntity = Convert.ToDecimal(ConfigurationManager.AppSettings["GenericEntityId"]);
            //return db.Users.Where(r => r.Entity_ID == iEntityId);
            return _repo.GetPartyValues(iEntityId, "CUSTODIAN", refGenericEntity);

        } 
    }
}
