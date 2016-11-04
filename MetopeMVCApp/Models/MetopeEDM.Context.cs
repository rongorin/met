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
    using System.Diagnostics;
    
    public partial class MetopeDbEntities : DbContext
    {
        public MetopeDbEntities()
            : base("name=MetopeDbEntities")
        {
            #if DEBUG
                Database.Log = s => Debug.Write(s);
                //Database.Log = message => Trace.WriteLine(message);
             #endif
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
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<Security_Detail> Security_Detail { get; set; }
    }
}
