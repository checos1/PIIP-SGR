namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Proyecto
{
    using Interfaces;
    using Interfaces.Proyecto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Data.SqlClient;
    using System.Linq;

    public class ProyectoPersistencia: Persistencia, IProyectoPersistencia
    {
        public ProyectoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public object ActualizarEstado(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            var proyectoBpin = parametrosActualizar.Contenido.ObjetoNegocioId;
            var nivelId = parametrosActualizar.Contenido.NivelId;
            var errorNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            Contexto.uspPostCambioEstadoProyecto(proyectoBpin, Guid.Parse(nivelId), usuario, errorNegocio);

            if (string.IsNullOrEmpty(Convert.ToString(errorNegocio.Value))) return true;
            var mensajeError = Convert.ToString(errorNegocio.Value);
            throw new ServiciosNegocioException(mensajeError);
        }

        public object ActualizarNombre(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            var proyectoBpin = parametrosActualizar.Contenido.ObjetoNegocioId;
            var nivelId = parametrosActualizar.Contenido.NivelId;
            var errorNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            Contexto.uspPostActualizarNombreProyecto(proyectoBpin, Guid.NewGuid(), usuario, errorNegocio);
            // Contexto.uspPostActualizarNombreProyecto(proyectoBpin, Guid.Parse(nivelId), usuario, errorNegocio);

            if (string.IsNullOrEmpty(Convert.ToString(errorNegocio.Value))) return true;
            var mensajeError = Convert.ToString(errorNegocio.Value);
            throw new ServiciosNegocioException(mensajeError);
        }

        public string GetInstaciasProyectoSGP(string ObjetoNegocioId)
        {
            string IdInstanciaAnterior = Contexto.Database.SqlQuery<String>("Transferencia.uspGetInstaciasProyectoSGP @ObjetoNegocioId ",
                                 new SqlParameter("ObjetoNegocioId", ObjetoNegocioId)
                                  ).SingleOrDefault();
            return IdInstanciaAnterior;
        }

        public object PostRecuperaDatosSGP(string idInstanciaAnterior, string idInstanciaDestino)
        {
            var resultado = Contexto.Database.ExecuteSqlCommand("Exec Transferencia.uspPostRecuperaDatosSGP @InstanciaAnterior, @InstanciaDestino",
                                      new SqlParameter("InstanciaAnterior", Guid.Parse(idInstanciaAnterior)),
                                      new SqlParameter("InstanciaDestino", Guid.Parse(idInstanciaDestino))
                                      );
            return string.Empty;
        }
    }
}