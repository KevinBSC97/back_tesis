using TesisAdvocorp.Modelos;

namespace TesisAdvocorp.Repositorios.IRepositorios
{
    public interface IRolRepositorio
    {
        Task<Rol> GetByIdAsync(int rolId);
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol> AddAsync(Rol rol);
        Task UpdateAsync(Rol rol);
        Task DeleteAsync(int rolId);
    }
}
