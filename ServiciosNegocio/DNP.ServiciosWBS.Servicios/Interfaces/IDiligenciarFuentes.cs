using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;
using System.Net.Http;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IDiligenciarFuentes
    {
        DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentes(ParametrosConsultaDto parametrosConsulta);
        DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentesPreview();
        ParametrosGuardarDto<DiligenciarFuentesProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, DiligenciarFuentesProyectoDto contenido);
        void Guardar(ParametrosGuardarDto<DiligenciarFuentesProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
