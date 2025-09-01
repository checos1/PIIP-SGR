using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.SeguimientoControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.SeguimientoControl
{
    public class ReporteAvanceRegionalizacionServicio : IReporteAvanceRegionalizacionServicio
    {

        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;


        #region Get

        public ReporteAvanceRegionalizacionServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<string> ConsultarAvanceRegionalizacion(Guid? instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["UriConsultarAvanceRegionalizacion"];
            uri += "?instanciaId=" + instanciaId + "&proyectoId=" + proyectoId + "&codigoBpin=" + codigoBpin + "&vigencia=" + vigencia + "&periodoPeriodicidad=" + periodoPeriodicidad;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ConsultarResumenAvanceRegionalizacion(Guid? instanciaId, int proyectoId, string codigoBpin, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["UriResumenAvanceRegionalizacion"];
            uri += "?instanciaId=" + instanciaId + "&proyectoId=" + proyectoId + "&codigoBpin=" + codigoBpin;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerDetalleRegionalizacionProgramacionSeguimiento(string json, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["UriObtenerDetalleRegionalizacionProgramacionSeguimiento"];
            uri += "?json=" + json;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;


        }



        #endregion


        #region Post

        public async Task<ReponseHttp> GuardarAvanceRegionalizacion(AvanceRegionalizacionDto IndicadorDto, string Usuario)
        {
            var uri = ConfigurationManager.AppSettings["UriGuardarAvanceRegionalizacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, IndicadorDto, Usuario, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }



        #endregion


    }
}