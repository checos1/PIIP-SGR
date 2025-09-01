namespace DNP.Backbone.Servicios.Implementaciones.Cache
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using Comunes.Enums;
    using Dominio.Dto.AutorizacionNegocio;
    using Interfaces;
    using Interfaces.Cache;
    using Newtonsoft.Json;

    public class CacheEntidadesNegocioServicios : ICacheEntidadesNegocioServicios
    {
        public IClienteHttpServicios _clienteHttpServicios;

        public CacheEntidadesNegocioServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<List<EntidadNegocioDto>> ConsultarEntidadesPorTipoEntidad(
            string usuarioDnp, string tipoEntidad)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiCache"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarEntidadesPorTipoEntidad"];
            var parametros = $"?usuarioDnp={usuarioDnp}&tipoEntidad={tipoEntidad}";

            return JsonConvert.DeserializeObject<List<EntidadNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<RolNegocioDto>> ConsultarRoles(string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiCache"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarRoles"];
            var parametros = $"?usuarioDnp={usuarioDnp}";
            return JsonConvert.DeserializeObject<List<RolNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<SectorNegocioDto>> ConsultarSectores(string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiCache"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarSectores"];
            var parametros = $"?usuarioDnp={usuarioDnp}";
            return JsonConvert.DeserializeObject<List<SectorNegocioDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp, useJWTAuth: false));
        }
    }
}
