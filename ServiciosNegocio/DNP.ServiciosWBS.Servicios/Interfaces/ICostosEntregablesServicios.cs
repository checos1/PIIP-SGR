namespace DNP.ServiciosWBS.Servicios.Interfaces
{
    using System.Net.Http;
    using DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
  
    public interface ICostosEntregablesServicios

    {
        CostosEntregablesDto ObtenerCostosEntregables(ParametrosConsultaDto parametrosConsulta);
        CostosEntregablesDto ObtenerCostosEntregablesPreview();
        ParametrosGuardarDto<CostosEntregablesDto> ConstruirParametrosGuardado(HttpRequestMessage request, CostosEntregablesDto contenido);
        void Guardar(ParametrosGuardarDto<CostosEntregablesDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
    }
}
