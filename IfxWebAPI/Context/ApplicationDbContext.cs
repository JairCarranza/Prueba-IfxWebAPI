using IfxWebAPI.Entities;
using IfxWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IfxWebAPI.Contents
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roleAdmin = new IdentityRole()
            {
                Id = "89086180-b978-4f90-9dbd-a7040bc93f41",
                Name = "admin",
                NormalizedName = "admin"
            };

            builder.Entity<IdentityRole>().HasData(roleAdmin);//Creación de Rol Admin

            base.OnModelCreating(builder);
        }

        public DbSet<Entidad> Entidades { get; set; }

        public DbSet<Empleado> Empleados { get; set; }


    }
}
