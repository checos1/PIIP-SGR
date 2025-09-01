using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IAjusteRegionalizacionMetasyRecursosPersistencia
    {
        AjusteRegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosAjuste(string bpin);
        AjusteRegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosAjustePreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<AjusteRegMetasRecursosDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
