using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class UserHistoriesConfiguration : IEntityTypeConfiguration<UserHistories>
    {
        public void Configure(EntityTypeBuilder<UserHistories> builder)
        {
            builder.ToTable("UserHistories");
            builder.HasKey(r => r.Idx);
        }
    }
}
