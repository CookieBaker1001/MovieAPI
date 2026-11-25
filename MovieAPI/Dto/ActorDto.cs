using MovieAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Dto
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BirthYear { get; set; }
    }
}
