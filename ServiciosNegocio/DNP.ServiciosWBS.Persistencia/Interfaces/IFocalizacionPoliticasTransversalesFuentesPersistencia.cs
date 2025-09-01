using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;


namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    public interface IFocalizacionPoliticasTransversalesFuentesPersistencia
    {
        FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentes(string bpin);
        FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentesPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionPoliticaTFuentesDto> parametrosGuardar, string usuario);
    }
}
