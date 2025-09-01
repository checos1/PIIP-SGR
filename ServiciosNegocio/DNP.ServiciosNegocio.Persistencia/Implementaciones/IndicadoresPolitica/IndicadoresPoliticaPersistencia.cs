using AutoMapper;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.IndicadoresPolitica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.IndicadoresPolitica
{
    public class IndicadoresPoliticaPersistencia: Persistencia, IIndicadoresPoliticaPersistencia
    {
        #region Constructor

        public IndicadoresPoliticaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }

        #endregion

        #region Obtener Datos Indicadores Politica

        /// <summary>
        /// Obtener Datos Indicadores Politica
        /// </summary>
        /// <param name="Bpin"></param>
        /// <returns></returns>
        public string ObtenerDatosIndicadoresPolitica(string Bpin)
        {
            var listadoDatos = Contexto.uspGetPoliticasTransversalesFuentesIndicadores_JSON(Bpin).FirstOrDefault();
            return listadoDatos;
        }

        #endregion
    }
}
