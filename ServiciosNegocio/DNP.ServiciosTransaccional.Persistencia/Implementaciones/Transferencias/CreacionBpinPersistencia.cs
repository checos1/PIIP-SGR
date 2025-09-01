namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Transferencias
{
    using System;
    using System.Data.Entity.Core.Objects;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using Interfaces;
    using Interfaces.Transferencias;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;

    public class CreacionBpinPersistencia : Persistencia, ICreacionBpinPersistencia
    {
        public CreacionBpinPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public object GuardarDefinitivamente(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            var parametroExitoso = new ObjectParameter("Exitoso", typeof(bool));
            var proyectoId = decimal.Parse(parametrosGuardar.Contenido.ObjetoNegocioId);

            try
            {
                Contexto.uspValidaciones(proyectoId, parametroExitoso);
            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            return Convert.ToBoolean(parametroExitoso.Value);
        }
    }
}
