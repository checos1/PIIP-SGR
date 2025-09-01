using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.Tramites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace DNP.Backbone.Servicios.Implementaciones.ServiciosNegocio
{
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.Conpes;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Orfeo;
    using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
    using DNP.Backbone.Dominio.Dto.Productos;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Dominio.Dto.Tramites.VigenciaFutura;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Dto.Usuario;
    using DNP.Backbone.Dominio.Dto.VigenciasFuturas;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using Interfaces;
    using Interfaces.ServiciosNegocio;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using DNP.Backbone.Dominio.Dto.Transferencias;
    using DNP.Backbone.Dominio.Dto.Acciones;
    using DNP.Backbone.Dominio.Dto.Tramites.Reprogramacion;
    using DNP.Backbone.Dominio.Dto.Tramites.ProgramacionDistribucion;

    /// <summary>
    /// Clase responsable de consumir los servicios del proyecto Servicio Negocio
    /// </summary>
    public class ServiciosNegocioServicios : IServiciosNegocioServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        public ServiciosNegocioServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// Lista de proyectos por trámite
        /// </summary>
        /// <param name="parametros">Paramentros del proyectos</param>
        /// <returns></returns>
        public async Task<List<ProyectoDto>> ObtenerListaProyectoPorTramite(ParametrosProyectosDto parametros)
        {
            var uri = ConfigurationManager.AppSettings["uriProyectosPorTramite"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoDto>>(respuesta);
        }
        /// <summary>
        /// Lista para obter catalogo
        /// </summary>
        /// <param name="peticion">peticion</param>
        /// <param name="catalogoEnum">Enum Catalogo</param>
        /// <returns>Task<List<CatalogoDto>></returns>
        public async Task<List<CatalogoDto>> ObtenerListaCatalogo(ProyectoParametrosDto peticion, CatalogoEnum catalogoEnum)
        {
            var uri = ConfigurationManager.AppSettings["uriListaCatalogo"] + catalogoEnum.ToString();

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<CatalogoDto>>(respuesta);
        }

        /// <summary>
        /// Lista para obter catalogo
        /// </summary>
        /// <param name="peticion">peticion</param>
        /// <param name="catalogoEnum">Enum Catalogo</param>
        /// <returns>Task<List<CatalogoDto>></returns>
        public async Task<string> ObtenerCatalogoReferencia(EntidadesPorCodigoParametrosDto peticion, string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia)
        {
            var uri = ConfigurationManager.AppSettings["uriListaCatalogo"] + nombreCatalogo + "/" + idCatalogo + "/" + nombreCatalogoReferencia;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            //return JsonConvert.DeserializeObject<List<CatalogoDto>>(respuesta);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerListaCatalogoEntidades(ProyectoParametrosDto peticion, CatalogoEnum catalogoEnum)
        {
            var uri = ConfigurationManager.AppSettings["uriListaCatalogo"] + catalogoEnum.ToString();

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="peticionObtenerProyecto"></param>
        /// <param name="catalogoEnum"></param>
        /// <returns></returns>
        public async Task<List<EntidadCatalogoSTDto>> ObtenerListaCatalogoDT(ProyectoParametrosDto peticion, CatalogoEnum catalogoEnum)
        {
            var uri = ConfigurationManager.AppSettings["uriListaCatalogo"] + catalogoEnum.ToString();

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EntidadCatalogoSTDto>>(respuesta);
        }

        /// <summary>
        /// Lista de estado del proyecto
        /// </summary>
        /// <param name="peticion">peticion</param>
        /// <returns>Task<List<EstadoDto>></returns>
        public async Task<List<EstadoDto>> ObtenerListaEstado(ProyectoParametrosDto peticion)
        {
            var uri = ConfigurationManager.AppSettings["uriListaEstado"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EstadoDto>>(respuesta);
        }

        /// <summary>
        /// Lista de estado del proyecto
        /// </summary>
        /// <param name="peticion">peticion</param>
        /// <returns>Task<List<EstadoDto>></returns>
        public async Task<List<ConfiguracionUnidadMatrizDTO>> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto peticion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerMatrizEntidadDestino"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ConfiguracionUnidadMatrizDTO>>(respuesta);
        }

        /// <summary>
        /// Lista de estado del proyecto
        /// </summary>
        /// <param name="peticion">peticion</param>
        /// <returns>Task<List<EstadoDto>></returns>
        public async Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto peticion, string idUsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriActualizarMatrizEntidadDestino"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, peticion, idUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<List<ProyectosEntidadesDto>> ObtenerProyectos(ParametrosProyectosDto dto, string idUsuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectos"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, dto, idUsuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectosEntidadesDto>>(respuesta);
        }

        /// <summary>
        ///  Invoca un el API para insertar un cambio de entidad para el proyecto actual
        /// </summary>
        /// <param name="auditoriaEntidad">Información del cambio de entidad del proyecto actual</param>
        /// <returns></returns>
        public async Task<object> InsertAuditoriaEntidadProyecto(Dominio.Dto.Proyecto.AuditoriaEntidadDto auditoriaEntidad, string idUsuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriInsertaAuditoriaProyecto"];

            var resultado = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, new { auditoria = auditoriaEntidad }, idUsuarioDNP, useJWTAuth: false);

            var tipoAnonimo = new { Datos = 0, EsExcepcion = false, MensajeExcepcion = string.Empty };
            return JsonConvert.DeserializeAnonymousType(resultado, tipoAnonimo);
        }

        /// <summary>
        ///  Obtiene el historial de cambio de entidades del proyecto actual
        /// </summary>
        /// <param name="proyectoId">Identificador del proyecto actual</param>
        /// <param name="idUsuarioDNP">Cuenta de usuario que ejecuta el cambio</param>
        /// <returns></returns>
        public async Task<object> ObtenerAuditoriaEntidadProyecto(int proyectoId, string idUsuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerAuditoriaProyecto"];

            var resultado = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?proyectoId={proyectoId}", null, idUsuarioDNP, useJWTAuth: false);

            var tipoAnonimo = new { Datos = new List<Dominio.Dto.Proyecto.AuditoriaEntidadDto>(), EsExcepcion = false, MensajeExcepcion = string.Empty };
            return JsonConvert.DeserializeAnonymousType(resultado, tipoAnonimo);
        }


        public async Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocio(int TramiteId, string usuarioDnp, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectosTramiteNegocio"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?TramiteId={TramiteId}", null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(response)) return new List<ProyectosEnTramiteDto>();

            return JsonConvert.DeserializeObject<List<ProyectosEnTramiteDto>>(response);

        }


        public async Task<string> ObtenerProyectoListaLocalizaciones(string bpin, string usuarioDnp, string tokenAutorizacion)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarProyectosLocalizaciones"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramite(int TipoTramiteId, string Rol, int tramiteId, string usuarioDnp, string tokenAutorizacion, string nivelId= null)
        {
            Guid roltmp = string.IsNullOrEmpty(Rol) ? new Guid() : new Guid(Rol);
            if (string.IsNullOrEmpty(nivelId) || nivelId.Equals("null"))  nivelId = "0";
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTipoDocumentoTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo = uriMetodo + "?TipoTramiteId=" + TipoTramiteId + "&Rol=" + roltmp + "&tramiteId=" + tramiteId + "&nivelId=" + nivelId;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(response)) return new List<TipoDocumentoTramiteDto>();

            return JsonConvert.DeserializeObject<List<TipoDocumentoTramiteDto>>(response);

        }

        public async Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarProyectosTramiteNegocio"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }


        public async Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriEliminarProyectosTramiteNegocio"];

            var url = uri + "?TramiteId=" + string.Join("&TramiteId=", instanciaTramiteDto.TramiteFiltroDto.TramiteId) + "&ProyectoId=" + string.Join("&ProyectoId=", instanciaTramiteDto.TramiteFiltroDto.ProyectoId);

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, url, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.TramiteFiltroDto.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);

        }


        public async Task<T> GetJsonObject<T>(string url, Dictionary<TipoCabecerasPeticion, string> cabecerasPeticion) where T : new()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.DefaultRequestHeaders.Accept.Clear();

                foreach (var cabecera in cabecerasPeticion)
                {
                    clienteHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", cabecera.Value);
                    clienteHttp.DefaultRequestHeaders.Add(ReemplazarCaracteresNombreCabecera(cabecera.Key.ToString()), cabecera.Value);
                }

                var respuesta = await clienteHttp.GetAsync(url);
                if (respuesta.IsSuccessStatusCode) return await respuesta.Content.ReadAsAsync<T>();

                var errorDeRespuesta = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(errorDeRespuesta);
            }

        }

        private static string ReemplazarCaracteresNombreCabecera(string nombreCabecera)
        {
            return string.IsNullOrEmpty(nombreCabecera) ? nombreCabecera : nombreCabecera.Replace('_', '-');
        }

        public async Task<RespuestaGeneralDto> ActualizarInstanciaProyecto(ProyectosTramiteDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarInstanciaProyecto"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }


        public async Task<List<JustificacionTramiteProyectoDto>> ObtenerPreguntasJustificacion(int TramiteId, int ProyectoId, int TipoTramiteId, int TipoRolId, string usuarioDnp, string IdNivel, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerPreguntasJustificacion"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, "?TramiteId=" + string.Join("&TramiteId=", TramiteId) + "&ProyectoId=" + string.Join("&ProyectoId=", ProyectoId) + "&TipoTramiteId=" + string.Join("&TipoTramiteId=", TipoTramiteId) + "&TipoRolId=" + string.Join("&TipoRolId=", TipoRolId) + "&IdNivel=" + string.Join("&IdNivel=", IdNivel), null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(response)) return new List<JustificacionTramiteProyectoDto>();
            return JsonConvert.DeserializeObject<List<JustificacionTramiteProyectoDto>>(response);
        }

        private Task<string> ConsumirServicioGet(string uriMetodo, string tokenAutorizacion)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tokenAutorizacion);

                var response = client.GetAsync(endPoint + uriMetodo).Result;

                return response.Content.ReadAsStringAsync();
            }
        }

        private static Task<bool> ConsumirServicioPost(object catalogo, string tokenAutorizacion, string uriMetodo)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", tokenAutorizacion);

                var response = client.PostAsJsonAsync(endPoint + uriMetodo, catalogo).Result;

                return Task.FromResult(response.IsSuccessStatusCode);
            }
        }

        public async Task<RespuestaGeneralDto> GuardarRespuestasJustificacion(List<JustificacionTramiteProyectoDto> parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarRespuestasJustificacion"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<FuentePresupuestalDto>> ObtenerFuentesInformacionPresupuestal(string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerFuentesInformacionPresupuestal"];

            return JsonConvert.DeserializeObject<List<FuentePresupuestalDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<ProyectoFuentePresupuestalDto>> ObtenerProyectoFuentePresupuestalPorTramite(int pProyectoId, int? pTramiteId, string pTipoProyecto, string usuarioDnp)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectoFuentePresupuestalPorTramite"];
            uriMetodo += "?pProyectoId=" + pProyectoId.ToString() + "&pTramiteId=" + pTramiteId + "&pTipoProyecto=" + pTipoProyecto;

            return JsonConvert.DeserializeObject<List<ProyectoFuentePresupuestalDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));

        }


        public async Task<List<ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, string usuarioDnp, bool isCDP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectoRequisitosPorTramite"];
            uriMetodo += "?pProyectoId=" + pProyectoId.ToString() + "&pTramiteId=" + pTramiteId + "&isCDP=" + isCDP;
            return JsonConvert.DeserializeObject<List<Dominio.Dto.Tramites.Proyectos.ProyectoRequisitoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));

        }

        public async Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarTramiteInformacionPresupuestal"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarTramiteTipoRequisito"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }


        public async Task<RespuestaGeneralDto> ActualizarValoresProyecto(ProyectosTramiteDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarValoresProyecto"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocio(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarEnviarDatosTramiteNegocio"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<JustificacionTematicaDto>> ObtenerPreguntasProyectoActualizacion(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerPreguntasProyectoActualizacion"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, "?TramiteId=" + string.Join("&TramiteId=", TramiteId) + "&ProyectoId=" + string.Join("&ProyectoId=", ProyectoId) + "&TipoTramiteId=" + string.Join("&TipoTramiteId=", TipoTramiteId) + "&IdNivel=" + string.Join("&IdNivel=", IdNivel) + "&TipoRolId=" + string.Join("&TipoRolId=", TipoRolId), null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(response)) return new List<JustificacionTematicaDto>();
            return JsonConvert.DeserializeObject<List<JustificacionTematicaDto>>(response);
        }

        public async Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioAprobacion(int TramiteId, int TipoRolId, string usuarioDnp, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectosTramiteNegocioAprobacion"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?TramiteId=" + TramiteId.ToString() + "&TipoRolId=" + TipoRolId;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<ProyectosEnTramiteDto>();

            return JsonConvert.DeserializeObject<List<ProyectosEnTramiteDto>>(response);

        }

        public async Task<List<TipoRequisitoDto>> ObtenerTiposRequisito(string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTiposRequisito"];
            return JsonConvert.DeserializeObject<List<TipoRequisitoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<List<FuentesTramiteProyectoAprobacionDto>> ObtenerFuentesTramiteProyectoAprobacion(int tramiteId, int proyectoId, string pTipoProyecto, string usuarioDnp, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerFuentesTramiteProyectoAprobacion"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?tramiteId=" + tramiteId.ToString() + "&proyectoId=" + proyectoId + "&pTipoProyecto=" + pTipoProyecto;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<FuentesTramiteProyectoAprobacionDto>();

            return JsonConvert.DeserializeObject<List<FuentesTramiteProyectoAprobacionDto>>(response);

        }


        public async Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarFuentesTramiteProyectoAprobacion"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<CodigoPresupuestalDto> ObtenerCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCodigoPresupuestal"];
            uriMetodo += "?tramiteId=" + tramiteId.ToString() + "&proyectoId=" + proyectoId + "&entidadId=" + entidadId;
            return JsonConvert.DeserializeObject<CodigoPresupuestalDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> ActualizarCodigoPresupuestal(int proyectoId, int entidadId, int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarCodigoPresupuestal"];
            uriMetodo += "?tramiteId=" + tramiteId.ToString() + "&proyectoId=" + proyectoId + "&entidadId=" + entidadId;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<ResponseDto<EnvioSubDireccionDto>> SolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto)
        {
            EnvioSubDireccionDto EnvioSubDireccionDto_ = JsonConvert.DeserializeObject<EnvioSubDireccionDto>(peticionObtenerProyecto.IdFiltro);
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarSolicitarConcepto"];
            return JsonConvert.DeserializeObject<ResponseDto<EnvioSubDireccionDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, EnvioSubDireccionDto_, peticionObtenerProyecto.IdUsuario));
        }

        public async Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConcepto(ProyectoParametrosDto peticionObtenerProyecto)
        {
            EnvioSubDireccionDto EnvioSubDireccionDto_ = new EnvioSubDireccionDto()
            {
                TramiteId = Convert.ToInt32(peticionObtenerProyecto.IdFiltro),
            };
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSolicitarConcepto"];
            List<EnvioSubDireccionDto> lstResutl = JsonConvert.DeserializeObject<List<EnvioSubDireccionDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, EnvioSubDireccionDto_, peticionObtenerProyecto.IdUsuario));
            if (lstResutl == null)
                return new List<EnvioSubDireccionDto>();
            foreach (EnvioSubDireccionDto item in lstResutl)
            {
                item.Usuario = "";
                item.Visible = true;
            }
            return lstResutl;
        }

        public async Task<List<TramitesProyectosDto>> ObtenerTarmitesPorProyectoEntidad(int proyectoId, int entidadId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTarmitesPorProyectoEntidad"];
            uriMetodo += "?proyectoId=" + proyectoId + "&entidadId=" + entidadId + "&usuarioDnp=" + usuarioDnp;
            return JsonConvert.DeserializeObject<List<TramitesProyectosDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<TramiteValoresProyectoDto> ObtenerValoresProyectos(int proyectoId, int tramiteId, int entidadId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerValoresProyectos"];
            uriMetodo += "?proyectoId=" + proyectoId + "&tramiteId=" + tramiteId + "&entidadId=" + entidadId;
            return JsonConvert.DeserializeObject<TramiteValoresProyectoDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> DevolverProyecto(DevolverProyectoDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriDevolverProyecto"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<ConceptoDireccionTecnicaTramiteDto>> ObtenerConceptoDireccionTecnicaTramite(int tramiteId, Guid nivelid, string usuario)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerConceptoDireccionTecnicaTramite"];
            uriMetodo += "?tramiteId=" + tramiteId + "&nivelid=" + nivelid;
            return JsonConvert.DeserializeObject<List<ConceptoDireccionTecnicaTramiteDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuario));
        }

        public async Task<RespuestaGeneralDto> GuardarConceptoDireccionTecnicaTramite(List<ConceptoDireccionTecnicaTramiteDto> parametros, string usuario)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarConceptoDireccionTecnicaTramite"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuario));
        }

        public RespuestaDocumentoCONPES ObtenerProyectoConpes(string conpes, string idUsuario)
        {
            RespuestaDocumentoCONPES respuesta = new RespuestaDocumentoCONPES();
            try
            {
                string titulo = string.Empty;
                string fechaInicial = string.Empty; ;
                string fechaFinal = string.Empty; ;
                string ano = string.Empty; ;
                string numeroCONPES = string.Empty; ;
                string id = string.Empty; ;

                if (DNP.Backbone.Comunes.Utilidades.Utilidad.IsNumeric(conpes))
                {
                    numeroCONPES = conpes;
                }
                else
                {
                    titulo = conpes;
                }


                String urlServicioSharepoint = "https://colaboracion.dnp.gov.co/CDT/_api/web/lists/getbytitle('Conpes')/items?$";
                String filtros = "";
                if (!string.IsNullOrEmpty(titulo))
                {
                    filtros = filtros + " and substringof('" + titulo + "',Title)";
                }
                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    filtros = filtros + " and Fecha_x0020_Documento ge datetime'" + fechaInicial + "T00:00:00Z' ";
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    filtros = filtros + " and Fecha_x0020_Documento le datetime'" + fechaFinal + "T23:59:59Z' ";
                }
                if (!string.IsNullOrEmpty(ano))
                {
                    filtros = filtros + " and A_x00f1_io eq '" + ano + "'";
                }
                if (!string.IsNullOrEmpty(numeroCONPES))
                {
                    String[] numeros = numeroCONPES.Split(',');
                    filtros = filtros + " and (";
                    String or = "";
                    numeros.ToList().ForEach(delegate (string numero)
                    {

                        filtros = filtros + or + " substringof('" + numero + "', N_x00fa_mero)";
                        or = "or";
                    });
                    filtros = filtros + ") ";

                }
                if (!String.IsNullOrEmpty(id))
                {
                    filtros = filtros + " and Id eq '" + id + "' ";
                }
                var resultCONPES = GETMethod(urlServicioSharepoint + "select=ID,N_x00fa_mero,Title,Fecha_x0020_Documento,Tipo_x0020_Conpes&$filter=Orden eq '1' " + filtros + " &$orderby=Fecha_x0020_Documento desc&$top=10000", true);


                JObject resultadoCONPESJO = JObject.Parse(Convert.ToString(resultCONPES));
                List<DocumentoCONPES> lstObjDocConpes = null;
                IEnumerable<JToken> documentosConpes = from p in resultadoCONPESJO["d"]["results"] orderby p["Id"] select p;
                if (documentosConpes.Count() > 0)
                {
                    lstObjDocConpes = new List<DocumentoCONPES>();
                    foreach (var periodo in documentosConpes)
                    {

                        string tipoConpesVal = "";
                        if (periodo["Tipo_x0020_Conpes"]["results"].Count() > 0)
                        {
                            tipoConpesVal = periodo["Tipo_x0020_Conpes"]["results"][0]["Label"].ToString();
                        }

                        DocumentoCONPES vDocumentoCONPES = new DocumentoCONPES();
                        vDocumentoCONPES.id = Convert.ToInt32(periodo["Id"].ToString());
                        //vDocumentoCONPES.ano = periodo["A_x00f1_io"].ToString();
                        vDocumentoCONPES.titulo = periodo["Title"].ToString();
                        vDocumentoCONPES.numeroCONPES = periodo["N_x00fa_mero"].ToString();
                        vDocumentoCONPES.fechaAprobacion = Convert.ToDateTime(periodo["Fecha_x0020_Documento"].ToString());//.ToString("yyyy-MM-dd hh:mm:ss"),
                        vDocumentoCONPES.tipoCONPES = tipoConpesVal;
                        //vDocumentoCONPES.docNombre = periodo["OData__dlc_DocId"].ToString();
                        //vDocumentoCONPES.docUrl = periodo["OData__dlc_DocIdUrl"]["Url"].ToString();

                        lstObjDocConpes.Add(vDocumentoCONPES);
                    }
                    respuesta.mensaje = "OK";
                }
                else
                {
                    respuesta.mensaje = "";
                }
                respuesta.estado = true;

                respuesta.documentosCONPES = lstObjDocConpes;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.estado = false;
                respuesta.mensaje = "Error en el Servicio Externo de SharePoint CONPES. " + ex.Message;
                return respuesta;
            }
        }

        public dynamic GETMethod(string url, bool returnJSON)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            if (returnJSON)
            {
                request.Accept = "application/json; odata=verbose";
            }
            var response = request.GetResponse() as HttpWebResponse;
            var stream = response.GetResponseStream();

            string textResult;
            using (var reader = new StreamReader(stream))
            {
                textResult = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject(textResult);
        }

        public async Task<RespuestaGeneralDto> ValidarEnviarDatosTramiteNegocioAprobacion(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarEnviarDatosTramiteNegocioAprobacion"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<PlantillaCarta> ObtenerPlantillaCarta(string nombreSeccion, int tipoTramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPlantillaCarta"];
            uriMetodo += "?tipoTramite=" + tipoTramite + "&nombreSeccion=" + nombreSeccion;
            return JsonConvert.DeserializeObject<PlantillaCarta>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<List<Carta>> ObtenerDatosCartaPorSeccion(int tramiteId, int plantillaSeccionId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosCartaPorSeccion"];
            uriMetodo += "?tramiteId=" + tramiteId + "&plantillaSeccionId=" + plantillaSeccionId;
            return JsonConvert.DeserializeObject<List<Carta>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }
        //Alejandro

        public async Task<string> ObtenerCartaConceptoDatosDespedida(int tramiteId, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarCartaConceptoDatosDespedida"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?tramiteId={tramiteId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<RespuestaGeneralDto> ActualizarCartaConceptoDatosDespedida(DatosConceptoDespedidaDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarCartaConceptoDatosDespedida"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<UsuarioTramite> VerificaUsuarioDestinatario(UsuarioTramite parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriVerificaUsuarioDestinatario"];
            return JsonConvert.DeserializeObject<UsuarioTramite>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> ActualizarCartaDatosIniciales(Carta parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarCartaDatosIniciales"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<UsuarioTramite>> ObtenerUsuariosRegistrados(int tramiteId, string numeroTramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerUsuariosRegistrados"];
            uriMetodo += "?tramiteId=" + tramiteId + "&numeroTramite=" + numeroTramite;
            return JsonConvert.DeserializeObject<List<UsuarioTramite>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }
        public async Task<CapituloConpes> CargarProyectoConpes(string proyectoid, Guid InstanciaId, string GuiMacroproceso, string idUsuario, string NivelId, string FlujoId)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriCargarProyectoConpes"];
            uriMetodo += "?proyectoid=" + proyectoid + "&GuiMacroproceso=" + GuiMacroproceso + "&InstanciaId=" + InstanciaId + "&NivelId=" + NivelId + "&FlujoId=" + FlujoId;
            return JsonConvert.DeserializeObject<CapituloConpes>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, idUsuario));
        }

        public async Task<EncabezadoGeneralDto> ObtenerEncabezadoGeneral(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioEncabezadoPie"];
            var uriMetodo = ConfigurationManager.AppSettings["uriEncabezadoGeneral"];
            var result = JsonConvert.DeserializeObject<EncabezadoGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
            return result;
        }

        public async Task<EncabezadoSGRDto> ObtenerEncabezadoSGR(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriEncabezadoSGR"];
            return JsonConvert.DeserializeObject<EncabezadoSGRDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<EncabezadoSGPDto> ObtenerEncabezadoSGP(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriEncabezadoSGP"];
            return JsonConvert.DeserializeObject<EncabezadoSGPDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<string> ObtenerDesagregarRegionalizacion(string bpin, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarDesagregarRegionalizacion"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<RespuestaGeneralDto> ActualizarDesagregarRegionalizacion(DesagregarRegionalizacionDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarDesagregarRegionalizacion"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> CargarFirma(string firma, string rolId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriCargarFirma"];
            FileToUploadDto parametros = new FileToUploadDto();
            parametros.FileAsBase64 = firma;
            parametros.RolId = new Guid(rolId);
            parametros.UsuarioId = usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> ValidarSiExisteFirmaUsuario(string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarSiExisteFirmaUsuario"];
            uriMetodo += "?idUsuario=" + usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> Firmar(int tramiteId, string radicadoSalida, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriFirmar"];
            uriMetodo += ("?tramiteId=" + tramiteId + "&radicadoSalida=" + radicadoSalida);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<List<SeccionCapituloDto>> SeccionesCapitulosModificadosByMacroproceso(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriSeccionesCapitulosModificadosByMacroproceso"];
            uriMetodo += "?guiMacroproceso=" + guiMacroproceso;
            uriMetodo += "&idProyecto=" + IdProyecto;
            uriMetodo += "&idInstancia=" + IdInstancia;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<List<SeccionCapituloDto>>(result);
        }

        public async Task<List<SeccionCapituloDto>> SeccionesCapitulosByMacroproceso(string guiMacroproceso, string usuarioDnp, string NivelId, string FlujoId)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriSeccionesCapitulosByMacroproceso"];
            uriMetodo += "?guiMacroproceso=" + guiMacroproceso;
            uriMetodo += "&NivelId=" + NivelId;
            uriMetodo += "&FlujoId=" + FlujoId;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<List<SeccionCapituloDto>>(result);
        }

        public async Task<List<RelacionPlanificacionDto>> CambiosRelacionPlanificacion(int IdProyecto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriCambiosRelacionPlanificacion"];
            uriMetodo += "?proyectoId=" + IdProyecto;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<List<RelacionPlanificacionDto>>(result);
        }

        public async Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes conpes, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriAdicionarProyectoConpes"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, conpes, usuarioDnp));
        }

        public async Task<List<DocumentoCONPES>> EliminarProyectoConpes(string proyectoid, string conpesid, string idUsuario)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarProyectoConpes"];
            uriMetodo += "?proyectoid=" + proyectoid + "&conpesid=" + conpesid;
            return JsonConvert.DeserializeObject<List<DocumentoCONPES>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, idUsuario));
        }

        public async Task<List<CuerpoConceptoCDP>> ObtenerCuerpoConceptoCDP(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCuerpoConceptoCDP"];
            uriMetodo += "?tramiteId=" + tramiteId;
            return JsonConvert.DeserializeObject<List<CuerpoConceptoCDP>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<List<CuerpoConceptoAutorizacion>> ObtenerCuerpoConceptoAutorizacion(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCuerpoConceptoAutorizacion"];
            uriMetodo += "?tramiteId=" + tramiteId;
            return JsonConvert.DeserializeObject<List<CuerpoConceptoAutorizacion>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> GuardarCambiosRelacionPlanificacion(CapituloModificado parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarCambiosRelacionPlanificacion"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> ValidarSeccionesCapitulosByMacroproceso(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriValidarSeccionesCapitulosByMacroproceso"];
            uriMetodo += "?guiMacroproceso=" + guiMacroproceso;
            uriMetodo += "&idProyecto=" + IdProyecto;
            uriMetodo += "&idInstancia=" + IdInstancia;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(result);
        }

        public async Task<Carta> ConsultarCarta(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarCarta"];
            uriMetodo += "?tramiteId=" + tramiteId;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<Carta>(result);
        }

        public async Task<string> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uriMetodo = ConfigurationManager.AppSettings["uriReasignarRadicadoORFEO"];
            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuario));
        }

        public async Task<ResponseDto<bool>> CargarDocumentoElectronicoOrfeo(DatosDocumentoElectronicoDSDto parametros, string usuarioDnp)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uriMetodo = ConfigurationManager.AppSettings["UriCargarDocumentoElectronicoORFEO"];
            return JsonConvert.DeserializeObject<ResponseDto<bool>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarEstadoAjusteProyecto"];
            uriMetodo += "?tipoDevolucion=" + tipoDevolucion + "&ObjetoNegocioId=" + objetoNegocioId + "&tramiteId=" + tramiteId + "&observacion=" + observacion;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> GuardarCambiosJustificacionHorizonte(CapituloModificado parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarCambiosJustificacionHorizonte"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<JustificacionHorizontenDto>> ObtenerJustificacionHorizonte(int IdProyecto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerJustificacionHorizonte"];
            uriMetodo += "?proyectoId=" + IdProyecto;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<List<JustificacionHorizontenDto>>(result);
        }

        public async Task<CapituloModificado> ObtenerCapitulosModificadosCapitoSeccion(string guiMacroproceso, int idProyecto, Guid idInstancia, string capitulo, string seccion, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCapitulosModificadosCapitoSeccion"];
            uriMetodo += "?guiMacroproceso=" + guiMacroproceso;
            uriMetodo += "&idProyecto=" + idProyecto;
            uriMetodo += "&idInstancia=" + idInstancia;
            uriMetodo += "&capitulo=" + capitulo;
            uriMetodo += "&seccion=" + seccion;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<CapituloModificado>(result);
        }

        public async Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerErroresProyecto"];
            uriMetodo += "?GuidMacroproceso=" + guiMacroproceso;
            uriMetodo += "&idProyecto=" + IdProyecto;
            uriMetodo += "&GuidInstancia=" + IdInstancia;
            uriMetodo += "&usuarioDNP=" + usuarioDnp;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ErroresProyectoDto>>(result);
        }

        public async Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(string guiMacroproceso, int IdProyecto, string IdInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerErroresSeguimiento"];
            uriMetodo += "?GuidMacroproceso=" + guiMacroproceso;
            uriMetodo += "&idProyecto=" + IdProyecto;
            uriMetodo += "&GuidInstancia=" + IdInstancia;
            uriMetodo += "&usuarioDNP=" + usuarioDnp;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ErroresProyectoDto>>(result);
        }

        public async Task<ResponseDto<bool>> ConsultarRadicado(string radicado, string usuarioDnp)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uriMetodo = ConfigurationManager.AppSettings["UriConsultarRadicado"];
            uriMetodo += "?radicadoSalida=" + radicado.ToString();
            return JsonConvert.DeserializeObject<ResponseDto<bool>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, null, usuarioDnp));

        }

        public async Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uriMetodo = ConfigurationManager.AppSettings["UriCerrarRadicado"];
            return JsonConvert.DeserializeObject<ResponseDto<bool>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));

        }

        public async Task<int> TramiteEnPasoUno(Guid InstanciaId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["UriTramiteEnPasoUno"];
            uriMetodo += "?InstanciaId=" + InstanciaId;
            return JsonConvert.DeserializeObject<int>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<ResponseDto<List<TramiteConpesDetailDto>>> ObtenerConpesTramite(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerConpesTramite"];
            uriMetodo = string.Format("{0}/{1}", uriMetodo, tramiteId);
            return JsonConvert.DeserializeObject<ResponseDto<List<TramiteConpesDetailDto>>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<string> ObtenerFocalizacionPoliticasTransversalesFuentes(string bpin, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarPoliticasTranversalesFuentes"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?Bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDetalleAjustesFuenteFinanciacion"];
            uriMetodo += "?Bpin=" + bpin;
            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, null, usuarioDNP));
        }

        public async Task<ResponseDto<bool>> AsociarConpesTramite(AsociarConpesTramiteRequestDto model, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["UriAsociarConpesTramite"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, model, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<ResponseDto<bool>>(respuesta);
        }

        public async Task<ResponseDto<bool>> RemoverAsociacionConpesTramite(RemoverAsociacionConpesTramiteDto model, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["UriRemoverAsociacionConpesTramite"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, model, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<ResponseDto<bool>>(respuesta);
        }

        public async Task<ResponseDto<PeriodoPresidencialDto>> ObtenerPeriodoPresidencial(string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerPeriodoPresidencial"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);

            return JsonConvert.DeserializeObject<ResponseDto<PeriodoPresidencialDto>>(respuesta);
        }

        public async Task<List<ErroresTramiteDto>> ObtenerErroresTramite(string guiMacroproceso, string IdInstancia, string accionid, string usuarioDnp, bool tieneCDP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerErroresTramite"];
            uriMetodo += "?GuidMacroproceso=" + guiMacroproceso;
            uriMetodo += "&GuidInstancia=" + IdInstancia;
            uriMetodo += "&accionid=" + accionid;
            uriMetodo += "&usuarioDNP=" + usuarioDnp;
            uriMetodo += "&tieneCDP=" + tieneCDP;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ErroresTramiteDto>>(result);
        }

        public async Task<List<ErroresTramiteDto>> ObtenerErroresViabilidad(string guiMacroproceso, int IdProyecto, string IdNivel, string IdInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerErroresViabilidad"];
            uriMetodo += "?GuiMacroproceso=" + guiMacroproceso;
            uriMetodo += "&ProyectoId=" + IdProyecto;
            uriMetodo += "&NivelId=" + IdNivel;
            uriMetodo += "&InstanciaId=" + IdInstancia;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ErroresTramiteDto>>(result);
        }

        public async Task<ResultadoProcedimientoDto> guardarLocalizacion(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriBackboneGuardarDefLocalizaciones"];
            uriMetodo += "?usuario=" + usuarioDNP;
            var response = JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, objLocalizacion, usuarioDNP, useJWTAuth: false));
            return response;
        }

        public async Task<List<AgrupacionCodeDto>> ConsultarAgrupacionesCompleta(string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriBackboneObtenerAgrupacionCompleta"];

            var respuesta = JsonConvert.DeserializeObject<List<AgrupacionCodeDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false));
            return respuesta;
        }

        public async Task<List<DepartamentoCatalogoDto>> obtenerDepartamento(string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriBackboneObtenerDepartamentos"];

            var respuesta = JsonConvert.DeserializeObject<List<DepartamentoCatalogoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false));
            return respuesta;
        }

        public async Task<IHttpActionResult> obtenerMunicipio(ProyectoParametrosDto peticion)
        {
            var uri = ConfigurationManager.AppSettings["uriBackboneObtenerMunicipios"];

            var respuesta = JsonConvert.DeserializeObject<IHttpActionResult>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, peticion, peticion.IdUsuario.ToString(), useJWTAuth: false));
            return respuesta;
        }

        public async Task<string> EliminarAsociacionVFO(EliminacionAsociacionDto eliminacionAsociacionDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriEliminarAsociacionVFO"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, eliminacionAsociacionDto, usuarioDnp);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }
        public async Task<List<SeccionesTramiteDto>> ObtenerSeccionesTramite(string IdMacroproceso, string IdInstancia, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerSeccionesTramite"];
            uriMetodo += "?GuidMacroproceso=" + IdMacroproceso;
            uriMetodo += "&GuidInstancia=" + IdInstancia;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<SeccionesTramiteDto>>(result);
        }

        public async Task<List<SeccionesTramiteDto>> ObtenerSeccionesTramite(string IdMacroproceso, string IdInstancia, string FaseId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerSeccionesTramite"];
            var uriMetodoPorFase = ConfigurationManager.AppSettings["urlObtenerSeccionesPorFase"];
            string result = string.Empty;

            if (string.IsNullOrEmpty(FaseId))
            {
                uriMetodo += "?guidMacroproceso=" + IdMacroproceso;
                uriMetodo += "&guidInstancia=" + IdInstancia;
                result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            }
            else
            {
                uriMetodoPorFase += "?guidInstancia=" + IdInstancia;
                uriMetodoPorFase += "&guidFaseNivel=" + FaseId;
                result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodoPorFase, string.Empty, null, usuarioDnp, useJWTAuth: false);
                if (string.IsNullOrEmpty(result) || result.Equals("[]"))
                {
                    uriMetodo += "?GuidMacroproceso=" + IdMacroproceso;
                    uriMetodo += "&GuidInstancia=" + IdInstancia;
                    result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
                }
            }

            return JsonConvert.DeserializeObject<List<SeccionesTramiteDto>>(result);
        }
        public async Task<SeccionCapituloDto> ObtenerSeccionCapitulo(string faseGuid, string capitulo, string seccion, string idUsuario, string Nivelid, string FlujoId)
        {
            SeccionCapituloDto vSeccionCapituloDto = null;
            /*
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriSeccionesCapitulosByMacroproceso"];
            uriMetodo += "?guiMacroproceso=" + faseGuid;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, idUsuario);
            List<SeccionCapituloDto> LstSeccionCapitulos = JsonConvert.DeserializeObject<List<SeccionCapituloDto>>(result);
            */
            List<SeccionCapituloDto> LstSeccionCapitulos = await SeccionesCapitulosByMacroproceso(faseGuid, idUsuario, Nivelid, FlujoId);

            if (LstSeccionCapitulos != null && LstSeccionCapitulos.Count != 0)
            {
                vSeccionCapituloDto = LstSeccionCapitulos.Where(x => x.Capitulo.ToUpper() == capitulo.ToUpper() && x.Seccion.ToUpper() == seccion.ToUpper()).FirstOrDefault();
                if (vSeccionCapituloDto != null)
                {
                    return vSeccionCapituloDto;
                }
                else
                {
                    return vSeccionCapituloDto;
                }
            }
            else
            {
                return vSeccionCapituloDto;
            }
        }

        public async Task<List<TramiteProyectoVFODto>> ObtenerProyectoAsociacionVFO(string bpin, int tramite, string tipoTramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerProyectoAsociacionVFO"];
            //if (tipoTramite=="AL")
            //    uriMetodo = ConfigurationManager.AppSettings["urlObtenerProyectoAsociacionAL"];//Se deja todo en el mismo SP y se llama segun sea el tipo de tramite

            uriMetodo += "?Bpin=" + bpin;
            uriMetodo += "&TramiteId=" + tramite;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TramiteProyectoVFODto>>(result);
        }

        public async Task<string> AsociarProyectoVFO(TramiteProyectoVFODto tramiteProyectoVFODto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriAsociarProyectoVFO"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, tramiteProyectoVFODto, usuarioDnp);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }
        //aqui
        public async Task<string> ObtenerPoliticasTransversalesCrucePoliticas(string bpin, int idFuente, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarFocalizacionPoliticaCrucePolitica"];
            uriMetodo += "?Bpin=" + bpin;
            uriMetodo += "&IdFuente=" + idFuente;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<dynamic> CrearRadicadoEntradaTramite(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uri = ConfigurationManager.AppSettings["UriCrearRadicadoEntrada"];

            var model = new
            {
                ObjetoNegocioId = tramiteId
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, model, usuarioDnp);
            return JsonConvert.DeserializeObject<dynamic>(respuesta);
        }

        public async Task<dynamic> CerrarRadicadosTramite(string numeroTramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uri = ConfigurationManager.AppSettings["UriCrearRadicadoEntrada"];

            var model = new {
                ObjetoNegocioId = numeroTramite
            };

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, model, "CC353535");
            return JsonConvert.DeserializeObject<dynamic>(respuesta);
        }

        public async Task<DatosProyectoTramiteDto> ObtenerDatosProyectoTramite(int tramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosProyectoTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?tramiteId=" + tramiteId.ToString();

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new DatosProyectoTramiteDto();

            return JsonConvert.DeserializeObject<DatosProyectoTramiteDto>(response);

        }


        public async Task<RespuestaGeneralDto> ActualizarPoliticasTransversalesCrucePoliticas(PoliticasTCrucePoliticasDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocioWBS"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarFocalizacionPoliticaCrucePolitica"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }
        public Task<List<CatalogoDto>> ObtenerListaCatalogo(EntidadesPorCodigoParametrosDto peticionObtenerProyecto, CatalogoEnum catalogoEnum)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DatosProyectoTramiteDto>> ObtenerDatosProyectosPorTramite(int tramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosProyectosPorTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?tramiteId=" + tramiteId.ToString();

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<DatosProyectoTramiteDto>();

            return JsonConvert.DeserializeObject<List<DatosProyectoTramiteDto>>(response);

        }
        #region Vigencias Futuras

        public async Task<string> ObtenerDatosCronograma(Guid instanciaId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosCronograma"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?instanciaId=" + instanciaId.ToString();

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;

            return response;

        }

        public async Task<List<JustificacionPasoDto>> ObtenerPreguntasProyectoActualizacionPaso(int TramiteId, int ProyectoId, int TipoTramiteId, Guid IdNivel, int TipoRolId, string usuarioDnp)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerPreguntasProyectoActualizacionPaso"];
            uriMetodo += "?TramiteId=" + string.Join("&TramiteId=", TramiteId) + "&ProyectoId=" + string.Join("&ProyectoId=", ProyectoId) + "&TipoTramiteId=" + string.Join("&TipoTramiteId=", TipoTramiteId) + "&IdNivel=" + string.Join("&IdNivel=", IdNivel) + "&TipoRolId=" + string.Join("&TipoRolId=", TipoRolId);

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<JustificacionPasoDto>();

            return JsonConvert.DeserializeObject<List<JustificacionPasoDto>>(response);




        }

        public async Task<List<TramiteDeflactoresDto>> ObtenerDeflactores(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDeflactores"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<TramiteDeflactoresDto>();

            return JsonConvert.DeserializeObject<List<TramiteDeflactoresDto>>(response);

        }

        public async Task<List<TramiteProyectoDto>> ObtenerProyectoTramite(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ObtenerProyectoTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?proyectoId={ProyectoId}&tramiteID={TramiteId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<TramiteProyectoDto>();

            return JsonConvert.DeserializeObject<List<TramiteProyectoDto>>(response);

        }

        public async Task<string> ActualizaVigenciaFuturaProyectoTramite(TramiteProyectoDto tramiteProyectoDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ActualizaVigenciaFuturaProyectoTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, tramiteProyectoDto, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;

            return response;


        }

        public async Task<VigenciaFuturaCorrienteDto> ObtenerFuentesFinanciacionVigenciaFuturaCorriente(string bpin, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ObtenerFuentesFinanciacionVigenciaFuturaCorriente"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new VigenciaFuturaCorrienteDto();

            return JsonConvert.DeserializeObject<VigenciaFuturaCorrienteDto>(response);

        }

        public async Task<VigenciaFuturaConstanteDto> ObtenerFuentesFinanciacionVigenciaFuturaConstante(string bpin, int tramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ObtenerFuentesFinanciacionVigenciaFuturaConstante"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={bpin}&tramiteID={tramiteId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new VigenciaFuturaConstanteDto();

            return JsonConvert.DeserializeObject<VigenciaFuturaConstanteDto>(response);

        }

        public async Task<InformacionPresupuestalValoresDto> ObtenerInformacionPresupuestalValores(int tramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerInformacionPresupuestalValores"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?tramiteId=" + tramiteId.ToString();

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new InformacionPresupuestalValoresDto();

            return JsonConvert.DeserializeObject<InformacionPresupuestalValoresDto>(response);
        }

        public async Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaFuente(VigenciaFuturaCorrienteFuenteDto fuente, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ActualizarVigenciaFuturaFuente"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, fuente, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new VigenciaFuturaResponse();

            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(response);

        }

        public async Task<VigenciaFuturaResponse> ActualizarVigenciaFuturaProducto(ProductosConstantesVFDetalleProducto prod, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ActualizarVigenciaFuturaProducto"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, prod, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new VigenciaFuturaResponse();

            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(response);

        }

        public async Task<string> GuardarInformacionPresupuestalValores(InformacionPresupuestalValoresDto informacionPresupuestalValoresDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarInformacionPresupuestalValores"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, informacionPresupuestalValoresDto, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;

            return response;


        }

        #endregion Vigencias Futuras 


        public async Task<List<EnvioSubDireccionDto>> ObtenerSolicitarConceptoPorTramite(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerSolicitarConceptoPorTramite"];
            return JsonConvert.DeserializeObject<List<EnvioSubDireccionDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?tramiteId={tramiteId}", null, usuarioDnp, useJWTAuth: false));

        }

        public async Task<CrearRadicadoResponseDto> CrearRadicadoSalida(RadicadoSalidaRequestDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uriMetodo = ConfigurationManager.AppSettings["uriCrearRadicadoSalidaORFEO"];
            var respuesta = JsonConvert.DeserializeObject<CrearRadicadoResponseDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> EliminarCapitulosModificados(CapituloModificado parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarCapitulosModificados"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }
        public async Task<int> EliminarPermisosAccionesUsuarios(string usuarioDestino, int tramiteId, string aliasNivel, string usuarioDnp, Guid InstanciaId = default(Guid))
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriEliminarPermisosAccionesUsuarios"];
            uriMetodo += "?usuarioDestino=" + usuarioDestino + "&tramiteId=" + tramiteId + "&aliasNivel=" + aliasNivel + "&InstanciaId=" + InstanciaId;
            return JsonConvert.DeserializeObject<int>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, null, usuarioDnp));
        }

        public async Task<AccionDto> ObtenerAccionActualyFinal(int tramiteId, string bpin, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerAccionActualyFinal"];
            uriMetodo += "?tramiteId=" + tramiteId + "&bpin=" + bpin;
            return JsonConvert.DeserializeObject<AccionDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, null, usuarioDnp));
        }

        public async Task<string> ObtenerTiposRecursosEntidad(ProyectoParametrosDto peticion, int entityTypeCatalogId)
        {
            //ConsultaTiposRecursosEntidad(int entityTypeCatalogId)
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTiposRecursosEntidad"];
            uriMetodo += "?entityTypeCatalogId=" + entityTypeCatalogId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> EnviarConceptoDireccionTecnicaTramite(int tramiteId, string usuarioDnp, string usuarioLogueado)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriEnviarConceptoDireccionTecnicaTramite"];
            uriMetodo += "?tramiteId=" + tramiteId + "&usuario=" + usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, null, usuarioLogueado, useJWTAuth: false));

        }

        public async Task<List<TramiteModalidadContratacionDto>> ObtenerModalidadesContratacion(int mostrar, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerModalidadesContratacion"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?mostrar=" + mostrar;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<TramiteModalidadContratacionDto>();

            return JsonConvert.DeserializeObject<List<TramiteModalidadContratacionDto>>(response);

        }
        public async Task<ActividadPreContractualDto> ActualizarActividadesCronograma(ActividadPreContractualDto parametros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarActividadesCronograma"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var respuesta = JsonConvert.DeserializeObject<ActividadPreContractualDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
            return respuesta;
        }

        public async Task<ActividadPreContractualDto> ObtenerActividadesPrecontractualesProyectoTramite(int ModalidadContratacionId, int ProyectoId, int TramiteId, bool eliminarActividades, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerActividadesPrecontractualesProyectoTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?ModalidadContratacionId=" + string.Join("&ModalidadContratacionId=", ModalidadContratacionId) + "&ProyectoId=" + string.Join("&ProyectoId=", ProyectoId) + "&TramiteId=" + string.Join("&TramiteId=", TramiteId) + "&EliminarActividades=" + string.Join("&EliminarActividades=", eliminarActividades);

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new ActividadPreContractualDto();

            return JsonConvert.DeserializeObject<ActividadPreContractualDto>(response);

        }

        public async Task<ProductosConstantesVF> ObtenerProductosVigenciaFuturaConstante(string Bpin, int TramiteId, int AnioBase, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ObtenerProductosVigenciaFuturaConstante"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={Bpin}&tramiteID={TramiteId}&AnioBase={AnioBase}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new ProductosConstantesVF();

            return JsonConvert.DeserializeObject<ProductosConstantesVF>(response);

        }

        public async Task<ProductosCorrientesVF> ObtenerProductosVigenciaFuturaCorriente(string Bpin, int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ObtenerProductosVigenciaFuturaCorriente"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={Bpin}&tramiteID={TramiteId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new ProductosCorrientesVF();

            return JsonConvert.DeserializeObject<ProductosCorrientesVF>(response);

        }

        public async Task<string> ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDetalleAjustesJustificaionFacalizacionPT"];
            uriMetodo += "?Bpin=" + bpin;
            return JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, null, usuarioDNP));
        }

        public async Task<List<TipoDocumentoTramiteDto>> ObtenerTipoDocumentoTramitePorNivel(int tipoTramiteId, string nivelId, string rolId, string usuarioDnp)
        {
            Guid roltmp = string.IsNullOrEmpty(rolId) ? new Guid() : new Guid(rolId);
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTipoDocumentoTramitePorNivel"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo = uriMetodo + "?tipoTramiteId=" + tipoTramiteId + "&nivelId=" + nivelId + "&rolId=" + roltmp ;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(response)) return new List<TipoDocumentoTramiteDto>();

            return JsonConvert.DeserializeObject<List<TipoDocumentoTramiteDto>>(response);
        }

        public async Task<List<int?>> ObtenerListaVigenciasProyecto(ProyectoParametrosDto peticion)
        {
            var uri = ConfigurationManager.AppSettings["uriListaVigencias"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<int?>>(respuesta);
        }
        public async Task<List<DatosUsuarioDto>>  ObtenerDatosUsuario(string idUsuarioDnp, int idEntidad, Guid idAccion, Guid idIntancia, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerDatosUsuariosTramite"];
            uri = uri + "?idUsuarioDnp=" + idUsuarioDnp + "&idEntidad=" + idEntidad + "&idAccion=" + idAccion.ToString() +
            "&idIntancia=" + idIntancia.ToString();
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<DatosUsuarioDto>>(respuesta);
        }
        public async Task<ModificacionLeyendaDto> ObtenerModificacionLeyenda(int TramiteId, int ProyectoId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerModificacionLeyenda"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?TramiteId=" + string.Join("&TramiteId=", TramiteId) + "&ProyectoId=" + string.Join("&ProyectoId=", ProyectoId);


            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new ModificacionLeyendaDto();

            return JsonConvert.DeserializeObject<ModificacionLeyendaDto>(response);

        }
        public async Task<string> ActualizarModificacionLeyenda(ModificacionLeyendaDto parametros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarModificacionLeyenda"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var respuesta = JsonConvert.DeserializeObject<string>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
            return respuesta;
        }

        public async Task<List<EntidadCatalogoDTDto>> ObtenerListaDirecionesDNP(Guid idEntididad,  string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerListaDireccionesDNP"];
            uri = uri + "?idEntidad="  + idEntididad.ToString();
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EntidadCatalogoDTDto>>(respuesta);
        }

        public async Task<List<EntidadCatalogoDTDto>> ObtenerListaSubdirecionesPorParentId(int IdEntityType, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerListaSubdirecciones"];
            uri = uri + "?idEntidadType=" + IdEntityType.ToString();
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<EntidadCatalogoDTDto>>(respuesta);
        }

        public async Task<RespuestaGeneralDto> BorrarFirma(string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriBorrarFirma"];
            FileToUploadDto parametros = new FileToUploadDto();
            parametros.UsuarioId = usuarioDnp;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, parametros, usuarioDnp));
        }

        public async Task<ProyectosCartaDto> ObtenerProyectosCartaTramite(int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectosCartaTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?tramiteId=" + string.Join("&tramiteId=", TramiteId);


            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new ProyectosCartaDto();

            return JsonConvert.DeserializeObject<ProyectosCartaDto>(response);

        }

        public async Task<DetalleCartaConceptoALDto> ObtenerDetalleCartaAL(int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDetalleCartaAL"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?tramiteId=" + string.Join("&tramiteId=", TramiteId);


            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new DetalleCartaConceptoALDto();

            return JsonConvert.DeserializeObject<DetalleCartaConceptoALDto>(response);

        }

        public async Task<int> ObtenerAmpliarDevolucionTramite(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["ObtenerAmpliarDevolucionTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?ProyectoId=" + string.Join("&ProyectoId=", ProyectoId);
            uriMetodo += "&TramiteId=" + string.Join("&TramiteId=", TramiteId);

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return 0;

            return Convert.ToInt32(response);

        }

        public async Task<DatosProyectoTramiteDto> ObtenerDatosProyectoConceptoPorInstancia(Guid instanciaId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDatosProyectoConceptoPorInstancia"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?instanciaId=" + instanciaId.ToString();

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new DatosProyectoTramiteDto();

            return JsonConvert.DeserializeObject<DatosProyectoTramiteDto>(response);

        }

        public async Task<RespuestaGeneralDto> FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriFocalizacionActualizaPoliticasModificadas"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<TramiteLiberacionVfDto>> ObtenerLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerLiberacionVigenciasFuturas"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?ProyectoId=" + string.Join("&ProyectoId=", ProyectoId);
            uriMetodo += "&TramiteId=" + string.Join("&TramiteId=", TramiteId);

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<TramiteLiberacionVfDto>();

            return JsonConvert.DeserializeObject<List<TramiteLiberacionVfDto>>(response);
        }

        public async Task<VigenciaFuturaResponse> InsertaAutorizacionVigenciasFuturas(TramiteALiberarVfDto autorizacion, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriInsertaAutorizacionVigenciasFuturas"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, autorizacion, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(respuesta);

        }

        public async Task<VigenciaFuturaResponse> InsertaValoresUtilizadosLiberacionVF(TramiteALiberarVfDto autorizacion, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriInsertaValoresUtilizadosLiberacionVF"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, autorizacion, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(respuesta);
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerListaProyectosFuentes"];
            uri += "?tramiteId=" + tramiteId.ToString();
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoTramiteFuenteDto>>(respuesta);

        }


        public async Task<List<EntidadesAsociarComunDto>> obtenerEntidadAsociarProyecto(Guid InstanciaId, string AccionTramiteProyecto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerEntidadAsociarProyecto"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?InstanciaId=" + string.Join("&InstanciaId=", InstanciaId);
            uriMetodo += "&AccionTramiteProyecto=" + string.Join("&AccionTramiteProyecto=", AccionTramiteProyecto);

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<EntidadesAsociarComunDto>();

            return JsonConvert.DeserializeObject<List<EntidadesAsociarComunDto>>(response);
        }
        public async Task<CartaConcepto> ConsultarCartaConcepto(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarCartaConcepto"];
            uriMetodo += "?tramiteId=" + tramiteId;
            return JsonConvert.DeserializeObject<CartaConcepto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<int> ValidacionPeriodoPresidencial(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriValidacionPeriodoPresidencial"];
            uriMetodo += "?tramiteId=" + tramiteId;
            return JsonConvert.DeserializeObject<int>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<string> GuardarMontosTramite(List<ProyectosEnTramiteDto> proyectosEnTramiteDto, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarMontosTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, proyectosEnTramiteDto, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;

            return response;
        }

        public async Task<List<ResumenLiberacionVfDto>> ObtenerResumenLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerResumenLiberacionVigenciasFuturas"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?ProyectoId=" + string.Join("&ProyectoId=", ProyectoId);
            uriMetodo += "&TramiteId=" + string.Join("&TramiteId=", TramiteId);

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<ResumenLiberacionVfDto>();

            return JsonConvert.DeserializeObject<List<ResumenLiberacionVfDto>>(response);
        }

        public async Task<List<ProyectoJustificacioneDto>> ObtenerPreguntasJustificacionPorProyectos(int TramiteId, int TipoTramiteId, int TipoRolId, Guid IdNivel, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerPreguntasJustificacionPorProyectos"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo = uriMetodo + "?TramiteId=" + TramiteId + "&TipoTramiteId=" + TipoTramiteId + "&TipoRolId=" + TipoRolId + "&IdNivel=" + IdNivel.ToString();
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoJustificacioneDto>>(response);

        }

        public async Task<List<tramiteVFAsociarproyecto>> ObtenerTramitesVFparaLiberar(string numTramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerTramitesVFparaLiberar"];
            
            uriMetodo += "?proyectoId=" + numTramite;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<tramiteVFAsociarproyecto>>(result);
        }

        public async Task<string> GuardarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto liberacionVigenciasFuturasDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriGuardarLiberacionVigenciaFutura"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, liberacionVigenciasFuturasDto, usuarioDnp);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }

        public async Task<ValoresUtilizadosLiberacionVfDto> ObtenerValUtilizadosLiberacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuariodnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerValUtilizadosLiberacionVigenciasFuturas"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?ProyectoId=" + string.Join("&ProyectoId=", ProyectoId);
            uriMetodo += "&TramiteId=" + string.Join("&TramiteId=", TramiteId);

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuariodnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new ValoresUtilizadosLiberacionVfDto();

            return JsonConvert.DeserializeObject<ValoresUtilizadosLiberacionVfDto>(response);
        }

        public async Task<int> TramiteAjusteEnPasoUno(int tramiteId, int proyectoId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["UriTramiteAjusteEnPasoUno"];
            uriMetodo += "?tramiteId=" + tramiteId + "&proyectoId=" + proyectoId;
            return JsonConvert.DeserializeObject<int>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp));
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerListaProyectosFuentesAprobado"];
            uri += "?tramiteId=" + tramiteId.ToString();
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoTramiteFuenteDto>>(respuesta);

        }

        public async Task<AlcanceTramiteMGADto> CrearAlcanceTramite(AlcanceTramiteDto alcanceTramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uri = ConfigurationManager.AppSettings["UriTransversalCrearAlcanceTramite"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, alcanceTramite, usuarioDnp);
            return JsonConvert.DeserializeObject<AlcanceTramiteMGADto>(respuesta);
        }

        public async Task<List<TipoMotivoAnulacionDto>> ObtenerTiposMotivoAnulacion(string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiPiipCore"];
            var uri = ConfigurationManager.AppSettings["UriTransversalObtenerTiposMotivoAnulacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<List<TipoMotivoAnulacionDto>>(respuesta);
        }

        public async Task<RespuestaGeneralDto> ActualizarCargueMasivo(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uri = ConfigurationManager.AppSettings["UriActualizarCargueMasivo"];
       
            var respuestaSalida = new RespuestaGeneralDto();
            var response = new HttpResponseMessage();

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, contenido, usuarioDnp);

            response = JsonConvert.DeserializeObject<HttpResponseMessage>(respuesta);


            respuestaSalida.Exito = true;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                respuestaSalida.Exito = false;
                respuestaSalida.Mensaje = response.ReasonPhrase;
            }

            return respuestaSalida;
            //return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> ConsultarCargueExcel(ObjetoNegocioDto contenido, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioTransaccional"];
            var uri = ConfigurationManager.AppSettings["UriConsultarCargueExcel"];            
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, contenido, usuarioDnp);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;

        }

        public async Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFCorrientes(DetalleProductosCorrientesDto productosCorrientes, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriInsertaValoresproductosLiberacionVFCorrientes"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, productosCorrientes, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(respuesta);
        }

        public async Task<VigenciaFuturaResponse> InsertaValoresproductosLiberacionVFConstantes(DetalleProductosConstantesDto productosConstantes, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriInsertaValoresproductosLiberacionVFConstantes"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, productosConstantes, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(respuesta);
        }

        public async Task<List<EntidadesAsociarComunDto>> ObtenerEntidadTramite(string numeroTramite, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerEntidadTramite"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            uriMetodo += "?numeroTramite=" + numeroTramite;

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return new List<EntidadesAsociarComunDto>();

            return JsonConvert.DeserializeObject<List<EntidadesAsociarComunDto>>(response);
        }

        public async Task<VigenciaFuturaResponse> EliminarLiberacionVigenciaFutura(LiberacionVigenciasFuturasDto eliminarLiberacionVigenciasFuturasDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriEliminaLiberacionVF"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, eliminarLiberacionVigenciasFuturasDto, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(respuesta);
        }

        public async Task<List<CalendarioPeriodoDto>> ObtenerCalendartioPeriodo(string bpin, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerCalendartioPeriodo"];
            uri += "?bpin=" + bpin;

            
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<CalendarioPeriodoDto>>(respuesta);
        }

        public async Task<PresupuestalProyectosAsociadosDto> ObtenerPresupuestalProyectosAsociados(int TramiteId, Guid InstanciaId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerPresupuestalProyectosAsociados"];
            uri += "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId;


            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<PresupuestalProyectosAsociadosDto>(respuesta);
        }

        public async Task<string> ObtenerPresupuestalProyectosAsociados_Adicion(int TramiteId, Guid InstanciaId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerPresupuestalProyectosAsociados_Adicion"];
            uri += "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerResumenReprogramacionPorVigencia(int TramiteId, Guid InstanciaId, int ProyectoId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerResumenReprogramacionPorVigencia"];
            uri += "?TramiteId=" + TramiteId + "&InstanciaId=" + InstanciaId + "&ProyectoId=" + ProyectoId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarDatosReprogramacion(DatosReprogramacionDto Reprogramacion, string idUsuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriGuardarDatosReprogramacion"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, Reprogramacion, idUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> PermisosAccionPaso(AccionFlujoDto parametros)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriPermisosAccionPaso"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, parametros, parametros.UsuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;

        }

        public async Task<OrigenRecursosDto> GetOrigenRecursosTramite(int TramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriGetOrigenRecursosTramite"];
            uri += "?TramiteId=" + TramiteId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<OrigenRecursosDto>(respuesta);
        }

        public async Task<VigenciaFuturaResponse> SetOrigenRecursosTramite(OrigenRecursosDto origenRecurso, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriSetOrigenRecursosTramite"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, origenRecurso, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<VigenciaFuturaResponse>(respuesta);
        }

        public async Task<SystemConfigurationDto> ConsultarSystemConfiguracion(string VariableKey, string Separador, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriConsultarSystemConfiguracion"];
            uri += "?VariableKey=" + VariableKey + "&Separador=" + Separador;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<SystemConfigurationDto>(respuesta);
        }

        public async Task<string> ObtenerResumenReprogramacionPorProductoVigencia(Guid InstanciaId, int TramiteId, int? ProyectoId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerResumenReprogramacionPorProductoVigencia"];
            uri += "?instanciaId=" + InstanciaId + "&proyectoId=" + ProyectoId.ToString() + "&tramiteId=" + TramiteId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<int> ObtenerModalidadContratacionVigenciasFuturas(int ProyectoId, int TramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerModalidadContratacionVigenciasFuturas"];
            uri += "?ProyectoId=" + ProyectoId + "&TramiteId=" + TramiteId;

            int respuesta = JsonConvert.DeserializeObject<int>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp));
            return respuesta;
        }

        public async Task<List<TramiteRVFAutorizacionDto>> ObtenerAutorizacionesParaReprogramacion(string bpin, int tramite, string tipoTramite, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerAutorizacionesParaReprogramacion"];
            //if (tipoTramite=="AL")
            //    uriMetodo = ConfigurationManager.AppSettings["urlObtenerProyectoAsociacionAL"];//Se deja todo en el mismo SP y se llama segun sea el tipo de tramite

            uriMetodo += "?Bpin=" + bpin;
            uriMetodo += "&TramiteId=" + tramite;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TramiteRVFAutorizacionDto>>(result);
        }

        public async Task<string> AsociarAutorizacionRVF(tramiteRVFAsociarproyecto reprogramacionDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriAsociarAutorizacionRVF"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, reprogramacionDto, usuarioDnp);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }

        public async Task<TramiteRVFAutorizacionDto> ObtenerAutorizacionAsociada(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerAutorizacionAsociada"];
            //if (tipoTramite=="AL")
            //    uriMetodo = ConfigurationManager.AppSettings["urlObtenerProyectoAsociacionAL"];//Se deja todo en el mismo SP y se llama segun sea el tipo de tramite

            uriMetodo += "?TramiteId=" + tramiteId;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<TramiteRVFAutorizacionDto>(result);
        }

        public async Task<string> EliminaReprogramacionVF(ReprogramacionDto reprogramacionDto, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriEliminaReprogramacionVF"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, reprogramacionDto, usuarioDnp);
            return JsonConvert.DeserializeObject<string>(respuesta);
        }

        public async Task<List<ErroresTramiteDto>> ObtenerErroresProgramacion(string IdInstancia, string accionid, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["urlObtenerErroresProgramacion"];
            uriMetodo += "?guidInstancia=" + IdInstancia;
            uriMetodo += "&accionId=" + accionid;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ErroresTramiteDto>>(result);
        }

        //TramiteSGP

        public async Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyectoSGP(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarEstadoAjusteProyectoSGP"];
            uriMetodo += "?tipoDevolucion=" + tipoDevolucion + "&ObjetoNegocioId=" + objetoNegocioId + "&tramiteId=" + tramiteId + "&observacion=" + observacion;
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, null, usuarioDnp));
        }

        public async Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocioSGP(InstanciaTramiteDto instanciaTramiteDto)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriEliminarProyectosTramiteNegocioSGP"];

            var url = uri + "?TramiteId=" + string.Join("&TramiteId=", instanciaTramiteDto.TramiteFiltroDto.TramiteId) + "&ProyectoId=" + string.Join("&ProyectoId=", instanciaTramiteDto.TramiteFiltroDto.ProyectoId);

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, url, string.Empty, instanciaTramiteDto.TramiteFiltroDto, instanciaTramiteDto.TramiteFiltroDto.IdUsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        //TramiteSGP - Información Presupuestal
        public async Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestalSGP(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarTramiteInformacionPresupuestalSGP"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesSGP(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerListaProyectosFuentesSGP"];
            uri += "?tramiteId=" + tramiteId.ToString();
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoTramiteFuenteDto>>(respuesta);
        }

        public async Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacionSGP(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarFuentesTramiteProyectoAprobacionSGP"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobadoSGP(int tramiteId, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriObtenerListaProyectosFuentesAprobadoSGP"];
            uri += "?tramiteId=" + tramiteId.ToString();
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoTramiteFuenteDto>>(respuesta);
        }

        public async Task<RespuestaGeneralDto> GuardarTramiteTipoRequisitoSGP(List<TramiteRequitoDto> parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarTramiteTipoRequisitoSGP"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<List<ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramiteSGP(int pProyectoId, int? pTramiteId, string usuarioDnp, bool isCDP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectoRequisitosPorTramiteSGP"];
            uriMetodo += "?pProyectoId=" + pProyectoId.ToString() + "&pTramiteId=" + pTramiteId + "&isCDP=" + isCDP;
            return JsonConvert.DeserializeObject<List<Dominio.Dto.Tramites.Proyectos.ProyectoRequisitoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp, useJWTAuth: false));
        }

        public async Task<List<ProyectosEnTramiteDto>> ObtenerProyectosTramiteNegocioSGP(int TramiteId, string usuarioDnp, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectosTramiteNegocioSGP"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?TramiteId={TramiteId}", null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(response)) return new List<ProyectosEnTramiteDto>();

            return JsonConvert.DeserializeObject<List<ProyectosEnTramiteDto>>(response);
        }

        public async Task<RespuestaGeneralDto> GuardarProyectosTramiteNegocioSGP(DatosTramiteProyectosDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarProyectosTramiteNegocioSGP"];

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<string> ValidacionProyectosTramiteNegocio(int TramiteId, string usuarioDnp, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriValidacionProyectosTramiteNegocio"];
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?TramiteId={TramiteId}", null, usuarioDnp, useJWTAuth: false);
            return JsonConvert.DeserializeObject<string>(response);
        }
    }
}