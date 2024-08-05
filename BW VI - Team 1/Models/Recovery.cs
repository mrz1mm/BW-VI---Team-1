using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Recovery
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [Required]
        public DateOnly EndDate { get; set; }
        [Required]
        public Animal Animal { get; set; }
        [Required]
        public bool IsRefound { get; set; } = false;
    }
}
