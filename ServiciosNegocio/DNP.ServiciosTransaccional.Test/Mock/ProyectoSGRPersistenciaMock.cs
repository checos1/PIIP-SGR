namespace DNP.ServiciosTransaccional.Test.Mock
{
    using Persistencia.Interfaces.Proyecto;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class ProyectoSGRPersistenciaMock : IProyectoSGRPersistencia
    {
        public object ActualizarEstadoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }

        public object IniciarFlujoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }
        
    }
}
