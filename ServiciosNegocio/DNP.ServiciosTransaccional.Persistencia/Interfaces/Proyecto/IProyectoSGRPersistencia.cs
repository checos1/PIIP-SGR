namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Proyecto
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public interface IProyectoSGRPersistencia
    {
        object ActualizarEstadoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario);
        object IniciarFlujoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario);
    }
}
