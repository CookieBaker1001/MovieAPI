using MovieAPI.Dto.Movie;
using MovieAPI.Models;

namespace MovieAPI.Dto.Actor
{
    public class ActorWithMoviesDto : IActorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public List<MovieDto> Movies { get; set; }
    }
}
