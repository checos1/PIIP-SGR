using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using System;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Dto;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.Viabilidad
{
    public class CTUSServicio : ServicioBase<ConceptoCTUSDto>, ICTUSServicio
    {
        private readonly ICTUSPersistencia _ObjPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        #region Constructor
        public CTUSServicio(ICTUSPersistencia objPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _ObjPersistencia = objPersistencia;
        }
        #endregion

        #region "Métodos"
        public ConceptoCTUSDto SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId)
        {
            return _ObjPersistencia.SGR_CTUS_LeerProyectoCtusConcepto(proyectoCtusId);
        }

        public ResultadoProcedimientoDto SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto json, string usuario)
        {
            var result = _ObjPersistencia.SGR_CTUS_GuardarProyectoCtusConcepto(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<ConceptoCTUSDto>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_CTUS_GuardarProyectoCtusConcepto");

            return result;
        }

        public ResultadoProcedimientoDto SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto json, string usuario)
        {
            var result = _ObjPersistencia.SGR_CTUS_GuardarAsignacionUsuarioEncargado(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<AsignacionUsuarioCTUSDto>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_CTUS_GuardarAsignacionUsuarioEncargado");

            return result;
        }

        public UsuarioEncargadoCTUSDto SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId)
        {
            return _ObjPersistencia.SGR_CTUS_LeerProyectoCtusUsuarioEncargado(proyectoCtusId, instanciaId);
        }

        public ResultadoProcedimientoDto SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto json, string usuario)
        {
            var result = _ObjPersistencia.SGR_CTUS_GuardarResultadoConceptoCtus(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<ResultadoConceptoCTUSDto>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_CTUS_GuardarResultadoConceptoCtus");

            return result;
        }

        public RolApruebaCTUSDto SGR_CTUS_LeerRolDirectorProyectoCtus(int proyectoId, Guid instanciaId)
        {
            return _ObjPersistencia.SGR_CTUS_LeerRolDirectorProyectoCtus(proyectoId, instanciaId);
        }

        public string ValidarInstanciaCTUSNoFinalizada(int idProyecto)
        {
            return _ObjPersistencia.ValidarInstanciaCTUSNoFinalizada(idProyecto);
        }

        /// <summary>
        /// Actualizar entidad adscrita CTUS
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="user"></param> 
        /// <returns>bool</returns> 
        public bool SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo, string user)
        {
            return _ObjPersistencia.SGR_Proyectos_ActualizarEntidadAdscritaCTUS(proyectoId, entityId, tipo, user);
        }

        protected override ConceptoCTUSDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ConceptoCTUSDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
