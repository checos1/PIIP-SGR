using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;
using System.Net.Http;

namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesFuentesServicios
    {
        FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentes(ParametrosConsultaDto parametrosConsultaDto);
        FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentesPreview();

        void Guardar(ParametrosGuardarDto<FocalizacionPoliticaTFuentesDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<FocalizacionPoliticaTFuentesDto> ConstruirParametrosGuardado(HttpRequestMessage request, FocalizacionPoliticaTFuentesDto contenido);

    }
}
