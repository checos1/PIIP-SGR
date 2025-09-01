namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Transferencias
{
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using Persistencia.Interfaces.Transferencias;
    using Interfaces.Transferencias;
    using Interfaces.Transversales;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes;

    public class CreacionBpinServicio : ServicioBase<ObjetoNegocio>, ICreacionBpinServicio
    {
        private readonly ICreacionBpinPersistencia _creacionBpinPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;

        public CreacionBpinServicio(ICreacionBpinPersistencia creacionBpinPersistencia, IAuditoriaServicios auditoriaServicios) : base(auditoriaServicios)
        {
            _creacionBpinPersistencia = creacionBpinPersistencia;
            _auditoriaServicios = auditoriaServicios;
        }

        public override object Guardar(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = GuardadoDefinitivo(parametrosGuardar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaValidacion, parametrosGuardar.Contenido);
            GenerarAuditoria(parametrosGuardar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Creacion,
                             mensajeAccion);
            return resultado;
        }

        protected override object GuardadoDefinitivo(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario)
        {
            return _creacionBpinPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
