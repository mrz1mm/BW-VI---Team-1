using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Telephone { get; set; }
        public List<Product> Products { get; set; } = [];
    }
}