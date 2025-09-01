using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;
using System.Net.Http;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesServicios
    {
        PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversales(ParametrosConsultaDto parametrosConsulta);
        PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversalesPreview();
        void Guardar(ParametrosGuardarDto<PoliticaTRelacionadasDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoliticaTRelacionadasDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoliticaTRelacionadasDto contenido);
    }
}
