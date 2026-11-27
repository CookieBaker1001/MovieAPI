using MovieAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dto.Actor
{
    public class ActorDto : IActorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BirthYear { get; set; }
    }
}
