using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class RolePermissionsConfiguration : IEntityTypeConfiguration<RolePermissions>
    {
        public void Configure(EntityTypeBuilder<RolePermissions> builder)
        {
            builder.ToTable("RolePermissions");
            builder.HasKey(r =>new { r.RoleIdx, r.PermissionNo });
            builder.HasOne(r => r.Roles).WithMany(rp => rp.RolePermissions).HasForeignKey(rp => rp.RoleIdx);
            builder.HasOne(p => p.Permissions).WithMany(rp => rp.RolePermissions).HasForeignKey(rp => rp.PermissionNo);
        }
    }
}
