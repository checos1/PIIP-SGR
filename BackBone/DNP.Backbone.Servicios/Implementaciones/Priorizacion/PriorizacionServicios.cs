using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
using DNP.Backbone.Dominio.Dto.Priorizacion;
using DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Dominio.Dto.SGR.GestionRecursos;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Priorizacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DNP.Backbone.Servicios.Implementaciones.Priorizacion
{
    public class PriorizacionServicios : IPriorizacionServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        public PriorizacionServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// Lista para obter catalogo
        /// </summary>
        /// <param name="bpins">número bpin</param>
        /// <param name="usuarioDNP">Usuario DNP</param>
        /// <returns>Task<List<CatalogoDto>></returns>
        public async Task<List<PriorizacionDatosBasicosDto>> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarPriorizacionPorBPINs"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, string.Empty, bpins, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<PriorizacionDatosBasicosDto>>(respuesta);
        }

        public async Task<string> ObtenerFuentesSGR(string bpin, Guid? instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarFuentesSGR"];
            if (instanciaId == null)
            {
                instanciaId = Guid.Empty;
            }
            uri += "?bpin=" + bpin + "&instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<ReponseHttp> RegistrarFuentesSGR(List<EtapaSGRDto> jsonEtapa, string UsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriRegistrarFuentesSGR"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, jsonEtapa, UsuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<string> ObtenerFuentesNoSGR(string bpin, Guid? instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarFuentesNoSGR"];
            if (instanciaId == null)
            {
                instanciaId = Guid.Empty;
            }
            uri += "?bpin=" + bpin + "&instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<ReponseHttp> RegistrarFuentesNoSGR(List<EtapaNoSGRDto> jsonEtapa, string UsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriRegistrarFuentesNoSGR"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, jsonEtapa, UsuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<string> ObtenerResumenFuentesCostos(string bpin, Guid? instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarResumenFuentesCostos"];
            if (instanciaId == null)
            {
                instanciaId = Guid.Empty;
            }
            uri += "?bpin=" + bpin + "&instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerTiposCofinanciaciones(string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriListaCatalogo"];

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, "TiposCofinanciaciones", null, usuarioDNP, useJWTAuth: false);
            return respuesta;
        }

        public async Task<ReponseHttp> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto jsonVigencias, string UsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriRegistrarDatosAdicionalesCofinanciadorFuentesNoSGR"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, jsonVigencias, UsuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<string> ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, int? vigencia, int? vigenciaFuente, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarDatosAdicionalesCofinanciadorFuentesNoSGR"];
            uri += "?bpin=" + bpin + "&vigencia=" + vigencia + "&vigenciaFuente=" + vigenciaFuente;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerPriorizacionProyecto(Guid? instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPriorizacionProyecto"];
            uri += "?instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerAprobacionProyecto(Guid? instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerAprobacionProyecto"];
            uri += "?instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<IEnumerable<ProyectoPriorizacionDetalleDto>> ObtenerPriorizionProyectoDetalleSGR(Nullable<Guid> instanciaId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPriorizionProyectoDetalleSGR"];
            uri += "?instanciaId=" + instanciaId;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<ProyectoPriorizacionDetalleDto>>(respuesta);
        }

        public async Task<ProyectoPriorizacionDetalleResultado> GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string UsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarPriorizionProyectoDetalleSGR"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, proyectoPriorizacionDetalleDto, UsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ProyectoPriorizacionDetalleResultado>(respuesta);
        }

        public async Task<ProyectoPriorizacionDetalleResultado> GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string UsuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarPermisosPriorizionProyectoDetalleSGR"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, proyectoPriorizacionDetalleDto, UsuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ProyectoPriorizacionDetalleResultado>(respuesta);
        }

        public async Task<AprobacionProyectoCreditoDto> ObtenerAprobacionProyectoCredito(Guid instancia, int entidad, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerAprobacionProyectoCredito"];
            uri += "?instancia=" + instancia + "&entidad=" + entidad;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<AprobacionProyectoCreditoDto>(respuesta);
        }

        public async Task<ReponseHttp> GuardarAprobacionProyectoCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarAprobacionProyectoCredito"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, aprobacionProyectoCreditoDto, usuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }
    }
}