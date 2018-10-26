using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeaStore.Entities.Entities;

namespace SeaStore.Entities.Configuration
{
  public class BoatTypeConfiguration : IEntityTypeConfiguration<BoatType>
  {
    public void Configure(EntityTypeBuilder<BoatType> builder)
    {
      builder.ToTable("BoatTypes");
      builder.HasKey(cv => cv.Id);
    }
  }
}
