using System;
using Microsoft.EntityFrameworkCore;
using CMS.Core.Entities;

namespace CMS.Storage
{
    public class CmsLogDbContext : DbContext
    {
        public CmsLogDbContext(DbContextOptions<CmsLogDbContext> options) : base(options)
        {

        }
 
        public DbSet<Logs> Logs { get; set; }
          
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CmsLogDbContext).Assembly);
        }
    }
}
