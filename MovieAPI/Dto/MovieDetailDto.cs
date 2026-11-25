namespace MovieAPI.Dto
{
    public class MovieDetailDto
    {
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Duration { get; set; }

        public string? Genre { get; set; } = string.Empty;
        public string? Synopsis { get; set; } = string.Empty;
        public string? Language { get; set; } = string.Empty;
        public int? Budget { get; set; }

        public List<ReviewDto>? Reviews { get; set; }
        public List<ActorDto>? Actors { get; set; }
    }
}
