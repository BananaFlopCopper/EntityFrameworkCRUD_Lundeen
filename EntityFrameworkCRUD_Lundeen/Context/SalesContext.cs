using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using EntityFrameworkCRUD_Lundeen.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Data.SqlClient;

namespace EntityFrameworkCRUD_Lundeen.Context
{
    public partial class SalesContext : DbContext
    {
        public static IConfiguration Config; 
        public SalesContext()
        { 
        }
        
        public SalesContext(DbContextOptions<SalesContext> options, IConfiguration conf)
            : base(options)
        {
            Config = conf;
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;

        public void SaveCustomer(Customer Customer) // This function is so that sql does not reassign 0 to every new customer
        {
            string conn = Config.GetConnectionString("Conn");
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("AddCustomer", connection);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName",   Customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName",    Customer.LastName);
                cmd.Parameters.AddWithValue("@City",        Customer.City);
                cmd.Parameters.AddWithValue("@Country",     Customer.Country);
                cmd.Parameters.AddWithValue("@Phone",       Customer.Phone);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string conn = Config.GetConnectionString("Conn");
                optionsBuilder.UseSqlServer(conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.Country).HasMaxLength(40);

                entity.Property(e => e.FirstName).HasMaxLength(40);

                entity.Property(e => e.LastName).HasMaxLength(40);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
