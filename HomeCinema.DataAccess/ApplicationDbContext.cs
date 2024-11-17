using HomeCinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeCinema.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        { 

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }
        public DbSet<Company> Companies { get; set; }



        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Company>().HasData(
                new Company { Id =1,Name= "Com1",Address="",State="",City="",PhoneNumber="",PostalCode="",Email="" },
                new Company { Id = 2, Name = "Com2", Address = "", State = "", City = "", PhoneNumber = "", PostalCode = "", Email = "" },
                new Company { Id = 3, Name = "Com3", Address = "", State = "", City = "", PhoneNumber = "", PostalCode = "", Email = "" }
                );
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }



    }
}
