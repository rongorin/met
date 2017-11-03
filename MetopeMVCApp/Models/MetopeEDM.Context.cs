﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MetopeMVCApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MetopeDbEntities : DbContext
    {
        public MetopeDbEntities()
            : base("name=MetopeDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Entity> Entities { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Currency_Pair> Currency_Pair { get; set; }
        public virtual DbSet<Portfolio_List> Portfolio_List { get; set; }
        public virtual DbSet<Exchange> Exchanges { get; set; }
        public virtual DbSet<Security_Type> Security_Type { get; set; }
        public virtual DbSet<Code_Miscellaneous> Code_Miscellaneous { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<Security_Price> Security_Price { get; set; }
        public virtual DbSet<Security_Price_History> Security_Price_History { get; set; }
        public virtual DbSet<Party_Debt_Analysis> Party_Debt_Analysis { get; set; }
        public virtual DbSet<Debt_Expiry_Profile> Debt_Expiry_Profile { get; set; }
        public virtual DbSet<Security_Dividend_Detail> Security_Dividend_Detail { get; set; }
        public virtual DbSet<Party_Financials> Party_Financials { get; set; }
        public virtual DbSet<Party_Financials_History> Party_Financials_History { get; set; }
        public virtual DbSet<Security_Analytics> Security_Analytics { get; set; }
        public virtual DbSet<Portfolio_Valuation> Portfolio_Valuation { get; set; }
        public virtual DbSet<Portfolio_Valuation_History> Portfolio_Valuation_History { get; set; }
        public virtual DbSet<Security_Detail> Security_Detail { get; set; }
        public virtual DbSet<Security_Dividend_Split> Security_Dividend_Split { get; set; }
        public virtual DbSet<Security_List> Security_List { get; set; }
    
        public virtual int sp_TestRun(Nullable<decimal> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_TestRun", entityIDParameter);
        }
    
        public virtual int sp_TestRunNum2(Nullable<decimal> entityID, string aTextvalue)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            var aTextvalueParameter = aTextvalue != null ?
                new ObjectParameter("ATextvalue", aTextvalue) :
                new ObjectParameter("ATextvalue", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_TestRunNum2", entityIDParameter, aTextvalueParameter);
        }
    
        public virtual ObjectResult<string> sp_GenerateUpdateDividendSchedule(Nullable<decimal> entityID, Nullable<decimal> security, string securitiesList, string securityType)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            var securityParameter = security.HasValue ?
                new ObjectParameter("Security", security) :
                new ObjectParameter("Security", typeof(decimal));
    
            var securitiesListParameter = securitiesList != null ?
                new ObjectParameter("SecuritiesList", securitiesList) :
                new ObjectParameter("SecuritiesList", typeof(string));
    
            var securityTypeParameter = securityType != null ?
                new ObjectParameter("SecurityType", securityType) :
                new ObjectParameter("SecurityType", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_GenerateUpdateDividendSchedule", entityIDParameter, securityParameter, securitiesListParameter, securityTypeParameter);
        }
    
        public virtual ObjectResult<string> sp_TestRun1(Nullable<decimal> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("sp_TestRun1", entityIDParameter);
        }
    
        public virtual int sp_GenerateDividendSchedule(Nullable<decimal> entityID, Nullable<decimal> security, string securitiesList, string securityType)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            var securityParameter = security.HasValue ?
                new ObjectParameter("Security", security) :
                new ObjectParameter("Security", typeof(decimal));
    
            var securitiesListParameter = securitiesList != null ?
                new ObjectParameter("SecuritiesList", securitiesList) :
                new ObjectParameter("SecuritiesList", typeof(string));
    
            var securityTypeParameter = securityType != null ?
                new ObjectParameter("SecurityType", securityType) :
                new ObjectParameter("SecurityType", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_GenerateDividendSchedule", entityIDParameter, securityParameter, securitiesListParameter, securityTypeParameter);
        }
    
        public virtual int sp_GenerateUpdateDividendSchedule1(Nullable<decimal> entityID, Nullable<decimal> security, string securitiesList, string securityType, string userName)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            var securityParameter = security.HasValue ?
                new ObjectParameter("Security", security) :
                new ObjectParameter("Security", typeof(decimal));
    
            var securitiesListParameter = securitiesList != null ?
                new ObjectParameter("SecuritiesList", securitiesList) :
                new ObjectParameter("SecuritiesList", typeof(string));
    
            var securityTypeParameter = securityType != null ?
                new ObjectParameter("SecurityType", securityType) :
                new ObjectParameter("SecurityType", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_GenerateUpdateDividendSchedule1", entityIDParameter, securityParameter, securitiesListParameter, securityTypeParameter, userNameParameter);
        }
    
        public virtual int sp_BatchSet(Nullable<decimal> entityID, Nullable<System.DateTime> effectiveDate, Nullable<decimal> sessionID, string vfListcode, string userName, Nullable<bool> fromFrontend)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            var effectiveDateParameter = effectiveDate.HasValue ?
                new ObjectParameter("EffectiveDate", effectiveDate) :
                new ObjectParameter("EffectiveDate", typeof(System.DateTime));
    
            var sessionIDParameter = sessionID.HasValue ?
                new ObjectParameter("SessionID", sessionID) :
                new ObjectParameter("SessionID", typeof(decimal));
    
            var vfListcodeParameter = vfListcode != null ?
                new ObjectParameter("VfListcode", vfListcode) :
                new ObjectParameter("VfListcode", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var fromFrontendParameter = fromFrontend.HasValue ?
                new ObjectParameter("FromFrontend", fromFrontend) :
                new ObjectParameter("FromFrontend", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_BatchSet", entityIDParameter, effectiveDateParameter, sessionIDParameter, vfListcodeParameter, userNameParameter, fromFrontendParameter);
        }
    
        public virtual int sp_GenerateUpdateDividendSchedule2(Nullable<decimal> entityID, Nullable<decimal> security, string securitiesList, string securityType, string userName)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(decimal));
    
            var securityParameter = security.HasValue ?
                new ObjectParameter("Security", security) :
                new ObjectParameter("Security", typeof(decimal));
    
            var securitiesListParameter = securitiesList != null ?
                new ObjectParameter("SecuritiesList", securitiesList) :
                new ObjectParameter("SecuritiesList", typeof(string));
    
            var securityTypeParameter = securityType != null ?
                new ObjectParameter("SecurityType", securityType) :
                new ObjectParameter("SecurityType", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_GenerateUpdateDividendSchedule2", entityIDParameter, securityParameter, securitiesListParameter, securityTypeParameter, userNameParameter);
        }
    }
}
