using SeaStore.Dto.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaStore.Services.Interfaces
{
  public interface IBoatService
  {
    IEnumerable<BoatDto> GetAllBoats();
    BoatDto GetBoatById(int id);    
  }
}
