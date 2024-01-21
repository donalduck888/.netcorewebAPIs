using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _authContext;

        public CustomerController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;

        }

        [HttpPost("add")]
        public async Task<IActionResult> RegisterFuel([FromBody] Customer customer)
        {

            if (customer == null)
                return BadRequest(); 

            await _authContext.Customer.AddAsync(customer);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Customer Registered" 
            });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuel(long id, [FromBody] Customer customer)
        {
            if (customer == null || id != customer.id)
            {
                return BadRequest();
            }

            var existingFuel = await _authContext.Customer.FindAsync(id);
            if (existingFuel == null)
            {
                return NotFound(); 
            }

            existingFuel.name = customer.name;
            existingFuel.vehi_id = customer.vehi_id;
            existingFuel.username = customer.username;
            existingFuel.email = customer.email;
            existingFuel.phone = customer.phone;

            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Customer Updated" 
            });
        }

        [HttpGet]

        public async Task<ActionResult<Customer>> GetAllFuels() 
        {

            return Ok(await _authContext.Customer.ToListAsync());  


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetFuelById(long id)
        {
            var fuel = await _authContext.Customer.FindAsync(id);

            if (fuel == null)
            {
                return NotFound();
            }

            return Ok(fuel); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuel(long id) 
        {
            var existingVehicle = await _authContext.Customer.FindAsync(id);
            if (existingVehicle == null)
            {
                return NotFound();
            }

            _authContext.Customer.Remove(existingVehicle);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Customer Deleted"  
            });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetVehicleCount()
        {
            var count = await _authContext.Customer.CountAsync();
            return Ok(new
            {
                Count = count
            });
        }

        [HttpGet("search")] 
        public async Task<ActionResult<IEnumerable<Customer>>> SearchVehiclesByTitle(string name)
        {
            var tutorials = await _authContext.Customer
                .Where(t => t.name.Contains(name))
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
            var allfuel = await _authContext.Customer.ToListAsync();
            if (allfuel.Count == 0)
            {
                return NotFound();
            }

            foreach (var vehi in allfuel)
            {
                _authContext.Customer.Remove(vehi);
            }

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "All Customers Deleted"  
            });
        }

    }


}

