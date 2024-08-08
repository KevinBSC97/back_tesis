using System.ComponentModel.DataAnnotations;

namespace TesisAdvocorp.Modelos
{
    public class Rol
    {
        [Key]
        public int RolId { get; set; }
        public string Descripcion { get; set; }
    }
}
