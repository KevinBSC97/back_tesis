using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesisAdvocorp.Modelos
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        [ForeignKey("Rol")]
        public int RolId { get; set; }
        [ForeignKey("Especialidad")]
        public int? EspecialidadId { get; set; }
        public string Estado { get; set; }

        public Rol Rol { get; set; }
        public Especialidad Especialidad { get; set; }
        public ICollection<Cita> CitasComoCliente { get; set; } // Citas donde el usuario es el cliente
        public ICollection<Cita> CitasComoAbogado { get; set; } // Citas donde el usuario es el abogado
    }
}
