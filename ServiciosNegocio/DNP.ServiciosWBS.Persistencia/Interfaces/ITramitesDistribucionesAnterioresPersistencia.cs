using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesDistribucionesAnteriores;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface  ITramitesDistribucionesAnterioresPersistencia
    {
        TramitesDistribucionesAnterioresDto ObtenerTramitesDistribucionAnterior(Guid Instancia);

        TramitesDistribucionesAnterioresDto ObtenertramitesDistribucionAnterioresPreview();

        void GuardarDefinitivamente(ParametrosGuardarDto<TramitesDistribucionesAnterioresDto> parametrosGuardar, string usuario);
    }
}
