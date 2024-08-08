using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;
using XSystem.Security.Cryptography;

namespace TesisAdvocorp.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AppDbContext _db;
        private string claveSecreta;

        public UsuarioRepositorio(AppDbContext db, IConfiguration config)
        {
            _db = db;
            this.claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _db.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Especialidad) // Asegúrate de incluir especialidades
                .ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(int usuarioId)
        {
            return await _db.Usuarios
                .Include(u => u.Rol)  // Asegúrate de cargar el rol
                .Include(u => u.Especialidad)  // y la especialidad si es necesario
                .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
        }

        public bool IsUniqueUserAsync(string usuario)
        {
            var usuariobd = _db.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario);

            if (usuariobd == null)
            {
                return true;
            }

            return false;
        }

        public async Task<ResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Contraseña))
            {
                throw new ArgumentException("La contraseña proporcionada es nula o vacía.");
            }

            var passwordEncriptado = obtenermd5(loginDTO.Contraseña);
            //var usuario = _db.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToLower() == loginDTO.NombreUsuario.ToLower()
            //                && u.Contraseña == passwordEncriptado);
            var usuario = _db.Usuarios
                        .Include(u => u.Rol)  // Incluye el Rol para asegurar que no sea null
                        .FirstOrDefault(u => u.NombreUsuario.ToLower() == loginDTO.NombreUsuario.ToLower()
                                             && u.Contraseña == passwordEncriptado);

            if (usuario == null)
            {
                return new ResponseDTO { Token = "", Usuario = null };
            }

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);  // Asegúrate de que claveSecreta no sea null

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, usuario.Rol.Descripcion)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            return new ResponseDTO
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };
        }

        public async Task<Usuario> RegisterAsync(UsuarioRegistroDTO usuarioRegistroDTO)
        {
            var passwordEncriptado = obtenermd5(usuarioRegistroDTO.Password);

            Usuario usuario = new Usuario()
            {
                Identificacion = usuarioRegistroDTO.Identificacion,
                Nombre = usuarioRegistroDTO.Nombre,
                Apellido = usuarioRegistroDTO.Apellido,
                NombreUsuario = usuarioRegistroDTO.NombreUsuario,
                Email = usuarioRegistroDTO.Email,
                Contraseña = obtenermd5(usuarioRegistroDTO.Password),
                RolId = usuarioRegistroDTO.RolId,
                
                EspecialidadId = usuarioRegistroDTO.EspecialidadId,
                Estado = usuarioRegistroDTO.Estado
            };

            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
            return usuario;
        }

        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosPorRolAsync(int rolId)
        {
            return await _db.Usuarios.Where(u => u.RolId == rolId).ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetAbogadosByEspecialidadAsync(int especialidadId)
        {
            return await _db.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Especialidad)
                .Where(u => u.Rol.Descripcion == "Abogado" && u.EspecialidadId == especialidadId)
                .ToListAsync();
        }

        public void Update(Usuario usuario)
        {
            _db.Entry(usuario).State = EntityState.Modified;  // Marca la entidad como modificada
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();  // Guarda los cambios en la base de datos
        }

        public bool IsUniqueIdentificationAsync(string identificacion)
        {
            return !_db.Usuarios.Any(u => u.Identificacion == identificacion);
        }
    }
}
