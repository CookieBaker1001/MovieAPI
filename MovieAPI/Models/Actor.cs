using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(1800, 2026)]
        public int BirthYear { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
