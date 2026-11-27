using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dto;
using MovieAPI.Dto.Actor;
using MovieAPI.Dto.Movie;
using MovieAPI.Dto.Review;
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

    // GET: api/movies
    // Get all movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie([FromQuery] string? genre, int? year, string? actor)
    {
        var moviesQuery = _context.Movie.AsQueryable();

        if (genre is not null) {
            moviesQuery = moviesQuery
                .Where(m => (m.Details != null && m.Details.Genre == genre));
        }
        if (year is not null)
        {
            moviesQuery = moviesQuery
                .Where(m => m.Year == year);
        }
        if (actor is not null)
        {
            moviesQuery = moviesQuery
                .Where(m => m.Actors.Any(a => a.Name.Contains(actor)));
        }

        var movies = await moviesQuery.Select(m => new MovieDto
        {
            Id = m.Id,
            Title = m.Title,
            Year = m.Year,
            Duration = m.Duration,
        }).ToListAsync();

        return movies;
    }

    // GET: api/movies/5
    // Get a specific movie by id
    [HttpGet("{movieId}")]
    public async Task<ActionResult<IMovieDto>> GetMovie(int movieId, [FromQuery] bool? withActors)
    {
        var movie = await _context.Movie.FindAsync(movieId);

        if (movie is null)
            return NotFound($"Movie with id {movieId} does not exist.");

        if (withActors.HasValue && withActors.Value)
        {
            var actors = await _context.Actors
                .Where(a => a.Movies.Any(m => m.Id == movieId))
                .ToListAsync();
            List<ActorDto> actorsDto = actors.Select(a => new ActorDto {
                Id = a.Id,
                Name = a.Name,
                BirthYear = a.BirthYear
            }).ToList();
            var movieWithActorsToReturn = new MovieWithActorsDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Duration = movie.Duration,
                Actors = actorsDto
            };

            return movieWithActorsToReturn;
        }

        var movieToReturn = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Duration = movie.Duration,
        };

        return movieToReturn;
    }

    // GET: api/movies/5/details
    // Get details for a specific movie
    [HttpGet("{movieId}/details")]
    public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int movieId) 
    {
        var movie = await _context.Movie
            .Where(m => m.Id == movieId)
            .Include(m => m.Details)
            .Include(m => m.Actors)
            .Include(m => m.Reviews)
            .FirstAsync();

        if (movie is null)
            return NotFound($"Could not provide details. Movie with id {movieId} does not exist.");

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

    // POST: api/movies
    // Post a new movie
    [HttpPost]
    public async Task<ActionResult<MovieCreateDto>> PostMovie(MovieCreateDto incommingMovie)
    {
        var movieToCreate = new Movie
        {
            Title = incommingMovie.Title,
            Year = incommingMovie.Year,
            Duration = incommingMovie.Duration,
        };
        _context.Movie.Add(movieToCreate);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovie", new { id = movieToCreate.Id }, incommingMovie);
    }

    // POST: api/movies/5
    // Post details to a movie
    [HttpPost("{movieId}")]
    public async Task<ActionResult<DetailsCreateDto>> PostMovieDetails(int movieId, DetailsCreateDto details)
    {
        var movie = await _context.Movie
            .Include(m => m.Details)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie is null)
            return NotFound($"Could not add details. Movie with id {movieId} does not exist.");
        if (movie.Details is not null)
            return BadRequest($"Details already exist for movie with id {movieId}. Consider using PUT method instead.");

        MovieDetails md = new MovieDetails()
        {
            Genre = details.Genre,
            Synopsis = details.Synopsis,
            Language = details.Language,
            Budget = details.Budget,
            MovieId = movieId
        };

        movie.Details = md;
        _context.Movie.Update(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction("PostMovieDetails", new { id = md.Id }, details);
    }

    // PUT: api/movies/5
    // Update a specific movie by id
    [HttpPut("{movieId}")]
    public async Task<IActionResult> PutMovie(int? movieId, MovieUpdateDto movieDto)
    {
        if (movieId != movieDto.Id)
            return BadRequest($"The provided movie id ({movieId}) does not match the id in the request body ({movieDto.Id}).");

        var movie = await _context.Movie.FindAsync(movieId);
        if (movie is null)
            return NotFound($"Movie with id {movieId} does not exist.");

        bool isUpdated = (movieDto.Title != null && movieDto.Title != movie.Title)
            || (movieDto.Year != null && movieDto.Year != movie.Year)
            || (movieDto.Duration != null && movieDto.Duration != movie.Duration);

        if (!isUpdated)
            return Ok("No changes detected in the provided data. No updates required.");

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
            if (!MovieExists(movieId))
            {
                return NotFound($"Movie with id {movieId} does not exist.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/movies/5
    // Delete a specific movie by id
    [HttpDelete("{movieId}")]
    public async Task<IActionResult> DeleteMovie(int? movieId)
    {
        var movie = await _context.Movie.FindAsync(movieId);
        if (movie is null)
            return NotFound($"Movie with id {movieId} does not exist.");

        _context.Movie.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}
