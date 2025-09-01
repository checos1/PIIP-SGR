using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.CostosEntregables;


namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface ICostosEntregablesPersistencia
    {
        CostosEntregablesDto ObtenerCostosEntregables(string bpin);
        CostosEntregablesDto ObtenerCostosEntregablesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<CostosEntregablesDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
