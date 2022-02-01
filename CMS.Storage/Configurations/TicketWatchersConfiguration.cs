using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketWatchersConfiguration : IEntityTypeConfiguration<TicketWatchers>
    {
        public void Configure(EntityTypeBuilder<TicketWatchers> builder)
        {
            builder.ToTable("TicketWatchers");
            builder.HasKey(r => new { r.TicketIdx, r.UserIdx });
            builder.HasOne(r => r.Tickets).WithMany(rp => rp.TicketWatchers).HasForeignKey(rp => rp.TicketIdx);
            builder.HasOne(p => p.Users).WithMany(rp => rp.TicketWatchers).HasForeignKey(rp => rp.UserIdx);
        }
    }
}
