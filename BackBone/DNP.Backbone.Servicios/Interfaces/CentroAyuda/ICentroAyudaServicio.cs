using DNP.Backbone.Dominio.Dto.CentroAyuda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.CentroAyuda
{
    public interface ICentroAyudaServicio
    {
        /// <summary>
        /// Listar los temas de ayudas por FiltroDto
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        Task<IEnumerable<AyudaTemaListaItemDto>> ObtenerListaTemas(AyudaTemaFiltroDto dto, string usuarioDnp);

        /// <summary>
        /// Crear o Actualizar un tema de ayuda
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        Task<AyudaTemaListaItemDto> CrearActualizarTema(AyudaTemaListaItemDto dto, string usuarioDnp);

        /// <summary>
        /// Elimina un tema por id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuarioDnp"></param>
        /// <returns></returns>
        Task<bool> EliminarTema(int id, string usuarioDnp);

    }
}
