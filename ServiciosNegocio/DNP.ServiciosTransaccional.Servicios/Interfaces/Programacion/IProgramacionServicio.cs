using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Programacion
{
    public interface IProgramacionServicio
    {
        TramitesResultado GuardarDatosProgramacionDistribucion(string NumeroTramite, string usuario);
        TramitesResultado InclusionFuentesProgramacion(string NumeroTramite, string usuario);
    }
}
