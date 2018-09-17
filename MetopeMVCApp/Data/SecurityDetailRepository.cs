using Metope.DAL;
using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Data.Entity;
// for this using of generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data.Repositories
{
    public class SecurityDetailRepository : GenericRepository<MetopeDbEntities, Security_Detail>, 
                                        ISecurityDetailRepository
    {
        public IQueryable<Security_Detail> GetAll(Expression<Func<Security_Detail, bool>> predicate)
        {
            IQueryable<Security_Detail> query = Context.Set<Security_Detail>().Where(predicate);
            return query; 
        }

        public string RunGenerateDividendsSp(decimal iEntity, decimal? iSecurity, string iSecuritiesList, string iSecurityType, string iUserName)
        {
            return Context.Database.SqlQuery<string>(
                 "sp_GenerateUpdateDividendSchedule {0},{1},{2},{3},{4}", iEntity, iSecurity, iSecuritiesList, iSecurityType, iUserName).Single();

            //return Context.Database.ExecuteSqlCommand("sp_TestRun {0}", iEntity);
            //return Context.sp_TestRun(iEntity);
        } 
        public string RunSecAnalyticBatchsetSp(decimal iEntity, DateTime? ieffectiveDate, int? iSessionID, string iVfListcode, string iUserName)
        {
            return Context.Database.SqlQuery<string>(
                 "sp_BatchSet {0},{1},{2},{3},{4},{5}", iEntity, ieffectiveDate, iSessionID,
                            iVfListcode, iUserName, 1).Single();
        }
    }
    public class SecurityDividendDetailRepository : GenericRepository<MetopeDbEntities, Security_Dividend_Detail>,
                                        ISecurityDividendDetailRepository
    {
        public IQueryable<Security_Dividend_Detail> GetAll(Expression<Func<Security_Dividend_Detail, bool>> predicate)
        {
            IQueryable<Security_Dividend_Detail> query = Context.Set<Security_Dividend_Detail>().Where(predicate);
            return query; 

        }
        public decimal GetMaxDividendSeqNo(decimal iEntity, decimal iSecurityId)
        {
            decimal? maxNumber = Context.Set<Security_Dividend_Detail>().Where(o => o.Entity_ID == iEntity && o.Security_ID == iSecurityId)
                    .Max(o => (decimal?)o.Dividend_Seq_Number) ;  
             
            if (maxNumber == null)
               return 0;
            else
               return Convert.ToDecimal(maxNumber); 
        }
        public string RunGenerateDividendsSp(decimal iEntity ,decimal iSecurity, string iSecuritiesList,  string iSecurityType, string iUserName)
        {
            return Context.Database.SqlQuery<string>(
                 "sp_GenerateUpdateDividendSchedule {0},{1},{2},{3},{4}", iEntity, iSecurity, iSecuritiesList, iSecurityType, iUserName).Single();
  
            //return Context.Database.ExecuteSqlCommand("sp_TestRun {0}", iEntity);
            //return Context.sp_TestRun(iEntity);
        }

    }
    public class PortfolioPerformanceRepository : GenericRepository<MetopeDbEntities, Portfolio_Performance>, IPortfolioPerformanceRepository
    {
        public IEnumerable<Portfolio_Performance> GetAllRecs(Expression<Func<Portfolio_Performance, bool>> predicate) // return an IEnumerable
        {
            IQueryable<Portfolio_Performance> query;
            query = Context.Set<Portfolio_Performance>().Include(r => r.Portfolio).Where(predicate);
            return query.ToList();
        } 
    }
    public class ForexForecastRepository : GenericRepository<MetopeDbEntities, Forex_Forecast>, IForexForecastRepository
    {
        public IEnumerable<Forex_Forecast> GetAllRecs(Expression<Func<Forex_Forecast, bool>> predicate) // return an IEnumerable
        {
            IQueryable<Forex_Forecast> query; 
            query = Context.Set<Forex_Forecast>().Include(r => r.Security_Detail).Where(predicate); 
            return query.ToList();
        }
    }
    public class SecurityClassificationIndustryRepository : GenericRepository<MetopeDbEntities, Security_Classification_Industry>, ISecurityClassificationIndustryRepository
    {
        public IEnumerable<Security_Classification_Industry> GetAllRecs(Expression<Func<Security_Classification_Industry, bool>> predicate) // return an IEnumerable
        {
            IQueryable<Security_Classification_Industry> query;
            //query = Context.Set<Security_Classification_Industry>().Include(r => r.Portfolio).Where(predicate);
            query = Context.Set<Security_Classification_Industry>().Where(predicate)
                                                                        .OrderBy(x => x.Security_ID)
                                                                            .ThenByDescending(x => x.Effective_Date)
                                                                            .ThenBy(x => x.Classification_Code);
                                                                            
            return query.ToList();
        }
    }
    public class SecurityAnalyticsRepository : GenericRepository<MetopeDbEntities, Security_Analytics>, ISecurityAnalyticsRepository
    {
        public IEnumerable<Security_Analytics> GetAllRecs() //rather return an IEnumerable
        {
            IQueryable<Security_Analytics> query;
            query = Context.Set<Security_Analytics>().Include(r => r.Security_Detail);
            return query.ToList();
        }
        //public IQueryable<Security_Analytics> GetAll(Expression<Func<Security_Analytics, bool>> predicate)
        //{
        //    IQueryable<Security_Analytics> query = Context.Set<Security_Analytics>().Where(predicate);
        //    return query;
        //}
    }
    //SecurityAttributionRepository
    public class SecurityAttributionRepository : GenericRepository<MetopeDbEntities, Security_Attribution>,
                                       ISecurityAttributionRepository
    {
        public IQueryable<Security_Attribution> GetAll(Expression<Func<Security_Attribution, bool>> predicate)
        {
            IQueryable<Security_Attribution> query = Context.Set<Security_Attribution>().Where(predicate);
            return query; 
        } 
    }
    public class SecurityPriceRepository : GenericRepository<MetopeDbEntities, Security_Price>,
                          ISecurityPriceRepository
    {
        public IQueryable<Security_Price> GetAll(Expression<Func<Security_Price, bool>> predicate)
        {
            IQueryable<Security_Price> query = Context.Set<Security_Price>().Where(predicate);
            return query; 
        }

    }
    public class OrderDetailRepository : GenericRepository<MetopeDbEntities, Order_Detail>,
                          IOrderDetailRepository
    {
        public IQueryable<Order_Detail> GetAll(Expression<Func<Order_Detail, bool>> predicate)
        {
            IQueryable<Order_Detail> query = Context.Set<Order_Detail>().Where(predicate);
            return query;
        } 
    }
    public class OrderAllocationRepository : GenericRepository<MetopeDbEntities, Order_Allocation>,
                          IOrderAllocationRepository
    {
        public IQueryable<Order_Allocation> GetAll(Expression<Func<Order_Allocation, bool>> predicate)
        {
            IQueryable<Order_Allocation> query = Context.Set<Order_Allocation>().Where(predicate);
            return query;
        }
        public void DeleteBulk(decimal[] Ids)
        {
            Context.Set<Order_Allocation>().RemoveRange(Context.Order_Allocation.Where(r => Ids.Contains(r.Allocation_ID ))); 
        } 
    }
    public class ClassificationRepository : GenericRepository<MetopeDbEntities, Classification>,
                                       IClassificationRepository
    {
        public IQueryable<Classification> GetAll(Expression<Func<Classification, bool>> predicate)
        {
            IQueryable<Classification> query = Context.Set<Classification>().Where(predicate);
            return query;
        }
    }
    public class ClassificationIndustryRepository : GenericRepository<MetopeDbEntities, Classification_Industry>,
                                    IClassificationIndustryRepository
    {
        public IQueryable<Classification_Industry> GetAll(Expression<Func<Classification_Industry, bool>> predicate)
        {
            IQueryable<Classification_Industry> query = Context.Set<Classification_Industry>().Where(predicate);
            return query;
        }
    }
    public class SecurityPerformanceRepository : GenericRepository<MetopeDbEntities, Security_Performance>,
                          ISecurityPerformanceRepository
    {
        public IQueryable<Security_Performance> GetAll(Expression<Func<Security_Performance, bool>> predicate)
        {
            IQueryable<Security_Performance> query = Context.Set<Security_Performance>().Where(predicate);
            return query;
        }
    }
    public class CashTransactionsRepository : GenericRepository<MetopeDbEntities, Cash_Transactions>,
                        ICashTransactionsRepository
    {
        public IQueryable<Cash_Transactions> GetAll(Expression<Func<Cash_Transactions, bool>> predicate)
        {
            IQueryable<Cash_Transactions> query = Context.Set<Cash_Transactions>().Where(predicate);
            return query;
        }
    } 

    public class ExchangeRepository : GenericRepository<MetopeDbEntities, Exchange>,
                                     IExchangeRepository
    {
        //possibly override the Update ..here override and update the Version property:  
    } 
    public class CountryRepository : GenericRepository<MetopeDbEntities, Country>,
                                     ICountryRepository
    {
        //possibly override the Update ..here override and update the Version property:  
    }
    public class SecurityTypeRepository : GenericRepository<MetopeDbEntities, Security_Type>,
                                         ISecurityTypesRepository
    {
        //possibly override the Update ..here override and update the Version property:  
    }
    public class CurrencyRepository : GenericRepository<MetopeDbEntities, Currency>,
                                      ICurrencyRepository
    { 
    }
    public class UsersRepository : GenericRepository<MetopeDbEntities, User>,
                                      IUsersRepository
    {
    }
    public class CurrencyPairRepository : GenericRepository<MetopeDbEntities, Currency_Pair>,
                                  ICurrencyPairRepository
    { 
    }
     
    public class PartyRepository : GenericRepository<MetopeDbEntities, Party>,
                          IPartyRepository
    {
        public IQueryable<Party> GetAllPartyValues(decimal iEntity,  decimal iGenericEntityId)
        {
            return GetAll().Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity);

        }
        public IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId)
        {
            return GetAll().Where(c => c.Party_Type == iType)
                                .Where(r => r.Entity_ID == iGenericEntityId || r.Entity_ID == iEntity);

        }
        public Party Get(string PartyCode)
        {

            return Context.Set<Party>().Find(PartyCode); 

        } 
    } 
    #region Security List Detail Repository :
     
    public class SecurityListDetailRepository : GenericRepository<MetopeDbEntities, Security_List_Detail>,
                                        ISecurityListDetailRepository
    {
        public IQueryable<Security_List_Detail> GetAll(Expression<Func<Security_List_Detail, bool>> predicate)
        {
            IQueryable<Security_List_Detail> query = Context.Set<Security_List_Detail>().Where(predicate);
            return query;
        }

        //here, customise the Add to save parent, then each children (Security_list) recs
        public void Add(decimal entityId, string securityListCode, string securityListName, string description, bool systemlocked, List<decimal> securitieslist)
        {
                //1. Load the SecurityListDetail
            var sLD = new Security_List_Detail()
            {
               Entity_ID = entityId,
               Security_List_Code = securityListCode,
               Security_List_Name = securityListName,
               Description = description,
               System_Locked = systemlocked
            };
               //2. Load and Save each security from the secList to the Security_List 
            foreach (var secID in securitieslist) 
            {
                var sl = new Security_List()
                {
                    Entity_ID = entityId,
                    Security_ID = secID,
                    Security_List_Code = securityListCode
                };
                Context.Set<Security_List>().Add(sl);  
            }
              //3. Save the SecurityListDetail 
            Context.Set<Security_List_Detail>().Add(sLD); 
        }

        public void Delete(Security_List_Detail SecurityListDetail)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //1 delete all Security_List for this List_Code:  
                Context.Set<Security_List>().RemoveRange(Context.Set<Security_List>()
                                        .Where(c => c.Security_List_Code == SecurityListDetail.Security_List_Code &&
                                                    c.Entity_ID == SecurityListDetail.Entity_ID));
                base.Delete(SecurityListDetail);
            }
        }
        public void Edit(decimal entityId, string securityListCode, string securityListName, string description,
                                    bool systemlocked, List<decimal> securitieslist)
        { 
            using (TransactionScope scope = new TransactionScope())
            {
                //1. get and update the SecurityListDetail
                Security_List_Detail sLD = FindBy(r => r.Security_List_Code == securityListCode && r.Entity_ID == entityId).FirstOrDefault();
                sLD.Security_List_Code = securityListCode;
                sLD.Security_List_Name = securityListName;
                sLD.Description = description;
                sLD.System_Locked = systemlocked;
                // 2. delete all Security_List for this List_Code:  
                Context.Set<Security_List>().RemoveRange(Context.Set<Security_List>()
                                        .Where(c => c.Security_List_Code == securityListCode && c.Entity_ID == entityId));
                //3. Load and Save each security from the secList to the Security_List  
                foreach (var secID in securitieslist)
                {
                    var sl = new Security_List()
                    {
                        Entity_ID = entityId,
                        Security_ID = secID,
                        Security_List_Code = securityListCode
                    };
                    Context.Set<Security_List>().Add(sl);
                }
            }
        }
    } 
#endregion

    public class SecurityListRepository : GenericRepository<MetopeDbEntities, Security_List>,
                                        ISecurityListRepository
    {
        public IQueryable<Security_List> GetAll(Expression<Func<Security_List, bool>> predicate)
        {
            IQueryable<Security_List> query = Context.Set<Security_List>().Where(predicate);
            return query;
        }
    }
    public class CodeMiscellaneousRepository : GenericRepository<MetopeDbEntities, Code_Miscellaneous>,
                                  ICodeMiscellaneous
    { 
    } 
}   