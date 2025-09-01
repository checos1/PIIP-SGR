namespace DNP.ServiciosNegocio.Servicios.Interfaces.Transferencias
{
    using Comunes.Dto;
    using Dominio.Dto.Transferencias;

    public interface ITransferenciaServicio
    {
        TransferenciaEntidadDto IdentificarEntidadDestino(int proyectoId, int entidadTransfiereId, ParametrosAuditoriaDto parametrosAuditoriaDto);
    }
}
