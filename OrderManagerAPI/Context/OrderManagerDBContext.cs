using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using OrderManagerAPI.Models;

namespace OrderManagerAPI.Context
{
    public class OrderManagerDBContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public OrderManagerDBContext() : base("ConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Client>()
            .HasIndex(c => c.Email)
            .IsUnique();
        }

    }
}