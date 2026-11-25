using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dto;
using MovieAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly MovieAPIContext _context;
    public ActorsController(MovieAPIContext context)
    {
        _context = context;
    }

    // GET: api/Actor
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
    {
        var actors = await _context.Actors.Select(a => new ActorDto
        {
            Id = a.Id,
            Name = a.Name,
            BirthYear = a.BirthYear
        }).ToListAsync();

        return actors;
    }

    // GET: api/Actor/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetActor(int id)
    {
        var actor = await _context.Actors.FindAsync(id);

        if (actor == null)
        {
            return NotFound();
        }

        ActorDto actorToReturn = new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            BirthYear = actor.BirthYear
        };

        return actorToReturn;
    }

    // PUT: api/Actor/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutActor(int? id, ActorUpdateDto actorDto)
    {
        if (id != actorDto.Id)
        {
            return BadRequest();
        }

        var actor = await _context.Actors.FindAsync(id);
        if (actor is null)
        {
            return NotFound();
        }

        bool isUpdated = (actorDto.Name != null && actorDto.Name != actor.Name)
            || (actorDto.BirthYear != null && actorDto.BirthYear != actor.BirthYear);
        if (!isUpdated)
        {
            return BadRequest("No changes detected in the provided data.");
        }

        actor.Name = actorDto.Name ?? actor.Name;
        actor.BirthYear = actorDto.BirthYear ?? actor.BirthYear;

        _context.Entry(actor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ActorExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Actor
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Actor>> PostActor(ActorCreateDto actorDto)
    {
        var actor = new Actor
        {
            Name = actorDto.Name,
            BirthYear = actorDto.BirthYear
        };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetActor", new { id = actor.Id }, actorDto);
    }

    // DELETE: api/Actor/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActor(int? id)
    {
        var actor = await _context.Actors.FindAsync(id);
        if (actor == null)
        {
            return NotFound();
        }

        _context.Actors.Remove(actor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ActorExists(int? id)
    {
        return _context.Actors.Any(e => e.Id == id);
    }
}
