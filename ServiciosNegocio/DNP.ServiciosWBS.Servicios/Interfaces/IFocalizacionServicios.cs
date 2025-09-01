namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;

    public interface IFocalizacionServicios
    {
        FocalizacionProyectoDto ObtenerProyectoFocalizacion(ParametrosConsultaDto parametrosConsulta);
        FocalizacionProyectoDto ObtenerProyectoFocalizacionPreview();
        void Guardar(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<FocalizacionProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, FocalizacionProyectoDto contenido);
    }
}
