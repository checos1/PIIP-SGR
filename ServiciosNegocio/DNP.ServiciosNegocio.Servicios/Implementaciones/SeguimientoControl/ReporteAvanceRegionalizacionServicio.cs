using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Implementaciones.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl
{
    public class ReporteAvanceRegionalizacionServicio : IReporteAvanceRegionalizacionServicio
    {

        private readonly IReporteAvanceRegionalizacionServicio _ReporteAvanceRegionalizacionServicio;

        #region Constructor

        /// <summary>
        /// Reporte Avance Regionalizacion Servicio
        /// </summary>
        /// <param name="ReporteAvanceRegionalizacionServicio"></param>
        public ReporteAvanceRegionalizacionServicio(IReporteAvanceRegionalizacionServicio ReporteAvanceRegionalizacionServicio)
        {
            _ReporteAvanceRegionalizacionServicio = ReporteAvanceRegionalizacionServicio;
        }

        #endregion

        //public string ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        //{
        //    return _ReporteAvanceProductoPersistencia.ConsultarAvanceMetaProducto(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad);
        //}

        //public Task<ReponseHttp> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string usuario)
        //{
        //    try
        //    {
        //        _ReporteAvanceProductoPersistencia.ActualizarAvanceMetaProducto(IndicadorDto, usuario);
        //        return Task.FromResult<ReponseHttp>(new ReponseHttp() { Status = true });
        //    }
        //    catch (ServiciosNegocioException e)
        //    {
        //        return Task.FromResult<ReponseHttp>(new ReponseHttp()
        //        {
        //            Status = false,
        //            Message = e.Message
        //        });
        //    }
        //}

        public string ConsultarAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            return _ReporteAvanceRegionalizacionServicio.ConsultarAvanceRegionalizacion(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad);
        }
    }
}
