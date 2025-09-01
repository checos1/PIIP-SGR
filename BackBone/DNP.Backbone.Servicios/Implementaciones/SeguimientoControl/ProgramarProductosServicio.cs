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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DNP.Backbone.Servicios.Implementaciones.SeguimientoControl
{
    public class ProgramarProductosServicio : IProgramarProductosServicio
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public ProgramarProductosServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        

        #region Get
        public async Task<string> ObtenerListadoObjProdNiveles(string bpin, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerListadoObjProdNivelesProgramarProductos"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        #endregion

        public async Task<string> GuardarProgramarProducto(ProgramarProductoDto programarProducto)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarProgramarProducto"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, programarProducto, programarProducto.usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}
