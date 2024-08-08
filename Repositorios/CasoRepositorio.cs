using Microsoft.EntityFrameworkCore;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Repositorios
{
    public class CasoRepositorio : ICasoRepositorio
    {
        private readonly AppDbContext _db;

        public CasoRepositorio(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Caso> AddAsync(Caso caso)
        {
            _db.Casos.Add(caso);
            await _db.SaveChangesAsync();
            return caso;
        }

        public async Task DeleteAsync(int casoId)
        {
            var caso = await _db.Casos.FindAsync(casoId);
            if (caso != null)
            {
                _db.Casos.Remove(caso);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CasoDTO>> GetAllAsync()
        {
            var casos = await _db.Casos
                .Include(c => c.Cliente)
                .Include(c => c.Abogado)
                .Select(c => new CasoDTO
                {
                    CasoId = c.CasoId,
                    Descripcion = c.Descripcion,
                    Asunto = c.Asunto,
                    Estado = c.Estado,
                    NombreAbogado = c.Abogado.Nombre + " " + c.Abogado.Apellido,
                    NombreCliente = c.Cliente.Nombre + " " + c.Cliente.Apellido,
                    ClienteId = c.ClienteId,
                    AbogadoId = c.AbogadoId,
                    CitaId = c.CitaId,
                    EspecialidadDescripcion = c.Descripcion,
                    FechaCita = c.Cita.FechaHora
                })
                .ToListAsync();

            return casos;
        }

        public async Task<IEnumerable<Caso>> GetByAbogadoIdAsync(int abogadoId)
        {
            return await _db.Casos
                .Where(c => c.AbogadoId == abogadoId && c.Cita.Estado == "Aceptada")
                .ToListAsync();
        }

        public async Task<IEnumerable<Caso>> GetByClienteIdAsync(int clienteId)
        {
            return await _db.Casos.Where(c => c.ClienteId == clienteId).ToListAsync();
        }

        public async Task<Caso> GetByIdAsync(int casoId)
        {
            return await _db.Casos.FindAsync(casoId);
        }

        public async Task UpdateAsync(Caso caso)
        {
            _db.Entry(caso).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Caso>> GetCasosByCitaIdAsync(int citaId)
        {
            return await _db.Casos
                .Where(c => c.CitaId == citaId)
                .ToListAsync();
        }
    }
}
