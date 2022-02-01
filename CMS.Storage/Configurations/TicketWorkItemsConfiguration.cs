using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketWorkItemsConfiguration : IEntityTypeConfiguration<TicketWorkItems>
    {
        public void Configure(EntityTypeBuilder<TicketWorkItems> builder)
        {
            builder.ToTable("TicketWorkItems");
            builder.HasKey(r => r.Idx);
        }
    }
}
