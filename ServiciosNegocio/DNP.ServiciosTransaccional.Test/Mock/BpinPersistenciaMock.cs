namespace DNP.ServiciosTransaccional.Test.Mock
{
    using Persistencia.Interfaces.Proyecto;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class BpinPersistenciaMock : IBpinPersistencia
    {
        public object GenerarBPIN(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            return int.Parse(parametrosActualizar.Contenido.ObjetoNegocioId) % 2 == 0;
        }
    }
}
