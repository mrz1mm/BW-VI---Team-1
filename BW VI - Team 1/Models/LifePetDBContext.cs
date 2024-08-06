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
        public virtual DbSet<Drawer> Drawers { get; set; }

        public LifePetDBContext(DbContextOptions<LifePetDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Locker)
                .WithMany(l => l.Products)
                .HasForeignKey(p => p.LockerId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Product>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Locker>()
        .HasMany(l => l.Drawers)
        .WithOne(d => d.Locker)
        .HasForeignKey(d => d.LockerId);
        }
    }
}
