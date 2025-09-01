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

    public class TransferenciaServicio : ServicioBase<ObjetoNegocio>, ITransferenciaServicio
    {
        private readonly ITransferenciaPersistencia _transferenciaPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;

        public TransferenciaServicio(ITransferenciaPersistencia transferenciaPersistencia, IAuditoriaServicios auditoriaServicios) : base(auditoriaServicios)
        {
            _transferenciaPersistencia = transferenciaPersistencia;
            _auditoriaServicios = auditoriaServicios;
        }

        public override object Guardar(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = GuardadoDefinitivo(parametrosGuardar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaTransferencia, parametrosGuardar.Contenido);
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
            return _transferenciaPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
