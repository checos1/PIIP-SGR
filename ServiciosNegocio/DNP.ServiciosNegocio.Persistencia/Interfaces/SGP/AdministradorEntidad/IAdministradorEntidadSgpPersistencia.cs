using DNP.ServiciosNegocio.Dominio.Dto.AdministradorEntidad;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.AdministradorEntidad
{
    public interface IAdministradorEntidadSgpPersistencia
    {
        string ObtenerSectores();
        string ObtenerFlowCatalog();
        List<ConfiguracionMatrizEntidadDestinoSGRDto> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario);
        RespuestaGeneralDto ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario);
    }
}
