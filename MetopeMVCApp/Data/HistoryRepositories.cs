  
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
namespace MetopeMVCApp.Data
{
    // Here most the history repositories:

    public class HistoryRepositories
    { 
        public class SecurityAttributionHistoryRepository : GenericRepository<MetopeDbEntities, Security_Attribution_History>, ISecurityAttributionHistoryRepository
        {
            public IEnumerable<Security_Attribution_History> GetRecsTop100(Expression<Func<Security_Attribution_History, bool>> predicate)
            {
                IQueryable<Security_Attribution_History> query;
                query = Context.Set<Security_Attribution_History>().Include(r => r.Security_Detail).Where(predicate)
                                    .OrderByDescending(r => r.Record_Date).Take(100);
                return query.ToList();
            } 
        }
        public class SecurityPerformanceHistoryRepository : GenericRepository<MetopeDbEntities, Security_Performance_History>, ISecurityPerformanceHistoryRepository
        {
            public IEnumerable<Security_Performance_History> GetRecsTop100(Expression<Func<Security_Performance_History, bool>> predicate)
            {
                IQueryable<Security_Performance_History> query;
                query = Context.Set<Security_Performance_History>().Include(r => r.Security_Detail).Where(predicate)
                                    .OrderByDescending(r => r.Record_Date).Take(100); 
                return query.ToList();
            }
        } 
        public class SecurityAnalyticsHistoryRepository : GenericRepository<MetopeDbEntities, Security_Analytics_History>, ISecurityAnalyticsHistoryRepository
        {
            public IEnumerable<Security_Analytics_History> GetRecsTop100(Expression<Func<Security_Analytics_History, bool>> predicate)
            {

                IQueryable<Security_Analytics_History> query;
                query = Context.Set<Security_Analytics_History>().Include(r => r.Security_Detail).Where(predicate)
                                        .OrderByDescending(r => r.Record_Date).Take(100); 
                return query.ToList();
            } 
        }
    }
}