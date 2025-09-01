namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Transferencias
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public interface ITransferenciaServicio
    {
        object Guardar(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria);
    }
}
