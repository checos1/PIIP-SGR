namespace DNP.ServiciosWBS.Persistencia.Interfaces
{
    using ServiciosNegocio.Dominio.Dto.Tramites;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesCrucePoliticas;

    public interface IPoliticasTransversalesCrucePoliticasPersistencia
    {
        PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticas(string Bpin, int IdFuente);
        PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticasPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<PoliticasTCrucePoliticasDto> parametrosGuardar, string usuario);
    }
}
