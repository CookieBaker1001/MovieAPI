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
        var movies = await _context.Movie.Select(m => new MovieDto
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
        var movie = await _context.Movie.FindAsync(id);

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

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int? id, MovieUpdateDto movieDto)
    {
        if (id != movieDto.Id)
        {
            return BadRequest();
        }

        var movie = await _context.Movie.FindAsync(id);
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
        _context.Movie.Add(movieToCreate);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMovie", new { id = movieToCreate.Id }, incommingMovie);
    }

    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movie.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }



    [HttpGet("/reviews")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
    {
        var reviews = await _context.Reviews.Select(r => new ReviewDto
        {
            ReviewerName = r.ReviewerName,
            Comment = r.Comment,
            Rating = r.Rating
        }).ToListAsync();
        return reviews;
    }

    [HttpGet("/reviews{id}")]
    public async Task<ActionResult<ReviewDto>> GetReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        ReviewDto reviewToReturn = new ReviewDto
        {
            ReviewerName = review.ReviewerName,
            Comment = review.Comment,
            Rating = review.Rating
        };

        return reviewToReturn;
    }

    [HttpPost("/reviews")]
    public async Task<ActionResult<ReviewCreateDto>> PostReview(ReviewCreateDto incomingReview)
    {
        var movie = await _context.Movie.FindAsync(incomingReview.MovieId);

        if (movie is null)
        {
            return NotFound($"Movie with ID {incomingReview.MovieId} not found.");
        }

        var reviewToCreate = new Review
        {
            ReviewerName = incomingReview.ReviewerName,
            Comment = incomingReview.Comment,
            Rating = incomingReview.Rating,
            MovieId = incomingReview.MovieId
        };

        _context.Reviews.Add(reviewToCreate);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetReview", new { id = reviewToCreate.Id }, incomingReview);
    }



    [HttpGet("/actors")]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
    {
        var actors = await _context.Actors.Select(a => new ActorDto
        {
            Name = a.Name,
            BirthYear = a.BirthYear
        }).ToListAsync();

        return actors;
    }
}
