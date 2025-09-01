using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using Swashbuckle.Swagger.Annotations;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Entidades
{
    using System;
    using System.Net.Http;
    using Dominio.Dto.Entidades;
    using Servicios.Interfaces.Entidades;

    public class EntidadesAccionesController : ApiController
    {
        private readonly IEntidadAccionesServicio _entidadesAccionesServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public EntidadesAccionesController(IEntidadAccionesServicio entidadesAccionesServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _entidadesAccionesServicio = entidadesAccionesServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/EntidadesAcciones")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna los listados de Entidades Destino", typeof(EntidadAcciones))]
        [HttpPost]
        public async Task<IHttpActionResult> Definitivo(EntidadAccionesEntrada contenido)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings
                                                               ["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings
                                                               ["idObtenerEntidadesAcciones"]).
                                            Result;

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido != null)
                {
                    ValidacionInternaContenido(contenido);
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));
                }

                var result = await Task.Run(() => _entidadesAccionesServicio.ObtenerEntidadesAcciones(contenido));

                if (result.ListadoEntidadDestino.Count > 0)
                    return Ok(result);

                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.RespuestaSinresultadosEntidadesAcciones));
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

        private void ValidacionInternaContenido(EntidadAccionesEntrada contenido)
        {
            var bpin = contenido.Bpin;
            var listadoRoles = contenido.ListadoRoles;

            if (string.IsNullOrWhiteSpace(bpin) && listadoRoles == null)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos));

            if (listadoRoles == null)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(listadoRoles)));

            if (string.IsNullOrWhiteSpace(bpin))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(bpin)));

            //if (!long.TryParse(bpin, out var outBpin) || bpin.Length > 100)
            //    throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));
            //  2021-10-31 se deja en comentario porque en el campo bpin puede venir datos alfanumericos ya que el origen 
            //  puede ser el campo objetoNegocioId de la tabla ejecucion.instancias; en este campo hay datos numericos como el bpin y 
            //  datos alfanumericos como los numeros de tramite
            if ( bpin.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(bpin)));
        }
    }
}