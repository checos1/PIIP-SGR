namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public interface IRegionalizacionProyectoServicios
    {
        RegionalizacionProyectoDto ObtenerRegionalizacion(ParametrosConsultaDto parametrosConsulta);
        RegionalizacionProyectoDto ObtenerRegionalizacionPreview();
        void Guardar(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<RegionalizacionProyectoDto> ConstruirParametrosGuardado(HttpRequestMessage request, RegionalizacionProyectoDto contenido);
    }
}
