namespace DNP.Backbone.Servicios.Implementaciones.SGR
{

    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;

    using Interfaces;
    using Interfaces.SGR;

    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.SGR;
    using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
    using DNP.Backbone.Dominio.Dto.SGR.Transversal;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.SeguimientoControl;
    using System;

    public class SGRViabilidadServicios : ISGRViabilidadServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="clienteHttpServicios">Instancia de clienteHttp</param>
        public SGRViabilidadServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<ParametroDto> SGR_Transversal_LeerParametro(string parametro, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_Transversal_LeerParametro"];
            var parametros = $"?parametro={parametro}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<ParametroDto>(respuesta);
            return lst;
        }

        public async Task<List<ListaParametrosDto>> SGR_Transversal_LeerListaParametros(string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Transversal_LeerListaParametros"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<List<ListaParametrosDto>>(respuesta);

            return lst;
        }
        /// <summary>
        /// Obtiene los requisitos de un proyecto
        /// </summary>
        public async Task<List<LstAcuerdoSectorClasificadorDto>> SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Acuerdo_LeerProyecto"];
            var parametros = $"?proyectoId={proyectoId}&nivelId={nivelId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<List<LstAcuerdoSectorClasificadorDto>>(respuesta);

            return lst;
        }


        public async Task<string> SGR_Acuerdo_GuardarProyecto(AcuerdoSectorClasificadorDto obj, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_Acuerdo_GuardarProyecto"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostJson, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;

            return response;
        }

        public async Task<List<ListaDto>> SGR_Proyectos_LeerListas(System.Guid nivelId, int proyectoId, string nombreLista, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Proyectos_LeerListas"];
            var parametros = $"?nivelId={nivelId}&proyectoId={proyectoId}&nombreLista={nombreLista}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var lst = JsonConvert.DeserializeObject<List<ListaDto>>(respuesta);

            return lst;
        }

        public async Task<LeerInformacionGeneralViabilidadDto> SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string usuarioDNP, string tipoConceptoViabilidadCode)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Viabilidad_LeerInformacionGeneral"];
            var parametros = $"?proyectoId={proyectoId}&instanciaId={instanciaId}&tipoConceptoViabilidadCode={tipoConceptoViabilidadCode}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            var response = JsonConvert.DeserializeObject<LeerInformacionGeneralViabilidadDto>(respuesta);
            return response;
        }

        public async Task<List<LeerParametricasViabilidadDto>> SGR_Viabilidad_LeerParametricas(int proyectoId, System.Guid nivelId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_Viabilidad_LeerParametricas"];
            var parametros = $"?proyectoId={proyectoId}&nivelId={nivelId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            respuesta = JsonConvert.DeserializeObject<string>(respuesta);
            var response = JsonConvert.DeserializeObject<List<LeerParametricasViabilidadDto>>(respuesta);
            return response;
        }

        public async Task<ResultadoProcedimientoDto> SGR_Viabilidad_GuardarInformacionBasica(InformacionBasicaViabilidadDto obj, string usuarioDNP)
        {
            var jsonObjeect = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var uriMetodo = ConfigurationManager.AppSettings["uriSGR_Viabilidad_GuardarInformacionBasica"];
            var mensaje = JsonConvert.SerializeObject(obj);
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostJson, urlBase, uriMetodo, null, obj, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(response);
        }

        public async Task<string> SGR_Viabilidad_ObtenerPuntajeProyecto(System.Guid instanciaId, int entidadId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_ObtenerPuntajeProyecto"];
            var parametros = $"?instanciaId={instanciaId}&entidadId={entidadId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, parametros, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;

            respuesta = JsonConvert.DeserializeObject<string>(respuesta);

            return respuesta;
        }

        public async Task<ResultadoProcedimientoDto> SGR_Viabilidad_GuardarPuntajeProyecto(string puntajesProyecto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriSGR_GuardarPuntajeProyecto"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, puntajesProyecto, usuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(respuesta);
            return json;
        }

    }
}
