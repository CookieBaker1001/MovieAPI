namespace MovieAPI.Dto.Actor
{
    public class ActorUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int? BirthYear { get; set; }
    }
}
