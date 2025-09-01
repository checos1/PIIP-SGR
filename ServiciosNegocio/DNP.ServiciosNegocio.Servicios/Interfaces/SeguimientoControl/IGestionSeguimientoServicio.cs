using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl
{
    public interface IGestionSeguimientoServicio
    {
        Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(GestionSeguimientoDto proyecto);

        Task<List<TransversalSeguimientoDto>> ObtenerListadoUnidadesMedida();

    }
}
