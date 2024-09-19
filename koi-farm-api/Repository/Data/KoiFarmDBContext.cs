using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Data.Entity;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}