using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.Programacion;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Programacion;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Programacion
{
    public class ProgramacionServicio : IProgramacionServicio
    {
        private readonly IProgramacionPersistencia _programacionPersistencia;

        public ProgramacionServicio(IProgramacionPersistencia ProgramacionPersistencia)
        {
            _programacionPersistencia = ProgramacionPersistencia;
        }
        public TramitesResultado GuardarDatosProgramacionDistribucion(string NumeroTramite, string usuario)
        {
            return _programacionPersistencia.GuardarDatosProgramacionDistribucion(NumeroTramite, usuario);
        }

        public TramitesResultado InclusionFuentesProgramacion(string NumeroTramite, string usuario)
        {
            return _programacionPersistencia.InclusionFuentesProgramacion(NumeroTramite, usuario);
        }
    }
}
