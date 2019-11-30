using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Weather.Core;
using Weather.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weather.Api
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly WeatherDbContext _weatherDbContext;
        private readonly ILogger _logger;

        public CitiesController(WeatherDbContext weatherDbContext, ILogger<CitiesController> logger)
        {
            _weatherDbContext = weatherDbContext;
            _logger = logger;
        }
        // GET: api/cities
        [HttpGet]
        public IEnumerable<City> Get()
        { 
            return _weatherDbContext.Cities;
        }

        // GET api/cities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = await _weatherDbContext.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // POST api/cities
        [HttpPost]
        public async Task<IActionResult> PostCity([FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _weatherDbContext.Cities.Add(city);
            city.Id = 0;
            await _weatherDbContext.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE api/cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = await _weatherDbContext.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _weatherDbContext.Cities.Remove(city);
            await _weatherDbContext.SaveChangesAsync();

            _logger.LogInformation(message: "{CityName} with temperature {Temperature} deleted from database", city.Name, city.Temperature);
            return Ok(city);
        }
    }
}
