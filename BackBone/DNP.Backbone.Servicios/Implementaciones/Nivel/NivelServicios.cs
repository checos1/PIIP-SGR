using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Nivel;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Nivel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Nivel
{
    public class NivelServicios : INivelServicios
    {

        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly string ENDPOINT = ConfigurationManager.AppSettings["ApiPiipCore"];

        public NivelServicios(IClienteHttpServicios clienteHttpServicios)
        {
            this._clienteHttpServicios = clienteHttpServicios;
        }

        async public Task<List<NivelDto>> ObtenerPorIdPadreIdNivelTipo(Guid? idPadre, string claveNivelTipo, string idUsuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerNivelesPorIdPadreIdNivel"];
            Guid idNivelTipo = ObtenerIdNivelTipo(claveNivelTipo);
            string aplicacion = ConfigurationManager.AppSettings["IdAplicacionBackbone"];
            var parametros = $"?idNivel={idPadre}&idTipoNivel={idNivelTipo}&usuarioDnp={idUsuarioDnp}&aplicacion={aplicacion}";

            return JsonConvert.DeserializeObject<List<NivelDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, parametros, null, idUsuarioDnp, useJWTAuth: false));
        }

        private Guid ObtenerIdNivelTipo(string claveNivelTipo)
        {
            switch (claveNivelTipo)
            {
                case "MACROPROCESO":
                    return new Guid(ConfigurationManager.AppSettings["idNivelMacroproceso"]);
                case "PROCESO":
                    return new Guid(ConfigurationManager.AppSettings["idNivelProceso"]);
                case "SUBPROCESO":
                    return new Guid(ConfigurationManager.AppSettings["idNivelSubproceso"]);
            }
            return Guid.NewGuid();
        }
    }
}
