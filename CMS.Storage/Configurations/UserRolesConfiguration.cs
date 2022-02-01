using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasKey(r => new { r.RoleIdx, r.UserIdx});
            builder.HasOne(u => u.User).WithMany(ur =>ur.UserRoles).HasForeignKey(ur => ur.UserIdx);
            builder.HasOne(r => r.Roles).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.RoleIdx);
        }
    }
}
