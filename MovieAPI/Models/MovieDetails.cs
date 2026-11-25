using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }
        [MinLength(1), MaxLength(100)]
        public string Genre { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Synopsis { get; set; } = string.Empty;
        [MinLength(2), MaxLength(25)]
        public string Language { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Budget { get; set; }
        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = new Movie();
    }
}
