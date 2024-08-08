using Microsoft.EntityFrameworkCore;
using TesisAdvocorp.Modelos;

namespace TesisAdvocorp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario>  Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Caso> Casos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Cliente)
                .WithMany(u => u.CitasComoCliente)  // Asegúrate de que 'CitasComoCliente' sea una propiedad en 'Usuario'
                .HasForeignKey(c => c.ClienteId);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Abogado)
                .WithMany(u => u.CitasComoAbogado)  // Asegúrate de que 'CitasComoAbogado' sea una propiedad en 'Usuario'
                .HasForeignKey(c => c.AbogadoId)   
                .IsRequired(false);  // Dado que 'AbogadoId' es opcional
        }
    }
}