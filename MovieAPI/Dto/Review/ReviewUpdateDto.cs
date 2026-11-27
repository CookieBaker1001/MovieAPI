namespace MovieAPI.Dto.Review
{
    public class ReviewUpdateDto
    {
        public int Id { get; set; }
        public string? ReviewerName { get; set; } = string.Empty;
        public string? Comment { get; set; } = string.Empty;
        public int? Rating { get; set; }
    }
}
