using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeaStore.Entities.Entities;

namespace SeaStore.Entities.Configuration
{
  public class PayTypeConfiguration : IEntityTypeConfiguration<PayType>
  {
    public void Configure(EntityTypeBuilder<PayType> builder)
    {
      builder.ToTable("PayTypes");
      builder.HasKey(cv => cv.Id);
    }
  }
}
