using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;

namespace TesisAdvocorp.Repositorios.IRepositorios
{
    public interface ICasoRepositorio
    {
        Task<Caso> GetByIdAsync(int casoId);
        //Task<IEnumerable<Caso>> GetAllAsync();
        Task<IEnumerable<CasoDTO>> GetAllAsync();
        Task<IEnumerable<Caso>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Caso>> GetByAbogadoIdAsync(int abogadoId);
        Task<IEnumerable<Caso>> GetCasosByCitaIdAsync(int citaId);
        Task<Caso> AddAsync(Caso caso);
        Task UpdateAsync(Caso caso);
        Task DeleteAsync(int casoId);
    }
}
