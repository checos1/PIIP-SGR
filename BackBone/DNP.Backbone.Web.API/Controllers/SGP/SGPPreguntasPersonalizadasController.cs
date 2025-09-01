namespace DNP.Backbone.Web.API.Controllers.Preguntas
{
    using Comunes.Excepciones;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Servicios.Interfaces.Autorizacion;
    using System.Net.Http;
    using DNP.Backbone.Servicios.Interfaces.SGP;
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
    public class SGPPreguntasPersonalizadasController : Base.BackboneBase
    {
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly ISGPPreguntasPersonalizadasServicios _preguntasServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="preguntasServicios">Instancia de servicios de preguntas</param>
        public SGPPreguntasPersonalizadasController(IAutorizacionServicios autorizacionUtilidades, ISGPPreguntasPersonalizadasServicios preguntasServicios)
            : base(autorizacionUtilidades)
        {
            _autorizacionUtilidades = autorizacionUtilidades;
            _preguntasServicios = preguntasServicios;
        }

        /// <summary>
        /// Api para obtener lista de preguntas.
        /// </summary>
        /// <returns>Lista de preguntas personalizadas</returns>
        [Route("SGP/PreguntasPersonalizadas/ConsultarPreguntasSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPreguntasSGP(string bPin, Guid nivelId, Guid instanciaId, string listaRoles)
        {
            try
            {
                var result = await _preguntasServicios.ConsultarPreguntasSGP(bPin, nivelId, instanciaId, listaRoles, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);

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
        [Route("SGP/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadasComponente")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadasSGPComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerPreguntasPersonalizadasSGPComponente(bPin, nivelId, instanciaId, nombreComponente, listaRoles, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/PreguntasPersonalizadas/ObtenerPreguntasPersonalizadasComponenteSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasPersonalizadasSGPComponenteSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles)
        {
            try
            {
                var result = await _preguntasServicios.ObtenerPreguntasPersonalizadasSGPComponenteSGP(bPin, nivelId, instanciaId, nombreComponente, listaRoles, UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
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
        [Route("SGP/PreguntasPersonalizadas/ObtenerDatosGeneralesProyecto")]
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
        [Route("SGP/PreguntasPersonalizadas/ObtenerConfiguracionEntidades")]
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

        [Route("SGP/PreguntasPersonalizadas/GuardarPreguntasPersonalizadas")]
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

        [Route("SGP/PreguntasPersonalizadas/GuardarPreguntasPersonalizadasCustomSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasPersonalizadasSGP(ServicioPreguntasPersonalizadasDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _preguntasServicios.GuardarPreguntasPersonalizadasSGP(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/PreguntasPersonalizadas/DevolverCuestionarioProyecto")]
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
      
    }
}