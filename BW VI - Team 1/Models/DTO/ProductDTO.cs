namespace BW_VI___Team_1.Models.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public List<Supplier> Suppliers { get; set; } = [];
        public Type Type { get; set; }
        public List<Usage> Usages { get; set; } = [];
        public Locker? Locker { get; set; }
        public int? LockerId { get; set; } 
        public int? DrawerId { get; set; }
    }
}
