namespace BW_VI___Team_1.Models.DTO
{
    public class RecoveryDTO
    {
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly EndDate { get; set; }
        public Animal Animal { get; set; }
        public bool IsRefound { get; set; } = false;
    }
}
