using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Focalizacion
{
    public interface IIndicadoresPolitica
    {
        #region Obtener Indicadores Politica

        /// <summary>
        /// Obtener Indicadores Politica
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDnp"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ObtenerIndicadoresPolitica(string bpin, string usuarioDnp, string tokenAutorizacion);

        #endregion
    }
}
