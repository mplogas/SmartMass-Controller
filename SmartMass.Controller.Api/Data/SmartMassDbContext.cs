using Microsoft.EntityFrameworkCore;
using SmartMass.Controller.Api.Models.DTOs;

namespace SmartMass.Controller.Api.Data
{
    public class SmartMassDbContext : DbContext
    {
        public SmartMassDbContext(DbContextOptions<SmartMassDbContext> options) : base (options)
        {
            ConfigureDatabase();
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

        public DbSet<ManufacturerDto> Manufacturers { get; set; }
        public DbSet<MaterialDto> Materials { get; set; }
        public DbSet<SpoolDto> Spools { get; set; }
        public DbSet<DeviceDto> Devices { get; set; }
        public DbSet<HistoryEventDto> MqttValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManufacturerDto>().HasKey(k => k.Id);
            modelBuilder.Entity<ManufacturerDto>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<MaterialDto>().HasKey(k => k.Id);
            modelBuilder.Entity<MaterialDto>().Property(p => p.Type).IsRequired();
            modelBuilder.Entity<MaterialDto>().Property(p => p.DefaultBedTemp).HasDefaultValue(0);
            modelBuilder.Entity<MaterialDto>().Property(p => p.DefaultNozzleTemp).HasDefaultValue(0);

            modelBuilder.Entity<SpoolDto>().HasKey(k => k.Id);
            modelBuilder.Entity<SpoolDto>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<SpoolDto>().Property(p => p.EmptySpoolWeight).HasDefaultValue(0);
            modelBuilder.Entity<SpoolDto>().Property(p => p.NozzleTemp).HasDefaultValue(0);
            modelBuilder.Entity<SpoolDto>().Property(p => p.BedTemp).HasDefaultValue(0);
            modelBuilder.Entity<SpoolDto>().Property(p => p.Color).IsRequired();
            modelBuilder.Entity<SpoolDto>().HasOne(s => s.ManufacturerDto).WithMany(s => s.Spools)
                .HasForeignKey(s => s.ManufacturerId).IsRequired();
            modelBuilder.Entity<SpoolDto>().HasOne(s => s.MaterialDto).WithMany(s => s.Spools)
                .HasForeignKey(s => s.MaterialId).IsRequired();
            modelBuilder.Entity<SpoolDto>().Property(p => p.Created).HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<DeviceDto>().HasKey(k => k.Id);
            modelBuilder.Entity<DeviceDto>().HasIndex(i => i.ClientId).IsUnique();
            modelBuilder.Entity<DeviceDto>().Property(p => p.ClientId).IsRequired();
            modelBuilder.Entity<DeviceDto>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<DeviceDto>().Property(p => p.CalibrationFactor).HasDefaultValue(981).IsRequired();
            modelBuilder.Entity<DeviceDto>().Property(p => p.ScaleUpdateInterval).HasDefaultValue(1000).IsRequired();
            modelBuilder.Entity<DeviceDto>().Property(p => p.ScaleSamplingSize).HasDefaultValue(1).IsRequired();
            modelBuilder.Entity<DeviceDto>().Property(p => p.ScaleCalibrationWeight).HasDefaultValue(100).IsRequired();
            modelBuilder.Entity<DeviceDto>().Property(p => p.ScaleDisplayTimeout).HasDefaultValue(60000).IsRequired();
            modelBuilder.Entity<DeviceDto>().Property(p => p.RfidDecay).HasDefaultValue(15000).IsRequired();

            modelBuilder.Entity<HistoryEventDto>().HasKey(k => k.Id);
            modelBuilder.Entity<HistoryEventDto>().Property(p => p.SpoolId).IsRequired();
            modelBuilder.Entity<HistoryEventDto>().Property(p => p.Value).IsRequired();
            modelBuilder.Entity<HistoryEventDto>().Property(p => p.Received).IsRequired();
        }
    }
}
