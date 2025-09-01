namespace DNP.ServiciosWBS.Persistencia.Interfaces.Transversales
{
    using Modelo;
    using ServiciosNegocio.Comunes.Dto.Formulario;

    public interface IPersistenciaTemporal
    {
        void GuardarTemporalmente<T>(ParametrosGuardarDto<T> parametrosGuardar);
        AlmacenamientoTemporal ObtenerTemporal(ParametrosConsultaDto parametrosConsultaDto);
    }
}