
using Metope.DAL;
using MetopeMVCApp.Controllers;
using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
namespace MetopeMVCApp.Services
{
            /*  -------------------------------------------------------------
             * This service is to populate the various dropdowns and lookups
             ------------------------------------------------------------*/ 
    public interface IServices
    {
        IEnumerable<Metope.DAL.Country> ListCountry();
        IEnumerable<Metope.DAL.Currency> ListCurrencies();
        IEnumerable<Metope.DAL.User> ListUsers(decimal iEntity);
        IEnumerable<Metope.DAL.Currency_Pair> ListCurrencyPairs();
        IEnumerable<Metope.DAL.Security_Type> ListSecTypeCode();

        IEnumerable<Metope.DAL.Exchange> ListExchanges();
        IEnumerable<Metope.DAL.Code_Miscellaneous> ListMiscellanousTypes(string iCodeType);
        IEnumerable<Metope.DAL.Portfolio> ListPortfolios(decimal iUser, string iStatus);

        IEnumerable<Metope.DAL.Party> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId);
        //IEnumerable<Country> List();
        void Dispose();
         
    }

    public class Services : IDisposable, IServices   
    {

        Metope.DAL.MetopeDbEntities _context;
        private bool disposed = false;

        public Services(bool createContxt)
        {
            if (createContxt)
                 _context = new MetopeDbEntities();
        }

        public IEnumerable<Metope.DAL.Country> ListCountry()
        {
            ICountryRepository dbCntx = new CountryRepository();
              
            return dbCntx.GetAll().OrderBy(s => s.Country_Name).ToList();

            //return  _context.Countries;
             
                     //.Select(x => new SelectListItem { Text = x.Country_Name, Value = x.Country_Code })
                    // .ToList();
             
                //var newItem = new SelectListItem { Text = "a select me", Value = "01" };
                //xxx.Add(newItem); 
              
            ////return   _context.Countries.ToList() ;
            //return _context.Countries.OrderBy(album => album.Country_Name)
            // .Select(album =>

            //     new SelectListItem
            //     { 
            //         Text = album.Country_Name,
            //         Value = album.Country_Code
            //     });
              
        }
        public IEnumerable<Metope.DAL.Security_Type> ListSecTypeCode()
        {
            ISecurityTypesRepository dbCntx = new SecurityTypeRepository();
            return dbCntx.GetAll().OrderBy(r => r.Security_Type_Code).ToList()  ; 
        }

        public IEnumerable<Metope.DAL.Currency> ListCurrencies()
        {
            ICurrencyRepository dbCntx = new CurrencyRepository(); 
            return dbCntx.GetAll()
                  .OrderBy(s => s.Currency_Name).ToList();
           // return _context.Currencies
                //.Select(x => new SelectListItem { Text = x.Currency_Name, Value = x.Currency_Code })
                  
        }

        public IEnumerable<Metope.DAL.Exchange> ListExchanges()
        {
          IExchangeRepository dbCntx = new  ExchangeRepository (); 
          //using (IExchangeRepository dbCntx = new  ExchangeRepository ())
       
              return dbCntx.GetAll()
                    .OrderBy(s => s.Exchange_Name).ToList();
        
            //return _context.Exchanges   
            //    //.Where(p => p.Entity_ID == 1)
            //    // .Select(x => new SelectListItem { Text = x.Exchange_Name, Value = x.Exchange_Code })
            //     .OrderBy(s => s.Exchange_Name);
        }

        public IEnumerable<Metope.DAL.Code_Miscellaneous> ListMiscellanousTypes(string iCodeType)
        {

            ICodeMiscellaneous dbCntx = new CodeMiscellaneousRepository();
            return dbCntx.GetAll().Where(c => c.MisCode_Type == iCodeType).ToList(); 
         
        }
        public IEnumerable<Metope.DAL.Portfolio> ListPortfolios(decimal iUser, string iStatus)
        {
            IPortfolioRepository3 dbCntx = new PortfolioRepository3();
            return dbCntx.GetAll().Where(c => c.Entity_ID == iUser && c.Portfolio_Status == iStatus).OrderBy(r => r.Portfolio_Name)
                .ToList() ;

        }
        public IEnumerable<Metope.DAL.Classification> ListClassifications(decimal iUser)
        {
            IClassificationRepository  dbCntx = new ClassificationRepository();
            return dbCntx.GetAll().Where(c => c.Entity_ID == iUser ).OrderBy(r => r.Classification_Code)
                   .ToList();  
             
        }

        public IEnumerable<Metope.DAL.Classification_Industry> ListIndustries(decimal iUser, string  iClassificationCode ="")
        {
            IClassificationIndustryRepository dbCntx = new ClassificationIndustryRepository();
            return dbCntx.GetAll().Where(c => c.Entity_ID == iUser &&  
                    ((iClassificationCode != "") ? c.Classification_Code == iClassificationCode : c.Classification_Code != ""))
                    .OrderBy(r => r.Description)
                   .ToList();

        }
        public IEnumerable<Metope.DAL.Security_Detail> ListSecurities2(decimal iEntity, decimal iGenericEntity, string iSecurityTypeCode = "", bool thisEntityOnly = false)
        {
            ISecurityDetailRepository dbCntx;
            dbCntx = new SecurityDetailRepository();
            var results = dbCntx.GetAll()
                .MatchCriteria(c => c.Entity_ID == iEntity || c.Entity_ID == iGenericEntity)
                .MatchCriteria(c => ((iSecurityTypeCode != "") ?
                           c.Security_Type_Code == iSecurityTypeCode : c.Security_Type_Code != ""))
                .OrderBy(r => r.Security_Name);

            if (thisEntityOnly == true)
                return results.MatchCriteria(c => c.Entity_ID == iEntity).ToList();
            else
                return results.ToList();

            //return dbCntx.GetAll().Where(c => c.Entity_ID == iUser);

        } 
        public IEnumerable<Metope.DAL.Security_Detail> ListSecurities (decimal iEntity, decimal iGenericEntity, string iSecurityTypeCode = "", bool thisEntityOnly = false)
        {
            ISecurityDetailRepository dbCntx  ;
            dbCntx = new  SecurityDetailRepository();
             var results =   dbCntx.GetAll()
                 .MatchCriteria(c => c.Entity_ID == iEntity || c.Entity_ID == iGenericEntity)
                 .MatchCriteria(c => ((iSecurityTypeCode != "") ?
                            c.Security_Type_Code == iSecurityTypeCode : c.Security_Type_Code != ""))
                 .OrderBy(r => r.Security_Name);

             if (thisEntityOnly == true)
                 return results.MatchCriteria(c => c.Entity_ID == iEntity).ToList();
             else
                 return results.ToList();
                  
            //return dbCntx.GetAll().Where(c => c.Entity_ID == iUser);
                
        }
        public IEnumerable<Metope.DAL.Security_Detail> ListSecuritiesForE(decimal iEntity, decimal iGenericEntity, string iSecurityTypeCode = "", bool thisEntityOnly = false)
        {
            ISecurityDetailRepository dbCntx;
            dbCntx = new SecurityDetailRepository();
            var results = dbCntx.GetAll()
                .MatchCriteria(c => c.Entity_ID == iEntity || c.Entity_ID == iGenericEntity)
                .MatchCriteria(c => ((iSecurityTypeCode != "") ?
                           c.Security_Type_Code == iSecurityTypeCode : c.Security_Type_Code != ""))
                .OrderBy(r => r.Security_Name);

            if (thisEntityOnly == true)
                return results.MatchCriteria(c => c.Entity_ID == iEntity).ToList();
            else
                return results.ToList();

            //return dbCntx.GetAll().Where(c => c.Entity_ID == iUser);

        }
        public IEnumerable<Metope.DAL.User> ListUsers(decimal iEntity)
        {
            IUsersRepository dbCntx = new UsersRepository();
            return dbCntx.GetAll().Where(s => s.Entity_ID == iEntity).ToList();
        }

        public IEnumerable<Metope.DAL.Order_Detail> ListOrderDetail(decimal iEntity)
        {
            IOrderDetailRepository dbCntx;
            dbCntx = new OrderDetailRepository();
            var results = dbCntx.GetAll()
                .MatchCriteria(c => c.Entity_ID == iEntity).ToList();

            return results;
             
        }
        public IEnumerable<Metope.DAL.Order_Allocation> ListOrderAllocations(decimal iEntity)
        {
            IOrderAllocationRepository dbCntx;
            dbCntx = new OrderAllocationRepository();
            var results = dbCntx.GetAll() 
                .MatchCriteria(c => c.Entity_ID == iEntity ) 
                .OrderBy(r => r.Portfolio_Code)
                .ToList();
             
             return results ;
              
        }
        public IEnumerable<Metope.DAL.Party> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId)
        {
            IPartyRepository dbCntx = new PartyRepository();

            return dbCntx.GetPartyValues(iEntity, iType, iGenericEntityId)
                      .OrderBy(s => s.Party_Name)
                      .ToList();  
        }


        public IEnumerable<Metope.DAL.Party> ListPartyAllIssuers(decimal iEntity, decimal iGenericEntityId)
        {
            IPartyRepository dbCntx = new PartyRepository();

            return dbCntx.GetAllPartyValues(iEntity, iGenericEntityId)    
                        .MatchCriteria( o => o.Party_Type != "CUSTODIAN")
                        .OrderBy(s => s.Party_Name)
                        .ToList();  
        }
         
        //public IEnumerable<SelectListItem> ListPartyValues(string iType, decimal iEntity, decimal iGenericEntityId)
        //{ 
        //    return _context.Parties.Where(c => c.Party_Type == iType)
        //            .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity)
        //            .Select(x => new SelectListItem { Text = x.Party_Name, Value = x.Party_Code }) ;
             
        //}
        public IEnumerable<Metope.DAL.Currency_Pair> ListCurrencyPairs()
        {
            ICurrencyPairRepository dbCntx = new CurrencyPairRepository();
            return dbCntx.GetAll().ToList();
        }

        public List<SelectListItem> ListTrueFalse()
        {  
            return  new List<SelectListItem>
                    {
                        new SelectListItem { Text = "True", Value = bool.TrueString },
                        new SelectListItem { Text = "False", Value = bool.TrueString }
                    }; 
        }
        public void Dispose()
        {
            if (!disposed)
            {
                _context.Dispose();
                disposed = true;
            }
        }
        //public IEnumerable<Country> List()
        //{
        //    return new List<Country>
        //            {
        //                new Country { Country_Code = "AK", Country_Name = "Alaska" },
        //                new Country { Country_Code = "AL", Country_Name = "Alabama" }
        //            };
        //}
    }

 
}