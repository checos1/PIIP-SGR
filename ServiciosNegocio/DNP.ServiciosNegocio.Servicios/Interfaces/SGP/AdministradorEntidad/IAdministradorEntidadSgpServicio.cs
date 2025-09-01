using DNP.ServiciosNegocio.Dominio.Dto.AdministradorEntidad;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.AdministradorEntidad
{
    public interface IAdministradorEntidadSgpServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        string ObtenerSectores();
        string ObtenerFlowCatalog();
        List<ConfiguracionMatrizEntidadDestinoSGRDto> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario);
        Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario);
    }
}
