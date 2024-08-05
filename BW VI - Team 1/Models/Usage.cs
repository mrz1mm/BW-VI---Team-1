using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Usage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public List<Product> Products { get; set; } = [];
    }
}