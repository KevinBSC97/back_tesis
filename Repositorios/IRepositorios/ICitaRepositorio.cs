using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;

namespace TesisAdvocorp.Repositorios.IRepositorios
{
    public interface ICitaRepositorio
    {
        Task<Cita> GetByIdAsync(int citaId);
        Task<CitaDTO> GetCitaDetailsAsync(int citaId);
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<IEnumerable<Cita>> GetByClienteIdAsync(int clienteId); // Para que los clientes vean sus citas
        //Task<IEnumerable<Cita>> GetByAbogadoIdAsync(int abogadoId); // Para que los abogados vean las citas asignadas a ellos
        Task<IEnumerable<Cita>> GetByAbogadoIdAsync(int abogadoId);
        Task<Cita> AddAsync(Cita cita); // Para que los clientes agreguen nuevas citas
        Task UpdateAsync(Cita cita); // Para actualizar el estado de la cita, como aceptar o rechazar
        Task DeleteAsync(int citaId);
        Task<int> GetNumeroDeCitasPendientes(int abogadoId);
        Task<IEnumerable<Cita>> GetCitasAceptadasPorAbogado(int abogadoId);
    }
}
