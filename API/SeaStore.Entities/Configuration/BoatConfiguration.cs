using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeaStore.Entities.Entities;

namespace SeaStore.Entities.Configuration
{
  public class BoatConfiguration : IEntityTypeConfiguration<Boat>
  {
    public void Configure(EntityTypeBuilder<Boat> builder)
    {
      builder.ToTable("Boats");
      builder.HasKey(cv => cv.Id);
      builder.HasOne(cv => cv.BoatType).WithMany().HasForeignKey(cv => cv.TypeId);
      builder.HasOne(cv => cv.PayType).WithMany().HasForeignKey(cv => cv.PayTypeId);
    }
  }
}
