using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Configuration;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Implementaciones.FuenteFinanciacion
{
    public class FuentesAprobacionServicio : IFuentesAprobacionServicio
    {

        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly IClienteHttpServicios _clienteHttpServicios;

        public FuentesAprobacionServicio(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<string> ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuentesObtenerPreguntasAprobacionRol"]; //+ $"?PreguntasSeguimientoProyectoDto={objPreguntasSeguimientoProyectoDto}"
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objPreguntasSeguimientoProyectoDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuentesGuardarPreguntasAprobacionRol"] + "?usuario=" + usuarioDNP + "&tokenAutorizacion=" + tokenAutorizacion;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objPreguntasSeguimientoProyectoDto, usuarioDNP, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuentesObtenerPreguntasAprobacionJefe"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objPreguntasSeguimientoProyectoDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion)
        {
            //var uri = ConfigurationManager.AppSettings["uriFuentesGuardarPreguntasAprobacionJefe"] + "?usuario=" + usuarioDNP + "&tokenAutorizacion=" + tokenAutorizacion;
            var uri = ConfigurationManager.AppSettings["uriFuentesGuardarPreguntasAprobacionJefe"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objPreguntasSeguimientoProyectoDto, usuarioDNP, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

    }
}
