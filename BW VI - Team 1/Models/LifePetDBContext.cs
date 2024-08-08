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

            // Configura la relazione molti-a-molti tra Order e Product
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderProduct",
                    j => j
                        .HasOne<Product>()
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Order>()
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                );

            // Configurazioni aggiuntive
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Locker)
                .WithMany(l => l.Products)
                .HasForeignKey(p => p.LockerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Locker>()
                .HasMany(l => l.Drawers)
                .WithOne(d => d.Locker)
                .HasForeignKey(d => d.LockerId);

            modelBuilder.Entity<Owner>()
                .HasMany(o => o.Animals)
                .WithOne(a => a.Owner)
                .HasForeignKey(a => a.OwnerId);
        }



    }
}
