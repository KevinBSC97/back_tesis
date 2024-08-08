using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Controller
{
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRolRepositorio _rolRepo;
        private readonly IMapper _mapper;

        public RolesController(IRolRepositorio rolRepo, IMapper mapper)
        {
            _rolRepo = rolRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetRoles()
        {
            var roles = await _rolRepo.GetAllAsync();
            var rolesDTO = _mapper.Map<IEnumerable<RolDTO>>(roles);
            return Ok(rolesDTO);
        }

        [HttpGet("{rolId}")] // Asegúrate de que el nombre del parámetro en la ruta coincida con el nombre del parámetro del método.
        public async Task<ActionResult<RolDTO>> GetRol(int rolId) // Cambia 'id' a 'rolId'
        {
            var rol = await _rolRepo.GetByIdAsync(rolId);
            if (rol == null)
            {
                return NotFound();
            }
            var rolDTO = _mapper.Map<RolDTO>(rol);
            return Ok(rolDTO);
        }

        [HttpPost("crearRol")]
        public async Task<ActionResult<RolDTO>> PostRol(RolDTO rolDTO)
        {
            var rol = _mapper.Map<Rol>(rolDTO);
            await _rolRepo.AddAsync(rol);
            var rolDTOCreated = _mapper.Map<RolDTO>(rol);
            return CreatedAtAction(nameof(GetRol), new { id = rol.RolId }, rolDTOCreated);
        }
    }
}
