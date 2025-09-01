using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Dto;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.Viabilidad 
{
    public class AcuerdoSGPServicio : ServicioBase<object>, IAcuerdoSGPServicio
    {
        #region Constructor

        private readonly IAcuerdoSgpPersistencia _ObjPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public AcuerdoSGPServicio(IAcuerdoSgpPersistencia objPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _ObjPersistencia = objPersistencia;
        }
        #endregion

        #region "Métodos"

        public string SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId)
        {
            return _ObjPersistencia.SGPAcuerdoLeerProyecto(proyectoId, nivelId);
        }

        public ResultadoProcedimientoDto SGPAcuerdoGuardarProyecto(string json, string usuario)
        {
            var result = _ObjPersistencia.SGPAcuerdoGuardarProyecto(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<string>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGPAcuerdoGuardarProyecto");

            return result;
        }
        public string SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            return _ObjPersistencia.SGPProyectosLeerListas(nivelId, proyectoId, nombreLista);
        }

        protected override object ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<object> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
