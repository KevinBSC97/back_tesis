using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesisAdvocorp.Modelos
{
    public class Caso
    {
        [Key]
        public int CasoId { get; set; }
        public string Descripcion { get; set; }
        public string Asunto { get; set; }  // Detalle específico del caso
        public string Estado { get; set; }  // Ejemplo: "Abierto", "Cerrado"
        [ForeignKey("Abogado")]
        public int AbogadoId { get; set; }
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        [ForeignKey("Cita")]  // Relacionar caso con cita
        public int CitaId { get; set; }
        public Usuario Abogado { get; set; }
        public Usuario Cliente { get; set; }
        public Cita Cita { get; set; }  // Incluir la relación de navegación a Cita
    }
}
