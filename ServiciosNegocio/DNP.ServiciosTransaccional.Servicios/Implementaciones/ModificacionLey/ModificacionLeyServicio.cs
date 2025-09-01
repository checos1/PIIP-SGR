using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Transferencias;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.ModificacionLey;
using DNP.ServiciosTransaccional.Servicios.Interfaces;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Transversales;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.ModificacionLey
{
    public class ModificacionLeyServicio: ServicioBase<ObjetoNegocio>, IModificacionLeyServicio
    {
        private readonly IModificacionLeyPersistencia _modificacionLeyPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;

        public ModificacionLeyServicio(IModificacionLeyPersistencia modificacionLeyPersistencia, IAuditoriaServicios auditoriaServicios) : base(auditoriaServicios)
        {
            _modificacionLeyPersistencia = modificacionLeyPersistencia;
            _auditoriaServicios = auditoriaServicios;
        }

        public object ActualizarValoresPoliticasML(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _modificacionLeyPersistencia.ActualizarValoresPoliticasML(parametrosActualizar, parametrosAuditoria.Usuario);
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
