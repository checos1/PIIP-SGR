namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Transferencias
{
    using Dominio.Dto.Transferencias;

    public interface ITransferenciaPersistencia
    {
        TransferenciaEntidadDto IdentificarEntidadDestino(int proyectoId, int entidadTransfiereId);
    }
}
