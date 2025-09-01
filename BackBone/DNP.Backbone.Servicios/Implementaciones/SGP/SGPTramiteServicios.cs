using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.Tramites;
using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
using DNP.Backbone.Servicios.Implementaciones.Flujos;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Servicios.Interfaces.SGP;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.SGP
{
    public class SGPTramiteServicios : ISGPTramiteServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;
        private readonly IFlujoServicios _flujoServicios;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="serviciosNegocioServicios">Instancia de servicios de flujos</param>     
        /// <param name="clienteHttpServicios">Instancia de servicios de flujos</param>     
        public SGPTramiteServicios(IClienteHttpServicios clienteHttpServicios, IServiciosNegocioServicios serviciosNegocioServicios, IFlujoServicios flujoServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
            _serviciosNegocioServicios = serviciosNegocioServicios;
            _flujoServicios = flujoServicios;
        }

        public async Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ActualizarEstadoAjusteProyectoSGP(tipoDevolucion, objetoNegocioId, tramiteId, observacion, usuarioDnp);
        }

        public Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {
            return _serviciosNegocioServicios.EliminarProyectoTramiteNegocioSGP(instanciaTramiteDto);
        }

        public async Task<Dominio.Dto.InstanciaResultado> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uri = ConfigurationManager.AppSettings["uriEliminarInstanciaCerrada_AbiertaProyectoTramite"];
            var uriParametros = $"?instanciaTramite=" + instanciaTramite + "&Bpin=" + Bpin;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, uriParametros, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Dominio.Dto.InstanciaResultado>(respuesta);
        }

        //Información Presupuestal

        public async Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarTramiteInformacionPresupuestalSGP(parametros, usuarioDnp);
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerListaProyectosFuentesSGP(tramiteId, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarFuentesTramiteProyectoAprobacionSGP(parametros, usuarioDnp);
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.ObtenerListaProyectosFuentesAprobadoSGP(tramiteId, usuarioDnp);
        }

        public async Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp)
        {
            return await _serviciosNegocioServicios.GuardarTramiteTipoRequisitoSGP(parametros, usuarioDnp);
        }

        public async Task<List<ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, string usuarioDNP, bool isCDP)
        {
            return await _serviciosNegocioServicios.ObtenerProyectoRequisitosPorTramiteSGP(pProyectoId, pTramiteId, usuarioDNP, isCDP);
        }

        public async Task<IEnumerable<ProyectoCreditoDto>> ObtenerContracreditosSgp(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            var proyectosTramite = await _flujoServicios.ObtenerProyectosTramite(ObtenerInstanciaTramite(parametros, usuarioDnp));
            bool? grupoPermitidos = true;
            if (proyectosTramite != null && proyectosTramite.Count > 0)
            {
                grupoPermitidos = proyectosTramite.FirstOrDefault().GruposPermitidos;
            }

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerContracreditoSgp"];
            var contraCredito = JsonConvert.DeserializeObject<IEnumerable<ProyectoCreditoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
            var result = contraCredito.Where(p => !proyectosTramite.Select(c => c.IdObjetoNegocio).Contains(p.BPIN));
            //result.ToList().ForEach(p => p.GruposPermitidos = grupoPermitidos);
            return result;
        }

        public async Task<IEnumerable<ProyectoCreditoDto>> ObtenerCreditosSgp(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            var proyectosTramite = await _flujoServicios.ObtenerProyectosTramite(ObtenerInstanciaTramite(parametros, usuarioDnp));
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCreditoSgp"];

            var credito = JsonConvert.DeserializeObject<IEnumerable<ProyectoCreditoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
            var result = credito.Where(p => !proyectosTramite.Select(c => c.IdObjetoNegocio).Contains(p.BPIN));
            return result;
        }

        private InstanciaTramiteDto ObtenerInstanciaTramite(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            var instanciaTramite = new InstanciaTramiteDto();
            instanciaTramite.InstanciaId = parametros.IdInstancia;
            var parametrosInbox = new ParametrosInboxDto();
            parametrosInbox.IdUsuario = usuarioDnp;
            var tramiteFiltro = new TramiteFiltroDto();
            tramiteFiltro.InstanciaId = parametros.IdInstancia;
            instanciaTramite.ParametrosInboxDto = parametrosInbox;
            instanciaTramite.TramiteFiltroDto = tramiteFiltro;
            return instanciaTramite;

        }

        public async Task<string> ObtenerTiposValorPorEntidadSgp(int IdEntidad, int IdTipoEntidad,string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTiposValorPorEntidadSgp"];
            uriMetodo = uriMetodo + "?IdEntidad=" + IdEntidad + "&IdTipoEntidad=" + IdTipoEntidad;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> ObtenerDatosAdicionSgp(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerDatosAdicionSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?tramiteId={tramiteId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

       
        public async Task<string> GuardarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriGuardarDatosAdicionSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objConvenioDonanteDto, usuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        
        public async Task<string> EiliminarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriEiliminarDatosAdicionSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objConvenioDonanteDto, usuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}
