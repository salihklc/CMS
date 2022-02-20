using System;
using Microsoft.EntityFrameworkCore;
using CMS.Core.Entities;

namespace CMS.Storage
{
    public class CmsDbContext : DbContext
    {
        public CmsDbContext(DbContextOptions<CmsDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Districts> Districts { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserGroups> UserGroups { get; set; }
        public DbSet<UserHistories> UserHistories { get; set; }
        public DbSet<Pages> Pages { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketStatusCategories> TicketStatusCategories { get; set; }
        public DbSet<TicketLogTimes> TicketLogTimes { get; set; }
        public DbSet<TicketLabels> TicketLabels { get; set; }
        public DbSet<TicketWatchers> TicketWatchers { get; set; }
        public DbSet<TicketWorkItems> TicketWorkItems { get; set; }
        public DbSet<TicketRelatedTickets> TicketRelatedTickets { get; set; }
        public DbSet<TicketAttachments> TicketAttachments { get; set; }
        public DbSet<TicketComments> TicketComments { get; set; }
        public DbSet<TicketHistories> TicketHistories { get; set; }
        public DbSet<TicketTypes> TicketTypes { get; set; }
        public DbSet<Firms> Firms { get; set; }
        public DbSet<MailTicketWatchHistory> MailTicketWatchHistories { get; set; }
        public DbSet<MailTicketIntegrations> MailTicketIntegrations { get; set; }
        public DbSet<ProductFields> ProductFields { get; set; }
        public DbSet<Fields> Fields { get; set; }
        public DbSet<FieldTypes> FieldTypes { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Priorities> Priorities { get; set; }
        public DbSet<FirmProducts> FirmProducts { get; set; }
        public DbSet<FirmProductFields> FirmProductFields { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CmsDbContext).Assembly);
        }
    }
}
