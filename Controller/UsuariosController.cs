using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;
using XAct.Library.Settings;

namespace TesisAdvocorp.Controller
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usrepo;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioRepositorio usrepo, IMapper mapper)
        {
            _usrepo = usrepo;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var usuarios = _usrepo.GetAllAsync().Result;
            var usuarioDTOs = usuarios.Select(u => new UsuarioDTO
            {
                UsuarioId = u.UsuarioId,
                Identificacion = u.Identificacion,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                NombreUsuario = u.NombreUsuario,
                Email = u.Email,
                RolId = u.RolId,
                RolDescripcion = u.Rol?.Descripcion, // Asegúrate de que el Rol no sea null
                EspecialidadId = u.EspecialidadId,
                EspecialidadDescripcion = u.Especialidad?.Descripcion, // Asegúrate de que la Especialidad no sea null
                Estado = u.Estado
            }).ToList();

            return Ok(usuarioDTOs);
        }

        //[HttpPost("registro")]
        //public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDTO usuarioRegistroDTO)
        //{
        //    bool validarNombreUsuarioUnico = _usrepo.IsUniqueUserAsync(usuarioRegistroDTO.NombreUsuario);
        //    if (!validarNombreUsuarioUnico)
        //    {
        //        ModelState.AddModelError("NombreUsuaurio", "El usuario ya está registrado.");
        //        return BadRequest(ModelState);
        //    }

        //    var usuario = await _usrepo.RegisterAsync(usuarioRegistroDTO);
        //    if (usuario == null)
        //    {
        //        return BadRequest("Error al registrar el usuario.");
        //    }

        //    return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.UsuarioId }, usuario);
        //}

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDTO usuarioRegistroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validación de campo numérico para identificación
            if (!int.TryParse(usuarioRegistroDTO.Identificacion, out _))
            {
                return BadRequest("La identificación debe ser numérica.");
            }

            // Verificar la unicidad de la identificación
            bool esIdentificacionUnica = _usrepo.IsUniqueIdentificationAsync(usuarioRegistroDTO.Identificacion);
            if (!esIdentificacionUnica)
            {
                return BadRequest("La identificación ya está en uso.");
            }

            // Verificar la unicidad del nombre de usuario
            bool esNombreUnico = _usrepo.IsUniqueUserAsync(usuarioRegistroDTO.NombreUsuario);
            if (!esNombreUnico)
            {
                return BadRequest("El nombre de usuario ya está en uso.");
            }

            var usuario = await _usrepo.RegisterAsync(usuarioRegistroDTO);
            if (usuario == null)
            {
                return BadRequest("Error al registrar el usuario.");
            }

            return Ok(usuario);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var respuestaLogin = await _usrepo.LoginAsync(loginDTO);

            if(respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                return Unauthorized("Nombre de usuario o contraseña son incorrectos");
            }

            return Ok(respuestaLogin);
        }

        [HttpGet("abogados")]
        public async Task<IActionResult> GetAbogados()
        {
            var abogados = await _usrepo.GetUsuariosPorRolAsync(3); // Asumiendo que el rolId para Abogado es 3
            var abogadosDTO = _mapper.Map<List<UsuarioDTO>>(abogados);
            return Ok(abogadosDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usrepo.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(usuarioDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioEnBd = await _usrepo.GetByIdAsync(id);
            if (usuarioEnBd == null)
            {
                return NotFound();
            }

            // Actualiza las propiedades que deseas permitir cambiar
            usuarioEnBd.Nombre = usuarioDTO.Nombre;
            usuarioEnBd.Apellido = usuarioDTO.Apellido;
            usuarioEnBd.Email = usuarioDTO.Email;
            usuarioEnBd.Estado = usuarioDTO.Estado;
            // No actualizar el nombre de usuario, contraseña, y otras propiedades sensibles directamente

            _usrepo.Update(usuarioEnBd);
            await _usrepo.SaveChangesAsync();

            return NoContent();  // O puedes devolver un resultado 200 con el usuario actualizado
        }
    }
}
