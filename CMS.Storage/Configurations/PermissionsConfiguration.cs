using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class PermissionsConfiguration : IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(e => e.PermissionNo);
            builder.HasOne(r => r.Page).WithMany(k => k.Permissions).HasForeignKey(l => l.PageIdx);
        }
    }
}
