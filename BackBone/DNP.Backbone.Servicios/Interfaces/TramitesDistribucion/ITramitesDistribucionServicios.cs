using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.TramitesDistribucion
{
    public interface ITramitesDistribucionServicios
    {
        Task<string> ObtenerTramitesDistribucionAnteriores(Guid? instanciaId, string usuarioDNP);
    }
}
