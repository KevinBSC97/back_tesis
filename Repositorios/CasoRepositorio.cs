using Microsoft.EntityFrameworkCore;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;

using XAct;

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
          .ToListAsync();

      var casoDTOs = casos.Select(c => new CasoDTO
      {
        CasoId = c.CasoId,
        Descripcion = c.Descripcion,
        Asunto = c.Asunto,
        Estado = c.Estado,
        NombreAbogado = c.Abogado != null ? c.Abogado.Nombre + " " + c.Abogado.Apellido : "N/A",
        NombreCliente = c.Cliente != null ? c.Cliente.Nombre + " " + c.Cliente.Apellido : "N/A",
        ClienteId = c.ClienteId,
        AbogadoId = c.AbogadoId,
        CitaId = c.CitaId,
        EspecialidadDescripcion = c.Descripcion,
        Imagenes = c.Imagenes != null ? c.Imagenes.Split('|').ToList() : new List<string>()
      }).ToList();


      return casoDTOs;
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
