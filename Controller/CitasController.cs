using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using TesisAdvocorp.Data;
using TesisAdvocorp.Modelos;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Repositorios.IRepositorios;
using XAct;

namespace TesisAdvocorp.Controller
{
    [Route("api/citas")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ICitaRepositorio _citaRepo;
        private readonly IUsuarioRepositorio _usuarioRepo;
        private readonly IMapper _mapper;

        public CitasController(ICitaRepositorio citaRepo, IUsuarioRepositorio usuarioRepo, IMapper mapper)
        {
            _citaRepo = citaRepo;
            _usuarioRepo = usuarioRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaDTO>>> GetCitas()
        {
            var citas = await _citaRepo.GetAllAsync();
            var citasDTO = _mapper.Map<IEnumerable<CitaDTO>>(citas);
            return Ok(citasDTO);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<CitaDTO>>> GetCitasByCliente(int clienteId)
        {
            var citas = await _citaRepo.GetByClienteIdAsync(clienteId);
            var citasDTO = _mapper.Map<IEnumerable<CitaDTO>>(citas);
            return Ok(citasDTO);
        }

        [HttpGet("abogado/{abogadoId}")]
        public async Task<ActionResult<IEnumerable<CitaDTO>>> GetCitasByAbogado(int abogadoId)
        {
            var citas = await _citaRepo.GetByAbogadoIdAsync(abogadoId);

            var citasDTO = citas.Select(c => new CitaDTO
            {
                CitaId = c.CitaId,
                FechaHora = c.FechaHora,
                Descripcion = c.Descripcion,
                NombreCliente = c.Cliente != null ? $"{c.Cliente.Nombre} {c.Cliente.Apellido}" : "No disponible",
                NombreAbogado = c.Abogado != null ? $"{c.Abogado.Nombre} {c.Abogado.Apellido}" : "No disponible",
                Especialidad = c.Abogado?.Especialidad?.Descripcion ?? "No especificada",
                Estado = c.Estado,
                Duracion = c.Duracion
            }).ToList();

            return Ok(citasDTO);
        }

        [HttpPost]
        [HttpPost]
    public async Task<ActionResult<CitaDTO>> CrearCita(CitaDTO citaDTO)
    {
      var usuario = await _usuarioRepo.GetByIdAsync(citaDTO.ClienteId);
      if (usuario == null || usuario.RolId != 2) // Verifica si el usuario es un cliente válido
      {
        return BadRequest("Cliente no válido");
      }

      // Comprueba si hay citas pendientes con el mismo abogado que no estén rechazadas
      var citasPrevias = await _citaRepo.GetByClienteIdAsync(citaDTO.ClienteId);
      var citaPendiente = citasPrevias.FirstOrDefault(c => c.AbogadoId == citaDTO.AbogadoId && c.Estado != "Rechazada");
      if (citaPendiente != null)
      {
        return BadRequest($"Ya existe una cita pendiente con el abogado. Espera que se resuelva antes de crear una nueva.");
      }

      // Verifica si la nueva cita se solapa con citas existentes del mismo abogado
      var todasCitas = await _citaRepo.GetAllAsync();

      foreach (var itemCita in todasCitas)
      {
        if (itemCita.AbogadoId == citaDTO.AbogadoId && itemCita.Estado != "Rechazada")
        {
          var fechaInicioExistente = itemCita.FechaHora;
          var fechaFinExistente = fechaInicioExistente.AddMinutes(itemCita.Duracion); // Asumiendo que la duración está en minutos

          var nuevaFechaInicio = citaDTO.FechaHora;
          var nuevaFechaFin = nuevaFechaInicio.AddMinutes(citaDTO.Duracion);

          // Verificar si la nueva cita se solapa con una existente
          if (nuevaFechaInicio < fechaFinExistente && nuevaFechaFin > fechaInicioExistente)
          {
            return BadRequest("La nueva cita se solapa con otra existente en el mismo horario.");
          }
        }
      }

      // Mapear y guardar la nueva cita
      var cita = _mapper.Map<Cita>(citaDTO);
      cita.Estado = "Pendiente";

      var nuevaCita = await _citaRepo.AddAsync(cita);
      var nuevaCitaDTO = _mapper.Map<CitaDTO>(nuevaCita);

      var abogado = await _usuarioRepo.GetByIdAsync(cita.AbogadoId);
      if (abogado != null)
      {
        await EnviarCorreoAsync(abogado.Email, "Nueva Cita Asignada",
            $"Se te ha asignado una nueva cita con detalles: {cita.Descripcion} para el {cita.FechaHora}");
      }

      return CreatedAtAction(nameof(GetCitas), new { id = nuevaCitaDTO.CitaId }, nuevaCitaDTO);
    }


    [HttpGet("cita/{citaId}/detalles")]
        public async Task<ActionResult<CitaDTO>> GetCitaDetails(int citaId)
        {
            var citaDto = await _citaRepo.GetCitaDetailsAsync(citaId);
            if (citaDto == null)
                return NotFound("Cita no encontrada");

            return Ok(citaDto);
        }

        [HttpGet("abogado/{abogadoId}/pendientes")]
        public async Task<IActionResult> GetNumeroDeCitasPendientes(int abogadoId)
        {
            var numeroDeCitasPendientes = await _citaRepo.GetNumeroDeCitasPendientes(abogadoId);
            return Ok(new { numeroDeCitasPendientes });
        }

        private async Task EnviarCorreoAsync(string destinatario, string asunto, string mensaje)
        {
            using (var client = new SmtpClient("smtp.office365.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("kevin.sotomayorc@ug.edu.ec", "Bsc97123456258");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("kevin.sotomayorc@ug.edu.ec"),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(destinatario);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
