namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Transferencias
{
    using System;
    using System.Data.Entity.Core.Objects;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using Interfaces;
    using Interfaces.Transferencias;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;

    public class TransferenciaPersistencia : Persistencia, ITransferenciaPersistencia
    {
        public TransferenciaPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        private static TransferenciaEntidadDto MapearResultadoADto(ObjectParameter parametroEntidadDestino, ObjectParameter parametroEntidadDestinoId, ObjectParameter parametroError, decimal proyectoId)
        {
            return new TransferenciaEntidadDto()
            {
                EntidadDestino = parametroEntidadDestino.Value.ToString(),
                EntidadDestinoId = parametroEntidadDestinoId.Value != DBNull.Value ? Convert.ToInt32(parametroEntidadDestinoId.Value) : 0,
                MensajeError = parametroError.Value.ToString(),
                ProyectoId = proyectoId
            };
        }

        public object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            var parametroEntidadDestino = new ObjectParameter("EntidadDestino", typeof(string));
            var parametroEntidadDestinoId = new ObjectParameter("EntidadDestinoId", typeof(int));
            var parametroError = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var proyectoId = decimal.Parse(parametrosGuardar.Contenido.ObjetoNegocioId);

            Contexto.uspPostIdentificarEntidadDestino(proyectoId, parametroEntidadDestino, parametroEntidadDestinoId, parametroError);

            if (string.IsNullOrEmpty(Convert.ToString(parametroError.Value))) return MapearResultadoADto(parametroEntidadDestino, parametroEntidadDestinoId, parametroError, proyectoId);
            var mensajeError = Convert.ToString(parametroError.Value);
            throw new ServiciosNegocioException(mensajeError);
        }
    }
}
