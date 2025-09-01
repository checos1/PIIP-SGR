namespace DNP.Backbone.Web.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DNP.Backbone.Dominio.Dto.UsuarioNotificacion;
    using DNP.Backbone.Servicios.Interfaces.UsuarioNotificacion;
    using Servicios.Interfaces;

    public class MensajeNotificacionController : ApiController
    {
        private readonly IMensajeNotificacionServicio _servicio; 

        public MensajeNotificacionController(IMensajeNotificacionServicio servicio)
        {
            _servicio = servicio;
        }
        
        [Route("api/mensajesnotificacion/obtener")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMensajes([FromBody] UsuarioNotificacionMensajesFiltroDto filtro)
        {
            var result = await Task.Run(() => _servicio.OtenerMensajeNotificaciones(filtro, User.Identity.Name));
            return Ok(result);
        }

        [Route("api/mensajesnotificacion/obtenerexcel")]
        [HttpPost]
        public IHttpActionResult ObtenerExcelNotificaciones(List<UsuarioNotificacionDto> datos) {

            try
            {
                var transformaLista = datos.Select(p => new { 
                        Notificacion = p.UsuarioConfigNotificacion.ContenidoNotificacion, 
                        FechaCadena  = p.UsuarioConfigNotificacion.FechaInicio.ToShortDateString(), 
                        Estado       = p.UsuarioYaLeyo  ? "Leída" : "No leída"
                }).ToList();

                // obtener el binario del archivo excel.
                var archivo       = ExcelUtilidades.ObtenerExcelNotificaciones(transformaLista);
                var nombreArchivo = $"{nombreDelArchivo("NotificacionesMensajes")}.xls";

                return Ok(new
                {
                    Datos = new
                    {
                        FileContent = archivo,
                        ContentType = System.Net.Mime.MediaTypeNames.Application.Octet,
                        FileName = nombreArchivo,
                    },
                    EsExcepcion = false
                });
            }
            catch (Exception excepcion) {
                return Ok(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = $"ConsolaProyectos.ObtenerExcelConsolaProyectos: {excepcion.Message}\\n{excepcion.InnerException?.Message ?? String.Empty}"
                });
            }
        }

        /// <summary>
        /// Método que devuelve el nombre del archivo concatenando fecha y hora
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns>String: Devuelve el nombre del archivo</returns>
        private string nombreDelArchivo(string fuente)
        {
            var data = $"{ DateTime.Now.Year }-{ DateTime.Now.Month.ToString().PadLeft(2, '0')}-{ DateTime.Now.Day.ToString().PadLeft(2, '0')}";
            var hora = $"{ DateTime.Now.TimeOfDay.Hours}h{ DateTime.Now.TimeOfDay.Minutes}m{DateTime.Now.TimeOfDay.Seconds.ToString().PadLeft(2, '0')}";
            return $"{fuente}_{data}_{hora}";
        }
    }
}