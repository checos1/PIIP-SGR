namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Proyecto
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public interface IMergePersistencia
    {
        object AplicarMerge(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario);
    }
}
