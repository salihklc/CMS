using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class TicketStatusCategoriesConfiguration : IEntityTypeConfiguration<TicketStatusCategories>
    {
        public void Configure(EntityTypeBuilder<TicketStatusCategories> builder)
        {
            builder.ToTable("TicketStatusCategories");
            builder.HasKey(r => r.Idx);
        }
    }
}
