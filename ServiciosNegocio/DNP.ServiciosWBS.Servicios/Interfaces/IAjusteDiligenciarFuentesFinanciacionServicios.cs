using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IAjusteDiligenciarFuentesFinanciacionServicios
    {
        FuentesFinanciacionAjusteDto ObtenerFuenteFinanciacionAjusteServicios(ParametrosConsultaDto parametrosConsulta);
        FuentesFinanciacionAjusteDto ObtenerFuenteFinanciacionAjusteServiciosPreview();
        ParametrosGuardarDto<FuentesFinanciacionAjusteDto> ConstruirParametrosGuardado(HttpRequestMessage request, FuentesFinanciacionAjusteDto contenido);
        void Guardar(ParametrosGuardarDto<FuentesFinanciacionAjusteDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);

    }
}
