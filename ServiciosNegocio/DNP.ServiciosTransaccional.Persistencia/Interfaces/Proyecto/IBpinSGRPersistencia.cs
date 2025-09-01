namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Proyecto
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public interface IBpinSGRPersistencia
    {
        object GenerarBPINSgr(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar);
    }
}
