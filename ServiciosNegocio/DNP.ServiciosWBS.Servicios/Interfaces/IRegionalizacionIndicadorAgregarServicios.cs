namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public interface IRegionalizacionIndicadorAgregarServicios
    {
        RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregar(ParametrosConsultaDto parametrosConsulta);
        RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregarPreview();
        void Guardar(ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> ConstruirParametrosGuardado(HttpRequestMessage request, RegionalizacionIndicadorAgregarDto contenido);
    }
}
