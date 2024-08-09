using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FiscalCode { get; set; }
        [Required]
        public List<Animal> Animals { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
