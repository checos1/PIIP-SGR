using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto;
using System;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.Viabilidad
{
    public class ViabilidadServicio : ServicioBase<ProyectoDto>, IViabilidadServicio
    {
        #region Constructor

        private readonly IViabilidadPersistencia _ObjPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public ViabilidadServicio(IViabilidadPersistencia objPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _ObjPersistencia = objPersistencia;
        }



        #endregion

        #region "Métodos"

        public LeerInformacionGeneralViabilidadDto SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            return _ObjPersistencia.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode);
        }

        public string SGR_Viabilidad_LeerParametricas(int proyectoId, System.Guid nivelId)
        {
            return _ObjPersistencia.SGR_Viabilidad_LeerParametricas(proyectoId, nivelId);
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_GuardarInformacionBasica(string json, string usuario)
        {
            var result = _ObjPersistencia.SGR_Viabilidad_GuardarInformacionBasica(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<string>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_Viabilidad_GuardarInformacionBasica");

            return result;
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_FirmarUsuario(string json, string usuario)
        {
            var result = _ObjPersistencia.SGR_Viabilidad_FirmarUsuario(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<string>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_Viabilidad_FirmarUsuario");

            return result;
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_EliminarFirmaUsuario(string json, string usuario)
        {
            var result = _ObjPersistencia.SGR_Viabilidad_EliminarFirmaUsuario(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<string>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Modificacion, "SGR_Viabilidad_EliminarFirmaUsuario");

            return result;
        }

        public string SGR_Viabilidad_ObtenerPuntajeProyecto(Guid instanciaId, int entidadId)
        {
            return _ObjPersistencia.SGR_Viabilidad_ObtenerPuntajeProyecto(instanciaId, entidadId);
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_GuardarPuntajeProyecto(string json, string usuario)
        {
            return _ObjPersistencia.SGR_Viabilidad_GuardarPuntajeProyecto(json, usuario);
        }

        protected override ProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
