namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Transferencias
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    
    public interface ICreacionBpinPersistencia
    {
        object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario);
    }
}
