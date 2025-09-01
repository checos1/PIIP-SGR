namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Proyecto
{
    using Interfaces;
    using Interfaces.Proyecto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Data.Entity.Core.Objects;

    public class BpinSGRPersistencia : PersistenciaSGR, IBpinSGRPersistencia
    {
        public BpinSGRPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
        }

        public object GenerarBPINSgr(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar)
        {
            var proyectoId = int.Parse(parametrosActualizar.Contenido.ObjetoNegocioId);
            var parametroExitoso = new ObjectParameter("Exitoso", typeof(bool));

            try
            {
                Contexto.uspPostGeneraBpinSgr(proyectoId, parametroExitoso);
            }
            catch (ServiciosNegocioException)
            {
                throw;
            }

            return Convert.ToBoolean(parametroExitoso.Value);
        }
    }
}
