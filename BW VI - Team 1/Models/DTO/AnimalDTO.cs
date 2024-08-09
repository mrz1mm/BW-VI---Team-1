namespace BW_VI___Team_1.Models.DTO
{
    public class AnimalDTO
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateOnly RegisterDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool Microchip { get; set; }
        public string MicrochipNumber { get; set; }
        public Owner Owner { get; set; }
        public IFormFile ImageFile { get; set; }
        public int? OwnerId { get; set; }
    }
}
