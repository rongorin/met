using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Metope.DAL;

namespace Metope.WebAPI.Controllers
{
    public class SecurityDividendSplitController : ApiController
    {
        private MetopeDbEntities db = new MetopeDbEntities();

        // GET: api/SecurityDividendSplit 
        public IHttpActionResult GetSecurity_Dividend_Split(decimal entityID, decimal securityID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //decimal EntityId = Convert.ToDecimal(entityID);
            IEnumerable<Security_Dividend_Split> pvs = db.Security_Dividend_Split.Include(c => c.Security_Detail)
                                                    .Where(c => c.Entity_ID == entityID &&
                                                            c.Security_ID == securityID)
                                                     .OrderBy(s => s.Security_ID).ThenBy(n => n.Dividend_Annual_Number)
                                                    .ToList(); 
            return Ok(pvs); 
        }
         

        // GET: api/SecurityDividendSplit/5
        //[HttpGet, Route("GetByID/{entityID},{securityID},{dividendAnnNumber}")] 
        public IHttpActionResult GetSecurity_Dividend_Split(decimal entityID, decimal securityID , decimal dividendAnnNumber)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Security_Dividend_Split security_Dividend_Split = db.Security_Dividend_Split.Include(c => c.Security_Detail).ToList().
                                                            SingleOrDefault(x => x.Dividend_Annual_Number == dividendAnnNumber
                                                                                    && x.Entity_ID == entityID
                                                                                    && x.Security_ID == securityID);

            //this is less efficeint:
            //Security_Dividend_Split security_Dividend_Split = db.Security_Dividend_Split.Include(c => c.Security_Detail).ToList()
            //                        .Find(x => x.Dividend_Annual_Number == dividendAnnNumber && x.Security_ID == securityID && x.Entity_ID == entityID);
                             
            if (security_Dividend_Split == null)
            {
                return NotFound();
            }

            return Ok(security_Dividend_Split);
        }

        // PUT: api/SecurityDividendSplit/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSecurity_Dividend_Split(decimal entityID, decimal securityID, decimal dividendAnnNumber, 
                                                            Security_Dividend_Split security_Dividend_Split)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (securityID != security_Dividend_Split.Security_ID || 
                dividendAnnNumber != security_Dividend_Split.Dividend_Annual_Number)
            {
                return BadRequest();
            }

            db.Entry(security_Dividend_Split).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Security_Dividend_SplitExists( entityID,   securityID ,   dividendAnnNumber))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SecurityDividendSplit
        [ResponseType(typeof(Security_Dividend_Split))]
        public IHttpActionResult PostSecurity_Dividend_Split(Security_Dividend_Split security_Dividend_Split)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Security_Dividend_Split.Add(security_Dividend_Split);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Security_Dividend_SplitExists(security_Dividend_Split.Entity_ID,
                                                    security_Dividend_Split.Security_ID,
                                                    security_Dividend_Split.Dividend_Annual_Number))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = security_Dividend_Split.Dividend_Annual_Number }, security_Dividend_Split);
        }

        // DELETE: api/SecurityDividendSplit/5
        [ResponseType(typeof(Security_Dividend_Split))]
        public IHttpActionResult DeleteSecurity_Dividend_Split(decimal entityID, decimal securityID, decimal dividendAnnNumber)
        {
            Security_Dividend_Split security_Dividend_Split = db.Security_Dividend_Split.Include(c => c.Security_Detail).
                                                                           SingleOrDefault(x => x.Dividend_Annual_Number == dividendAnnNumber
                                                                               && x.Entity_ID == entityID
                                                                               && x.Security_ID == securityID);
             
            if (security_Dividend_Split == null)
            {
                return NotFound();
            }

            db.Security_Dividend_Split.Remove(security_Dividend_Split);
            db.SaveChanges();

            return Ok(security_Dividend_Split);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Security_Dividend_SplitExists(decimal  entityID, decimal securityID , decimal dividendAnnNumber)
        {
            return db.Security_Dividend_Split.Count(e => e.Entity_ID == entityID ||
                               e.Security_ID == securityID ||
                               e.Dividend_Annual_Number == dividendAnnNumber) > 0;
        }
    }
}