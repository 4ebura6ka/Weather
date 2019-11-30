using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weather.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weather.Api
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class TemperatureController : Controller
    {
        private readonly WeatherDbContext _weatherDbContext;
        private readonly ILogger<TemperatureController> _logger;

        public TemperatureController(WeatherDbContext weatherDbContext, ILogger<TemperatureController> logger, SignInManager<IdentityUser> signInManager)
        {
            _weatherDbContext = weatherDbContext;
            _logger = logger;

        }

        // GET api/values/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemperature(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = await _weatherDbContext.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound("City not found");
            }

            return Ok(city.Temperature);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemperature(int id, [FromBody]double value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = await _weatherDbContext.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound("City not found");
            }

            var previousTemperature = city.Temperature;

            city.Temperature = value;
            _weatherDbContext.Entry(city).State = EntityState.Modified;

            try
            {
                await _weatherDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            _logger.LogInformation(message: "{CityName} with temperature {previousTemperature} is set to {value} temperature",
                city.Name, previousTemperature, value);

            return Ok(city);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemperature(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = await _weatherDbContext.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound("City not found");
            }

            var previousTemperature = city.Temperature;
            city.Temperature = null;

            _weatherDbContext.Entry(city).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _weatherDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogInformation(message: "{CityName} with temperature {previousTemperature} was not delted. There were" +
                    "some issues with saving in database", city.Name, previousTemperature);
                return NotFound();
            }

            _logger.LogInformation(message: "{CityName} with temperature {previousTemperature} was deleted",
            city.Name, previousTemperature);

            return Ok(city);
        }
    }
}
