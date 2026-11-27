using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dto.Review;
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

    // GET: api/reviews
    // Get all reviews
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

    // GET: api/reviews/5
    // Get all reviews for a specific movie
    [HttpGet("{movieId}")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForMovie(int movieId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.MovieId == movieId)
            .ToListAsync();

        if (reviews is null)
            return NotFound($"Review with id {movieId} does not exist.");

        List<ReviewDto> reviewsToReturn = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            reviewsToReturn.Add(new ReviewDto
            {
                Id = review.Id,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
                Rating = review.Rating,
                MovieId = review.MovieId
            });
        }

        return reviewsToReturn;
    }

    // POST: api/reviews
    // Post a new review for a specific movie
    [HttpPost("{movieId}")]
    public async Task<ActionResult<ReviewCreateDto>> PostReview(int movieId, ReviewCreateDto incomingReview)
    {
        var movie = await _context.Movie.FindAsync(movieId);

        if (movie is null)
            return NotFound($"Movie with id {movieId} does not exist.");

        var reviewToCreate = new Review
        {
            ReviewerName = incomingReview.ReviewerName,
            Comment = incomingReview.Comment,
            Rating = incomingReview.Rating,
            MovieId = movieId
        };

        _context.Reviews.Add(reviewToCreate);
        await _context.SaveChangesAsync();

        return CreatedAtAction("PostReview", new { id = reviewToCreate.Id }, incomingReview);
    }

    // PUT: api/reviews/5
    // Update a specific review by id
    [HttpPut("{reviewId}")]
    public async Task<IActionResult> PutReview(int? reviewId, ReviewUpdateDto reviewDto)
    {
        if (reviewId != reviewDto.Id)
            return BadRequest($"The provided review id ({reviewId}) does not match the id in the request body ({reviewDto.Id}).");

        var review = await _context.Reviews.FindAsync(reviewId);
        if (review is null)
            return NotFound($"Review with id {reviewId} does not exist.");

        bool isUpdated = (reviewDto.ReviewerName != null && reviewDto.ReviewerName != review.ReviewerName)
            || (reviewDto.Comment != null && reviewDto.Comment != review.Comment)
            || (reviewDto.Rating != null && reviewDto.Rating != review.Rating);
        if (!isUpdated)
            return Ok("No changes detected in the provided data. No updates required.");

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
            if (!ReviewExists(reviewId))
            {
                return NotFound($"Review with id {reviewId} does not exist.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/reviews/5
    // Delete a specific review by id
    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReview(int? reviewId)
    {
        var review = await _context.Reviews.FindAsync(reviewId);
        if (review is null)
            return NotFound($"Review with id {reviewId} does not exist.");

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReviewExists(int? id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }
}
