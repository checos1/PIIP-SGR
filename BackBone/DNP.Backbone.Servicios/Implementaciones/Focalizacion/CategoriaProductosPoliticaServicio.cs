using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.Focalizacion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.Focalizacion
{
    public class CategoriaProductosPoliticaServicio : ICategoriaProductosPoliticaServicios
    {
        //private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public CategoriaProductosPoliticaServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        #region Obtener Categoria Productos Politica

        /// <summary>
        /// Obtener Indicadores Politica
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDnp"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> ObtenerCategoriaProductosPolitica(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriCatProductosPoliticaConsultar"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?Bpin={bpin}&fuenteId={fuenteId}&politicaId={politicaId}", null, usuarioDnp, useJWTAuth: false);

            return response;
        }

        #endregion

        #region Guardar Datos Solicitud Recursos

        /// <summary>
        /// Obtener Indicadores Politica
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDnp"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        public async Task<string> GuardarDatosSolicitudRecursos(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto, string usuarioDnp, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarDatosSolicitudRecursos"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, categoriaProductoPoliticaDto, usuarioDnp, useJWTAuth: false);

            return response;
        }

        #endregion

    }
}
