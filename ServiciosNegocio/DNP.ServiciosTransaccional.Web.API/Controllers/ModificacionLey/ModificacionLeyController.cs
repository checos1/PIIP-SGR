using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosTransaccional.Servicios.Interfaces;

namespace DNP.ServiciosTransaccional.Web.API.Controllers.ModificacionLey
{
    public class ModificacionLeyController : ApiController
    {
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;
        private readonly IModificacionLeyServicio _modificacionLeyServicio;

        public ModificacionLeyController(IModificacionLeyServicio modificacionLeyServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _modificacionLeyServicio = modificacionLeyServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/ModificacionLey/ActualizarValoresPoliticasML")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarValoresPoliticasML(ObjetoNegocio contenido)
        {
            try
            {
                var respuestaAutorizacion = await _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                           RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                           ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                           ConfigurationManager.AppSettings["actualizarEstadoProyecto"]);

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
                var response = await Task.Run(() => _modificacionLeyServicio.ActualizarValoresPoliticasML(parametrosActualizar, parametrosAuditoria));

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
            var NivelId = datosNegocio.NivelId;

            if (string.IsNullOrWhiteSpace(ObjetoNegocioId) && string.IsNullOrWhiteSpace(NivelId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametrosNoRecibidos));

            if (string.IsNullOrWhiteSpace(ObjetoNegocioId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(ObjetoNegocioId)));
           

            if (string.IsNullOrWhiteSpace(NivelId))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoRecibido, nameof(NivelId)));

            Guid nivelIdResult;
            if (!Guid.TryParse(NivelId, out nivelIdResult))
                throw new ServiciosNegocioException(string.Format(ServiciosNegocioRecursos.ParametroNoValidos, nameof(NivelId)));

        }        
    }
}