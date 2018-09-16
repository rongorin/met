using MetopeMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data.Repositories
{
        public abstract class RepositoryBase<TContext> : IDisposable
        where TContext : DbContext, IDisposedTracker, new() //says the context passed in must inherit from DbContext and have a default constructor( new()  
 
    {
        private TContext _DataContext;

        protected virtual TContext DataContext
        {
            get
            {
                if (_DataContext == null || _DataContext.IsDisposed)
                {
                    _DataContext = new TContext();
                    //See http://msdn.microsoft.com/en-us/library/dd456853.aspx for details on this property and what it does
                    //Disable proxy creation to allow serialization and prevent 
                    //the "In order to serialize the parameter, add the type to the known types collection for the operation using ServiceKnownTypeAttribute" error
                   //.....  AllowSerialization = true;
                }
                return _DataContext;
            }
        }

        protected virtual IQueryable<T> GetList<T>() where T : class
        {
            try
            {
                return DataContext.Set<T>();
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual void  Save<T>(T entity) where T : class
        { 

            try
            {
                using (DataContext)
                {
                    //Custom attaching/adding of entity could be done here
                   DataContext.SaveChanges() ;
                }
            }
            catch (Exception exp)
            {
                //opStatus = OperationStatus.CreateFromException("Error saving " + typeof(T) + ".", exp);
            }
             
        }


        public virtual void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }
    } 
}