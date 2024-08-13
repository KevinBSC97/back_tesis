using AutoMapper;
using TesisAdvocorp.Modelos.Dtos;
using TesisAdvocorp.Modelos;
using XAct.Users;

namespace TesisAdvocorp.AutoMapper
{
    public class TesisMapper : Profile
    {
        public TesisMapper()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Cita, CitaDTO>().ReverseMap();
            CreateMap<Caso, CasoDTO>()
              .ForMember(dest => dest.Imagenes, opt => opt.Ignore()) 
              .ReverseMap() 
              .ForMember(dest => dest.Imagenes, opt => opt.Ignore());
            CreateMap<Rol, RolDTO>().ReverseMap();
            CreateMap<Especialidad, EspecialidadDTO>().ReverseMap();
            CreateMap<Notificacion, NotificacionDTO>().ReverseMap();
        }
    }
}
