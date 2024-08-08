using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesisAdvocorp.Modelos
{
    public class Cita
    {
        [Key]
        public int CitaId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Usuario Cliente { get; set; }

        [ForeignKey("Abogado")]
        public int AbogadoId { get; set; }  // Este campo es opcional
        public Usuario Abogado { get; set; }

        public string Estado { get; set; }
    }
}
