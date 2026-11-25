using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dto;
using MovieAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieAPIContext _context;
    public MoviesController(MovieAPIContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie()
    {
        var movies = await _context.Movies.Select(m => new MovieDto
        {
            Id = m.Id,
            Title = m.Title,
            Year = m.Year,
            Duration = m.Duration,
        }).ToListAsync();
        return movies;
    }

    // GET: api/Movie/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        var movieToReturn = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Duration = movie.Duration
        };

        return movieToReturn;
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id) 
    {
        var movie = await _context.Movies
            .Where(m => m.Id == id)
            .Include(m => m.Details)
            .Include(m => m.Actors)
            .Include(m => m.Reviews)
            .FirstAsync();

        if (movie is null)
            return NotFound();

        MovieDetails md = movie.Details ?? new MovieDetails();
        List<Actor> actors = movie.Actors ?? new List<Actor>();
        List<Review> reviews = movie.Reviews ?? new List<Review>();

        MovieDetailDto movieDetailToReturn = new MovieDetailDto()
        {
            Title = movie.Title,
            Year = movie.Year,
            Duration = movie.Duration,
            Genre = md.Genre,
            Synopsis = md.Synopsis,
            Language = md.Language,
            Budget = md.Budget,
            Reviews = reviews.Select(r => new ReviewDto()
            {
                ReviewerName = r.ReviewerName,
                Comment = r.Comment,
                Rating = r.Rating
            }).ToList(),
            Actors = actors.Select(a => new ActorDto()
            {
                Name = a.Name,
                BirthYear = a.BirthYear
            }).ToList()
        };

        return movieDetailToReturn;
    }

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int? id, MovieUpdateDto movieDto)
    {
        if (id != movieDto.Id)
        {
            return BadRequest();
        }

        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        bool isUpdated = (movieDto.Title != null && movieDto.Title != movie.Title)
            || (movieDto.Year != null && movieDto.Year != movie.Year)
            || (movieDto.Duration != null && movieDto.Duration != movie.Duration);

        if (!isUpdated)
        {
            return BadRequest("No changes detected in the provided data.");
        }

        movie.Title = movieDto.Title ?? movie.Title;
        movie.Year = movieDto.Year ?? movie.Year;
        movie.Duration = movieDto.Duration ?? movie.Duration;

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id))
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

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MovieCreateDto>> PostMovie(MovieCreateDto incommingMovie)
    {
        var movieToCreate = new Movie
        {
            Title = incommingMovie.Title,
            Year = incommingMovie.Year,
            Duration = incommingMovie.Duration,
        };
        _context.Movies.Add(movieToCreate);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovie", new { id = movieToCreate.Id }, incommingMovie);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<DetailsCreateDto>> PostMovieDetails(int id, DetailsCreateDto details) { 
        var movie = await _context.Movies.FindAsync(id);

        if (movie is null)
            return NotFound();

        MovieDetails md = new MovieDetails()
        {
            Genre = details.Genre,
            Synopsis = details.Synopsis,
            Language = details.Language,
            Budget = details.Budget,
            MovieId = id
        };

        movie.Details = md;
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetMovieDetails", new { id = md.Id }, details);
    }

    [HttpGet("{id}/addactor")]
    public async Task<ActionResult> AddActorToMovie(int id, int actorId) {
        var movie = await _context.Movies
            .Where(m => m.Id == id)
            .Include(m => m.Actors)
            .FirstOrDefaultAsync();

        if (movie is null)
            return NotFound();

        var actor = await _context.Actors.FindAsync(actorId);

        if (actor is null)
            return NotFound();

        movie.Actors.Add(actor);
        await _context.SaveChangesAsync();

        return Ok($"Actor {actor.Name} added to the cast of {movie.Title}");
    }

    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}
