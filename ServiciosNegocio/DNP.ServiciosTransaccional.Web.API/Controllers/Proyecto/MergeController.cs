namespace DNP.ServiciosTransaccional.Web.API.Controllers.Proyecto
{
    using Servicios.Interfaces.Proyectos;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Autorizacion;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    public class MergeController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly IMergeServicio _mergeServicio;

        public MergeController(IMergeServicio mergeServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _mergeServicio = mergeServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/Proyecto/DNP_SN_Pla_Viabilidad_AplicarMergeDto")]
        [HttpPost]
        public async Task<IHttpActionResult> AplicarMerge(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["aplicarMergeProyecto"]);

                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                if (contenido == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, new Exception(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos))));

                ValidarParametros(contenido);

                var parametrosActualizar = new ParametrosGuardarDto<ObjetoNegocio>
                {
                    Contenido = contenido
                };
                var parametrosAuditoria = new ParametrosAuditoriaDto
                {
                    Usuario = RequestContext.Principal.Identity.Name,
                    Ip = UtilidadesApi.GetClientIp(Request)
                };
                var response = await Task.Run(() => _mergeServicio.AplicarMerge(parametrosActualizar, parametrosAuditoria));

                var respuesta = new HttpResponseMessage();
                respuesta.StatusCode = HttpStatusCode.OK;
                respuesta.ReasonPhrase = ServiciosNegocioRecursos.PostExitoso;

                return Ok(respuesta);
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
        private static void ValidarParametros(ObjetoNegocio datosNegocio)
        {
            var ObjetoNegocioId = datosNegocio.ObjetoNegocioId;

            if (string.IsNullOrWhiteSpace(ObjetoNegocioId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos));

            if (string.IsNullOrWhiteSpace(ObjetoNegocioId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(ObjetoNegocioId)));

            long negocioIdResult;
            if (!long.TryParse(ObjetoNegocioId, out negocioIdResult) || ObjetoNegocioId.Length > 100)
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(ObjetoNegocioId)));
        }
    }
}