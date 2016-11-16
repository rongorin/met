using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data
{
    public static class PortfolioQueryableExtensions
    {
        public static IQueryable<Portfolio> SearchPortfName(this IQueryable<Portfolio> port, string searchname)
        {
            return port.Where(r => searchname == null || r.Portfolio_Name.Contains(searchname));
           
        } 
        public static IQueryable<Security_Detail> SearchSecName(this IQueryable<Security_Detail> port, string searchname)
        {
            return port.Where(r => searchname == null || r.Security_Name.Contains(searchname));
           
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