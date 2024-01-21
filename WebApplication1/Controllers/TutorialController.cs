using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TutorialController : ControllerBase
    {
        private readonly AppDbContext _authContext;

        public TutorialController(AppDbContext appDbContext) 
        {
            _authContext = appDbContext;

        }

        [HttpPost("add")]
        public async Task<IActionResult> RegisterVehicle([FromBody] Tutorial tutorial)
        {

            if (tutorial == null)
                return BadRequest();

            await _authContext.Tutorial.AddAsync(tutorial); 
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Tutorial Registered"
            });

        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateTutorial(int id, [FromBody] Tutorial tutorial)
        {
            if (tutorial == null || id != tutorial.id)
            {
                return BadRequest();
            }

            var existingTutorial = await _authContext.Tutorial.FindAsync(id);
            if (existingTutorial == null)
            {
                return NotFound();
            }

            existingTutorial.id = tutorial.id; 
            existingTutorial.title = tutorial.title;
            existingTutorial.description = tutorial.description;
            existingTutorial.published = tutorial.published;
  

            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Tutorial Updated"
            });
        }

        [HttpGet]

        public async Task<ActionResult<Tutorial>> GetAllTutorial()
        {

            return Ok(await _authContext.Tutorial.ToListAsync()); 


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tutorial>> GetTutorialById(int id)
        {
            var tutorial = await _authContext.Tutorial.FindAsync(id);

            if (tutorial == null)
            {
                return NotFound();
            }

            return Ok(tutorial);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutorial(int id)
        {
            var existingTutorial = await _authContext.Tutorial.FindAsync(id);
            if (existingTutorial == null)
            {
                return NotFound();
            }

            _authContext.Tutorial.Remove(existingTutorial);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tutorial Deleted"
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllTutorials()
        {
            var allTutorials = await _authContext.Tutorial.ToListAsync();
            if (allTutorials.Count == 0)
            {
                return NotFound();
            }

            foreach (var tutorial in allTutorials)
            {
                _authContext.Tutorial.Remove(tutorial);
            }

            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "All Tutorials Deleted" 
            });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Tutorial>>> SearchTutorialsByTitle(string title)
        {
            var tutorials = await _authContext.Tutorial
                .Where(t => t.title.Contains(title))
                .ToListAsync();

            if (tutorials.Count == 0)
            {
                return NotFound();
            }

            return Ok(tutorials); 
        }

    }


}
