using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketCommentsConfiguration : IEntityTypeConfiguration<TicketComments>
    {
        public void Configure(EntityTypeBuilder<TicketComments> builder)
        {
            builder.ToTable("TicketComments");
            builder.HasKey(r => r.Idx);
            builder.HasOne(r => r.Tickets).WithMany(r => r.TicketComments).HasForeignKey(k => k.TicketIdx);
            builder.HasOne(r => r.User).WithMany(r => r.UserTicketComments).HasForeignKey(k => k.UserIdx);
        }
    }
}
