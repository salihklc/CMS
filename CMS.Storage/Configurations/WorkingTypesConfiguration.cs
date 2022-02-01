using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class WorkingTypesConfiguration : IEntityTypeConfiguration<WorkingTypes>
    {
        public void Configure(EntityTypeBuilder<WorkingTypes> builder)
        {
            builder.ToTable("WorkingTypes");
            builder.HasKey(r => r.Idx);

        }
    }
}
