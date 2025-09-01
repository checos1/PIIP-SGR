namespace DNP.Backbone.Servicios.Implementaciones.FuenteFinanciacion
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using Dominio.Dto.Proyecto;
    using Interfaces.Autorizacion;
    using Interfaces.ServiciosNegocio;
    using Interfaces.Tramites;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using DNP.Backbone.Comunes.Utilidades;
    using Newtonsoft.Json.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Interfaces.Auditoria;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.Focalizacion;

    public class FuenteFinanciacionServicio : IFuenteFinanciacionServicios
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;
        public FuenteFinanciacionServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// llamado al servicio para consultar fuente de financiacion
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> ObtenerFuenteFinanciacionAgregarN(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuentesFinanciacionVigencia"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }


        /// <summary>
        /// llamado al servicio para agregar fuente de financiacion
        /// </summary>
        /// <param name="proyectoFuenteFinanciacionAgregarDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> AgregarFuenteFinanciacion(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuenteFinanciacionAgregar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, proyectoFuenteFinanciacionAgregarDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarFuenteFinanciacion(string fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuenteFinanciacionEliminar"] + $"?fuenteFinanciacionId={fuenteId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerResumenCostosVsSolicitado(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriResumenCostosVsSolicitado"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// llamado al servicio para consultar el resumen de las fuentes de financiacion
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> ConsultarResumenFuentesFinanciacion(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriResumenFuenteFinanciacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// llamado al servicio para consultar el resumen de las fuentes de financiacion
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> ConsultarCostosPIIPvsFuentesPIIP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriCostosPIIPvsFuentesPIIP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerFuenteFinanciacionVigencia(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuenteFinanciacionVigencia"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ConsultarPoliticasTransversalesAjustes(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriPoliticasTransversalesAjustes"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> guardarPoliticasTransversalesAjustes(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriPoliticasTransversalesAjustesAgregar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objPoliticaTransversalDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ConsultarPoliticasTransversalesCategorias(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriPoliticasTransversalesCategoria"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarPoliticasProyecto(int proyectoId, int politicaId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarPoliticasProyecto"] + $"?proyectoId={proyectoId}&politicaId={politicaId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ConsultarPoliticasCategoriasPorPadre(int idPadre, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarPoliticasCategoriaPorPadre"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?idPadre={idPadre}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerCategoriasSubcategorias(int idPadre, int idEntidad, int esCategoria, int esGrupoEtnico, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerCategoriasSubcategorias"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?padreId={idPadre}&entidadId={idEntidad}&esCategoria={esCategoria}&esGruposEtnicos={esGrupoEtnico}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> guardarFocalizacionCategoriasPolitica(FocalizacionCategoriasAjusteDto objCategoriaPoliticaDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriPoliticasTransversalesCategoriasPoliticasAgregar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objCategoriaPoliticaDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerPoliticasTransversalesResumen(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesResumen"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ConsultarPoliticasCategoriasIndicadores(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriPoliticasConsultarPoliticasCategoriasIndicadores"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?Bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriModificarPoliticasCategoriasIndicadores"] + $"?usuario={usuario}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametrosGuardar, usuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId, int categoriaId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarCategoriaPoliticasProyecto"] + $"?proyectoId={proyectoId}&politicaId={politicaId}&categoriaId={categoriaId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerCrucePoliticasAjustes(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerCrucePoliticasAjustes"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarCrucePoliticasAjustes(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarCrucePoliticasAjustes"];
            var respuesta = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objListCruecePoliticasAjustesDto, usuarioDNP, useJWTAuth: false));
            return respuesta;
        }

        public async Task<string> ObtenerPoliticasSolicitudConcepto(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasSolicitudConcepto"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> FocalizacionSolicitarConceptoDT(List<FocalizacionSolicitarConceptoDto> objscDto)
        {
            var uri = ConfigurationManager.AppSettings["uriFocalizacionSolicitarConceptoDT"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objscDto, objscDto[0].IdUsuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerDireccionesTecnicasPoliticasFocalizacion(string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerDireccionesTecnicasPoliticasFocalizacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerResumenSolicitudConcepto(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerResumenSolicitudConcepto"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasEnvioPoliticaSubDireccionDto PreguntasEnvioPoliticaSubDireccion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPreguntasEnvioPoliticaSubDireccion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, PreguntasEnvioPoliticaSubDireccion, PreguntasEnvioPoliticaSubDireccion.IdUsuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarPreguntasEnvioPoliticaSubDireccionAjustes(PreguntasEnvioPoliticaSubDireccionAjustes objListCruecePoliticasAjustesDto)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarPreguntasEnvioPoliticaSubDireccionAjustes"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objListCruecePoliticasAjustesDto, objListCruecePoliticasAjustesDto.IdUsuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarRespuestaEnvioPoliticaSubDireccionAjustes(RespuestaEnvioPoliticaSubDireccionAjustes objListCruecePoliticasAjustesDto)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarRespuestaEnvioPoliticaSubDireccionAjustes"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objListCruecePoliticasAjustesDto, objListCruecePoliticasAjustesDto.IdUsuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

    }
}
