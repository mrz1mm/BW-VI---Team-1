using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin,
        Pharmacist,
        Veterinarian,
    }
}
