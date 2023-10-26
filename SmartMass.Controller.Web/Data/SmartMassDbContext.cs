using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SmartMass.Controller.Model;

namespace SmartMass.Controller.Web.Data
{
    public class SmartMassDbContext : DbContext
    {
        public SmartMassDbContext(DbContextOptions<SmartMassDbContext> options) : base (options)
        {
            //ConfigureDatabase();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        private void ConfigureDatabase()
        {
#if DEBUG
            //Database.EnsureDeleted();
            Database.EnsureCreated(); // Use this in testing so that we don't have to run a migration on every db change
#else
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
#endif
        }

        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Spool> Spools { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<MqttValue> MqttValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>().HasKey(k => k.Id);
            modelBuilder.Entity<Manufacturer>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<Material>().HasKey(k => k.Id);
            modelBuilder.Entity<Material>().Property(p => p.Type).IsRequired();
            modelBuilder.Entity<Material>().Property(p => p.DefaultBedTemp).HasDefaultValue(0);
            modelBuilder.Entity<Material>().Property(p => p.DefaultNozzleTemp).HasDefaultValue(0);

            modelBuilder.Entity<Spool>().HasKey(k => k.Id);
            modelBuilder.Entity<Spool>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Spool>().Property(p => p.EmptySpoolWeight).HasDefaultValue(0);
            modelBuilder.Entity<Spool>().Property(p => p.NozzleTemp).HasDefaultValue(0);
            modelBuilder.Entity<Spool>().Property(p => p.BedTemp).HasDefaultValue(0);
            modelBuilder.Entity<Spool>().Property(p => p.Color).IsRequired();
            modelBuilder.Entity<Spool>().HasOne(s => s.Manufacturer).WithMany(s => s.Spools)
                .HasForeignKey(s => s.ManufacturerId).IsRequired();
            modelBuilder.Entity<Spool>().HasOne(s => s.Material).WithMany(s => s.Spools)
                .HasForeignKey(s => s.MaterialId).IsRequired();
            modelBuilder.Entity<Spool>().Property(p => p.Created).HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<Device>().HasKey(k => k.Id);
            modelBuilder.Entity<Device>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Device>().Property(p => p.CalibrationFactor).HasDefaultValue(981).IsRequired();
            modelBuilder.Entity<Device>().Property(p => p.ScaleUpdateInterval).HasDefaultValue(1000).IsRequired();
            modelBuilder.Entity<Device>().Property(p => p.ScaleSamplingSize).HasDefaultValue(1).IsRequired();
            modelBuilder.Entity<Device>().Property(p => p.ScaleCalibrationWeight).HasDefaultValue(100).IsRequired();
            modelBuilder.Entity<Device>().Property(p => p.ScaleDisplayTimeout).HasDefaultValue(60000).IsRequired();

            modelBuilder.Entity<MqttValue>().HasKey(k => k.Id);
            modelBuilder.Entity<MqttValue>().Property(p => p.SpoolId).IsRequired();
            modelBuilder.Entity<MqttValue>().Property(p => p.Value).IsRequired();
            modelBuilder.Entity<MqttValue>().Property(p => p.Received).IsRequired();
        }
    }
}
