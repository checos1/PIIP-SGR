using DNP.Backbone.Dominio.Dto.Focalizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Focalizacion
{
    public interface ICategoriaProductosPoliticaServicios
    {
        #region Obtener Categoria Productos Politica

        /// <summary>
        /// Obtener Indicadores Politica
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDnp"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ObtenerCategoriaProductosPolitica(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion);

        /// <summary>
        /// Guardar Datos SolicitudRecursos
        /// </summary>
        /// <param name="categoriaProductoPoliticaDto"></param>
        /// <param name="usuarioDnp"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> GuardarDatosSolicitudRecursos(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto, string usuarioDnp, string tokenAutorizacion);

        #endregion
    }
}
