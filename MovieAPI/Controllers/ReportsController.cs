using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Dto;
using MovieAPI.Models;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly MovieAPIContext _context;
        public ReportsController(MovieAPIContext context)
        {
            _context = context;
        }

        // GET: api/<ReportsController>
        [HttpGet("movies/top5pergenre")]
        public async Task<ActionResult<IEnumerable<string>>> GetTop5Genre()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/<ReportsController>
        [HttpGet("movies/averageRatings")]
        public async Task<ActionResult<IEnumerable<GenreRatingDto>>> GetAverageRatings()
        {
            var details = await _context.MovieDetails
                .SelectMany(
                    md => md.Movie.Reviews.DefaultIfEmpty(),
                    (md, review) => new GenreRatingDto
                    { 
                        Genre = md.Genre,
                        Rating = review != null ? review.Rating : 0
                    })
                .ToListAsync();

            List<GenreRatingDto> genreRatings = new List<GenreRatingDto>();

            foreach (var detail in details) {
                if (!genreRatings.Any(g => g.Genre == detail.Genre))
                {
                    decimal? avg = details.Where(d => d.Genre == detail.Genre).Average(d => d.Rating);
                    avg = avg.HasValue ? Math.Round(avg.Value, 2) : null;
                    genreRatings.Add(new GenreRatingDto
                    {
                        Genre = detail.Genre,
                        Rating = avg
                    });
                }
            }

            return genreRatings;
        }

        // GET api/<ReportsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReportsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReportsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
