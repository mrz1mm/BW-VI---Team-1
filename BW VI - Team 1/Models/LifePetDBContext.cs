using Microsoft.EntityFrameworkCore;

namespace BW_VI___Team_1.Models
{
    public class LifePetDBContext : DbContext
    {
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Locker> Lockers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Recovery> Recoverys { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Usage> Usages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
        public LifePetDBContext(DbContextOptions<LifePetDBContext> options) : base(options) { }
    }
}
