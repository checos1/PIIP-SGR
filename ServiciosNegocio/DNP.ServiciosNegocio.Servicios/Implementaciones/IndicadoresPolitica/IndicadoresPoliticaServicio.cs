using DNP.ServiciosNegocio.Persistencia.Interfaces.IndicadoresPolitica;
using DNP.ServiciosNegocio.Servicios.Interfaces.IndicadoresPolitica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.IndicadoresPolitica
{
    public class IndicadoresPoliticaServicio : IIndicadoresPoliticaServicio
    {
        private readonly IIndicadoresPoliticaPersistencia _IndicadoresPoliticaPersistencia;

        public IndicadoresPoliticaServicio(IIndicadoresPoliticaPersistencia indicadoresPoliticaPersistencia) 
        {
            _IndicadoresPoliticaPersistencia = indicadoresPoliticaPersistencia;
        }

        public string ObtenerDatosIndicadoresPolitica(string Bpin)
        {
            return _IndicadoresPoliticaPersistencia.ObtenerDatosIndicadoresPolitica(Bpin);
        }
    }
}
