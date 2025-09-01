namespace DNP.Backbone.Web.API.Controllers
{
    using Comunes.Dto;
    using Comunes.Excepciones;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Inbox;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.SeguimientoControl;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.SeguimientoControl;
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
    public class DesagregarEdtController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IDesagregarEdtServicio _desagregarEdtServicio;
        private readonly IFlujoServicios _flujoServicios;

        /// <summary>
        /// Constructor de clases SeccionesCapitulosController
        /// </summary>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        /// <param name="desagregarEdtServicio">Instancia de servicios de Negocio Servicios</param>
        public DesagregarEdtController(
            IAutorizacionServicios autorizacionUtilidades,
            IDesagregarEdtServicio desagregarEdtServicio)
            : base(autorizacionUtilidades)
        {
            _desagregarEdtServicio = desagregarEdtServicio;
        }

        #region Get

        [Route("api/seguimientoControl/DesagregarEdt/ObtenerListadoObjProdNiveles")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto parametros)
        {
            try
            {
                parametros.UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _desagregarEdtServicio.ObtenerListadoObjProdNiveles(parametros));
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Post
            [Route("api/seguimientoControl/DesagregarEdt/RegistrarNivel")]
            [HttpPost]
            public async Task<IHttpActionResult> RegistrarNivel(RegistroModel NivelesNuevos)
            {
                try
                {
                    var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                    return Ok(await _desagregarEdtServicio.RegistrarNivel(UsuarioDNP, NivelesNuevos));
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }

            [Route("api/seguimientoControl/ObtenerErroresSeguimiento")]
            [HttpPost]
            public async Task<IHttpActionResult> ObtenerErrores(RegistroModel NivelesNuevos)
            {
                try
                {
                    var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                    return Ok(await _desagregarEdtServicio.RegistrarNivel(UsuarioDNP, NivelesNuevos));
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }

        #endregion

        #region Delete
            [Route("api/seguimientoControl/DesagregarEdt/EliminarNivel")]
            [HttpPost]
            public async Task<IHttpActionResult> EliminarNivel(RegistroModel NivelesNuevos)
            {
                try
                {
                    var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                    return Ok(await _desagregarEdtServicio.EliminarNivel(UsuarioDNP, NivelesNuevos));
                }
                catch (BackboneException e)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                }
            }
        #endregion

        [Route("api/seguimientoControl/ObtenerPreguntasAvanceFinanciero")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _desagregarEdtServicio.ObtenerPreguntasAvanceFinanciero(instancia, proyectoid, bpin, nivelid, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/seguimientoControl/GuardarPreguntasAvanceFinanciero")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasAvanceFinanciero(List<PreguntasReporteAvanceFinancieroDto> PreguntasReporteAvanceFinanciero)
        {
            try
            {
                var result = await Task.Run(() => _desagregarEdtServicio.GuardarPreguntasAvanceFinanciero(PreguntasReporteAvanceFinanciero));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/seguimientoControl/ObtenerAvanceFinanciero")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _desagregarEdtServicio.ObtenerAvanceFinanciero(instancia, proyectoid, bpin, vigenciaId, periodoPeriodicidadId, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/seguimientoControl/GuardarAvanceFinanciero")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarAvanceFinanciero(AvanceFinancieroDto ReporteAvanceFinanciero)
        {
            try
            {
                var result = await Task.Run(() => _desagregarEdtServicio.GuardarAvanceFinanciero(ReporteAvanceFinanciero));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}