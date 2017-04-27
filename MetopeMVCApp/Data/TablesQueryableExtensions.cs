using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data
{
    public static class TablesQueryableExtensions
    {
        public static IQueryable<Portfolio> SearchPortfName(this IQueryable<Portfolio> port, string searchname)
        {
            return port.Where(r => searchname == null || r.Portfolio_Name.Contains(searchname)); 
        }
 
        public static IQueryable<Security_Detail> SearchSecName(this IQueryable<Security_Detail> port, string searchname)
        {
            return port.Where(r => searchname == null || r.Security_Name.Contains(searchname)); 
        }

        public static IQueryable<T> MatchEntityID<T>(this IQueryable<T> qry, System.Linq.Expressions.Expression<Func<T, bool>> predic )
        /*  This ensures selecting only of records where the EntityId matches the 
            user's EntityInScope, or it is the generic Entity. */
        {
            return qry.Where(predic);
        }

        public static IQueryable<Security_Price> SearchPrices(this IQueryable<Security_Price> prices,
                                                               int? SecurityId,
                                                               string iPriceCurr = "")
        {
            var query1 = prices  
                       .Where(c => (SecurityId != null) ? c.Security_ID == SecurityId : c.Security_ID > 0);

            // -- removed as rather show ALL the PriceCurrency records for the selected Security
            //var filteredQuery = query1.
            //                    Where(c => (iPriceCurr != "") ? c.Price_Curr == iPriceCurr : c.Price_Curr != "");

            return query1;
             
        }
         //--some other ones that can maybe use: -------------------

        //public static IQueryable<Person> InRegion(this IQueryable<Person> people, string region)
        //{
        //    return people.Where(p => p.Addresses.Any(a => a.Region == region));
        //}

        //public static IOrderedQueryable<Person> OrderByName(this IQueryable<Person> people)
        //{
        //    return people.OrderBy(x => x.Name);
        //}
 
   


    }
}