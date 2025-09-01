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

    public class BpinServicio : ServicioBase<ObjetoNegocio>, IBpinServicio
    {
        private readonly IBpinPersistencia _bpinPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;
        private readonly IBpinSGRPersistencia _bpinSGRPersistencia;

        public BpinServicio(IBpinPersistencia bpinPersistencia, 
            IAuditoriaServicios auditoriaServicios,
            IBpinSGRPersistencia bpinSGRPersistencia) : base(auditoriaServicios)
        {
            _bpinPersistencia = bpinPersistencia;
            _auditoriaServicios = auditoriaServicios;
            _bpinSGRPersistencia = bpinSGRPersistencia;
        }

        public object GenerarBPIN(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _bpinPersistencia.GenerarBPIN(parametrosActualizar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaEstadoProyecto, parametrosActualizar.Contenido);
            GenerarAuditoria(parametrosActualizar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Modificacion,
                             mensajeAccion);
            return resultado;
        }

        public object GenerarBPINSgr(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _bpinSGRPersistencia.GenerarBPINSgr(parametrosActualizar);
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
