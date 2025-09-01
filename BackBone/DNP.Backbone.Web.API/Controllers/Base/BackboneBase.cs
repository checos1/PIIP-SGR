using System;

namespace DNP.Backbone.Web.API.Controllers.Base
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using Comunes.Properties;
    using System.Configuration;
    using System.Net;
    using System.Web.Http;
    using Servicios.Interfaces.Autorizacion;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using DNP.Backbone.Dominio.Dto;
    using System.Threading.Tasks;

    public class BackboneBase : ApiController
    {
        public HttpResponseMessage RespuestaAutorizacion;
        public HttpAuthenticationContext HttpAuthContext;
        protected UsuarioLogadoDto UsuarioLogadoDto
        {
            get
            {
                return new UsuarioLogadoDto()
                {
                    IdUsuario = User.Identity.Name,
                    IdAplicacionBackbone = ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                    IdNombreBackbone = ConfigurationManager.AppSettings["IdNombreBackbone"],
                    GuidPIIPAplicacion = Guid.Parse(ConfigurationManager.AppSettings["GuidPIIPAplicacion"]),
                    GuidAdministracionAplicacion = Guid.Parse(ConfigurationManager.AppSettings["GuidAdministracionAplicacion"]),
                    ApiAutorizacion = ConfigurationManager.AppSettings["ApiAutorizacion"],
                };
            }
        }

        private readonly IAutorizacionServicios _autorizacionUtilidades;
        public BackboneBase(IAutorizacionServicios autorizacionUtilidades)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        protected async Task<bool> ValidarParametrosRequest(ParametrosInboxDto parametrosInboxDto)
        {
            ValidarParametros(parametrosInboxDto);
            RespuestaAutorizacion =await _autorizacionUtilidades.
                                        ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                       RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                       ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                       ConfigurationManager.AppSettings["idObtenerInbox"]).ConfigureAwait(false);
            return RespuestaAutorizacion.IsSuccessStatusCode;
        }

        private void ValidarParametros(ParametrosInboxDto parametrosInboxDto)
        {

            if (parametrosInboxDto == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent,
                       Resources.ParametrosNoRecibidos));
            }

            if (string.IsNullOrWhiteSpace(parametrosInboxDto.Aplicacion))
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "Aplicacion"));

            if (parametrosInboxDto.IdObjeto == default || parametrosInboxDto.IdObjeto == Guid.Empty)
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdObjeto"));

            if (string.IsNullOrWhiteSpace(parametrosInboxDto.IdUsuario))
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdUsuario"));

            if (parametrosInboxDto.ListaIdsRoles == null || parametrosInboxDto.ListaIdsRoles.Count == 0)
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "ListaIdsRoles"));

        }

        protected async Task<bool> ValidarParametrosRequest(ProyectoParametrosDto parametrosProyectoDto)
        {
            ValidarParametros(parametrosProyectoDto);
            RespuestaAutorizacion = await _autorizacionUtilidades.
                                        ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                       RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                       ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                       ConfigurationManager.AppSettings["idObtenerInbox"]).ConfigureAwait(false);
            return RespuestaAutorizacion.IsSuccessStatusCode;
        }

        protected async Task<bool> ValidarParametrosRequest(ParametrosDto parametrosDto)
        {
            ValidarParametros(parametrosDto);

            RespuestaAutorizacion = await _autorizacionUtilidades.
                                        ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                       RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                       ConfigurationManager.AppSettings["IdAplicacionBackbone"],
                                                       ConfigurationManager.AppSettings["idObtenerInbox"]).ConfigureAwait(false);

            return RespuestaAutorizacion.IsSuccessStatusCode;
        }

        private void ValidarParametros(ProyectoParametrosDto parametrosProyectoDto)
        {

            if (parametrosProyectoDto == null)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent,
                       Resources.ParametrosNoRecibidos));
            }

            if (string.IsNullOrWhiteSpace(parametrosProyectoDto.Aplicacion))
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "Aplicacion"));

            if (parametrosProyectoDto.IdObjeto == default || parametrosProyectoDto.IdObjeto == Guid.Empty)
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdObjeto"));

            if (string.IsNullOrWhiteSpace(parametrosProyectoDto.IdUsuario))
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdUsuario"));

            if (parametrosProyectoDto.ListaIdsRoles == null || parametrosProyectoDto.ListaIdsRoles.Count == 0)
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "ListaIdsRoles"));

        }

        private void ValidarParametros(ParametrosDto parametrosDto)
        {
            if (parametrosDto == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, Resources.ParametrosNoRecibidos));

            if (string.IsNullOrWhiteSpace(parametrosDto.Aplicacion))
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "Aplicacion"));

            if (string.IsNullOrWhiteSpace(parametrosDto.IdUsuarioDNP))
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdUsuarioDNP"));

            if (parametrosDto.IdsRoles == null || parametrosDto.IdsRoles.Count == 0)
                throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdsRoles"));
        }

    }
}