namespace BW_VI___Team_1.Models.DTO
{
    public class OrderDTO
    {
        public List<Product> Products { get; set; } = [];
        public Owner Owner { get; set; }
        public string MedicalPrescription { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int[] SelectedProductIds { get; set; }
    }
}
