using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketLogTimesConfiguration : IEntityTypeConfiguration<TicketLogTimes>
    {
        public void Configure(EntityTypeBuilder<TicketLogTimes> builder)
        {
            builder.ToTable("TicketLogTimes");
            builder.HasKey(r => r.Idx);
            builder.HasOne(l => l.WorkingTypes).WithMany(w => w.TicketLogTimes).HasForeignKey(k => k.WorkingTypeIdx);
            builder.HasOne(l => l.User).WithMany(w => w.TicketLogTimes).HasForeignKey(k => k.UserIdx);
        }
    }
}
