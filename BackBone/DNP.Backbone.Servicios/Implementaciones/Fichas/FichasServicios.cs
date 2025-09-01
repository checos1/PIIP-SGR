using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Fichas;
using DNP.Backbone.Dominio.Dto.Transversal;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Fichas;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Fichas
{
    public class FichasServicios : IFichasServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiFichasProyectos"];

        public FichasServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<byte[]> GenerarFicha(RecibirParametrosDto parametros, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["UriGenerarFicha"];
            return await _clienteHttpServicios.RequestApiByteArray(MetodosServiciosWeb.Post, urlBase + uri, string.Empty, parametros, usuarioDNP);
        }

        public async Task<ReporteDto> ObtenerIdFicha(string nombre, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["UriObtenerIdFicha"] + nombre;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReporteDto>(respuesta);
            return json;
        }

        public async Task<string> GenerarFichaManualSubFlujoSGR(ObjetoNegocio objObjetoNegocio, string usuario)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uriMetodo = ConfigurationManager.AppSettings["UriGenerarFichaManualSubFlujoSGR"];
            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, objObjetoNegocio, usuario));
        }
    }
}