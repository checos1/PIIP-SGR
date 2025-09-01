using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl
{
    public class ReporteAvanceProductoServicio: IReporteAvanceProductoServicio
    {

        private readonly IReporteAvanceProductoPersistencia _ReporteAvanceProductoPersistencia;

        #region Constructor

        /// <summary>
        /// Constructor SeccionCapituloServicio
        /// </summary>
        /// <param name="secccionCapituloPersistencia"></param>
        /// <param name="fasePersistencia"></param>
        public ReporteAvanceProductoServicio(IReporteAvanceProductoPersistencia ReporteAvanceProductoPersistencia)
        {
            _ReporteAvanceProductoPersistencia = ReporteAvanceProductoPersistencia;
        }

        #endregion

        public string ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            return _ReporteAvanceProductoPersistencia.ConsultarAvanceMetaProducto(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad);
        }

        public string ConsultarAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            return _ReporteAvanceProductoPersistencia.ConsultarAvanceRegionalizacion(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad);
        }

        public Task<ReponseHttp> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string usuario)
        {
            try
            {
                _ReporteAvanceProductoPersistencia.ActualizarAvanceMetaProducto(IndicadorDto, usuario);
                return Task.FromResult<ReponseHttp>(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult<ReponseHttp>(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        public Task<ReponseHttp> GuardarRegionalizacion(AvanceRegionalizacionDto IndicadorDto, string usuario)
        {
            try
            {
                _ReporteAvanceProductoPersistencia.GuardarRegionalizacion(IndicadorDto, usuario);
                return Task.FromResult<ReponseHttp>(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult<ReponseHttp>(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }
        public string ObtenerDetalleRegionalizacionProgramacionSeguimiento(string json)
        {
            return _ReporteAvanceProductoPersistencia.ObtenerDetalleRegionalizacionProgramacionSeguimiento(json);
        }


        public string ConsultarResumenRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin)
        {
            return _ReporteAvanceProductoPersistencia.ConsultarResumenRegionalizacion(instanciaId, proyectoId, codigoBpin);
        }
    }
}
