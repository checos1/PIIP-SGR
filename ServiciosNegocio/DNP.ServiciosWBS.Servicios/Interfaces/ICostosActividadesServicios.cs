namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using DNP.ServiciosNegocio.Dominio.Dto.CostosActividades;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
  
    public interface ICostosActividadesServicios

    {
        CostosActividadesDto ObtenerCostosActividades(ParametrosConsultaDto parametrosConsulta);
        CostosActividadesDto ObtenerCostosActividadesPreview();
        ParametrosGuardarDto<CostosActividadesDto> ConstruirParametrosGuardado(HttpRequestMessage request, CostosActividadesDto contenido);
        void Guardar(ParametrosGuardarDto<CostosActividadesDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
