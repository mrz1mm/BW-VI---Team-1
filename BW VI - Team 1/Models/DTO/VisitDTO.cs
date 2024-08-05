namespace BW_VI___Team_1.Models.DTO
{
    public class VisitDTO
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Exam { get; set; }
        public string Diagnosis { get; set; }
        public Animal Animal { get; set; }
    }
}
