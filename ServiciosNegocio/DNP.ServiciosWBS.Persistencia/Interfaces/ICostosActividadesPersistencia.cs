using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.CostosActividades;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursos;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface ICostosActividadesPersistencia
    {
        CostosActividadesDto ObtenerCostosActividades(string bpin);
        CostosActividadesDto ObtenerCostosActividadesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<CostosActividadesDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
