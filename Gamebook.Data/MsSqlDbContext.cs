using Microsoft.AspNet.Identity.EntityFramework;
using Gamebook.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Gamebook.Data.Model.Contracts;

namespace Gamebook.Data
{
    public class MsSqlDbContext : IdentityDbContext<User>
    {
        public MsSqlDbContext()
            : base("MsSqlServerConnection", throwIfV1Schema: false)
        {

        }

        public IDbSet<Book> Books { get; set; }
        public IDbSet<Page> Pages { get; set; }
        public IDbSet<PageConnection> PageConnections { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            foreach(var entry in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable
                    && ((e.State == EntityState.Added) 
                        || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditable)entry.Entity;
                if (entry.State == EntityState.Added
                    && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        public static MsSqlDbContext Create()
        {
            return new MsSqlDbContext();
        }
    }
}
