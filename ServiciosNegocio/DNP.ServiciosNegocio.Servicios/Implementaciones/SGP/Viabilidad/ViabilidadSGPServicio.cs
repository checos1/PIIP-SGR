using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.Viabilidad
{
    public class ViabilidadSGPServicio : ServicioBase<ProyectoDto>, IViabilidadSGPServicio
    {
        #region Constructor

        private readonly IViabilidadSgpPersistencia _ObjPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public ViabilidadSGPServicio(IViabilidadSgpPersistencia objPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _ObjPersistencia = objPersistencia;
        }
        #endregion

        #region "Métodos"

        public string SGPTransversalLeerParametro(string Parametro)
        {
            return _ObjPersistencia.SGPTransversalLeerParametro(Parametro);
        }
        public LeerInformacionGeneralViabilidadDto SGPViabilidadLeerInformacionGeneral(int proyectoId, System.Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            return _ObjPersistencia.SGPViabilidadLeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode);
        }

        public string SGPViabilidadLeerParametricas(int proyectoId, System.Guid nivelId)
        {
            return _ObjPersistencia.SGPViabilidadLeerParametricas(proyectoId, nivelId);
        }

        public ResultadoProcedimientoDto SGPViabilidadGuardarInformacionBasica(string json, string usuario)
        {
            var result = _ObjPersistencia.SGPViabilidadGuardarInformacionBasica(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<string>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGPViabilidadGuardarInformacionBasica");

            return result;
        }

        public ResultadoProcedimientoDto SGPViabilidadFirmarUsuario(string json, string usuario)
        {
            var result = _ObjPersistencia.SGPViabilidadFirmarUsuario(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<string>
            {
                Contenido = json
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGPViabilidadFirmarUsuario");

            return result;
        }

        public IEnumerable<ProyectoViabilidadInvolucradosDto> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId)
        {
            return _ObjPersistencia.SGPProyectosLeerProyectoViabilidadInvolucrados(proyectoId, instanciaId, tipoConceptoViabilidadId);
        }

        public EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId)
        {
            return _ObjPersistencia.SGPProyectosObtenerEntidadDestinoResponsableFlujo(rolId, crTypeId, entidadResponsableId, proyectoId);
        }

        public EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId)
        {
            return _ObjPersistencia.SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(rolId, entidadResponsableId, tramiteId);
        }

        public IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            return _ObjPersistencia.SGPProyectosLeerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId);
        }

        public ProyectoViabilidadInvolucradosResultado EliminarProyectoViabilidadInvolucradosSGP(int id)
        {
            var result = _ObjPersistencia.EliminarProyectoViabilidadInvolucradosSGP(id);

            var parametrosGuardar = new ParametrosGuardarDto<int>
            {
                Contenido = id
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Eliminacion, "EliminarProyectoViabilidadInvolucradosSGP");

            return result;
        }

        public ProyectoViabilidadInvolucradosResultado GuardarProyectoViabilidadInvolucradosSGP(ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto> parametrosGuardar, string usuario)
        {
            var result = _ObjPersistencia.GuardarProyectoViabilidadInvolucradosSGP(parametrosGuardar, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarProyectoViabilidadInvolucradosSGP");

            return result;
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
