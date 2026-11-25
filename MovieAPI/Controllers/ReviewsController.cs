using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dto;
using MovieAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly MovieAPIContext _context;
    public ReviewsController(MovieAPIContext context)
    {
        _context = context;
    }

    // GET: api/Review
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
    {
        var reviews = await _context.Reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            ReviewerName = r.ReviewerName,
            Comment = r.Comment,
            Rating = r.Rating,
            MovieId = r.MovieId
        }).ToListAsync();
        return reviews;
    }

    // GET: api/Review/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDto>> GetReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        ReviewDto reviewToReturn = new ReviewDto
        {
            Id = review.Id,
            ReviewerName = review.ReviewerName,
            Comment = review.Comment,
            Rating = review.Rating,
            MovieId = review.MovieId
        };

        return reviewToReturn;
    }

    // PUT: api/Review/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReview(int? id, ReviewUpdateDto reviewDto)
    {
        if (id != reviewDto.Id)
        {
            return BadRequest();
        }

        var review = await _context.Reviews.FindAsync(id);
        if (review is null)
        {
            return NotFound();
        }

        bool isUpdated = (reviewDto.ReviewerName != null && reviewDto.ReviewerName != review.ReviewerName)
            || (reviewDto.Comment != null && reviewDto.Comment != review.Comment)
            || (reviewDto.Rating != null && reviewDto.Rating != review.Rating);
        if (!isUpdated)
        {
            return BadRequest("No changes detected in the provided data.");
        }

        review.ReviewerName = reviewDto.ReviewerName ?? review.ReviewerName;
        review.Comment = reviewDto.Comment ?? review.Comment;
        review.Rating = reviewDto.Rating ?? review.Rating;

        _context.Entry(review).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(id))
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

    // POST: api/Review
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
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

    // DELETE: api/Review/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int? id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReviewExists(int? id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}
