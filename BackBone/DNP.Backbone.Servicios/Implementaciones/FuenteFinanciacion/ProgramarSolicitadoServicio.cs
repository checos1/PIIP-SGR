
namespace DNP.Backbone.Servicios.Implementaciones.FuenteFinanciacion
{

    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.DatosAdicionales;
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
    using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;

    public class ProgramarSolicitadoServicio : IProgramarSolicitadoServicio
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteHttpServicios"></param>
        public ProgramarSolicitadoServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> ConsultarFuentesProgramarSolicitado(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarProgramaSolicitado"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objProgramacionValorFuenteDto"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<string> GuardarFuentesProgramarSolicitado(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuarioDNP)
        {
            //var uri = ConfigurationManager.AppSettings["uriGuardarProgramaSolicitado"] + $"?usuario={usuarioDNP}"; ;
            var uri = ConfigurationManager.AppSettings["uriGuardarProgramaSolicitado"] + $"?usuario={usuarioDNP}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objProgramacionValorFuenteDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> guardarFuentesFinanciacionRecursosAjustes(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriFuentesFinanciacionRecursosAjustes"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objFuenteFinanciacionAgregarAjusteDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }

}
