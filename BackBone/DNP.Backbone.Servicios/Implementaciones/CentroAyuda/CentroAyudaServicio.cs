using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Comunes.Extensiones;
using DNP.Backbone.Dominio.Dto.CentroAyuda;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.CentroAyuda;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.CentroAyuda
{
    public class CentroAyudaServicio : ICentroAyudaServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private string urlBaseNotificaciones => ConfigurationManager.AppSettings["ApiNotificacion"];

        public CentroAyudaServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// Listar los temas de ayudas por FiltroDto
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AyudaTemaListaItemDto>> ObtenerListaTemas(AyudaTemaFiltroDto dto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListaTemas"];
            var respuesta = JsonConvert.DeserializeObject<List<AyudaTemaListaItemDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBaseNotificaciones, uriMetodo, null, dto, usuarioDnp, useJWTAuth: false));
            return respuesta;
        }

        public async Task<AyudaTemaListaItemDto> CrearActualizarTema(AyudaTemaListaItemDto dto, string usuarioDnp)
        {
            string uriMetodo;
            if(dto.Id == default) uriMetodo = ConfigurationManager.AppSettings["uriCrearAyudaTema"];
            else uriMetodo = ConfigurationManager.AppSettings["uriEditarAyudaTema"];

            var serialize = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostAsync, urlBaseNotificaciones, uriMetodo, null, dto, usuarioDnp, useJWTAuth: false);
            var respuesta = JsonConvert.DeserializeObject<RespuestaViewModel>(serialize);
            if (respuesta != null && respuesta?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<AyudaTemaListaItemDto>(respuesta);

            throw HandleError(respuesta);
        }

        public async Task<bool> EliminarTema(int id, string usuarioDnp)
        {
            var dto = new AyudaTemaListaItemDto { Id = id };
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarAyudaTema"];
            var serialize = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.PostAsync, urlBaseNotificaciones, uriMetodo, null, dto, usuarioDnp, useJWTAuth: false);
            var respuesta = JsonConvert.DeserializeObject<RespuestaViewModel>(serialize);
            if (respuesta != null && respuesta?.HttpStatusCode == HttpStatusCode.OK)
                return (bool) HandleExito<dynamic>(respuesta).Exito;

            throw HandleError(respuesta);
        }

        private TResult HandleExito<TResult>(RespuestaViewModel respuesta)
        {
            var serialize = respuesta.Data.Serialize();
            if (serialize.TryDeserialize<TResult>(out var result))
                return result;

            throw HandleError(respuesta);
        }

        private Exception HandleError(RespuestaViewModel respuesta)
        {
            if (respuesta?.Data?.GetType().GetInterfaces().Contains(typeof(IEnumerable)) ?? false && respuesta?.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                var serialize = respuesta.Data.Serialize();
                serialize.TryDeserialize<string[]>(out var errors);
                return new BackboneException(resultado: false, errors);
            }

            return new Exception(respuesta?.MensajeRetorno ?? "Hubo un error al enviar la solicitud");
        }
    }
}
