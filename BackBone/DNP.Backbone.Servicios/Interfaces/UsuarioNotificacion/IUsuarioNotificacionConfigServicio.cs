using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.UsuarioNotificacion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.UsuarioNotificacion
{
    public interface IUsuarioNotificacionConfigServicio
    {
        /// <summary>
        /// Regresa una lista de Configuraciones de Notificaciones por filtro
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        Task<IEnumerable<UsuarioNotificacionConfigDto>> OtenerConfigNotificaciones(UsuarioNotificacionConfigFiltroDto filtro, string usuarioLogado);

        /// <summary>
        /// Crea una nueva Configuración de Notificación o actualiza una existente
        /// </summary>
        /// <param name="config"></param>
        /// <param name="usuarioLogado"></param>
        /// <returns></returns>
        Task<UsuarioNotificacionConfigDto> CrearActualizarConfigNotificacion(UsuarioNotificacionConfigDto config, string usuarioLogado);

        /// <summary>
        /// Elimina una matriz de Configuraciones de Notificaciones por una lista de Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        Task<RespuestaViewModel> EliminarConfigNotificacion(string usuarioLogado, params int[] ids);

        /// <summary>
        /// Marca una lista de notificaciones del usuários por filtro
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="usuarioLogado"></param>
        /// <returns></returns>
        Task<IEnumerable<UsuarioNotificacionDto>> MarcarNotificacionComoLeida(UsuarioNotificacionFiltroDto filtro, string usuarioLogado);

        /// <summary>
        /// Obtener todos los procedimientos de almacenados disponibles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProcedimientoAlmacenadoDto>> ObtenerProcedimentosAlmacenados(string usuarioLogado);

        /// <summary>
        /// Obtener um procedimiento de almacenado por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcedimientoAlmacenadoDto> ObtenerProcedimentoAlmacenadoPorId(string id, string usuarioLogado);

        /// <summary>
        /// Obtener una lista de Usuarios por procedimiento de almacenado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<UsuarioProcedimentoAlmacenadoDto>> ObtenerUsuariosPorProcedimentoAlmacenado(string id, string usuarioLogado);
    }
}
