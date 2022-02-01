using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class MailTicketIntegrationConfiguration : IEntityTypeConfiguration<MailTicketIntegrations>
    {
        public void Configure(EntityTypeBuilder<MailTicketIntegrations> builder)
        {
            builder.ToTable("MailTicketIntegrations");
            builder.HasKey(r => r.Idx);
            
            //builder.HasOne(r => r.TicketId).WithOne()
            //builder.HasMany(r => r.TicketWorkItems).WithOne(k => k.Tickets).HasForeignKey(x => x.TicketIdx);


        }
    }
}
