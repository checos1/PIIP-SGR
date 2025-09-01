namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public interface IRegionalizacionIndicadoresServicios
    {
        RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadores(ParametrosConsultaDto parametrosConsulta);
        RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadoresPreview();
        void Guardar(ParametrosGuardarDto<RegionalizacionIndicadorDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        ParametrosGuardarDto<RegionalizacionIndicadorDto> ConstruirParametrosGuardado(HttpRequestMessage request, RegionalizacionIndicadorDto contenido);


    }
}
