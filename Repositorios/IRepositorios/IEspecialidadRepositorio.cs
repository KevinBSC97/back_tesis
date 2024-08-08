using TesisAdvocorp.Modelos;

namespace TesisAdvocorp.Repositorios.IRepositorios
{
    public interface IEspecialidadRepositorio
    {
        Task<Especialidad> GetByIdAsync(int especialidadId);
        Task<IEnumerable<Especialidad>> GetAllAsync();
        Task<Especialidad> AddAsync(Especialidad especialidad);
        Task UpdateAsync(Especialidad especialidad);
        Task DeleteAsync(int especialidadId);
        Task<IEnumerable<Usuario>> GetAbogadosByEspecialidadAsync(int especialidadId);
    }
}
