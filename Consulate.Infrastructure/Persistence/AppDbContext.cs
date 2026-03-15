using Consulate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        //add other DbSet properties for your entities here
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            //base.OnModelCreating(modelBuilder);
            builder.Entity<UserRole>()
           .HasKey(ur => new { ur.UserId, ur.RoleId });

            base.OnModelCreating(builder);
        }
    }
}
