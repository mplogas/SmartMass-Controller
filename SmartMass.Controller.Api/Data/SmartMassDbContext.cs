using Microsoft.EntityFrameworkCore;
using SmartMass.Controller.Model.DTOs;

namespace SmartMass.Controller.Api.Data
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

        public DbSet<ManufacturerDTO> Manufacturers { get; set; }
        public DbSet<MaterialDTO> Materials { get; set; }
        public DbSet<SpoolDTO> Spools { get; set; }
        public DbSet<DeviceDTO> Devices { get; set; }
        public DbSet<MqttLogEntryDTO> MqttValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManufacturerDTO>().HasKey(k => k.Id);
            modelBuilder.Entity<ManufacturerDTO>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<MaterialDTO>().HasKey(k => k.Id);
            modelBuilder.Entity<MaterialDTO>().Property(p => p.Type).IsRequired();
            modelBuilder.Entity<MaterialDTO>().Property(p => p.DefaultBedTemp).HasDefaultValue(0);
            modelBuilder.Entity<MaterialDTO>().Property(p => p.DefaultNozzleTemp).HasDefaultValue(0);

            modelBuilder.Entity<SpoolDTO>().HasKey(k => k.Id);
            modelBuilder.Entity<SpoolDTO>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<SpoolDTO>().Property(p => p.EmptySpoolWeight).HasDefaultValue(0);
            modelBuilder.Entity<SpoolDTO>().Property(p => p.NozzleTemp).HasDefaultValue(0);
            modelBuilder.Entity<SpoolDTO>().Property(p => p.BedTemp).HasDefaultValue(0);
            modelBuilder.Entity<SpoolDTO>().Property(p => p.Color).IsRequired();
            modelBuilder.Entity<SpoolDTO>().HasOne(s => s.ManufacturerDto).WithMany(s => s.Spools)
                .HasForeignKey(s => s.ManufacturerId).IsRequired();
            modelBuilder.Entity<SpoolDTO>().HasOne(s => s.MaterialDto).WithMany(s => s.Spools)
                .HasForeignKey(s => s.MaterialId).IsRequired();
            modelBuilder.Entity<SpoolDTO>().Property(p => p.Created).HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<DeviceDTO>().HasKey(k => k.Id);
            modelBuilder.Entity<DeviceDTO>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<DeviceDTO>().Property(p => p.CalibrationFactor).HasDefaultValue(981).IsRequired();
            modelBuilder.Entity<DeviceDTO>().Property(p => p.ScaleUpdateInterval).HasDefaultValue(1000).IsRequired();
            modelBuilder.Entity<DeviceDTO>().Property(p => p.ScaleSamplingSize).HasDefaultValue(1).IsRequired();
            modelBuilder.Entity<DeviceDTO>().Property(p => p.ScaleCalibrationWeight).HasDefaultValue(100).IsRequired();
            modelBuilder.Entity<DeviceDTO>().Property(p => p.ScaleDisplayTimeout).HasDefaultValue(60000).IsRequired();

            modelBuilder.Entity<MqttLogEntryDTO>().HasKey(k => k.Id);
            modelBuilder.Entity<MqttLogEntryDTO>().Property(p => p.SpoolId).IsRequired();
            modelBuilder.Entity<MqttLogEntryDTO>().Property(p => p.Value).IsRequired();
            modelBuilder.Entity<MqttLogEntryDTO>().Property(p => p.Received).IsRequired();
        }
    }
}
