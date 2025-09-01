using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion
{
    public interface IFuentesAprobacionServicio
    {
        
        /// <summary>
        /// llamado al servicio para consultar las preguntas del cuestionario para aprobacionde rol presupesto en GR
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion);
        
        /// <summary>
        /// llamado al servicio para GUARDAR las preguntas del cuestionario para aprobacionde rol presupesto en GR
        /// </summary>
        /// <param name="objPreguntasSeguimientoProyectoDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> GuardarPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion);
        
        /// <summary>
        /// llamado al servicio para OBTENER las preguntas del cuestionario para aprobacionde rol jefe de planeacion en GR
        /// </summary>
        /// <param name="objPreguntasSeguimientoProyectoDto"></param>
        /// <returns></returns>
        Task<string> ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuarioDNP, string tokenAutorizacion);
        
        /// <summary>
        /// llamado al servicio para GUARDAR las preguntas del cuestionario para aprobacionde rol jefe de planeacion en GR
        /// </summary>
        /// <param name="objPreguntasSeguimientoProyectoDto"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        Task<string> GuardarPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto, string usuario, string tokenAutorizacion);

    }
}
