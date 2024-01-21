using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using WebApplication1.Context;
using WebApplication1.Models; 

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController] 
    public class ApproveController : ControllerBase
    {
        private readonly AppDbContext _authContext;

        public ApproveController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;

        }


        [HttpPost("add")]
        public async Task<IActionResult> RegisterFuel([FromBody] Approve fuel)
        {

            if (fuel == null)
                return BadRequest();

            await _authContext.Approve.AddAsync(fuel); 
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Fuel Registered"
            });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuel(int id, [FromBody] Approve approve)
        {
            if (approve == null || id != approve.id)
            {
                return BadRequest();
            }

            var existingApp = await _authContext.Approve.FindAsync(id);
            if (existingApp == null)
            {
                return NotFound();
            }

            existingApp.reqdate = approve.reqdate; 
            existingApp.tokento = approve.tokento;
            existingApp.isuamt = approve.isuamt; 
            existingApp.vehicletype = approve.vehicletype;
            existingApp.username = approve.username;
            existingApp.T_id = approve.T_id;

            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Token has been Generated, Token is : TVI" + approve.id + "TU" 
        });
        }

        [HttpGet]

        public async Task<ActionResult<Approve>> GetAllFuels()
        {

            return Ok(await _authContext.Approve.ToListAsync()); 


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Approve>> GetFuelById(int id)
        {
            var fuel = await _authContext.Approve.FindAsync(id);

            if (fuel == null)
            {
                return NotFound();
            }

            return Ok(fuel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuel(int id)
        {
            var existingVehicle = await _authContext.Approve.FindAsync(id);
            if (existingVehicle == null)
            {
                return NotFound(); 
            }

            _authContext.Approve.Remove(existingVehicle);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Fuel Deleted"
            });
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetVehicleCount()
        {
            var count = await _authContext.Approve.CountAsync();
            return Ok(new
            {
                Count = count
            });
        }

  

        [HttpDelete]
        public async Task<IActionResult> DeleteAllVehicles()
        {
            var allfuel = await _authContext.Approve.ToListAsync();
            if (allfuel.Count == 0)
            {
                return NotFound();
            }

            foreach (var vehi in allfuel) 
            {
                _authContext.Approve.Remove(vehi);
            }

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "All Fuels Deleted"
            });
        }

        [HttpGet("search")] 
        public async Task<ActionResult<IEnumerable<Approve>>> SearchTutorialsByTitle(string username)
        {
            var tutorials = await _authContext.Approve
                .Where(t => t.username.Contains(username))
                .ToListAsync();

            if (tutorials.Count == 0) 
            {
                return NotFound();
            }

            return Ok(tutorials);
        }

    }
}
