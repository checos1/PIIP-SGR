using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IAjusteRegionalizacionMetasyRecursosServicios
    {
        AjusteRegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosAjusteServicios(ParametrosConsultaDto parametrosConsulta);
        AjusteRegMetasRecursosDto ObtenerRegionalizacionMetasyRecursosServiciosAjustePreview();
        ParametrosGuardarDto<AjusteRegMetasRecursosDto> ConstruirParametrosGuardado(HttpRequestMessage request, AjusteRegMetasRecursosDto contenido);
        void Guardar(ParametrosGuardarDto<AjusteRegMetasRecursosDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
