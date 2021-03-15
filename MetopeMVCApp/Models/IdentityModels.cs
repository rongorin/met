using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Diagnostics;
namespace ASP.MetopeNspace.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public decimal EntityIdScope  { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public bool? IsEnabled { get; set; }
    } 

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MetopeSSFidentit")
        {

            Database.Log = sql => Debug.Write(sql);
        }
    }
}