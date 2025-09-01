using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl
{
    public interface IReporteAvanceProductoPersistencia
    {
        string ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad);
        string ConsultarAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad);
        string ConsultarResumenRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin);
        void ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string usuario);
        void GuardarRegionalizacion(AvanceRegionalizacionDto IndicadorDto, string usuario);
        string ObtenerDetalleRegionalizacionProgramacionSeguimiento(string json);
    }
}
