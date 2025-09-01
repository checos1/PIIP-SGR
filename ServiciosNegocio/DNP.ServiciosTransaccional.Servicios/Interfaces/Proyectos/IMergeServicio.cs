namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Proyectos
{
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public interface IMergeServicio
    {
        object AplicarMerge(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
    }
}
