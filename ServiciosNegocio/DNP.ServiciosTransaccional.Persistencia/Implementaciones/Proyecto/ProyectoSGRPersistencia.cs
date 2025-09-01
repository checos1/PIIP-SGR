namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Proyecto
{
    using Interfaces;
    using Interfaces.Proyecto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Data.Entity.Core.Objects;

    public class ProyectoSGRPersistencia: PersistenciaSGR, IProyectoSGRPersistencia
    {
        public ProyectoSGRPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
        }

        public object ActualizarEstadoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            var proyectoBpin = parametrosActualizar.Contenido.ObjetoNegocioId;
            var nivelId = parametrosActualizar.Contenido.NivelId;
            var flujoId = parametrosActualizar.Contenido.FlujoId;
            var errorNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            Contexto.uspPostCambioEstadoProyectoSgr(proyectoBpin, Guid.Parse(nivelId), Guid.Parse(flujoId), usuario, errorNegocio);

            if (string.IsNullOrEmpty(Convert.ToString(errorNegocio.Value))) return true;
            var mensajeError = Convert.ToString(errorNegocio.Value);
            throw new ServiciosNegocioException(mensajeError);
        }

        public object IniciarFlujoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            var proyectoBpin = parametrosActualizar.Contenido.ObjetoNegocioId;
            var nivelId = parametrosActualizar.Contenido.NivelId;
            var flujoId = parametrosActualizar.Contenido.FlujoId;
            var errorNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            Contexto.uspPostIniciarFlujoSgr(proyectoBpin, Guid.Parse(nivelId), Guid.Parse(flujoId), usuario, errorNegocio);

            if (string.IsNullOrEmpty(Convert.ToString(errorNegocio.Value))) return true;
            var mensajeError = Convert.ToString(errorNegocio.Value);
            throw new ServiciosNegocioException(mensajeError);
        }
    }
}