using MetopeMVCApp.Data;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
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
    public class SecurityAnalyticsRepository : GenericRepository<MetopeDbEntities, Security_Analytics>,
                                       ISecurityAnalyticsRepository
    {
        public IQueryable<Security_Analytics> GetAll(Expression<Func<Security_Analytics, bool>> predicate)
        {
            IQueryable<Security_Analytics> query = Context.Set<Security_Analytics>().Where(predicate);
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
    public class CodeMiscellaneousRepository : GenericRepository<MetopeDbEntities, Code_Miscellaneous>,
                                  ICodeMiscellaneous
    { 
    } 
}   