using IfxWebAPI.Entities;
using IfxWebAPI.Models;
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

        public DbSet<Entidad> Entidades { get; set; }

        public DbSet<Empleado> Empleados { get; set; }


    }
}
