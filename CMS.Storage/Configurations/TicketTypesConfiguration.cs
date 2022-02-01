using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketTypesConfiguration : IEntityTypeConfiguration<TicketTypes>
    {
        public void Configure(EntityTypeBuilder<TicketTypes> builder)
        {
            builder.ToTable("TicketTypes");
            builder.HasKey(r => r.Idx);
            builder.HasMany(k => k.Tickets).WithOne(l => l.TicketTypes).HasForeignKey(m => m.TypeIdx);
        }
    }
}
