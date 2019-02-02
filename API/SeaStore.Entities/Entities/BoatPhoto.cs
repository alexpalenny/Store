namespace SeaStore.Entities.Entities
{
  public class BoatPhoto
  {
    public int Id { get; set; }
    public int BoatId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }

    public virtual Boat Boat { get; set; }
  }
}
