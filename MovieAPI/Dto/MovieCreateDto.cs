using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dto
{
    public class MovieCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Range(1800, 2026)]
        public int Year { get; set; }
        [Range(0, int.MaxValue)]
        public int Duration { get; set; }
        //[MinLength(1), MaxLength(100)]
        //public string Genre { get; set; } = string.Empty;
        //[MaxLength(200)]
        //public string Synopsis { get; set; } = string.Empty;
        //[MinLength(1), MaxLength(3)]
        //public string Language { get; set; } = string.Empty;
        //[Range(0, int.MaxValue)]
        //public int Budget { get; set; }
    }
}
