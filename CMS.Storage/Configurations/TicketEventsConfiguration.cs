using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketEventsConfiguration : IEntityTypeConfiguration<TicketEvents>
    {
        public void Configure(EntityTypeBuilder<TicketEvents> builder)
        {
            builder.ToTable("TicketEvents");
            builder.HasKey(r => r.Idx);
            builder.HasOne(r => r.Tickets).WithMany(r => r.TicketEvents).HasForeignKey(k => k.TicketIdx);
        }
    }
}
