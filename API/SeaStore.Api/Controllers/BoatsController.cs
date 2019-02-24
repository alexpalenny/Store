using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SeaStore.Services.Interfaces;
using SeaStore.Dto.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace SeaStore.Controllers
{
  [Route("api/[controller]")]
  public class BoatsController : Controller
  {
    private IBoatService _boatService { get; set; }

    public BoatsController(IBoatService boatService)
    {
      _boatService = boatService;
    }
    // GET api/values
    [HttpGet]
    [Route("Test")]
    public string Test()
    {
      return "Test 1011 " + DateTime.Now;
    }
    // GET api/values
    //[Authorize]
    [HttpGet]
    public IEnumerable<BoatDto> Get()
    {
      return _boatService.GetAllBoats();
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public BoatDto Get(int id)
    {
      return _boatService.GetBoatById(id);
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
