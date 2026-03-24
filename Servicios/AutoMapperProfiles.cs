using AutoMapper;
using TareasMVC.Entidades;
using TareasMVC.Models;

namespace TareasMVC.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Aquí puedes definir tus perfiles de mapeo
            CreateMap<Tarea, TareaDTO>();
        }
    }
}
