using Metope.DAL;
using MetopeMVCApp.Data.GenericRepository;
using MetopeMVCApp.Data.Repositories;
using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
// for this generic repository technique see http://www.tugberkugurlu.com/archive/generic-repository-pattern-entity-framework-asp-net-mvc-and-unit-testing-triangle

namespace MetopeMVCApp.Data
{
    public interface ISecurityDetailRepository : IGenericRepository<Security_Detail>
    {
        IQueryable<Security_Detail> GetAll(Expression<Func<Security_Detail, bool>> predicate);
        //IQueryable<Security_Detail> GetAllActive( );
        string RunGenerateDividendsSp(decimal iEntity, decimal? iSecurity, string iSecuritiesList, string iSecurityType, string iUserName);
        string RunSecAnalyticBatchsetSp(decimal iEntity, DateTime? ieffectiveDate, int? iSessionID, string iVfListcode, string iUserName);

    }
    public interface ISecurityDividendDetailRepository : IGenericRepository<Security_Dividend_Detail>
    {
        IQueryable<Security_Dividend_Detail> GetAll(Expression<Func<Security_Dividend_Detail, bool>> predicate);

        decimal GetMaxDividendSeqNo(decimal iEntity, decimal iSecurityId);
        string RunGenerateDividendsSp(decimal iEntity, decimal iSecurity, string iSecuritiesList, string iSecurityType, string iUserName);
    }

    public interface ISecurityAnalyticsRepository : IGenericRepository<Security_Analytics>
    {
        IEnumerable<Security_Analytics> GetAllRecs();

    } 
    public interface ISecurityAttributionRepository : IGenericRepository<Security_Attribution>
    {
        IQueryable<Security_Attribution> GetAll(Expression<Func<Security_Attribution, bool>> predicate);

    }
    public interface ISecurityPriceRepository : IGenericRepository<Security_Price>
    {
        IQueryable<Security_Price> GetAll(Expression<Func<Security_Price, bool>> predicate);
 
    }
    //
    public interface IOrderDetailRepository : IGenericRepository<Order_Detail>
    {
        IQueryable<Order_Detail> GetAll(Expression<Func<Order_Detail, bool>> predicate); 

    }
    public interface IClassificationRepository : IGenericRepository<Classification>
    {
        IQueryable<Classification> GetAll(Expression<Func<Classification, bool>> predicate);

    }
    public interface IClassificationIndustryRepository : IGenericRepository<Classification_Industry>
    {
        IQueryable<Classification_Industry> GetAll(Expression<Func<Classification_Industry, bool>> predicate);

    }
    public interface IOrderAllocationRepository : IGenericRepository<Order_Allocation>
    {
        IQueryable<Order_Allocation> GetAll(Expression<Func<Order_Allocation, bool>> predicate);
        new void DeleteBulk(decimal[] Ids);   

    }
    // Security Performance History
    public interface ISecurityPerformanceHistoryRepository : IGenericRepository<Security_Performance_History>
    {
        IEnumerable<Security_Performance_History> GetRecsTop100(Expression<Func<Security_Performance_History, bool>> predicate);
    }

    public interface ISecurityAnalyticsHistoryRepository : IGenericRepository<Security_Analytics_History>
    {
        IEnumerable<Security_Analytics_History> GetRecsTop100(Expression<Func<Security_Analytics_History, bool>> predicate); 

    }
     
    public interface ISecurityAttributionHistoryRepository : IGenericRepository<Security_Attribution_History>
    {
        IEnumerable<Security_Attribution_History> GetRecsTop100(Expression<Func<Security_Attribution_History, bool>> predicate); 

    }
    // Security Performance   
    public interface ISecurityPerformanceRepository : IGenericRepository<Security_Performance>
    {
        IQueryable<Security_Performance> GetAll(Expression<Func<Security_Performance, bool>> predicate);
    } // CashTransactions
    public interface ICashTransactionsRepository : IGenericRepository<Cash_Transactions>
    {
        IQueryable<Cash_Transactions> GetAll(Expression<Func<Cash_Transactions, bool>> predicate);
    }
    // CashTransactions
     
    public interface IExchangeRepository : IGenericRepository<Exchange>
    {
    }
    public interface ICountryRepository : IGenericRepository<Country>
    {
    }
    public interface ISecurityTypesRepository : IGenericRepository<Security_Type>
    { 
    }
    public interface ICurrencyPairRepository : IGenericRepository<Currency_Pair>
    {

    }
    public interface ICurrencyRepository : IGenericRepository<Currency>
    { 
    }
    public interface IUsersRepository : IGenericRepository<User>
    { 
    }
    public interface IPartyRepository : IGenericRepository<Party>
    {
        IQueryable<Party> GetPartyValues(decimal iEntity, string iType, decimal iGenericEntityId);
        IQueryable<Party> GetAllPartyValues(decimal iEntity,  decimal iGenericEntityId);
        Party Get(string PartyCode);
    }


    public interface IDebtExpiryProfileRepository : IGenericRepository<Debt_Expiry_Profile>
    {
        IQueryable<Debt_Expiry_Profile> GetAllDebtExpiryValues(decimal iEntity, decimal iGenericEntityId);

    }
    
    public interface IPartyFinancialsRepository : IGenericRepository<Party_Financials>
    {
        //IQueryable<Security_Detail> GetAll(Expression<Func<Party_Financials, bool>> predicate);`
        //IQueryable<Security_Detail> GetAllActive( ); 
    }
    public interface IPortfolioValuationRepository : IGenericRepository<Portfolio_Valuation>
    { 
    }
    public interface IPartyFinancialsHistoryRepository : IGenericRepository<Party_Financials_History>
    { 
    }
    public interface IPartyDebtAnalysisRepository : IGenericRepository<Party_Debt_Analysis>
    { 
    }
    public interface ISecurityListDetailRepository : IGenericRepository<Security_List_Detail>
    {
        void Add(decimal entityId, string securityListCode, string securityListName, string description, bool systemlocked,
                    List<decimal> securitiesList);
        void Edit(decimal entityId, string securityListCode, string securityListName, string description, bool systemlocked,
                    List<decimal> securitiesList);
        new void Delete(Security_List_Detail SecurityListDetail);   
 

    }
    public interface ISecurityListRepository : IGenericRepository<Security_List>
    {
    }
  
    public interface ICodeMiscellaneous : IGenericRepository<Code_Miscellaneous>
    {
    } 
}