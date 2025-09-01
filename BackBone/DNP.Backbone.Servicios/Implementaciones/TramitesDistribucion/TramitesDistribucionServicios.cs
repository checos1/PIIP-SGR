using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.TramitesDistribucion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.TramitesDistribucion
{
    public class TramitesDistribucionServicios: ITramitesDistribucionServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        public TramitesDistribucionServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<string> ObtenerTramitesDistribucionAnteriores(Guid? instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarTramitesDistribucionAnteriores"];
            if (instanciaId == null)
            {
                instanciaId = Guid.Empty;
            }
            uri += "?instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}
