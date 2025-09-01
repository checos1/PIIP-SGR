namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IFocalizacionCuantificacionLocalizacionServicios
    {
        FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacion(ParametrosConsultaDto parametrosConsulta);
        FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacionPreview();
        void Guardar(ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, FocalizacionCuantificacionLocalizacionDto contenido);

    }
}
