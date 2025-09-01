
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Servicios.Interfaces.ProgramacionDistribucion;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;

namespace DNP.ServiciosNegocio.Web.API.Controllers.ProgramacionDistribucion
{
    public class ProgramacionDistribucionController : ApiController
    {
        private readonly IProgramacionDistribucionServicio _programacionDistribucionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ProgramacionDistribucionController(IProgramacionDistribucionServicio ProgramacionDistribucionServicioServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _programacionDistribucionServicio = ProgramacionDistribucionServicioServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/ProgramacionDistribucion/ObtenerDatosProgramacionDistribucion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Programacion Distribucion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionDistribucion(int EntidadDestinoId, int TramiteId)
        {
            try
            {
                var result = await Task.Run(() => _programacionDistribucionServicio.ObtenerDatosProgramacionDistribucion(EntidadDestinoId, TramiteId));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ProgramacionDistribucion/GuardarDatosProgramacionDistribucion")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Programacion Distribucion", typeof(ProgramacionDistribucionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto objProgramacionDistribucionDto)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionDistribucionServicio.GuardarDatosProgramacionDistribucion(objProgramacionDistribucionDto, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/ProgramacionDistribucion/ObtenerDatosProgramacionFuenteEncabezado")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Programacion Distribucion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionFuenteEncabezado(int EntidadDestinoId, int tramiteid)
        {
            try
            {
                var result = await Task.Run(() => _programacionDistribucionServicio.ObtenerDatosProgramacionFuenteEncabezado(EntidadDestinoId, tramiteid));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ProgramacionDistribucion/ObtenerDatosProgramacionFuenteDetalle")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Programacion Distribucion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionFuenteDetalle(int tramiteidProyectoId)
        {
            try
            {
                var result = await Task.Run(() => _programacionDistribucionServicio.ObtenerDatosProgramacionFuenteDetalle(tramiteidProyectoId));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ProgramacionDistribucion/GuardarDatosProgramacionFuente")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Programacion Distribucion", typeof(ProgramacionDistribucionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosProgramacionFuente(ProgramacionDistribucionDto ProgramacionDistribucion)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionDistribucionServicio.GuardarDatosProgramacionFuente(ProgramacionDistribucion, usuario));
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