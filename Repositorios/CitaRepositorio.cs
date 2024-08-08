using Microsoft.EntityFrameworkCore;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Repositorios
{
    public class CitaRepositorio : ICitaRepositorio
    {
        private readonly AppDbContext _db;

        public CitaRepositorio(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Cita> AddAsync(Cita cita)
        {
            _db.Citas.Add(cita);
            await _db.SaveChangesAsync();
            return cita;
        }

        public Task DeleteAsync(int citaId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _db.Citas.Include(c => c.Cliente).Include(c => c.Abogado).ToListAsync();
        }

        //public async Task<IEnumerable<Cita>> GetByAbogadoIdAsync(int abogadoId)
        //{
        //    // Asegúrate de que las inclusiones están correctamente configuradas
        //    return await _db.Citas
        //        .Include(c => c.Cliente)
        //        .Include(c => c.Abogado)
        //            .ThenInclude(a => a.Especialidad)
        //        .Where(c => c.AbogadoId == abogadoId && c.Abogado != null && c.Abogado.Especialidad != null)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Cita>> GetByAbogadoIdAsync(int abogadoId)
        {
            return await _db.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Abogado)
                    .ThenInclude(a => a.Especialidad)
                .Where(c => c.AbogadoId == abogadoId)
                .ToListAsync();
        }

        public async Task<CitaDTO> GetCitaDetailsAsync(int citaId)
        {
            var cita = await _db.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Abogado)
                    .ThenInclude(a => a.Especialidad)
                .FirstOrDefaultAsync(c => c.CitaId == citaId);

            if (cita == null)
                return null;

            return new CitaDTO
            {
                CitaId = cita.CitaId,
                Descripcion = cita.Descripcion,
                NombreCliente = cita.Cliente != null ? $"{cita.Cliente.Nombre} {cita.Cliente.Apellido}" : "No disponible",
                NombreAbogado = cita.Abogado != null ? $"{cita.Abogado.Nombre} {cita.Abogado.Apellido}" : "No asignado",
                Especialidad = cita.Abogado?.Especialidad?.Descripcion ?? "Sin Especialidad",
                FechaHora = cita.FechaHora,
                Estado = cita.Estado,
                ClienteId = cita.ClienteId,  // Asegurarse de que este valor se está capturando correctamente
                AbogadoId = cita.AbogadoId   // Asegurarse de que este valor se está capturando correctamente
            };
        }

        public async Task<IEnumerable<Cita>> GetByClienteIdAsync(int clienteId)
        {
            return await _db.Citas.Where(c => c.ClienteId == clienteId).ToListAsync();
        }

        public async Task<Cita> GetByIdAsync(int citaId)
        {
            return await _db.Citas.FindAsync(citaId);
        }

        public async Task UpdateAsync(Cita cita)
        {
            _db.Entry(cita).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<int> GetNumeroDeCitasPendientes(int abogadoId)
        {
            return await _db.Citas
                            .Where(c => c.AbogadoId == abogadoId && c.Estado == "Pendiente")
                            .CountAsync();
        }

        public async Task<IEnumerable<Cita>> GetCitasAceptadasPorAbogado(int abogadoId)
        {
            return await _db.Citas
                                 .Where(c => c.AbogadoId == abogadoId && c.Estado == "Aceptado")
                                 .ToListAsync();
        }
    }
}
