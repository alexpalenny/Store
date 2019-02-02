using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeaStore.Entities.Entities;

namespace SeaStore.Entities.Configuration
{
  public class BoatPhotoConfiguration : IEntityTypeConfiguration<BoatPhoto>
  {
    public void Configure(EntityTypeBuilder<BoatPhoto> builder)
    {
      builder.ToTable("BoatPhotoes");
      builder.HasKey(cv => cv.Id);
      builder.HasOne(cv => cv.Boat).WithMany(cv => cv.Photoes).HasForeignKey(cv => cv.BoatId);
    }
  }
}
