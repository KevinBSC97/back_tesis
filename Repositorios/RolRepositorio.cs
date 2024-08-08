using Microsoft.EntityFrameworkCore;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Repositorios
{
    public class RolRepositorio : IRolRepositorio
    {
        private readonly AppDbContext _db;

        public RolRepositorio(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Rol> AddAsync(Rol rol)
        {
            _db.Roles.Add(rol);
            await _db.SaveChangesAsync();
            return rol;
        }

        public Task DeleteAsync(int rolId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _db.Roles.ToListAsync();
        }

        public async Task<Rol> GetByIdAsync(int rolId)
        {
            return await _db.Roles.FindAsync(rolId);
        }

        public async Task UpdateAsync(Rol rol)
        {
            throw new NotImplementedException();
        }
    }
}
