using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketRelatedTicketsConfiguration : IEntityTypeConfiguration<TicketRelatedTickets>
    {
        public void Configure(EntityTypeBuilder<TicketRelatedTickets> builder)
        {
            builder.ToTable("TicketRelatedTickets");
            builder.HasKey(r => r.Idx);           
        }
    }
}
