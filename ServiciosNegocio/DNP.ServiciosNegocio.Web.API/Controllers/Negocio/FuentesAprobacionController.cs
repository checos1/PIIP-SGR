using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Negocio
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

    public class FuentesAprobacionController : ApiController
    {
        private readonly IFuentesAprobacionServicio _fuentesAprobacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public FuentesAprobacionController(IFuentesAprobacionServicio fuentesAprobacionServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _fuentesAprobacionServicio = fuentesAprobacionServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/FuentesObtenerPreguntasAprobacionRol")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna las preguntas para Aprobación Rol Presupuesto", typeof(PreguntasSeguimientoProyectoDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuenteFinanciacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuentesAprobacionServicio.ObtenerPreguntasAprobacionRol(objPreguntasSeguimientoProyectoDto));

            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        [Route("api/FuentesGuardarPreguntasAprobacionRol")]
        [SwaggerResponse(HttpStatusCode.OK, "Guarda las respuestas para las Preguntas de Aprobación Rol Presupuesto", typeof(PreguntasSeguimientoProyectoDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuario, string tokenAutorizacion)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuenteFinanciacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var parametrosGuardar = new ParametrosGuardarDto<PreguntasSeguimientoProyectoDto>();
            parametrosGuardar.Contenido = objPreguntasSeguimientoProyectoDto;

            var result = await Task.Run(() => _fuentesAprobacionServicio.GuardarPreguntasAprobacionRol(parametrosGuardar, usuario)); 
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/FuentesObtenerPreguntasAprobacionJefe")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna las preguntas para Aprobación Jefe Planeacion", typeof(PreguntasSeguimientoProyectoDto))]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuenteFinanciacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _fuentesAprobacionServicio.ObtenerPreguntasAprobacionJefe(objPreguntasSeguimientoProyectoDto));

            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        [Route("api/FuentesGuardarPreguntasAprobacionJefe")]
        [SwaggerResponse(HttpStatusCode.OK, "Guarda las respuestas para las Preguntas de  Aprobación Jefe Planeacion", typeof(PreguntasSeguimientoProyectoDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            string usuario = RequestContext.Principal.Identity.Name;
            //string tokenAutorizacion
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                               RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                               ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                               ConfigurationManager.AppSettings["consultarFuenteFinanciacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

            var parametrosGuardar = new ParametrosGuardarDto<PreguntasSeguimientoProyectoDto>();
            parametrosGuardar.Contenido = objPreguntasSeguimientoProyectoDto;

            var result = await Task.Run(() => _fuentesAprobacionServicio.GuardarPreguntasAprobacionJefe(parametrosGuardar, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

    }
}