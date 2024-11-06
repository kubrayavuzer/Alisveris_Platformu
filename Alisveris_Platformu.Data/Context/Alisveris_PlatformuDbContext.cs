using Alisveris_Platformu.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Data.Context
{
    public class Alisveris_PlatformuDbContext : DbContext
    {
        public Alisveris_PlatformuDbContext(DbContextOptions<Alisveris_PlatformuDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("YourConnectionString",
                    b => b.MigrationsAssembly("Alisveris_Platformu.WebApi")); // Burayı güncelleyin
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //fulent api

            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<SettingEntity>().HasData(
                new SettingEntity
                {
                    Id = 1,
                    MaintenenceMode = false
                });

            // OrderProductEntity için ilişkilerin yapılandırılması
            modelBuilder.Entity<OrderProductEntity>()
                .HasKey(op => new { op.OrderId, op.ProductId }); // Composite key tanımı

            modelBuilder.Entity<OrderProductEntity>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts) // Eğer ProductEntity'de OrderProducts koleksiyonu varsa
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Silme davranışını kısıtlıyoruz

            modelBuilder.Entity<OrderProductEntity>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts) // Eğer OrderEntity'de OrderProducts koleksiyonu varsa
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Silme davranışını kısıtlıyoruz


            base.OnModelCreating(modelBuilder);
        }


        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderProductEntity> OrderProducts => Set<OrderProductEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<SettingEntity> Settings => Set<SettingEntity>();


    }
}
