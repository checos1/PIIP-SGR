using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DNP.Backbone.Servicios.Interfaces.SeguimientoControl
{
    public interface IReporteAvanceProductoServicio
    {
        Task<string> ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad, string usuarioDNP);

        Task<ReponseHttp> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string Usuario);
    }
}
