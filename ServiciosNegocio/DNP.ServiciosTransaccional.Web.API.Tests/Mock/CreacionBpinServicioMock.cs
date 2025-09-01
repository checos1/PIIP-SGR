namespace DNP.ServiciosTransaccional.Web.API.Test.Mock
{
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using Persistencia.Interfaces.Transferencias;

    public class CreacionBpinServicioMock : ICreacionBpinPersistencia
    {
        public object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            return int.Parse(parametrosGuardar.Contenido.ObjetoNegocioId) % 2 == 0;
        }
    }
}
