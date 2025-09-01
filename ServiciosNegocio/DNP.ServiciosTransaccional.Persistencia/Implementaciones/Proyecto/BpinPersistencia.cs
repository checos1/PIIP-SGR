namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Proyecto
{
    using Interfaces;
    using Interfaces.Proyecto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Data.Entity.Core.Objects;

    public class BpinPersistencia : Persistencia, IBpinPersistencia
    {
        public BpinPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public object GenerarBPIN(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            var proyectoId = int.Parse(parametrosActualizar.Contenido.ObjetoNegocioId);
            var parametroExitoso = new ObjectParameter("Exitoso", typeof(bool));

            try
            {
                Contexto.uspPostGeneraBpin(proyectoId, parametroExitoso);
            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            return Convert.ToBoolean(parametroExitoso.Value);
        }
    }
}
