using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Drawer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        public int LockerId { get; set; }
        public Locker Locker { get; set; }
    }
}