using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;

namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesPersistencia
    {
        PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversales(string bpin);
        PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversalesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTRelacionadasDto> parametrosGuardar, string usuario);
        void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}
