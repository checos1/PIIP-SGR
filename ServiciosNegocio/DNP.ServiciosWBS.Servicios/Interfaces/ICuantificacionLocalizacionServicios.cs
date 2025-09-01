namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface ICuantificacionLocalizacionServicios
    {
        PoblacionDto ObtenerCuantificacionLocalizacion(ParametrosConsultaDto parametrosConsulta);
        PoblacionDto ObtenerCuantificacionLocalizacionPreview();
        void Guardar(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<PoblacionDto> ConstruirParametrosGuardado(HttpRequestMessage request, PoblacionDto contenido);


    }
}
