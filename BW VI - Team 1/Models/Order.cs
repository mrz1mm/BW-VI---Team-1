using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public List<Product> Products { get; set; } = [];
        [Required]
        public Owner Owner { get; set; }
        [Required]
        public string MedicalPrescription { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
