using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales
{
    public interface ICacheServicio
    {
        Task<ProyectoDto> ObtenerProyecto(string bpin);
    }
}
