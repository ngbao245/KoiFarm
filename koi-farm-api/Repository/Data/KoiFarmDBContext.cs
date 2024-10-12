using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Data.Entity;
using Repository.Interfaces;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data
{
    public class KoiFarmDbContext : DbContext
    {
        public KoiFarmDbContext()
        {
        }

        public KoiFarmDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> UserEntities { get; set; }
        public DbSet<Role> RoleEntities { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<Blog> BlogEntities { get; set; }
        public DbSet<Cart> CartEntities { get; set; }
        public DbSet<CartItem> CartItemEntities { get; set; }
        public DbSet<Certificate> CertificateEntities { get; set; }
        public DbSet<Order> OrderEntities { get; set; }
        public DbSet<OrderItem> OrderItemEntities { get; set; }
        public DbSet<Payment> PaymentEntities { get; set; }
        public DbSet<Product> ProductEntities { get; set; }
        public DbSet<ProductCertificate> ProductCertificateEntities { get; set; }
        public DbSet<ProductItem> ProductItemEntities { get; set; }
        public DbSet<Promotion> PromotionEntities { get; set; }
        public DbSet<Review> ReviewEntities { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    GetConnectionString(),
                    b => b.MigrationsAssembly("Repository")); // "Repository" is the name of the class library project
            }
        }



        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionStrings:KoiFarm"];
            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Configure the foreign key for the staff who processed the order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Staff)
                .WithMany()
                .HasForeignKey(o => o.StaffId)
                .OnDelete(DeleteBehavior.Restrict);  // Avoid cascade delete

            // Apply global soft delete filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ModelBuilder).GetMethod("Entity", new Type[] { }).MakeGenericMethod(entityType.ClrType);
                    dynamic entityBuilder = method.Invoke(modelBuilder, null);
                    entityBuilder.HasQueryFilter(CreateSoftDeleteFilter(entityType.ClrType));
                }
            }
        }

        private static LambdaExpression CreateSoftDeleteFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var propertyMethod = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
            var isDeletedProperty = Expression.Call(propertyMethod, parameter, Expression.Constant("IsDeleted"));
            var condition = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            return Expression.Lambda(condition, parameter);
        }
    }
}

