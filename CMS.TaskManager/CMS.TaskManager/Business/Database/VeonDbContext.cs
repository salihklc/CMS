using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.TaskManager.Business.Database.EntityModels;

namespace CMS.TaskManager.Business.Database
{
    public class CmsDbContext : DbContext
    {
        public CmsDbContext(DbContextOptions<CmsDbContext> options) : base(options)
        {

        }
        public DbSet<TaskJobs> Jobs { get; set; }
        public DbSet<TaskQueue> Queues { get; set; }
        public DbSet<TaskWorkers> Workers { get; set; }
        public DbSet<TaskWorkersHistories> WorkersHistories { get; set; }
        public DbSet<TaskManagerSettingsModel> TaskManagerSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CmsDbContext).Assembly);

        }
    }
}
