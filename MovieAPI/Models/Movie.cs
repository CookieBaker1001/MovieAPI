using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Range(1800, 2026)]
        public DateTime Year { get; set; }
        [Range(0, int.MaxValue)]
        public int Duration { get; set; }
        public MovieDetails Details { get; set; } = new MovieDetails();
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
