using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
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
    public class DesagregarEdtServicio : IDesagregarEdtServicio
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public DesagregarEdtServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        #region Get
            public async Task<DesagregarEdtNivelesDto> ObtenerListadoObjProdNiveles(ConsultaObjetivosProyecto ProyectosDto)
            {
                var uri = ConfigurationManager.AppSettings["uriObtenerListadoObjProdNiveles"];
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, ProyectosDto, ProyectosDto.UsuarioDNP, useJWTAuth: false);
                var json = JsonConvert.DeserializeObject<DesagregarEdtNivelesDto>(respuesta);
                return json;
            }
        #endregion


        #region Post
            public async Task<ReponseHttp> RegistrarNivel(string UsuarioDNP, RegistroModel NivelesNuevos)
            {
                var uri = ConfigurationManager.AppSettings["uriRegistrarNivelesNuevos"];
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, NivelesNuevos, UsuarioDNP, useJWTAuth: false);
                var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
                return json;
            }
        #endregion


        #region Delete
            public async Task<ReponseHttp> EliminarNivel(string UsuarioDNP, RegistroModel NivelesNuevos)
            {
                var uri = ConfigurationManager.AppSettings["uriEliminarNiveles"];
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, NivelesNuevos, UsuarioDNP, useJWTAuth: false);
                var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
                return json;
            }
        #endregion

        public async Task<string> ObtenerPreguntasAvanceFinanciero(Guid instancia, int proyectoid, string bpin, Guid nivelid, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPreguntasAvanceFinanciero"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instancia={instancia}&proyectoid={proyectoid}&bpin={bpin}&nivelid={nivelid}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarPreguntasAvanceFinanciero(List<PreguntasReporteAvanceFinancieroDto> PreguntasReporteAvanceFinanciero)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarPreguntasAvanceFinanciero"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, PreguntasReporteAvanceFinanciero, PreguntasReporteAvanceFinanciero[0].IdUsuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerAvanceFinanciero(Guid instancia, int proyectoid, string bpin, int vigenciaId, int periodoPeriodicidadId, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerAvanceFinanciero"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instancia={instancia}&proyectoid={proyectoid}&bpin={bpin}&vigenciaId={vigenciaId}&periodoPeriodicidadId={periodoPeriodicidadId}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarAvanceFinanciero(AvanceFinancieroDto reporteAvanceFinanciero)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarAvanceFinanciero"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, reporteAvanceFinanciero, reporteAvanceFinanciero.Usuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}
