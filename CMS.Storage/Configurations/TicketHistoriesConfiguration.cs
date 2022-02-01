using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketHistoriesConfiguration : IEntityTypeConfiguration<TicketHistories>
    {
        public void Configure(EntityTypeBuilder<TicketHistories> builder)
        {
            builder.ToTable("TicketHistories");
            builder.HasKey(r => r.Idx);
            builder.HasOne(r => r.Tickets).WithMany(r => r.TicketHistories).HasForeignKey(k => k.TicketIdx);
        }
    }
}
