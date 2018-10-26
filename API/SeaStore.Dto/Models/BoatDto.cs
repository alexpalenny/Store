namespace SeaStore.Dto.Models
{
  public class BoatDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int TypeId { get; set; }
    public string BoatType { get; set; }
    public int? Capacity { get; set; }
    public decimal? Length { get; set; }
    public decimal? Beam { get; set; }
    public string Descritption { get; set; }
    public decimal? Price { get; set; }
    public int? PayTypeId { get; set; }
    public string PayType { get; set; }
    public decimal? MinOrder { get; set; }
    public bool Default { get; set; }
  }
}
