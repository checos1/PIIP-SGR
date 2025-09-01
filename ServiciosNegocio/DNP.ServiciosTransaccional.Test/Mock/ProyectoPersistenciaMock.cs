namespace DNP.ServiciosTransaccional.Test.Mock
{
    using Persistencia.Interfaces.Proyecto;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;

    public class ProyectoPersistenciaMock: IProyectoPersistencia
    {
        public object ActualizarEstado(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }

        public object ActualizarNombre(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            if (parametrosActualizar.Contenido.ObjetoNegocioId.Equals("BPIN"))
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }

        public string GetInstaciasProyectoSGP(string ObjetoNegocioId)
        {
            if (ObjetoNegocioId == "0")
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return "";
        }

        public object PostRecuperaDatosSGP(string idInstanciaAnterior, string idInstanciaDestino)
        {
            if (idInstanciaAnterior == "0")
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ParametroNoExiste);

            return true;
        }
    }
}
