namespace DNP.ServiciosTransaccional.Test.Mock
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using Persistencia.Interfaces.Transferencias;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class TransferenciaPersistenciaMock : ITransferenciaPersistencia
    {
        public object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            return new TransferenciaEntidadDto();
        }
    }
}
