using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Comunes.Extensiones;
using DNP.Backbone.Dominio.Dto.UsuarioNotificacion;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.UsuarioNotificacion;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.UsuarioNotificacion
{
    public class MensajeNotificacionServicio : IMensajeNotificacionServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;

        private string urlBaseNotificaciones => ConfigurationManager.AppSettings["ApiNotificacion"];

        public MensajeNotificacionServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<IEnumerable<UsuarioNotificacionDto>> OtenerMensajeNotificaciones(UsuarioNotificacionMensajesFiltroDto filtro, string usuarioLogado)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerNotificacionesUsuario"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.Get,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: string.Empty,
                peticion: null,
                usuarioDnp: usuarioLogado);

            IEnumerable<UsuarioNotificacionDto> ret = JsonConvert.DeserializeObject<IEnumerable<UsuarioNotificacionDto>>(respuesta);
            if (ret != null &&  ret.Any())
                Filtrar(ref ret, filtro);
            return ret;
        }

        private void Filtrar(ref IEnumerable<UsuarioNotificacionDto> lista, UsuarioNotificacionMensajesFiltroDto filtroDto)
        {
            if (!string.IsNullOrEmpty(filtroDto.Notificacion))
            {
                lista = lista.Where(p => p.UsuarioConfigNotificacion.ContenidoNotificacion.Contains(filtroDto.Notificacion) || p.UsuarioConfigNotificacion.NombreNotificacion.Contains(filtroDto.Notificacion));
            }
            if (filtroDto.UsuarioYaLeyo.HasValue)
            {
                lista = lista.Where(p => p.UsuarioYaLeyo.Equals(filtroDto.UsuarioYaLeyo));
            }
            if (filtroDto.Fecha.HasValue)
            {
                lista = lista.Where(p => p.UsuarioConfigNotificacion.FechaInicio.Date.Equals(filtroDto.Fecha.Value.Date));
            }
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
