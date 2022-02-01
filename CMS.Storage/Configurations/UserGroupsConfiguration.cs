using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class UserGroupsConfiguration : IEntityTypeConfiguration<UserGroups>
    {
        public void Configure(EntityTypeBuilder<UserGroups> builder)
        {
            builder.ToTable("UserGroups");
            builder.HasKey(r => r.Idx);
        }
    }
}
