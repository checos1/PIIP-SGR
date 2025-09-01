using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl
{
    public interface IReporteAvanceRegionalizacionServicio
    {
        string ConsultarAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad);
        //Task<ReponseHttp> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string usuario);
    }
}
