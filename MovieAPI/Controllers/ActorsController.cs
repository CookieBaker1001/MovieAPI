using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dto.Actor;
using MovieAPI.Dto.Movie;
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

    // GET: api/actors
    // Get all actors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors([FromQuery] string? name, int? birthYear)
    {
        var actorsQuery = _context.Actors.AsQueryable();

        if (name is not null)
        {
            actorsQuery = actorsQuery.Where(a => a.Name.Contains(name));
        }
        if (birthYear.HasValue)
        {
            actorsQuery = actorsQuery.Where(a => a.BirthYear == birthYear.Value);
        }

        var actors = await actorsQuery.Select(a => new ActorDto
        {
            Id = a.Id,
            Name = a.Name,
            BirthYear = a.BirthYear
        }).ToListAsync();

        return actors;
    }

    // GET: api/actors/5
    // Get a specific actor by id
    [HttpGet("{actorId}")]
    public async Task<ActionResult<IActorDto>> GetActor(int actorId, [FromQuery] bool? withMovies)
    {
        var actor = await _context.Actors.FindAsync(actorId);

        if (actor is null)
            return NotFound($"Actor with id {actorId} does not exist.");

        if (withMovies.HasValue && withMovies.Value)
        {
            var movies = await _context.Movie
                .Where(m => m.Actors.Any(a => a.Id == actorId))
                .ToListAsync();
            List<MovieDto> moviesDto = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Duration = m.Duration
            }).ToList();
            var actorWithMoviesToReturn = new ActorWithMoviesDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthYear = actor.BirthYear,
                Movies = moviesDto
            };

            return actorWithMoviesToReturn;
        }

        ActorDto actorToReturn = new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            BirthYear = actor.BirthYear
        };

        return actorToReturn;
    }

    // POST: api/actors
    // Post a new actor
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

        return CreatedAtAction("PostActor", new { id = actor.Id }, actorDto);
    }

    // POST: api/movies/5/actors/3
    // Add an actor to a movie
    [HttpPost("{movieId}/actors/{actorId}")]
    public async Task<ActionResult> AddActorToMovie(int movieId, int actorId)
    {
        var movie = await _context.Movie
            .Where(m => m.Id == movieId)
            .Include(m => m.Actors)
            .FirstOrDefaultAsync();

        if (movie is null)
            return NotFound($"Movie with id {movieId} does not exist.");

        var actor = await _context.Actors.FindAsync(actorId);
        if (actor is null)
            return NotFound($"Actor with id {actorId} does not exist.");

        movie.Actors.Add(actor);
        await _context.SaveChangesAsync();

        return Ok($"Actor {actor.Name} added to the cast of {movie.Title}");
    }

    // PUT: api/actors/5
    // Update a specific actor by id
    [HttpPut("{actorId}")]
    public async Task<IActionResult> PutActor(int? actorId, ActorUpdateDto actorDto)
    {
        if (actorId != actorDto.Id)
            return BadRequest($"The provided actor id ({actorId}) does not match the id in the request body ({actorDto.Id}).");

        var actor = await _context.Actors.FindAsync(actorId);
        if (actor is null)
            return NotFound($"Actor with id {actorId} does not exist.");

        bool isUpdated = (actorDto.Name != null && actorDto.Name != actor.Name)
            || (actorDto.BirthYear != null && actorDto.BirthYear != actor.BirthYear);
        if (!isUpdated)
            return Ok("No changes detected in the provided data. No updates required.");

        actor.Name = actorDto.Name ?? actor.Name;
        actor.BirthYear = actorDto.BirthYear ?? actor.BirthYear;

        _context.Entry(actor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ActorExists(actorId))
            {
                return NotFound($"Actor with id {actorId} does not exist.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/actors/5
    // Delete a specific actor by id
    [HttpDelete("{actorId}")]
    public async Task<IActionResult> DeleteActor(int? actorId)
    {
        var actor = await _context.Actors.FindAsync(actorId);
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
