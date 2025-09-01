using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.SeguimientoControl
{
    public class ReporteAvanceProductoController : ApiController
    {
        private readonly IReporteAvanceProductoServicio _ReporteAvanceProductoServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        #region Costructor
        public ReporteAvanceProductoController(IReporteAvanceProductoServicio ReporteAvanceProductoServicio,
            IAutorizacionUtilidades autorizacionUtilidades
            )
        {
            _ReporteAvanceProductoServicio = ReporteAvanceProductoServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }
        #endregion


        [Route("api/SeguimientoControl/ReporteAvanceProducto/ConsultarAvanceMetaProducto")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Avance Meta Producto", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            var result = await Task.Run(() => _ReporteAvanceProductoServicio.ConsultarAvanceMetaProducto(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad));

            return Ok(result);
        }


        [Route("api/SeguimientoControl/ReporteAvanceProducto/ActualizarAvanceMetaProducto")]
        [SwaggerResponse(HttpStatusCode.OK, "Programar Actividades - Estado de transacción", typeof(DesagregarEdtNivelesDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto)
        {
            var usuario = RequestContext.Principal.Identity.Name;
            var result = await Task.Run(() => _ReporteAvanceProductoServicio.ActualizarAvanceMetaProducto( IndicadorDto, usuario));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/SeguimientoControl/ReporteAvanceProducto/ConsultarReporteAvanceRegionalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Avance Regionalizacion", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarReporteAvanceRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad)
        {
            var result = await Task.Run(() => _ReporteAvanceProductoServicio.ConsultarAvanceRegionalizacion(instanciaId, proyectoId, codigoBpin, vigencia, periodoPeriodicidad));

            return Ok(result);
        }

        [Route("api/SeguimientoControl/ReporteAvanceProducto/ConsultarReporteResumenRegionalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Avance Regionalizacion", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarReporteResumenRegionalizacion(Guid instanciaId, int proyectoId, string codigoBpin)
        {
            var result = await Task.Run(() => _ReporteAvanceProductoServicio.ConsultarResumenRegionalizacion(instanciaId, proyectoId, codigoBpin));

            return Ok(result);
        }

        [Route("api/SeguimientoControl/ReporteAvanceProducto/GuardarAvanceRegionalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Permite guardar el avance de la regionalizacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarAvanceRegionalizacion(AvanceRegionalizacionDto IndicadorDto)
        {
            var usuario = RequestContext.Principal.Identity.Name;
            var result = await Task.Run(() => _ReporteAvanceProductoServicio.GuardarRegionalizacion(IndicadorDto, usuario));
            return result != null ? Ok(result) : CrearRespuestaNoFound();
        }

        [Route("api/SeguimientoControl/ReporteAvanceProducto/ObtenerDetalleRegionalizacionProgramacionSeguimiento")]
        [SwaggerResponse(HttpStatusCode.OK, "Consultar Detalle por localizacion de la regionalizacion Seguimiento", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleRegionalizacionProgramacionSeguimiento(string json)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["postDatosAdicionalesAgregar"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _ReporteAvanceProductoServicio.ObtenerDetalleRegionalizacionProgramacionSeguimiento(json));

            return Ok(result);
        }

        /// <returns>IHttpActionResult</returns>
        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

    }
}