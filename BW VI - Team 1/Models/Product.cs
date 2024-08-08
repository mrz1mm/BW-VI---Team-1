using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Supplier> Suppliers { get; set; } = [];
        [Required]
        public Type Type { get; set; }
        [Required]
        public List<Usage> Usages { get; set; } = [];
        public Locker? Locker { get; set; }
        public int? LockerId { get; set; }
        public List<Order> Orders { get; set; } = [];

        public int? DrawerId { get; set; } 
        public Drawer? Drawer { get; set; }

    }

    public enum Type
    {
        AnimalFood,
        Medicine
    }
}
