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
    using MetopeMVCApp.Data.Repositories;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Linq;
    
    //public interface IMetopeDbEntities : IDisposable
    //{
    //     IQueryable<T> Query<T>() where T : class;
    //      DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    //      T Add<T>(T entity) where T : class;
    //      int SaveChanges();

    //}
    public partial class MetopeDbEntities : DbContext, IDisposedTracker  //, IMetopeDbEntities 
    {
        public MetopeDbEntities() : base("name=MetopeDbEntities")
        { 
            #if DEBUG
                Database.Log = s => Debug.Write(s);
                //Database.Log = message => Trace.WriteLine(message);
             #endif
        }
        //IQueryable<T> IMetopeDbEntities.Query<T>()
        //{
        //    return Set<T>();
        //}
        //DbEntityEntry<TEntity> IMetopeDbEntities.Entry<TEntity>(TEntity entity)
        //{ 
        //    return Entry<TEntity>(entity) ; 
        //} 
        //T  IMetopeDbEntities.Add<T>( T entity)
        //{
        //    return Set<T>().Add(entity); 
        //}
        //int IMetopeDbEntities.SaveChanges()
        //{

        //    return SaveChanges();
        //} 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Security_Detail>().HasOptional(t => t.Code_Miscellaneous ).WithRequired(t => t.)  Map(p => p.Requires("Type").HasValue("Car"));
             
            throw new UnintentionalCodeFirstException();

        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
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
        public bool IsDisposed { get; set; }

      
    
    }
}
