namespace DNP.ServiciosTransaccional.Servicios.Interfaces.Proyectos
{
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public interface IBpinServicio
    {
        object GenerarBPIN(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
        object GenerarBPINSgr(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria);
    }
}
