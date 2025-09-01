using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl
{
    public interface IGestionSeguimientoPersistencia
    {
        List<ErroresProyectoDto> ObtenerErroresProyecto(GestionSeguimientoDto proyecto);
        List<TransversalSeguimientoDto> ObtenerListadoUnidadesMedida();
    }
}
