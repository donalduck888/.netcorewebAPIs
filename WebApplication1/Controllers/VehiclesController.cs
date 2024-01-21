using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly AppDbContext _authContext; 

        public VehiclesController(AppDbContext appDbContext)
        {
            _authContext = appDbContext; 

        }

        [HttpPost("add")]
        public async Task<IActionResult> RegisterVehicle([FromBody] Vehicles vehicles)
        {

            if (vehicles == null)
                return BadRequest(); 

            await _authContext.Vehicles.AddAsync(vehicles);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Vehicle Registered"
            });

        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] Vehicles vehicles)
        {
            if (vehicles == null || id != vehicles.id)
            {
                return BadRequest();
            }

            var existingVehicle = await _authContext.Vehicles.FindAsync(id); 
            if (existingVehicle == null)
            {
                return NotFound();
            }

            existingVehicle.VehicleType = vehicles.VehicleType;
            existingVehicle.Nic = vehicles.Nic;
            existingVehicle.CNo = vehicles.CNo;
            existingVehicle.FuelType = vehicles.FuelType;
            existingVehicle.Email = vehicles.Email;
            existingVehicle.PhoneNumber = vehicles.PhoneNumber;
            existingVehicle.IsuAmount = vehicles.IsuAmount;
            existingVehicle.UserName = vehicles.UserName; 

            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Vehicle Updated"
            });
        }

        [HttpGet]

        public async Task<ActionResult<Vehicles>> GetAllVehicles()
        {

            return Ok(await _authContext.Vehicles.ToListAsync()); 


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicles>> GetVehicleById(int id)
        {
            var vehicle = await _authContext.Vehicles.FindAsync(id);  

            if (vehicle == null)
            { 
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var existingVehicle = await _authContext.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                return NotFound(); 
            }

            _authContext.Vehicles.Remove(existingVehicle); 
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Vehicles Deleted" 
            });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetVehicleCount()
        {
            var count = await _authContext.Vehicles.CountAsync(); 
            return Ok(new
            {
                Count = count
            });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Vehicles>>> SearchVehiclesByTitle(string VehicleType)
        {
            var tutorials = await _authContext.Vehicles
                .Where(t => t.VehicleType.Contains(VehicleType))
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
            var allvehicles = await _authContext.Vehicles.ToListAsync();
            if (allvehicles.Count == 0)
            {
                return NotFound();
            }

            foreach (var vehi in allvehicles)
            {
                _authContext.Vehicles.Remove(vehi); 
            }

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "All Vehicles Deleted" 
            });
        }

    } 
    

}

