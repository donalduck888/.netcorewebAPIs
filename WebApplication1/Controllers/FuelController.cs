using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FuelController : ControllerBase
    {
        private readonly AppDbContext _authContext;

        public FuelController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;

        }

        [HttpPost("add")]
        public async Task<IActionResult> RegisterFuel([FromBody] Fuel fuel)
        {

            if (fuel == null)
                return BadRequest(); 

            await _authContext.fuel.AddAsync(fuel);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Fuel Registered" 
            });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuel(int id, [FromBody] Fuel fuel)
        {
            if (fuel == null || id != fuel.id)
            {
                return BadRequest();
            }

            var existingFuel = await _authContext.fuel.FindAsync(id);
            if (existingFuel == null)
            {
                return NotFound(); 
            }

            existingFuel.fuelstationname = fuel.fuelstationname;
            existingFuel.district = fuel.district;
            existingFuel.addresse = fuel.addresse;
            existingFuel.fuelamt = fuel.fuelamt;
            existingFuel.email = fuel.email;
            existingFuel.PhoneNumber = fuel.PhoneNumber;  

            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Fuel Station Updated"  
            });
        }

        [HttpGet]

        public async Task<ActionResult<Fuel>> GetAllFuels() 
        {

            return Ok(await _authContext.fuel.ToListAsync()); 


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fuel>> GetFuelById(int id)
        {
            var fuel = await _authContext.fuel.FindAsync(id);

            if (fuel == null)
            {
                return NotFound();
            }

            return Ok(fuel); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuel(int id) 
        {
            var existingVehicle = await _authContext.fuel.FindAsync(id);
            if (existingVehicle == null)
            {
                return NotFound();
            }

            _authContext.fuel.Remove(existingVehicle);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Fuel Deleted" 
            });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetVehicleCount()
        {
            var count = await _authContext.fuel.CountAsync();
            return Ok(new
            {
                Count = count
            });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Fuel>>> SearchVehiclesByTitle(string fuelstationname)
        {
            var tutorials = await _authContext.fuel
                .Where(t => t.fuelstationname.Contains(fuelstationname))
                .ToListAsync();

            if (tutorials.Count == 0)
            {
                return NotFound();
            }

            return Ok(tutorials);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllVehicles()
        {
            var allfuel = await _authContext.fuel.ToListAsync();
            if (allfuel.Count == 0)
            {
                return NotFound();
            }

            foreach (var vehi in allfuel)
            {
                _authContext.fuel.Remove(vehi);
            }

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "All Fuels Deleted" 
            });
        }

    }


}

