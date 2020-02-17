using IfxWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace IfxWebAPI.Contents
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Entidad> Entidades { get; set; }

        public DbSet<Empleado> Empleados { get; set; }
    }
}
