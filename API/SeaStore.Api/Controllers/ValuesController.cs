using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeaStore.Services.Interfaces;
using SeaStore.Dto.Models;

namespace SeaStore.Controllers
{
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    private IBoatService _boatService { get; set; }
    public ValuesController(IBoatService boatService)
    {
      _boatService = boatService;
    }
    // GET api/values
    [HttpGet]
    public IEnumerable<BoatDto> Get()
    {
      return _boatService.GetAllBoats();
      //return new string[] { "value1", "value2", "value3" };
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
