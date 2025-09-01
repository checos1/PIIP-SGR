namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using Persistencia.Interfaces.Transferencias;

    public class TransferenciaServicioMock : ITransferenciaPersistencia
    {
        public object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            return new TransferenciaEntidadDto()
            {
                ProyectoId = int.Parse(parametrosGuardar.Contenido.ObjetoNegocioId),
                EntidadDestinoId = 1,
                MensajeError = string.Empty,
                EntidadDestino = "entidad"
            };
        }
    }
}
