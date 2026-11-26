namespace MovieAPI.Dto
{
    public class MovieWithActorsDto : IMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Duration { get; set; }
        public List<ActorDto> Actors { get; set; }
    }
}
