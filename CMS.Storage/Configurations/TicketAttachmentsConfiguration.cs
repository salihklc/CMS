using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketAttachmentsConfiguration : IEntityTypeConfiguration<TicketAttachments>
    {
        public void Configure(EntityTypeBuilder<TicketAttachments> builder)
        {
            builder.ToTable("TicketAttachments");
            builder.HasKey(r => r.Idx);
            builder.HasOne(t => t.Tickets).WithMany(a => a.TicketAttachments).HasForeignKey(t => t.TicketIdx);
            builder.HasOne(k => k.TicketComments).WithMany(l => l.TicketAttachments).HasForeignKey(m => m.CommentIdx).IsRequired(false);
        }
    }
}
