using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Locker
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Drawer { get; set; }
    }
}
