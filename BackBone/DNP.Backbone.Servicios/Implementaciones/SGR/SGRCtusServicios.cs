using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.SGR;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.SGR.CTUS;
using System;

namespace DNP.Backbone.Servicios.Implementaciones.SGR
{
    public class SGRCtusServicios : ISGRCtusServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="clienteHttpServicios">Instancia de clienteHttp</param>
        public SGRCtusServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<ConceptoCTUSDto> SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_CTUS_LeerProyectoCtusConcepto"];
            var parametros = $"?proyectoCtusId={proyectoCtusId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, parametros, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ConceptoCTUSDto>(respuesta);
        }

        public async Task<ResultadoProcedimientoDto> SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_CTUS_GuardarProyectoCtusConcepto"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
        }

        public async Task<ResultadoProcedimientoDto> SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_CTUS_GuardarAsignacionUsuarioEncargado"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
        }

        public async Task<UsuarioEncargadoCTUSDto> SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_CTUS_LeerProyectoCtusUsuarioEncargado"];
            var parametros = $"?proyectoCtusId={proyectoCtusId}&instanciaId={instanciaId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, parametros, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<UsuarioEncargadoCTUSDto>(respuesta);
        }

        public async Task<ResultadoProcedimientoDto> SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_CTUS_GuardarResultadoConceptoCtus"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
        }

        /// <summary>
        /// Actualizar entidad adscrita CTUS
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="usuarioDnp"></param> 
        /// <returns>int</returns> 
        public async Task<bool> SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_CTUS_ActualizarEntidadAdscritaCTUS"];
            uri = $"{uri}?proyectoId={proyectoId}&entityId={entityId}&tipo={tipo}&user={usuarioDnp}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            var listaEnt = JsonConvert.DeserializeObject<bool>(response);
            return listaEnt;
        }
    }
}
