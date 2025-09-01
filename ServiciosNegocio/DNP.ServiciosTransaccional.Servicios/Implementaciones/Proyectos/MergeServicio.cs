namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Proyectos
{
    using Interfaces.Proyectos;
    using Interfaces.Transversales;
    using Persistencia.Interfaces.Proyecto;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosNegocio.Dominio.Dto.Transferencias;

    public class MergeServicio : ServicioBase<ObjetoNegocio>, IMergeServicio
    {
        private readonly IMergePersistencia _mergePersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;

        public MergeServicio(IMergePersistencia mergePersistencia, IAuditoriaServicios auditoriaServicios) : base(auditoriaServicios)
        {
            _mergePersistencia = mergePersistencia;
            _auditoriaServicios = auditoriaServicios;
        }

        public object AplicarMerge(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _mergePersistencia.AplicarMerge(parametrosActualizar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaEstadoProyecto, parametrosActualizar.Contenido);
            GenerarAuditoria(parametrosActualizar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Modificacion,
                             mensajeAccion);
            return resultado;
        }

        protected override object GuardadoDefinitivo(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario) { throw new System.NotImplementedException(); }
    }
}
