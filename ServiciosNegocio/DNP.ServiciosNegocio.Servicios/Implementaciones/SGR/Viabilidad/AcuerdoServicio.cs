using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Dto;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.Viabilidad 
{
    public class AcuerdoServicio : ServicioBase<string>, IAcuerdoServicio
    {
        #region Constructor

        public string Usuario { get; set; }
        public string Ip { get; set; }


        private readonly IAcuerdoPersistencia _ObjPersistencia;

        public AcuerdoServicio(IAcuerdoPersistencia objPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _ObjPersistencia = objPersistencia;
        }

        #endregion

        #region "Métodos"

        public string SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId)
        {
            return _ObjPersistencia.SGR_Acuerdo_LeerProyecto(proyectoId, nivelId);
        }

        public ResultadoProcedimientoDto SGR_Acuerdo_GuardarProyecto(string json, string usuario)
        {
            var result = _ObjPersistencia.SGR_Acuerdo_GuardarProyecto(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<string>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_Acuerdo_GuardarProyecto");

            return result;
        }

        protected override string ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<string> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
