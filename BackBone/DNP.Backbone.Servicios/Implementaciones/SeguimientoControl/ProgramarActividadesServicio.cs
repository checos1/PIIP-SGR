using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Utilidades;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.SeguimientoControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.SeguimientoControl
{
    public class ProgramarActividadesServicio : IProgramarActividadesServicio
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public ProgramarActividadesServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        

        #region Get
        public async Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerListadoObjProdNivelesProgramar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, ProyectosDto, ProyectosDto.UsuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<DesagregarEdtNivelesDto>(respuesta);
            return json;
        }

        public async Task<List<CalendarioPeriodoDto>> ObtenerCalendarioPeriodo(ConsultaObjetivosProyecto ProyectosDto)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerCalendarioPeriodo"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, ProyectosDto, ProyectosDto.UsuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<List<CalendarioPeriodoDto>>(respuesta);
            return json;
        }
        #endregion


        #region Post
        public async Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNivelesXReporte(ConsultaObjetivosProyecto ProyectosDto)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerListadoObjProdNivelesReporte"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, ProyectosDto, ProyectosDto.UsuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<DesagregarEdtNivelesDto>(respuesta);
            return json;
        }
        public async Task<DesagregarIndicadoresPoliticasDto> ObtenerIndicadoresPoliticas(ConsultaObjetivosProyecto ProyectosDto)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerIndicadoresPoliticas"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post,  urlBase, uri, null, ProyectosDto, ProyectosDto.UsuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<DesagregarIndicadoresPoliticasDto>(respuesta);
            return json;
        }
        public async Task<ReponseHttp> EditarProgramarActividad(string Usuario, ProgramarActividadesDto ActividadDto)
        {
            var uri = ConfigurationManager.AppSettings["uriEditarProgramarActividad"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, ActividadDto, Usuario, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<ReponseHttp> ActividadProgramacionSeguimientoPeriodosValores(string Usuario, List<VigenciaEntregable> parametros)
        {
            var uri = ConfigurationManager.AppSettings["uriActividadProgramacionSeguimientoPeriodosValores"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametros, Usuario, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<ReponseHttp> ActividadReporteSeguimientoPeriodosValores(string Usuario, ReporteSeguimiento parametros)
        {
            var uri = ConfigurationManager.AppSettings["uriActividadReporteSeguimientoPeriodosValores"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametros, Usuario, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<ReponseHttp> IndicadorPoliticaSeguimientoPeriodosValores(string Usuario, ReporteIndicadorPoliticas parametros)
        {
            var uri = ConfigurationManager.AppSettings["uriIndicadorPoliticaSeguimientoPeriodosValores"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametros, Usuario, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<string> ObtenerFocalizacionProgramacionSeguimiento(string parametroConsulta, string usuarioDnp)
        {          
            var uri = ConfigurationManager.AppSettings["UriObtenerFocalizacionProgramacionSeguimiento"];
            uri += "?parametroConsulta=" + parametroConsulta;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;

        }

        public async Task<string> GuardarFocalizacionProgramacionSeguimiento(FocalizacionProgramacionSeguimientoDto objFocalizacionProgramacionSeguimientoDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["UriGuardarFocalizacionProgramacionSeguimiento"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objFocalizacionProgramacionSeguimientoDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerCruceProgramacionSeguimiento(Guid instanciaid, int proyectoid, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["UriObtenerCruceProgramacionSeguimiento"];
            uri += "?instanciaid=" + instanciaid + "&proyectoid=" + proyectoid;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDnp, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;

        }

        public async Task<string> GuardarCrucePoliticasSeguimiento(FocalizacionCrucePoliticaSeguimientoDto objFocalizacionCrucePoliticaSeguimientoDto, string usuarioDNP)
        {
            var js = JsonUtilidades.ACadenaJson(objFocalizacionCrucePoliticaSeguimientoDto);
            var uri = ConfigurationManager.AppSettings["UriGuardarCruceProgramacionSeguimiento"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objFocalizacionCrucePoliticaSeguimientoDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerFocalizacionProgramacionSeguimientoDetalle(string parametros, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["UriObtenerFocalizacionProgramacionSeguimientoDetalle"];
            uri += "?parametros=" + parametros;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;


        }

        #endregion


        #region Delete

        #endregion

    }
}
