using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Comunes.Extensiones;
using DNP.Backbone.Dominio.Dto;
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
using System.Threading.Tasks;
using System.Web;

namespace DNP.Backbone.Servicios.Implementaciones.UsuarioNotificacion
{
    public class UsuarioNotificacionConfigServicio : IUsuarioNotificacionConfigServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;

        public UsuarioNotificacionConfigServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        private string urlBaseNotificaciones => ConfigurationManager.AppSettings["ApiNotificacion"];
        private string urlBaseAutorizacion => ConfigurationManager.AppSettings["ApiAutorizacion"];

        public async Task<UsuarioNotificacionConfigDto> CrearActualizarConfigNotificacion(UsuarioNotificacionConfigDto dto, string usuarioLogado)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearActualizarUsuarioNotificacionConfig"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.PostAsync,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: string.Empty,
                peticion: dto,
                usuarioDnp: usuarioLogado,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<UsuarioNotificacionConfigDto>(respuestaViewModel);

            throw HandleError(respuestaViewModel);
        }

        public async Task<RespuestaViewModel> EliminarConfigNotificacion(string usuarioLogado, params int[] ids)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarUsuarioNotificacionConfig"];
            var queryString = HttpUtility.ParseQueryString(string.Empty);


            foreach (var id in ids)
                queryString.Add("idsConfig", id.ToString());

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.Delete,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: $"?{queryString}",
                peticion: null,
                usuarioDnp: usuarioLogado,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel == null && respuestaViewModel.HttpStatusCode != HttpStatusCode.OK)
                HandleError(respuestaViewModel);

            return respuestaViewModel;
        }

        public async Task<IEnumerable<UsuarioNotificacionDto>> MarcarNotificacionComoLeida(UsuarioNotificacionFiltroDto filtro, string usuarioLogado)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriMarcarNotificacionComoLeida"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.PostAsync,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: string.Empty,
                peticion: filtro,
                usuarioDnp: usuarioLogado,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<IEnumerable<UsuarioNotificacionDto>>(respuestaViewModel);

            throw HandleError(respuestaViewModel);
        }

        public async Task<IEnumerable<UsuarioNotificacionConfigDto>> OtenerConfigNotificaciones(UsuarioNotificacionConfigFiltroDto filtro, string usuarioLogado)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerListaConfigNotificaciones"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.PostAsync,
                urlBaseNotificaciones,
                uriMetodo,
                parametros: string.Empty,
                peticion: filtro,
                usuarioDnp: usuarioLogado,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<IEnumerable<UsuarioNotificacionConfigDto>>(respuestaViewModel);

            throw HandleError(respuestaViewModel);
        }

        public async Task<IEnumerable<ProcedimientoAlmacenadoDto>> ObtenerProcedimentosAlmacenados(string usuarioLogado)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProcedimientosDisponibles"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.GetAsync,
                urlBaseNotificaciones,
                uriMetodo,
                peticion: null,
                parametros: string.Empty,
                usuarioDnp: usuarioLogado,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<IEnumerable<ProcedimientoAlmacenadoDto>>(respuestaViewModel);

            throw HandleError(respuestaViewModel);
        }

        public async Task<ProcedimientoAlmacenadoDto> ObtenerProcedimentoAlmacenadoPorId(string id, string usuarioLogado)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProcedimientoPorId"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.GetAsync,
                urlBaseNotificaciones,
                uriMetodo,
                peticion: null,
                parametros: id,
                usuarioDnp: usuarioLogado,
                useJWTAuth: false);

            var respuestaViewModel = JsonConvert.DeserializeObject<RespuestaViewModel>(respuesta);
            if (respuestaViewModel != null && respuestaViewModel?.HttpStatusCode == HttpStatusCode.OK)
                return HandleExito<ProcedimientoAlmacenadoDto>(respuestaViewModel);

            throw HandleError(respuestaViewModel);
        }

        public async Task<IEnumerable<UsuarioProcedimentoAlmacenadoDto>> ObtenerUsuariosPorProcedimentoAlmacenado(string id, string usuarioLogado)
        {
            var procedimiento = await ObtenerProcedimentoAlmacenadoPorId(id, usuarioLogado);

            if (procedimiento.NombreProcedimiento.Equals("Pantalla Programación"))
            {
                return new List<UsuarioProcedimentoAlmacenadoDto>() 
                {
                    new UsuarioProcedimentoAlmacenadoDto { IdUsuarioDnp = "Pantalla Programación", NombreUsuario = "Pantalla Programación" }
                };
            }

            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuarioPorProcedimentoAlmacenado"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(
                MetodosServiciosWeb.PostAsync,
                urlBaseAutorizacion,
                uriMetodo,
                peticion: procedimiento.SelectQuery,
                parametros: string.Empty,
                usuarioDnp: usuarioLogado,
                useJWTAuth: false);

            if (string.IsNullOrWhiteSpace(respuesta))
                throw new BackboneException($"Hubo un error al consultar usuarios por Procedimiento Almacenado de Id {id}");
            
            return JsonConvert.DeserializeObject<IEnumerable<UsuarioProcedimentoAlmacenadoDto>>(respuesta);
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
