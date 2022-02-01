using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketLabelsConfiguration : IEntityTypeConfiguration<TicketLabels>
    {
        public void Configure(EntityTypeBuilder<TicketLabels> builder)
        {
            builder.ToTable("TicketLabels");
            builder.HasKey(r => new { r.TicketIdx, r.LabelIdx });
            builder.HasOne(r => r.Ticket).WithMany(rp => rp.TicketLabels).HasForeignKey(rp => rp.TicketIdx);
            builder.HasOne(p => p.Label).WithMany(rp => rp.TicketLabels).HasForeignKey(rp => rp.LabelIdx);
        }
    }
}
