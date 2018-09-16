using Metope.DAL;
using MetopeMVCApp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MetopeMVCApp.Data.Persist
{
    public class UnitOfWork : IDisposable
    {
        private readonly MetopeDbEntities _context;

        public ISecurityAttributionRepository secAttrib;
        public ISecurityDetailRepository secDetails;
        public UnitOfWork(MetopeDbEntities cntx)
        {
            _context = cntx;
            secAttrib = new SecurityAttributionRepository();
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();

        }

    }
}