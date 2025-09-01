namespace DNP.ServiciosTransaccional.Test.Mock
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using Persistencia.Interfaces.Transferencias;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class CreacionBpinPersistenciaMock : ICreacionBpinPersistencia
    {
        public object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
             return int.Parse(parametrosGuardar.Contenido.ObjetoNegocioId) % 2 == 0;
        }
    }
}
