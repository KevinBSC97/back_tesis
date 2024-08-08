using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;

namespace TesisAdvocorp.Repositorios.IRepositorios
{
    public interface IUsuarioRepositorio
    {
        Task<IEnumerable<Usuario>> GetAllAsync(); // Usando Task para operaciones asincrónicas
        Task<Usuario> GetByIdAsync(int usuarioId);
        bool IsUniqueUserAsync(string usuario); // Verifica si el usuario ya está registrado
        Task<ResponseDTO> LoginAsync(LoginDTO loginDTO); // Procesa el login
        Task<Usuario> RegisterAsync(UsuarioRegistroDTO usuarioRegistroDTO);
        Task<IEnumerable<Usuario>> GetUsuariosPorRolAsync(int rolId);
        Task<IEnumerable<Usuario>> GetAbogadosByEspecialidadAsync(int especialidadId);
        void Update(Usuario usuario);  // Añadir esta línea
        Task<int> SaveChangesAsync();
        bool IsUniqueIdentificationAsync(string identificacion);
    }
}
