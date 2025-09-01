namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Transferencias
{
    using System;
    using System.Data.Entity.Core.Objects;
    using Dominio.Dto.Transferencias;
    using Interfaces;
    using Interfaces.Transferencias;
    public class TransferenciaPersistencia : Persistencia, ITransferenciaPersistencia
    {
        public TransferenciaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }
        public TransferenciaEntidadDto IdentificarEntidadDestino(int proyectoId, int entidadTransfiereId)
        {
            var parametroEntidadDestino = new ObjectParameter("EntidadDestino", typeof(string));
            var parametroEntidadDestinoId = new ObjectParameter("EntidadDestinoId", typeof(int));
            var parametroError = new ObjectParameter("errorValidacionNegocio", typeof(string));

            Contexto.uspIdentificarEntidadDestino(proyectoId, entidadTransfiereId, parametroEntidadDestino, parametroEntidadDestinoId, parametroError);
            return MapearResultadoADto(parametroEntidadDestino, parametroEntidadDestinoId, parametroError, proyectoId, entidadTransfiereId);
        }

        private TransferenciaEntidadDto MapearResultadoADto(ObjectParameter parametroEntidadDestino, ObjectParameter parametroEntidadDestinoId, ObjectParameter parametroError, int proyectoId, int entidadTransfiereId)
        {
            return new TransferenciaEntidadDto()
                   {
                       EntidadDestino = parametroEntidadDestino.Value.ToString(),
                       EntidadDestinoId = parametroEntidadDestinoId.Value != System.DBNull.Value ? Convert.ToInt32(parametroEntidadDestinoId.Value) : 0,
                       MensajeError = parametroError.Value.ToString(),
                       ProyectoId = proyectoId,
                       EntidadTransfiereId = entidadTransfiereId
                   };
        }
    }
}
