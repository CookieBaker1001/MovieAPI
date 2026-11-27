namespace MovieAPI.Dto.Movie
{
    public class MovieUpdateDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public int? Duration { get; set; }
    }
}
