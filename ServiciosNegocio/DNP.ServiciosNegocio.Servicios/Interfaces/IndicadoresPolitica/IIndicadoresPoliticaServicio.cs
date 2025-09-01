using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica
{
    public interface IIndicadoresPoliticaServicio
    {
        #region Obtener Datos IndicadoresPolitica

        /// <summary>
        /// Obtener Datos IndicadoresPolitica
        /// </summary>
        /// <param name="Bpin"></param>
        /// <returns></returns>
        string ObtenerDatosIndicadoresPolitica(string Bpin);

        #endregion
    }
}
