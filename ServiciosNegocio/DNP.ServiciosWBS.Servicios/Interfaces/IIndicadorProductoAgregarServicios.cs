namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    public interface IIndicadorProductoAgregarServicios
    {
        IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregar(ParametrosConsultaDto parametrosConsulta);
        IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregarPreview();
        void Guardar(ParametrosGuardarDto<IndicadorProductoAgregarDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<IndicadorProductoAgregarDto> ConstruirParametrosGuardado(HttpRequestMessage request, IndicadorProductoAgregarDto contenido);

    }
}
