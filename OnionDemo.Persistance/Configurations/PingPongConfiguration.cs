using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionDemo.Domain.Entities;

namespace OnionDemo.Persistance.Configurations
{
    public sealed class PingPongConfiguration : IEntityTypeConfiguration<PingPong>
    {
        public void Configure(EntityTypeBuilder<PingPong> builder)
        {
            builder.HasKey(ping => ping.Id);

            builder.Property(ping => ping.Name).HasMaxLength(100);

            builder.Property(ping => ping.CreatedDate).IsRequired();
        }
    }
}
