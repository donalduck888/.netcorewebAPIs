using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers

{
    [Route("[controller]")]
    [ApiController] 
    public class VehicleTypeController : Controller
    {
        private readonly AppDbContext _authContext;

        public VehicleTypeController(AppDbContext appDbContext)
        {
            _authContext = appDbContext; 

        } 


        [HttpGet] 
         
        public async Task<ActionResult<VehicleType>> GetAllVehicleTypes()
        {

            return Ok(await _authContext.vehicleType.ToListAsync());


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleType>> GetVehicleTypesById(int id)
        {
            var tutorial = await _authContext.vehicleType.FindAsync(id);

            if (tutorial == null)
            {
                return NotFound();
            }

            return Ok(tutorial);
        }
    }
}
