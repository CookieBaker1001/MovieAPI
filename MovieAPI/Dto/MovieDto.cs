using MovieAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dto
{
    public class MovieDto : IMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Duration { get; set; }
    }
}
