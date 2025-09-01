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
    public class ReporteAvanceProductoServicio: IReporteAvanceProductoServicio
    {

        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public ReporteAvanceProductoServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        #region Get

        public async Task<string> ConsultarAvanceMetaProducto(Guid instanciaId, int proyectoId, string codigoBpin, int vigencia, int periodoPeriodicidad, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriConsultarAvanceMetaProducto"];
            uri += "?instanciaId=" + instanciaId + "&proyectoId=" + proyectoId + "&codigoBpin=" + codigoBpin + "&vigencia=" + vigencia + "&periodoPeriodicidad=" + periodoPeriodicidad;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;

        }

        #endregion


        #region Post

        public async Task<ReponseHttp> ActualizarAvanceMetaProducto(AvanceMetaProductoDto IndicadorDto, string Usuario)
        {
            var uri = ConfigurationManager.AppSettings["UriActualizarAvanceMetaProducto"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, IndicadorDto, Usuario, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        #endregion


    }
}
