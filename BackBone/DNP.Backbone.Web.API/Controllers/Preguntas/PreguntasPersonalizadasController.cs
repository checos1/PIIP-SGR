namespace DNP.Backbone.Web.API.Controllers.Preguntas
{
    using Comunes.Excepciones;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces.Autorizacion;
    using System.Net.Http;
    using DNP.Backbone.Servicios.Interfaces.Preguntas;
    using DNP.Backbone.Servicios.Interfaces.Tramites;
    using DNP.Backbone.Web.API.Controllers.Base;
    using System.Net.Http.Headers;
    using System.Web.Management;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using System.Collections.Generic;
    using DNP.Backbone.Dominio.Dto.Preguntas;
    using System;

    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class PreguntasPersonalizadasController : Base.BackboneBase
    {
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IPreguntasPersonalizadasServicios _preguntasServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="preguntasServicios">Instancia de servicios de preguntas</param>
        public PreguntasPersonalizadasController(IAutorizacionServicios autorizacionUtilidades,IPreguntasPersonalizadasServicios preguntasServicios)
            : base(autorizacionUtilidades)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            _preguntasServicios = preguntasServicios;
        }

        /// <summary>
        /// Api para obtener lista de preguntas.
        /// </summary>
        /// <returns>Lista de preguntas personalizadas</returns>
        [Route("api/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadas")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerPreguntasPersonalizadas(bPin, nivelId, instanciaId, listaRoles, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        // <summary>
        /// Api para obtener lista de preguntas.
        /// </summary>
        /// <returns>Lista de preguntas personalizadas</returns>
        [Route("api/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadasComponente")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerPreguntasPersonalizadasComponente(bPin, nivelId, instanciaId, nombreComponente, listaRoles, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadasComponenteSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadasComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerPreguntasPersonalizadasComponenteSGR(bPin, nivelId, instanciaId, nombreComponente, listaRoles, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener Datos generales del proyecto.
        /// </summary>
        /// <returns>Datos generales del proyecto</returns>
        [Route("api/PreguntasPersonalizadas/ObtenerDatosGeneralesProyecto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosGeneralesProyecto(int? ProyectoId, Guid NivelId)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerDatosGeneralesProyecto(ProyectoId, NivelId, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtener Datos generales del proyecto.
        /// </summary>
        /// <returns>Datos generales del proyecto</returns>
        [Route("api/PreguntasPersonalizadas/ObtenerConfiguracionEntidades")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerConfiguracionEntidades(int? ProyectoId, Guid NivelId)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerConfiguracionEntidades(ProyectoId, NivelId, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/PreguntasPersonalizadas/GuardarPreguntasPersonalizadas")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasPersonalizadas(ServicioPreguntasPersonalizadasDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _preguntasServicios.GuardarPreguntasPersonalizadas(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/PreguntasPersonalizadas/GuardarPreguntasPersonalizadasCustomSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasPersonalizadasCustomSGR(ServicioPreguntasPersonalizadasDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _preguntasServicios.GuardarPreguntasPersonalizadasCustomSGR(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/PreguntasPersonalizadas/DevolverCuestionarioProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia)
        {
            try
            {
                var result = await Task.Run(() => _preguntasServicios.DevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/PreguntasPersonalizadas/ObtenerConceptosPreviosEmitidos")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerConceptosPreviosEmitidos(string bPin, int? tipoConcepto = 1)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerConceptosPreviosEmitidos(bPin, tipoConcepto, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}