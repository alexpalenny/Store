using SeaStore.Entities.Enum;
using System.Collections.Generic;

namespace SeaStore.Entities.Entities
{
  public class Boat
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public BoatTypeEnum TypeId { get; set; }
    public BoatType BoatType { get; set; }
    public int? Capacity { get; set; }
    public decimal? Length { get; set; }
    public decimal? Beam { get; set; }
    public string Descritption { get; set; }
    public decimal? Price { get; set; }
    public PayTypeEnum? PayTypeId { get; set; }
    public PayType PayType { get; set; }
    public decimal? MinOrder { get; set; }
    public bool Default { get; set; }

    public virtual IEnumerable<BoatPhoto> Photoes { get; set; }
  }
}
