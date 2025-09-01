using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.SeguimientoControl
{
    public class GestionSeguimientoServicio : IGestionSeguimientoServicio
    {
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public GestionSeguimientoServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        #region Get

        public async Task<List<TransversalSeguimientoDto>> UnidadesMedida(string usuario)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerUnidadesMedida"];
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, null, null, usuario, useJWTAuth: false);
            return JsonConvert.DeserializeObject<List<TransversalSeguimientoDto>>(result);
        }

        #endregion


        #region Post
        public async Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(GestionProyectoDto proyecto)
            {
                var uri = ConfigurationManager.AppSettings["uriObtenerErroresSeguimiento"];
                var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, proyecto, proyecto.IdUsuario, useJWTAuth: false);
                return JsonConvert.DeserializeObject<List<ErroresProyectoDto>>(result);
            }

       
        #endregion


        #region Delete

        #endregion

    }
}
