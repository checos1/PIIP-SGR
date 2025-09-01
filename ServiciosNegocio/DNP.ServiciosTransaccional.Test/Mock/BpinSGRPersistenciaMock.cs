namespace DNP.ServiciosTransaccional.Test.Mock
{
    using Persistencia.Interfaces.Proyecto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class BpinSGRPersistenciaMock : IBpinSGRPersistencia
    {
        public object GenerarBPINSgr(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar)
        {
            return int.Parse(parametrosActualizar.Contenido.ObjetoNegocioId) % 2 == 0;
        }
    }
}
