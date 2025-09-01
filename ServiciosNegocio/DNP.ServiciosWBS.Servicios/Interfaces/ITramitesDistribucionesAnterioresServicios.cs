using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesDistribucionesAnteriores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface ITramitesDistribucionesAnterioresServicios
    {
        TramitesDistribucionesAnterioresDto ObtenerTramitesDistribucionAnterior(ParametrosConsultaDto parametrosConsultaDto);

        TramitesDistribucionesAnterioresDto ObtenertramitesDistribucionAnterioresPreview();
   
    }
}
