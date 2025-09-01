namespace DNP.ServiciosNegocio.Web.API.Test.Mock
{
    using Dominio.Dto.Transferencias;
    using Persistencia.Interfaces.Transferencias;
    public class TransferenciaPersistenciaMock : ITransferenciaPersistencia
    {
        public TransferenciaEntidadDto IdentificarEntidadDestino(int proyectoId, int entidadTransfiereId)
        {
            if (proyectoId > 0 && entidadTransfiereId > 0)
            {
                return new TransferenciaEntidadDto()
                       {
                           ProyectoId = proyectoId,
                           EntidadTransfiereId = entidadTransfiereId,
                           EntidadDestinoId = 1,
                           MensajeError = string.Empty,
                           EntidadDestino = "entidad"
                       }
                    ;
            }

            return new TransferenciaEntidadDto()
                   {
                       ProyectoId = proyectoId,
                       EntidadTransfiereId = entidadTransfiereId,
                       EntidadDestinoId = 0,
                       MensajeError = "error",
                       EntidadDestino = string.Empty
                   }
                ;
        }
    }
}
