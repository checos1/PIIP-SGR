using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesDistribucionesAnteriores
{
    [ExcludeFromCodeCoverage]
    public class TramitesDistribucionesAnterioresDto
    {
        public int ProyectoId { get; set; }
        public string BPIN { get; set; }

        public string NombreProyecto { get; set; }
        public string ContraCredito { get; set; }

        public string DistribucionesAutorizadas { get; set; }
        public string DetalleTramite { get; set; }
    }
}
