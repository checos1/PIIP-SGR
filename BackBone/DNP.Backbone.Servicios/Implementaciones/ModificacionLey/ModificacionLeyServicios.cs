using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces;
using System.Configuration;
using System.Threading.Tasks;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using Newtonsoft.Json;
using DNP.Backbone.Dominio.Dto.ModificacionLey;
using DNP.Backbone.Servicios.Interfaces.ModificacionLey;

namespace DNP.Backbone.Servicios.Implementaciones.ModificacionLey
{
    public class ModificacionLeyServicios: IModificacionLeyServicios
    {
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IAutorizacionServicios _autorizacionServicios;
        public ModificacionLeyServicios(IClienteHttpServicios clienteHttpServicios, IAutorizacionServicios autorizacionServicios)
        {
            this._clienteHttpServicios = clienteHttpServicios;
            _autorizacionServicios = autorizacionServicios;
        }

        public async Task<string> ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int TramiteId, string origen, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerInformacionPresupuestalMLEncabezado"];
            uriMetodo += "?EntidadDestinoId=" + EntidadDestinoId + "&TramiteId=" + TramiteId + "&origen=" + origen;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerInformacionPresupuestalMLDetalle(int TramiteProyectoId, string origen, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerInformacionPresupuestalMLDetalle"];
            uriMetodo += "?tramiteidProyectoId=" + TramiteProyectoId + "&origen=" + origen;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlNegocio, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto informacionPresupuestal, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarInformacionPresupuestalML"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlNegocio, uriMetodo, string.Empty, informacionPresupuestal, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }
    }
}
