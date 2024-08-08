using System.ComponentModel.DataAnnotations;

namespace TesisAdvocorp.Modelos
{
    public class Especialidad
    {
        [Key]
        public int EspecialidadId { get; set; }
        public string Descripcion { get; set; }

        // Navegación
        public ICollection<Usuario> Abogados { get; set; }
    }
}
