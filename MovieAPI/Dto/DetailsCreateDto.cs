using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dto
{
    public class DetailsCreateDto
    {
        public string Genre { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Budget { get; set; }
        public int MovieId { get; set; }
    }
}
