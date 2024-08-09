using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Species { get; set; }
        [Required]
        public string Breed { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }
        public DateOnly RegisterDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool Microchip { get; set; }
        public string MicrochipNumber { get; set; }
        public int? OwnerId { get; set; }
        [Required]
        public Owner Owner { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}
