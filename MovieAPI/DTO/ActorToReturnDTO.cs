namespace MovieAPI.DTO
{
    public class ActorToReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Image { get; set; }
    }
}
