using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Administracion;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Administracion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DNP.Backbone.Servicios.Implementaciones.Administracion
{
    public class AdministrarDocumentoServicio : IAdministrarDocumentoServicio
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        public AdministrarDocumentoServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }
        public async Task<string> AdministrarDocumentoConsultar(string usuarioDnp, string NombreDocumento)
        {                                                     
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoConsultar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, "?NombreDocumento=" + NombreDocumento, null, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }

        public async Task<string> AdministrarDocumentoCrear(string usuarioDnp, AdministracionDocumentoDto Documento)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoCrear"];
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, Documento,  usuarioDnp, useJWTAuth: false);
            return respuesta;
        }
        public async Task<string> AdministrarDocumentoActualizar(string usuarioDnp, AdministracionDocumentoDto Documento)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoActualizar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Put, urlBase, uriMetodo, null, Documento, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }
        public async Task<string> AdministrarDocumentoEliminar(string usuarioDnp, string IdDocumento)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoEliminar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Delete, urlBase, uriMetodo, "?IdDocumento=" + IdDocumento, null, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }
        public async Task<string> AdministrarDocumentoEstado(string usuarioDnp, AdministracionDocumentoDto Documento)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoEstado"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, Documento, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }
        public async Task<string> AdministrarDocumentoReferencias(string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoReferencias"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, null, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }

        /* Uso Documentos */
        public async Task<string> AdministrarDocumentoConsultarUso(string usuarioDnp)
        {   
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoConsultarUso"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, null, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }
        public async Task<string> AdministrarCrearUsoDocumento(string usuarioDnp, AdministracionDocumentoUsoDto Documento)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoCrearUso"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, Documento, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }
        public async Task<string> AdministrarActualizarUsoDocumento(string usuarioDnp, AdministracionDocumentoUsoDto Documento)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoActualizarUso"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Put, urlBase, uriMetodo, null, Documento, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }
        public async Task<string> AdministrarEliminarUsoDocumento(string usuarioDnp, string Id)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriDocumentoUsoEliminar"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Delete, urlBase, uriMetodo, "?Id=" + Id, null, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }


    }
}
