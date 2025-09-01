using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl
{
    public interface IReporteAvanceProductoServicio
    {
        string ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad);
        string ConsultarAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad);
        string ConsultarResumenRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin);
        Task<ReponseHttp> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string usuario);
        Task<ReponseHttp> GuardarRegionalizacion(AvanceRegionalizacionDto IndicadorDto, string usuario);
        string ObtenerDetalleRegionalizacionProgramacionSeguimiento(string json);
    }
}
