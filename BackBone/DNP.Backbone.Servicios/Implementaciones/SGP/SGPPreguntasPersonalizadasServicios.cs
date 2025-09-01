namespace DNP.Backbone.Servicios.Implementaciones.SGP
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Dominio.Dto.Tramites;

    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using DNP.Backbone.Dominio.Dto.Preguntas;
    using Interfaces;
    using Interfaces.SGP;

    public class SGPPreguntasPersonalizadasServicios : ISGPPreguntasPersonalizadasServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="clienteHttpServicios">Instancia de clienteHttp</param>
        public SGPPreguntasPersonalizadasServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// Lista de preguntas
        /// </summary>
        /// <returns>Task<List<ServicioPreguntasPersonalizadasDto>></returns>
        public async Task<ServicioPreguntasPersonalizadasDto> ConsultarPreguntasSGP(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarPreguntasSGP"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, "?bPin=" + string.Join("&bPin=", bPin) + "&nivelId=" + string.Join("&nivelId=", nivelId) + "&instanciaId=" + string.Join("&instanciaId=", instanciaId) + "&listaRoles=" + string.Join("&listaRoles=", listaRoles), null, usuarioDnp, useJWTAuth: false);
            
            if (string.IsNullOrEmpty(respuesta)) return new ServicioPreguntasPersonalizadasDto();
            return JsonConvert.DeserializeObject<ServicioPreguntasPersonalizadasDto>(respuesta);
        }

        /// <summary>
        /// Lista de preguntas
        /// </summary>
        /// <returns>Task<List<ServicioPreguntasPersonalizadasDto>></returns>
        public async Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasSGPComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPreguntasPersonalizadasSGPComponente"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo,
                "?bPin=" + string.Join("&bPin=", bPin) +
                "&nivelId=" + string.Join("&nivelId=", nivelId) +
                "&instanciaId=" + string.Join("&instanciaId=", instanciaId) +
                "&nombreComponente=" + string.Join("&nombreComponente=", nombreComponente) +
                "&listaRoles=" + string.Join("&listaRoles=", listaRoles),
                null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return new ServicioPreguntasPersonalizadasDto();
            return JsonConvert.DeserializeObject<ServicioPreguntasPersonalizadasDto>(respuesta);
        }

        /// <summary>
        /// Lista de preguntas
        /// </summary>
        /// <returns>Task<List<ServicioPreguntasPersonalizadasDto>></returns>
        public async Task<ServicioPreguntasPersonalizadasDto> ObtenerPreguntasPersonalizadasSGPComponenteSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPreguntasPersonalizadasSGPComponenteSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo,
                "?bPin=" + string.Join("&bPin=", bPin) +
                "&nivelId=" + string.Join("&nivelId=", nivelId) +
                "&instanciaId=" + string.Join("&instanciaId=", instanciaId) +
                "&nombreComponente=" + string.Join("&nombreComponente=", nombreComponente) +
                "&listaRoles=" + string.Join("&listaRoles=", listaRoles),
                null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return new ServicioPreguntasPersonalizadasDto();
            return JsonConvert.DeserializeObject<ServicioPreguntasPersonalizadasDto>(respuesta);
        }

        /// <summary>
        /// Datos generales del proyecto
        /// </summary>
        /// <returns>Task<List<DatosGeneralesProyectosDto>></returns>
        public async Task<DatosGeneralesProyectosDto> ObtenerDatosGeneralesProyecto(int? ProyectoId, Guid NivelId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosGeneralesProyectoSGP"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, "?ProyectoId=" + string.Join("&ProyectoId=", ProyectoId) + "&NivelId=" + string.Join("&NivelId=", NivelId), null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return new DatosGeneralesProyectosDto();
            return JsonConvert.DeserializeObject<DatosGeneralesProyectosDto>(respuesta);
        }

        /// <summary>
        /// Datos generales del proyecto
        /// </summary>
        /// <returns>Task<List<DatosGeneralesProyectosDto>></returns>
        public async Task<ConfiguracionEntidadDto> ObtenerConfiguracionEntidades(int? ProyectoId, Guid NivelId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerConfiguracionEntidadesSGP"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, "?ProyectoId=" + string.Join("&ProyectoId=", ProyectoId) + "&NivelId=" + string.Join("&NivelId=", NivelId), null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return new ConfiguracionEntidadDto();
            return JsonConvert.DeserializeObject<ConfiguracionEntidadDto>(respuesta);
        }

        /// <summary>
        /// Guardar Preguntas Personalizadas
        /// </summary>
        /// <returns>Task<RespuestaGeneralDto></returns>
        public async Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadas(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarPreguntasPersonalizadasSGP"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        /// <summary>
        /// Guardar Preguntas Personalizadas
        /// </summary>
        /// <returns>Task<RespuestaGeneralDto></returns>
        public async Task<RespuestaGeneralDto> GuardarPreguntasPersonalizadasSGP(ServicioPreguntasPersonalizadasDto parametros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarPreguntasPersonalizadasCustomSGP"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        /// <summary>
        /// Guardar Preguntas Personalizadas
        /// </summary>
        /// <returns>Task<RespuestaGeneralDto></returns>
        public async Task<RespuestaGeneralDto> DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDevolverCuestionarioProyectoSGP"];
            uriMetodo += "?nivelId=" + nivelId.ToString() + "&instanciaId=" + instanciaId.ToString() + "&estadoAccionesPorInstancia=" + estadoAccionesPorInstancia.ToString();
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, null, usuarioDnp));
        }
       
    }
}
