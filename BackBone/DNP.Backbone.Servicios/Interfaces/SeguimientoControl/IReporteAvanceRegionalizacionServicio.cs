using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DNP.Backbone.Servicios.Interfaces.SeguimientoControl
{
    public interface IReporteAvanceRegionalizacionServicio
    {
        Task<string> ConsultarAvanceRegionalizacion(Guid? instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad, string usuarioDnp);

        Task<ReponseHttp> GuardarAvanceRegionalizacion(AvanceRegionalizacionDto IndicadorDto, string Usuario);

        Task<string> ConsultarResumenAvanceRegionalizacion(Guid? instanciaId, int proyectoId, string codigoBpin, string usuarioDnp);
        Task<string> ObtenerDetalleRegionalizacionProgramacionSeguimiento(string json, string usuarioDNP);
    }
}