namespace DNP.Backbone.Servicios.Implementaciones.DatosAdicionales
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

    public class DatosAdicionalesServicio : IDatosAdicionalesServicios
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public DatosAdicionalesServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// llamado al servicio para consultar los datos adicionales de una fuente de financiacion
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> ObtenerDatosAdicionales(int fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriDatosAdicionalesConsultar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?fuenteId={fuenteId}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        /// <summary>
        /// llamado al servicio para agregar datos adicionales de una fuente de financiacion
        /// </summary>
        /// <param name="proyectoDatosAdicionalesAgregarDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<RespuestaGeneralDto> AgregarDatosAdicionales(DatosAdicionalesDto objDatosAdicionalesDto, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriDatosAdicionalesAgregar"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objDatosAdicionalesDto, usuarioDNP, useJWTAuth: false));
            //if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            //return respuesta;
        }

        /// <summary>
        /// llamado al servicio para eliminar datos adicionales de una fuente de financiacion
        /// </summary>
        /// <param name="cofinanciadorId"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> EliminarDatosAdicionales(int cofinanciadorId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriDatosAdicionalesEliminar"]+ $"?cofinanciadorId={cofinanciadorId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}
