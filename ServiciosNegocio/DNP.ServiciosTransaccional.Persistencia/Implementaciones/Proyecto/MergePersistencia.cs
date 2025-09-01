namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Proyecto
{
    using Interfaces;
    using Interfaces.Proyecto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Data.Entity.Core.Objects;

    public class MergePersistencia : Persistencia, IMergePersistencia
    {
        public MergePersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public object AplicarMerge(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, string usuario)
        {
            var proyectoId = int.Parse(parametrosActualizar.Contenido.ObjetoNegocioId);
            var parametroExitoso = new ObjectParameter("Exitoso", typeof(bool));

            try
            {
                Contexto.uspPostMergeAplicarMga(proyectoId, usuario, parametroExitoso);
            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
            return Convert.ToBoolean(parametroExitoso.Value);
        }
    }
}
