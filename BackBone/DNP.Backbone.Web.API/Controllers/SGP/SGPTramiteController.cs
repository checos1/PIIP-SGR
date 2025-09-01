using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using System;
    using System.Net;
    using System.Net.Http;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Web.API.Controllers.Base;
    using DNP.Backbone.Servicios.Interfaces.SGP;
    using DNP.Backbone.Dominio.Dto.Tramites;
    using System.Collections.Generic;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;

    [Route("api/[controller]")]
    public class SGPTramiteController : BackboneBase
    {
        private readonly ISGPTramiteServicios _tramiteServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="tramiteServicios">Instancia de servicios de trámites</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        public SGPTramiteController(ISGPTramiteServicios tramiteServicios, IAutorizacionServicios autorizacionUtilidades) : base(autorizacionUtilidades)
        {
            _tramiteServicios = tramiteServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/TramiteSGP/ActualizarEstadoAjusteProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ActualizarEstadoAjusteProyecto(tipoDevolucion, objetoNegocioId, tramiteId, observacion, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/EliminarProyectoTramiteNegocio")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminarProyectoTramiteNegocio(instanciaTramiteDto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/EliminarInstanciaCerrada_AbiertaProyectoTramite")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EliminarInstanciaCerrada_AbiertaProyectoTramite(instanciaTramite, Bpin, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        //Información Presupuestal

        [Route("api/TramiteSGP/GuardarTramiteInformacionPresupuestal")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarTramiteInformacionPresupuestal(parametros, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpGet]
        [Route("api/TramiteSGP/ObtenerListaProyectosFuentes")]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentes(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerListaProyectosFuentes(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/GuardarFuentesTramiteProyectoAprobacion")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarFuentesTramiteProyectoAprobacion(parametros, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerListaProyectosFuentesAprobado")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerListaProyectosFuentesAprobado(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/GuardarTramiteTipoRequisito")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarTramiteTipoRequisito(parametros, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerProyectoRequisitosPorTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerProyectoRequisitosPorTramite(pProyectoId, pTramiteId, User.Identity.Name, isCDP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerContracreditoSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerContracredito(ProyectoCreditoParametroDto prm)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerContracreditosSgp(prm, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerCreditoSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerCredito(ProyectoCreditoParametroDto prm)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerCreditosSgp(prm, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerTiposValorPorEntidadSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTiposValorPorEntidadSgp(int idEntidad, int idTipoEntidad)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerTiposValorPorEntidadSgp(idEntidad, idTipoEntidad, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/ObtenerDatosAdicionSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosAdicionSgp(int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.ObtenerDatosAdicionSgp(tramiteId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/GuardarDatosAdicionSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.GuardarDatosAdicionSgp(objConvenioDonanteDto, usuario));
                return Ok(result);
            }
            catch (Comunes.Excepciones.BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/TramiteSGP/EiliminarDatosAdicionSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> EiliminarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            try
            {
                var result = await Task.Run(() => _tramiteServicios.EiliminarDatosAdicionSgp(objConvenioDonanteDto, usuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }




    }
}