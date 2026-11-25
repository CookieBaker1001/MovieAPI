using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public string ReviewerName { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Comment { get; set; } = string.Empty;
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
