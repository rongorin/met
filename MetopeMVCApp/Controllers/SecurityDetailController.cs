using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MetopeMVCApp.Models;
using ASP.MetopeNspace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
namespace MetopeMVCApp.Controllers
{
    public class SecurityDetailController : Controller
    { 
        private MetopeDbEntities db = new MetopeDbEntities();
        private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                  
        //public SecurityDetailController() 
        //{
        //    this._repo = new SecurityDetailController(new MetopeDbEntities());
        //}

        //public SecurityDetailController(IPortfolioRepository repo)
        //{
        //    _repo = repo;
        //}


        // GET: /SecurityDetail/ 
        public ActionResult Index(int page=1, string searchTerm=null)  
        {  

            var security_detail = db.Security_Detail
                     .Where(r => searchTerm == null || r.Security_Name.Contains(searchTerm))
                    .Include(s => s.Country)
                    .Include(s => s.Country1)
                    .Include(s => s.Currency).Include(s => s.Currency1).Include(s => s.Currency2).Include(s => s.Currency3)
                    .Include(s => s.Currency_Pair)
                    .Include(s => s.Entity)
                              .OrderBy(s => s.Security_Name)
                  .ToPagedList(page, 12);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Securities", security_detail);

            }
            return View(security_detail);
        }

        // GET: /SecurityDetail/Details/5

        public ActionResult HistoryUnderConstruction(decimal id)
        {
            return View();

        }
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
            var currentUser = manager.FindById(User.Identity.GetUserId());

            ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name");
            ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name");
            ViewBag.Issuer_Code = new SelectList(db.Parties, "Party_Code", "Party_Name");
            ViewBag.Ultimate_Issuer_Code = new SelectList(db.Parties, "Party_Code", "Party_Name");
            ViewBag.Security_Type_Code = new SelectList(db.Security_Type, "Security_Type_Code", "Name");
            //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");

            ViewBag.Primary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name" );
            ViewBag.Secondary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name");
             
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code");
            ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Currency_Pair_Code");
            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code");

            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
            ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text");
            ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text");
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text");

            return View();
        }

        // POST: /SecurityDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Security_ID,Entity_ID,Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,System_Locked")] Security_Detail security_detail)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {

                security_detail.Entity_ID = currentUser.EntityIdScope;
                security_detail.Last_Update_Date = DateTime.Now;
                security_detail.Last_Update_User = User.Identity.Name; 

                db.Security_Detail.Add(security_detail);
                db.SaveChanges();
                TempData.Add("ResultMessage", "new Security \"" + security_detail.Security_Name + "\" created successfully!");

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "An error occurred trying to add a Security");

            ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
            ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
            //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
            ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
            ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Base_Currency_Code", security_detail.Currency_Pair_Code);
            ViewBag.Issuer_Code = new SelectList(db.Parties, "Party_Code", "Party_Name", security_detail.Issuer_Code);
            ViewBag.Ultimate_Issuer_Code = new SelectList(db.Parties, "Party_Code", "Party_Name", security_detail.Ultimate_Issuer_Code);
 
            ViewBag.Primary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name", security_detail.Primary_Exch);
            ViewBag.Secondary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name", security_detail.Secondary_Exch);
            
            ViewBag.Security_Type_Code = new SelectList(db.Security_Type, "Security_Type_Code", "Name", security_detail.Security_Type_Code);

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);
                
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
            ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text");
            ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text" );
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text");
            return View(security_detail);
        }

        //// GET: /SecurityDetail/Edit/5
        //public ActionResult Edit(decimal id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Security_Detail security_detail = db.Security_Detail.Find(id);
        //    if (security_detail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
        //    ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
        //    //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Issuer_Code);
        //    ViewBag.Ultimate_Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Ultimate_Issuer_Code);

        //    ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
        //    ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
        //    ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Currency_Pair_Code", security_detail.Currency_Pair_Code);
        //    ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);
            
        //    var selectListItems = new List<SelectListItem>();
        //    selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
        //    selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
        //    ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text", security_detail.Track_EOM_Flag);
        //    ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text", security_detail.Call_Account_Flag);
        //    ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", security_detail.System_Locked);

        //    return View(security_detail);
        //}

        //// GET: 
        //public ActionResult Edit2(decimal id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Security_Detail security_detail = db.Security_Detail.Find(id);
        //    if (security_detail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
        //    ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
        //    //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
        //    ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
        //    ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Currency_Pair_Code", security_detail.Currency_Pair_Code);
        //    ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);

        //    var selectListItems = new List<SelectListItem>();
        //    selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
        //    selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
        //    ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text", security_detail.Track_EOM_Flag);
        //    ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text", security_detail.Call_Account_Flag);
        //    ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", security_detail.System_Locked);

        //    return View(security_detail);

        //}
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
                ViewBag.Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Issuer_Code);
                ViewBag.Ultimate_Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Ultimate_Issuer_Code);

                ViewBag.Primary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name", security_detail.Primary_Exch);
                ViewBag.Secondary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name", security_detail.Secondary_Exch);
                
                ViewBag.Security_Type_Code = new SelectList(db.Security_Type.ToList(), "Security_Type_Code", "Name", security_detail.Security_Type_Code);

                ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
                ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
                ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
                ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Currency_Pair_Code", security_detail.Currency_Pair_Code);
                ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);

                var selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
                selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
                ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text", security_detail.Track_EOM_Flag);
                ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text", security_detail.Call_Account_Flag);
                ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", security_detail.System_Locked);

                return View(security_detail);

        }
        //// POST: /SecurityDetail/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include="Security_ID,Entity_ID,Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,System_Locked,Last_Update_User,Last_Update_Date")] Security_Detail security_detail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(security_detail).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
        //    ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
        //    //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
        //    ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
        //    ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Base_Currency_Code", security_detail.Currency_Pair_Code);
        //    ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);

        //    var selectListItems = new List<SelectListItem>();
        //    selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
        //    selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
        //    ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text", security_detail.Track_EOM_Flag);
        //    ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text", security_detail.Call_Account_Flag);
        //    ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", security_detail.System_Locked);

        //    return View(security_detail);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit2([Bind(Include = "Security_ID,Entity_ID,Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,System_Locked,Last_Update_User,Last_Update_Date")] Security_Detail security_detail)
        //{
        //     var currentUser = manager.FindById(User.Identity.GetUserId()); 


        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(security_detail).State = EntityState.Modified;

        //        security_detail.Entity_ID = currentUser.EntityIdScope;
        //        security_detail.Last_Update_Date = DateTime.Now;
        //        security_detail.Last_Update_User = User.Identity.Name; 

        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
        //    ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
        //    //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
        //    ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
        //    ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
        //    ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Base_Currency_Code", security_detail.Currency_Pair_Code);
        //    ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);

        //    var selectListItems = new List<SelectListItem>();
        //    selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
        //    selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
        //    ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text", security_detail.Track_EOM_Flag);
        //    ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text", security_detail.Call_Account_Flag);
        //    ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", security_detail.System_Locked);

        //    return View(security_detail);

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Security_ID,Entity_ID,Security_Name,Short_Name,Primary_Exch,Secondary_Exch,Country_Of_Domicile,Country_Of_Risk,Security_Type_Code,Price_Multiplier,Income_Frequency,Issuer_Code,Ultimate_Issuer_Code,Asset_Currency,Min_Lot_Size,Decimal_Precision,AvePrice_Rounding,Issue_Date,Maturity_Date,Coupon_Rate,Price_Exchange,Trade_Currency,Price_Curr,Currency_Pair_Code,Share_Class,Current_Market_Price,Index_Type,Clean_Price_Formula,Accrued_Income_Price_Formula,Odd_First_Coupon_Date,Odd_Last_Coupon_Date,Coupon_Anniversary_Indicator,Track_EOM_Flag,Next_Coupon_Date,Previous_Coupon_Date,Payment_Frequency,Coupon_BusDay_Adjustment,Next_Ex_Div_Date,Ex_Div_BusDay_Adjustment,Ex_Div_Period,Ticker,Inet_ID,Bloomberg_ID,External_Sec_ID,Reuters_ID,ISIN,Call_Account_Flag,System_Locked,Last_Update_User,Last_Update_Date")] Security_Detail security_detail)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            
            if (ModelState.IsValid)
            {
                db.Entry(security_detail).State = EntityState.Modified;

                security_detail.Entity_ID = currentUser.EntityIdScope;
                security_detail.Last_Update_Date = DateTime.Now;
                security_detail.Last_Update_User = User.Identity.Name; 
                db.SaveChanges();
                TempData.Add("ResultMessage", "Security \"" + security_detail.Security_Name + "\" editied successfully!");

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("Error", "An error occurred trying to edit the Security");

            ViewBag.Country_Of_Domicile = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Domicile);
            ViewBag.Country_Of_Risk = new SelectList(db.Countries, "Country_Code", "Country_Name", security_detail.Country_Of_Risk);
            //ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Price_Curr = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Price_Curr);
            ViewBag.Asset_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Asset_Currency);
            ViewBag.Trade_Currency = new SelectList(db.Currencies, "Currency_Code", "ISO_Currency_Code", security_detail.Trade_Currency);
            ViewBag.Currency_Pair_Code = new SelectList(db.Currency_Pair, "Currency_Pair_Code", "Base_Currency_Code", security_detail.Currency_Pair_Code);

            ViewBag.Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Issuer_Code);
            ViewBag.Ultimate_Issuer_Code = new SelectList(db.Parties.ToList(), "Party_Code", "Party_Name", security_detail.Ultimate_Issuer_Code);

            ViewBag.Primary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name", security_detail.Primary_Exch);
            ViewBag.Secondary_Exch = new SelectList(db.Exchanges.ToList(), "Exchange_Code", "Exchange_Name", security_detail.Secondary_Exch);

            ViewBag.Security_Type_Code = new SelectList(db.Security_Type.ToList(), "Security_Type_Code", "Name", security_detail.Security_Type_Code);

            ViewBag.Entity_ID = new SelectList(db.Entities, "Entity_ID", "Entity_Code", security_detail.Entity_ID);

            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "True", Value = bool.TrueString });
            selectListItems.Add(new SelectListItem { Text = "False", Value = bool.FalseString });
            ViewBag.MyTrackEOMFlagList = new SelectList(selectListItems, "Value", "Text", security_detail.Track_EOM_Flag);
            ViewBag.MyCallAccountFgList = new SelectList(selectListItems, "Value", "Text", security_detail.Call_Account_Flag);
            ViewBag.MySysLockedList = new SelectList(selectListItems, "Value", "Text", security_detail.System_Locked);

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
