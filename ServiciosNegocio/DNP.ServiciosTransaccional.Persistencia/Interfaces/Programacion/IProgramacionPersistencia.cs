using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Programacion
{
    public interface IProgramacionPersistencia
    {
        TramitesResultado GuardarDatosProgramacionDistribucion(string NumeroTramite, string usuario);
        TramitesResultado InclusionFuentesProgramacion(string NumeroTramite, string usuario);
    }
}



