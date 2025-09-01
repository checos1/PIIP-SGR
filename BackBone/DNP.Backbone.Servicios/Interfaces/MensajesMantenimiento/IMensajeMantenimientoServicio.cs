using DNP.Backbone.Dominio.Dto.MensajeMantenimiento;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.MensajesMantenimiento
{
    public interface IMensajeMantenimientoServicio
    {
        /// <summary>
        /// Listar las mensajes de mantenimiento por FiltroDto
        /// </summary>
        /// <param name="parametros.FiltroDto"></param>
        /// <returns></returns>
        Task<IEnumerable<MensajeMantenimientoDto>> ObtenerListaMensajes(ParametrosMensajeMantenimiento parametros);

        /// <summary>
        /// Crear nueva mensaje de mantenimiento o actualizar mensaje existente por una lista de mensajes
        /// </summary>
        /// <param name="parametros.MensajeMantenimientoDto"></param>
        /// <returns></returns>
        Task<MensajeMantenimientoDto> CrearActualizarMensaje(ParametrosMensajeMantenimiento parametros);


        /// <summary>
        /// Eliminar mensaje de mantenimiento por una lista de ids
        /// </summary>
        /// <param name="parametros.FiltroDot.Ids"></param>
        /// <returns></returns>
        Task EliminarMensaje(ParametrosMensajeMantenimiento parametros);
    }
}
