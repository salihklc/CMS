using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketsConfiguration : IEntityTypeConfiguration<Tickets>
    {
        public void Configure(EntityTypeBuilder<Tickets> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(r => r.Idx);
            builder.HasMany(r => r.TicketComments).WithOne(k => k.Tickets).HasForeignKey(x=> x.TicketIdx);
            builder.HasMany(r => r.TicketAttachments).WithOne(k => k.Tickets).HasForeignKey(x => x.TicketIdx);
            builder.HasMany(r => r.TicketHistories).WithOne(k => k.Tickets).HasForeignKey(x => x.TicketIdx);
            builder.HasMany(r => r.TicketLogTimes).WithOne(k => k.Tickets).HasForeignKey(x => x.TicketIdx);
            builder.HasMany(r => r.TicketRelatedTickets).WithOne(k => k.Ticket).HasForeignKey(x => x.TicketIdx);
            builder.HasMany(r => r.TicketWatchers).WithOne(k => k.Tickets).HasForeignKey(x => x.TicketIdx);
            builder.HasMany(r => r.TicketWorkItems).WithOne(k => k.Tickets).HasForeignKey(x => x.TicketIdx);
            builder.HasOne(r => r.FirmUser).WithMany(k => k.Tickets).HasForeignKey(x => x.FirmUserIdx);

        }
    }
}
