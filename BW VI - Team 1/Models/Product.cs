using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public Locker LockerCondition
        {
            get
            {
                EnsureIsMedicine();
                return Locker;
            }
            set
            {
                EnsureIsMedicine();
                Locker = value;
            }
        }

        private void EnsureIsMedicine()
        {
            if (Type != Type.Medicine)
            {
                throw new InvalidOperationException("Only medicines have a locker.");
            }
        }
    }

    public enum Type
    {
        AnimalFood,
        Medicine
    }
}
