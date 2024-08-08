using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios;
using TesisAdvocorp.Repositorios.IRepositorios;

namespace TesisAdvocorp.Controller
{
    [Route("api/especialidades")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadRepositorio _especialidadRepositorio;
        private readonly IUsuarioRepositorio _userRepositorio;
        private readonly IMapper _mapper;

        public EspecialidadesController(IEspecialidadRepositorio especialidadRepositorio, IUsuarioRepositorio userRepository, IMapper mapper)
        {
            _especialidadRepositorio = especialidadRepositorio;
            _userRepositorio = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEspecialidades()
        {
            var especialidades = await _especialidadRepositorio.GetAllAsync();
            return Ok(especialidades);
        }

        [HttpGet("{especialidadId}/abogados")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetAbogadosByEspecialidad(int especialidadId)
        {
            var abogados = await _userRepositorio.GetAbogadosByEspecialidadAsync(especialidadId);
            return Ok(_mapper.Map<IEnumerable<UsuarioDTO>>(abogados));
        }
    }
}
