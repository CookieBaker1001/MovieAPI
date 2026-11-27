using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dto.Review
{
    public class ReviewCreateDto
    {
        public string ReviewerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
