using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class MailTicketWatchHistoryConfiguration : IEntityTypeConfiguration<MailTicketWatchHistory>
    {
        public void Configure(EntityTypeBuilder<MailTicketWatchHistory> builder)
        {
            builder.ToTable("MailTicketWatchHistory");
            builder.HasKey(r => r.Idx);
        }
    }
}
