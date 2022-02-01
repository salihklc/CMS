using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketStatusConfiguration : IEntityTypeConfiguration<TicketStatus>
    {
        public void Configure(EntityTypeBuilder<TicketStatus> builder)
        {
            builder.ToTable("TicketStatus");
            builder.HasKey(r => r.Idx);
            builder.HasOne(s => s.TicketStatusCategories).WithMany(sc => sc.TicketStatuses).HasForeignKey(r => r.CategoryIdx);
            builder.HasMany(r => r.Tickets).WithOne(k => k.TicketStatus).HasForeignKey(x => x.Status);
        }
    }
}
