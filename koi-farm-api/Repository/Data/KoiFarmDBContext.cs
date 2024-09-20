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

        //Global query filter for soft deletion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply the global soft delete filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Check if the entity implements ISoftDelete
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    // Apply a global query filter
                    var method = typeof(ModelBuilder).GetMethod("Entity", new Type[] { }).MakeGenericMethod(entityType.ClrType);
                    dynamic entityBuilder = method.Invoke(modelBuilder, null);
                    entityBuilder.HasQueryFilter(CreateSoftDeleteFilter(entityType.ClrType));
                }
            }

            base.OnModelCreating(modelBuilder);
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