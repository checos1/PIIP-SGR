using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using Swashbuckle.Swagger.Annotations;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Requisitos
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;
    using Dominio.Dto.Requisitos;
    using Servicios.Interfaces.Requisitos;

    public class RequisitosController : ApiController
    {
        private readonly IRequisitosServicio _requisitosServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public RequisitosController(IRequisitosServicio requisitosServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _requisitosServicio = requisitosServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Requisitos")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna el listado de Requisitos", typeof(ServicioAgregarRequisitosDto))]
        [HttpGet]
        public async Task<IHttpActionResult> Consultar()
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["ConsultarRequisitos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                ValidacionParametros(Request);
                ValidacionInstanciaId(Request);

                var parametrosConsulta = _requisitosServicio.ConstruirParametrosConsulta(Request);

                var result = await Task.Run(() => _requisitosServicio.Obtener(parametrosConsulta));

                if (result.ListadoAtributos.Count > 0)
                    return Ok(result);

                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.SinResultados));
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Requisitos")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado de requisitos Definitivo", typeof(bool))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(ServicioAgregarRequisitosDto contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["DefinitivoRequisitos"]).Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var parametrosGuardar = _requisitosServicio.ConstruirParametrosGuardar(Request);

                if (contenido != null)
                {
                    parametrosGuardar.Contenido = contenido;
                    ValidacionInternaContenido(contenido);
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));
                }

                ValidacionInstanciaId(Request);

                var parametrosAuditoria = new ParametrosAuditoriaDto
                                          {
                                              Usuario = RequestContext.Principal.Identity.Name,
                                              Ip = UtilidadesApi.GetClientIp(Request)
                                          };

                await Task.Run(() => _requisitosServicio.Guardar(parametrosGuardar, parametrosAuditoria, false));

                return Ok(true);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));

            }
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionParametros(HttpRequestMessage peticion)
        {
            var bpin = HttpUtility.ParseQueryString(peticion.RequestUri.Query).Get("bpin");
            var idNivel = HttpUtility.ParseQueryString(peticion.RequestUri.Query).Get("IdNivel");

            if (string.IsNullOrWhiteSpace(bpin) && string.IsNullOrWhiteSpace(idNivel))
            {
                var e = new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos));
                e.Data.Add("NoContent", 204);
                throw e;
            }
            if (string.IsNullOrWhiteSpace(bpin))
            {
                var e = new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));
                e.Data.Add("BadRequest", 400);
                throw e;
            }
            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
            {
                var e = new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));
                e.Data.Add("BadRequest", 400);
                throw e;
            }
            if (string.IsNullOrWhiteSpace(idNivel))
            {
                var e = new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(idNivel)));
                e.Data.Add("BadRequest", 400);
                throw e;
            }
            if (!Guid.TryParse(idNivel, out Guid guidOutput))
            {
                var e = new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(idNivel)));
                e.Data.Add("BadRequest", 400);
                throw e;
            }

        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionInternaContenido(ServicioAgregarRequisitosDto contenido)
        {
            var bpin = contenido.Bpin;
            var idNivel = contenido.IdNivel.ToString();
            var listadoAtributos = contenido.ListadoAtributos;

            if (string.IsNullOrWhiteSpace(bpin) && string.IsNullOrWhiteSpace(idNivel) && listadoAtributos == null)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos));

             if (listadoAtributos == null)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(listadoAtributos)));

            if (string.IsNullOrWhiteSpace(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));

            if (string.IsNullOrWhiteSpace(idNivel))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(idNivel)));

            if (!Guid.TryParse(idNivel, out Guid guidOutput))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(idNivel)));
        }

        [SuppressMessage("ReSharper", "UnusedVariable")]
        private void ValidacionInstanciaId(HttpRequestMessage peticion)
        {
            if (peticion.Headers.Contains("piip-idInstanciaFlujo"))
            {
                if (string.IsNullOrEmpty(peticion.Headers.GetValues("piip-idInstanciaFlujo").First()))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, "piip-idInstanciaFlujo"));

                Guid outGuid = Guid.Empty;
                // ReSharper disable once UnusedVariable
                if (!Guid.TryParse(peticion.Headers.GetValues("piip-idInstanciaFlujo").First(), out outGuid))
                    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, "piip-idInstanciaFlujo"));
            }
            else
            {
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido,
                                                          "piip-idInstanciaFlujo"));
            }
        }
    }
}