using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Comunes.Extensiones;
using DNP.Backbone.Dominio.Dto.MensajeMantenimiento;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace DNP.Backbone.Servicios.Implementaciones.MensajesMantenimiento
{
    public class MensajeMantenimientoServicio : IMensajeMantenimientoServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private string urlBaseNotificaciones => ConfigurationManager.AppSettings["ApiNotificacion"];

        public MensajeMantenimientoServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<MensajeMantenimientoDto> CrearActualizarMensaje(ParametrosMensajeMantenimiento parametros)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearActualizarMensaje"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.PostAsync,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: string.Empty,               
                peticion: parametros.MensajeMantenimientoDto,
                usuarioDnp: parametros.ParametrosDto?.IdUsuarioDNP,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<MensajeMantenimientoDto>(respuestaViewModel);

            throw HandleError(respuestaViewModel);
        }

        public async Task EliminarMensaje(ParametrosMensajeMantenimiento parametros)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarMensaje"];
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            foreach (var id in parametros.FiltroDto.Ids ?? new int?[] { })
                queryString.Add("idsMensajes", id.ToString());

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.Delete,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: $"?{queryString.ToString()}",
                peticion: null,
                usuarioDnp: parametros.ParametrosDto?.IdUsuarioDNP,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel.HttpStatusCode == HttpStatusCode.OK)
                return;

            HandleError(respuestaViewModel);
        }

        public async Task<IEnumerable<MensajeMantenimientoDto>> ObtenerListaMensajes(ParametrosMensajeMantenimiento parametros)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListaMensajes"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.PostAsync,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: string.Empty,
                peticion: parametros.FiltroDto,
                usuarioDnp: parametros.ParametrosDto?.IdUsuarioDNP,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<IEnumerable<MensajeMantenimientoDto>>(respuestaViewModel);

            throw HandleError(respuestaViewModel);
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

        private TResult HandleExito<TResult>(RespuestaViewModel respuesta)
        {
            var serialize = respuesta.Data.Serialize();
            if (serialize.TryDeserialize<TResult>(out var result))
                return result;

            throw HandleError(respuesta);
        }
    }
}
