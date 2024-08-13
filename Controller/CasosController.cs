using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;

using XAct;

namespace TesisAdvocorp.Controller
{
    [Route("api/casos")]
    [ApiController]
    public class CasosController : ControllerBase
    {
        private readonly ICasoRepositorio _casoRepo;
        private readonly IMapper _mapper;

        public CasosController(ICasoRepositorio casoRepo, IMapper mapper)
        {
            _casoRepo = casoRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CasoDTO>>> GetCasos()
        {
            var casos = await _casoRepo.GetAllAsync();
            if (casos == null)
                return NotFound("No se encontraron casos.");

            return Ok(casos);
        }

        [HttpGet("abogado/{abogadoId}")]
        public async Task<ActionResult<IEnumerable<CasoDTO>>> GetCasosByAbogadoId(int abogadoId)
        {
            var casos = await _casoRepo.GetByAbogadoIdAsync(abogadoId);
            var casosDTO = _mapper.Map<IEnumerable<CasoDTO>>(casos);
            return Ok(casosDTO);
        }

    [HttpPost]
    public async Task<IActionResult> CrearCaso(CasoDTO casoDTO)
    {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }

        var caso = _mapper.Map<Caso>(casoDTO);

        string imagenesConcatenadas = string.Join("|", casoDTO.Imagenes);
        caso.Imagenes = imagenesConcatenadas;

        caso = await _casoRepo.AddAsync(caso);
        if (caso == null)
        {
          return BadRequest("Error al guardar el caso");
        }

        var casoResponse = _mapper.Map<CasoDTO>(caso);
        casoResponse.Imagenes = caso.Imagenes.Split('|').ToList();

        return CreatedAtAction(nameof(GetCasos), new { id = caso.CasoId }, casoResponse);
    }


    [HttpGet("cita/{citaId}")]
        public async Task<IActionResult> GetCasosByCitaId(int citaId)
        {
            var casos = await _casoRepo.GetCasosByCitaIdAsync(citaId);
            var casosDTO = _mapper.Map<IEnumerable<CasoDTO>>(casos);
            return Ok(casosDTO);
        }

        [HttpGet("{casoId:int}", Name = "GetByIdAsync")] // Asegúrate de que el nombre y el parámetro coincidan con los usados en CreatedAtAction
        public async Task<ActionResult<CasoDTO>> GetByIdAsync(int casoId)
        {
            var caso = await _casoRepo.GetByIdAsync(casoId);
            if (caso == null)
            {
                return NotFound();
            }

            var casoDTO = _mapper.Map<CasoDTO>(caso);
            return Ok(casoDTO);
        }

        [HttpPut("{casoId}")]
        public async Task<IActionResult> UpdateCaso(int casoId, CasoDTO casoDTO)
        {
            // Verificar si el DTO es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el caso existe
            var existingCaso = await _casoRepo.GetByIdAsync(casoId);
            if (existingCaso == null)
            {
                return NotFound($"Caso con ID {casoId} no encontrado.");
            }

            // Mapear el DTO al caso
            var casoToUpdate = _mapper.Map(casoDTO, existingCaso);

            try
            {
                await _casoRepo.UpdateAsync(casoToUpdate);
                return NoContent(); // Indica que la actualización fue exitosa pero no hay contenido para retornar
            }
            catch (Exception ex)
            {
                // Manejar errores, por ejemplo, errores de base de datos
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar el caso: {ex.Message}");
            }
        }
    }
}
