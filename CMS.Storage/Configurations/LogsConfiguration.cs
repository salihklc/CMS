using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CMS.Core.Entities;

namespace CMS.Storage.Configurations
{
    public class LogsConfiguration : IEntityTypeConfiguration<Logs>
    {
        public void Configure(EntityTypeBuilder<Logs> builder)
        {
            builder.ToTable("Logs");
            builder.HasKey(e => e.Idx);

        }
    }
}
