using Microsoft.EntityFrameworkCore;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Repositorios
{
    public class EspecialidadRepositorio : IEspecialidadRepositorio
    {
        private readonly AppDbContext _db;

        public EspecialidadRepositorio(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Especialidad> AddAsync(Especialidad especialidad)
        {
            _db.Especialidades.Add(especialidad);
            await _db.SaveChangesAsync();
            return especialidad;
        }

        public Task DeleteAsync(int especialidadId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Especialidad>> GetAllAsync()
        {
            return await _db.Especialidades.ToListAsync();
        }

        public Task<Especialidad> GetByIdAsync(int especialidadId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Especialidad especialidad)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> GetAbogadosByEspecialidadAsync(int especialidadId)
        {
            return await _db.Usuarios.Where(u => u.EspecialidadId == especialidadId && u.RolId == 3).ToListAsync();
        }
    }
}
