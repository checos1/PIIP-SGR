using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadoresAjuste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresServicios
    {
        PoliticaTIndicadoresAjusteDto ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadores(ParametrosConsultaDto parametrosConsulta);
        PoliticaTIndicadoresAjusteDto ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTIndicadoresAjusteDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTIndicadoresAjusteDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTIndicadoresAjusteDto contenido);
    }
}
