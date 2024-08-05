using System.ComponentModel.DataAnnotations;

namespace BW_VI___Team_1.Models
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [Required]
        public string Exam { get; set; }
        [Required]
        public string Diagnosis { get; set; }
        [Required]
        public Animal Animal { get; set; }
    }
}
