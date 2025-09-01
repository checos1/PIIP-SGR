namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Inbox;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
    using Servicios.Interfaces.Autorizacion;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class SeccionesCapitulosController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;
        private readonly IFlujoServicios _flujoServicios;

        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de Negocio Servicios</param>
        public SeccionesCapitulosController(
            IAutorizacionServicios autorizacionUtilidades,
            IServiciosNegocioServicios serviciosNegocioServicios)
            : base(autorizacionUtilidades)
        {
            _serviciosNegocioServicios = serviciosNegocioServicios;
        }

        /// <summary>
        /// Método que obtiene el listado de capitulos modificados por macroproceso
        /// </summary>
        /// <param name="guiMacroproceso">GUI de tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeccionesCapitulos/ObtenerCapitulosModificados")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCapitulosModificados(string guiMacroproceso, int IdProyecto, string IdInstancia)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.SeccionesCapitulosModificadosByMacroproceso(guiMacroproceso, IdProyecto, IdInstancia, UsuarioLogadoDto.IdUsuario);
                var result = seccionesCapitulos != null ? seccionesCapitulos.FindAll(p => p.Modificado.Value).ToList(): new List<SeccionCapituloDto>();
                result.ForEach(p=> {
                    p.NombreModificado = RemoveAccents(p.Seccion.ToLower()) + RemoveAccents(p.Capitulo.ToLower());
                });
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Método que obtiene el listado de capitulos modificados por macroproceso
        /// </summary>
        /// <param name="guiMacroproceso">GUI de tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeccionesCapitulos/ObtenerCapitulosModificadosLocalizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCapitulosModificadosLocalizacion(string guiMacroproceso, int IdProyecto, string IdInstancia, int seccionCapituloId)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.SeccionesCapitulosModificadosByMacroproceso(guiMacroproceso, IdProyecto, IdInstancia, UsuarioLogadoDto.IdUsuario);
                var result = seccionesCapitulos != null ? seccionesCapitulos.FindAll(p => p.SeccionCapituloId == seccionCapituloId).ToList() : new List<SeccionCapituloDto>();
                result.ForEach(p => {
                    p.NombreModificado = RemoveAccents(p.Seccion.ToLower()) + RemoveAccents(p.Capitulo.ToLower());
                });
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Método que obtiene el listado de capitulos modificados por macroproceso
        /// </summary>
        /// <param name="guiMacroproceso">GUI de tabla [Transversal].[fase]</param>
        /// <returns></returns>
        [Route("api/SeccionesCapitulos/ObtenerCapitulosByMacroproceso")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCapitulosByMacroproceso(string guiMacroproceso,string NivelId,string FlujoId)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.SeccionesCapitulosByMacroproceso(guiMacroproceso, UsuarioLogadoDto.IdUsuario, NivelId,FlujoId);
                var result = seccionesCapitulos != null ? seccionesCapitulos.ToList() : new List<SeccionCapituloDto>();
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ValidarCapitulos")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidarCapitulosModificados(string guiMacroproceso, int IdProyecto, string IdInstancia)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.ValidarSeccionesCapitulosByMacroproceso(guiMacroproceso, IdProyecto, IdInstancia, UsuarioLogadoDto.IdUsuario);
                
                return Ok(seccionesCapitulos);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ObtenerErroresProyecto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresProyecto(string guiMacroproceso, int IdProyecto, string IdInstancia)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.ObtenerErroresProyecto(guiMacroproceso, IdProyecto, IdInstancia, UsuarioLogadoDto.IdUsuario);

                return Ok(seccionesCapitulos);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionCapitulo/ObtenerErroresSeguimiento")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresSeguimiento(string guiMacroproceso, int IdProyecto, string IdInstancia)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.ObtenerErroresSeguimiento(guiMacroproceso, IdProyecto, IdInstancia, UsuarioLogadoDto.IdUsuario);

                return Ok(seccionesCapitulos);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ObtenerErroresViabilidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresViabilidad(string guiMacroproceso, int IdProyecto, string IdNivel, string IdInstancia)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.ObtenerErroresViabilidad(guiMacroproceso, IdProyecto, IdNivel, IdInstancia, UsuarioLogadoDto.IdUsuario);

                return Ok(seccionesCapitulos);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        private string RemoveAccents(string text)
        {
            var sbReturn = new StringBuilder();
            text = text.Replace(" ", "");
            text = text.Length > 15 ? text.Substring(0, 15) : text;
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        private string FirstCharToUpper(string input)
        {
             return input.First().ToString().ToUpper() + input.Substring(1);
        }

        [Route("api/SeccionesCapitulos/ObtenerCapitulosModificadosCapitoSeccion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCapitulosModificadosCapitoSeccion(string guiMacroproceso, int idProyecto, Guid idInstancia, string capitulo, string seccion, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerCapitulosModificadosCapitoSeccion(guiMacroproceso, idProyecto, idInstancia, capitulo, seccion, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ObtenerDetalleAjustesFuenteFinanciacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleAjustesFuenteFinanciacion(string Bpin, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerDetalleAjustesFuenteFinanciacion(Bpin, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ObtenerErroresTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresTramite(string guiMacroproceso, string IdInstancia, string accionid, bool tieneCDP)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.ObtenerErroresTramite(guiMacroproceso, IdInstancia, accionid, UsuarioLogadoDto.IdUsuario, tieneCDP);

                return Ok(seccionesCapitulos);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ObtenerSeccionesTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionesTramite(string IdMacroproceso, string IdInstancia, string IdFase)
        {
            try
            {
                var secciones = await _serviciosNegocioServicios.ObtenerSeccionesTramite(IdMacroproceso, IdInstancia, IdFase, UsuarioLogadoDto.IdUsuario);

                return Ok(secciones);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ObtenerSeccionCapitulo")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerSeccionCapitulo(string FaseGuid, string Capitulo, string Seccion, string idUsuario,string NivelId,string FlujoId)
        {
            try
            {
                 var seccioncapitulo = await _serviciosNegocioServicios.ObtenerSeccionCapitulo(FaseGuid, Capitulo, Seccion, idUsuario, NivelId, FlujoId);

                return Ok(seccioncapitulo);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/CambiosFirme/eliminarCapituloModificado")]
        [HttpPost]
        public async Task<IHttpActionResult> eliminarCapituloModificado(CapituloModificado parametros)
        {
            try
            {
                return Ok(await _serviciosNegocioServicios.EliminarCapitulosModificados(parametros, UsuarioLogadoDto.IdUsuario));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SeccionesCapitulos/ObtenerDetalleAjustesJustificaionFacalizacionPT")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleAjustesJustificaionFacalizacionPT(string Bpin, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerDetalleAjustesJustificaionFacalizacionPT(Bpin, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Trasversal/CrearAlcanceTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearAlcanceTramite(AlcanceTramiteDto alcanceTramite)
        {
            var result = await Task.Run(() => _serviciosNegocioServicios.CrearAlcanceTramite(alcanceTramite, UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/Trasversal/ObtenerTiposMotivoAnulacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTiposMotivoAnulacion()
        {
            var result = await Task.Run(() => _serviciosNegocioServicios.ObtenerTiposMotivoAnulacion(UsuarioLogadoDto.IdUsuario));
            return Ok(result);
        }

        [Route("api/SeccionesCapitulos/ObtenerErroresProgramacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerErroresProgramacion(string IdInstancia, string accionid)
        {
            try
            {
                var seccionesCapitulos = await _serviciosNegocioServicios.ObtenerErroresProgramacion(IdInstancia, accionid, UsuarioLogadoDto.IdUsuario);

                return Ok(seccionesCapitulos);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}