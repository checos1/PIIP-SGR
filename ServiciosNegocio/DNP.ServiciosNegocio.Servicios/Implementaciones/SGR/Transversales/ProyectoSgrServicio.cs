using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.AvalUso;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTEI;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.OcadPaz;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.Transversales
{
    public class ProyectoSgrServicio : ServicioBase<AsignacionUsuarioOcadPazDto>, IProyectoSgrServicio
    {
        #region Constructor

        private readonly IProyectoSgrPersistencia _objetoPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public ProyectoSgrServicio(IProyectoSgrPersistencia ProyectosSGRPersistencia, IAuditoriaServicios auditoriaServicios) : base(null, auditoriaServicios)
        {
            _objetoPersistencia = ProyectosSGRPersistencia;
        }

        #endregion

        #region "Métodos"

        /// <summary>
        ///Lee una lista para un proyecto según parámetro
        /// </summary>
        public string SGR_Proyectos_LeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerListas(nivelId, proyectoId, nombreLista);
        }

        /// <summary>
        /// Validar cumplimiento de un proyecto por instancia
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <returns>int</returns> 
        public int SGR_Proyectos_CumplimentoFlujoSGR(Guid instanciaId)
        {
            return _objetoPersistencia.SGR_Proyectos_CumplimentoFlujoSGR(instanciaId);
        }

        /// <summary>
        /// Leer entidades por id del proyecto
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipoEntidad"></param>  
        /// <returns>string</returns> 
        public string SGR_Proyectos_LeerEntidadesAdscritas(int proyectoId, string tipoEntidad)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerEntidadesAdscritas(proyectoId, tipoEntidad);
        }

        /// <summary>
        /// Validar entidad delegada
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipo"></param>  
        /// <returns>ResultadoProcedimientoDto</returns> 
        public ResultadoProcedimientoDto SGR_Proyectos_ValidarEntidadDelegada(int proyectoId, string tipo)
        {
            return _objetoPersistencia.SGR_Proyectos_ValidarEntidadDelegada(proyectoId, tipo);
        }

        /// <summary>
        /// Actualizar entidad adscrita
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="user"></param> 
        /// <param name="delegado"></param> 
        /// <returns>bool</returns> 
        public bool SGR_Proyectos_ActualizarEntidadAdscrita(int proyectoId, int entityId, bool delegado, string user)
        {
            return _objetoPersistencia.SGR_Proyectos_ActualizarEntidadAdscrita(proyectoId, entityId, delegado, user);
        }

        public IEnumerable<ProyectoViabilidadInvolucradosDto> SGR_Proyectos_LeerProyectoViabilidadInvolucrados(int proyectoId, int tipoConceptoViabilidadId)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerProyectoViabilidadInvolucrados(proyectoId, tipoConceptoViabilidadId);
        }
        
        public IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> SGR_Proyectos_LeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId);
        }

        public ProyectoViabilidadInvolucradosResultado EliminarProyectoViabilidadInvolucradoso(int id)
        {
            var result = _objetoPersistencia.EliminarProyectoViabilidadInvolucradoso(id);

            var parametrosGuardar = new ParametrosGuardarDto<int>
            {
                Contenido = id
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "EliminarProyectoViabilidadInvolucradoso");


            return result;
        }

        public ProyectoViabilidadInvolucradosResultado GuardarProyectoViabilidadInvolucrados(ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto> parametrosGuardar, string usuario)
        {
            var result = _objetoPersistencia.GuardarProyectoViabilidadInvolucrados(parametrosGuardar, usuario);

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "GuardarProyectoViabilidadInvolucrados");

            return result;
        }

        public string SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid instanciaId)
        {
            var result = _objetoPersistencia.SGR_Proyectos_GenerarMensajeEstadoProyecto(instanciaId);
            var parametrosGuardar = new ParametrosGuardarDto<Guid>
            {
                Contenido = instanciaId
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_Proyectos_GenerarMensajeEstadoProyecto");
            return result;
        }

        public bool SGR_Proyectos_PostAplicarFlujoSGR(AplicarFlujoSGRDto parametros)
        {
            return _objetoPersistencia.SGR_Proyectos_PostAplicarFlujoSGR(parametros);
        }

        public bool SGR_Proyectos_PostDevolverFlujoSGR(DevolverFlujoSGRDto parametros)
        {
            return _objetoPersistencia.SGR_Proyectos_PostDevolverFlujoSGR(parametros);
        }

        public ProyectoCtusDto SGR_Proyectos_LeerProyectoCtus(int ProyectoId, Guid instanciaId)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerProyectoCtus(ProyectoId,instanciaId);
        }

        public IEnumerable<EntidadesSolicitarCtusDto> SGR_Proyectos_LeerEntidadesSolicitarCtus(int ProyectoId)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerEntidadesSolicitarCtus(ProyectoId);
        }

        public ProyectoCtuResultado SGR_Proyectos_GuardarProyectoSolicitarCtus(ParametrosGuardarDto<ProyectoCtusDto> parametrosGuardar, string usuario)
        {
            return _objetoPersistencia.SGR_Proyectos_GuardarProyectoSolicitarCtus(parametrosGuardar, usuario);
        }

        public List<ProyectoEntidadVerificacionOcadPazDto> ConsultarProyectosVerificacionOcadPazSgr(ParametrosProyectoVerificacionSgrDto parametros)
        {
            var proyectos = new List<ProyectoEntidadVerificacionOcadPazDto>();

            if (parametros != null)
                proyectos = _objetoPersistencia.ObtenerProyectosVerificacionOcadPazSgr(parametros);

            if (proyectos.Count == 0) return new List<ProyectoEntidadVerificacionOcadPazDto>();

            return proyectos;
        }

        public IEnumerable<UsuariosVerificacionOcadPazDto> SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(Guid rolId, int entidadId)
        {
            return _objetoPersistencia.SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(rolId, entidadId);
        }

        public ResultadoProcedimientoDto SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioOcadPazDto json, string usuario)
        {
            return _objetoPersistencia.SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(json, usuario);
        }

        public string SGR_Proyectos_validarTecnicoOcadpaz(Nullable<System.Guid> instanciaId, Nullable<System.Guid> accionId)
        {
            return _objetoPersistencia.SGR_Proyectos_validarTecnicoOcadpaz(instanciaId, accionId);
        }

        public ResultadoProcedimientoDto SGR_Proyectos_GuardarAsignacionUsuarioEncargado(UsuarioEncargadoDto json, string usuario)
        {
            var result = _objetoPersistencia.SGR_Proyectos_GuardarAsignacionUsuarioEncargado(json, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<UsuarioEncargadoDto>
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

        public string SGR_Proyectos_LeerAsignacionUsuarioEncargado(int proyectoId, Guid instanciaId)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerAsignacionUsuarioEncargado(proyectoId, instanciaId);
        }

        public string SGR_Proyectos_LeerDatosAdicionalesCTEI(int proyectoId, Guid instanciaId)
        {
            return _objetoPersistencia.SGR_Proyectos_LeerDatosAdicionalesCTEI(proyectoId, instanciaId);
        }

        public ResultadoProcedimientoDto SGR_Proyectos_GuardarDatosAdicionalesCTEI(DatosAdicionalesCTEIDto datosAdicionalesCTEIDto, string usuario)
        {
            return _objetoPersistencia.SGR_Proyectos_GuardarDatosAdicionalesCTEI(datosAdicionalesCTEIDto, usuario);
        }

        public ResultadoProcedimientoDto SGR_Proyectos_RegistrarAvalUsoSgr(DatosAvalUsoDto datosAvalUsoDto, string usuario)
        {
            var result = _objetoPersistencia.SGR_Proyectos_RegistrarAvalUsoSgr(datosAvalUsoDto, usuario);

            var parametrosGuardar = new ParametrosGuardarDto<DatosAvalUsoDto>
            {
                Contenido = datosAvalUsoDto
            };

            var parametrosAuditoria = new ParametrosAuditoriaDto
            {
                Usuario = Usuario,
                Ip = Ip
            };

            GenerarAuditoriaGlobal(parametrosGuardar, parametrosAuditoria, Comunes.Enum.TipoMensajeEnum.Creacion, "SGR_Proyectos_RegistrarAvalUsoSgr");

            return result;
        }

        public string SGR_Proyectos_LeerAvalUsoSgr(int proyectoId, Guid instanciaId)
        {
            return _objetoPersistencia.SGR_Proyectos_LeeAvalUsoSgr(proyectoId, instanciaId);
        }

        protected override AsignacionUsuarioOcadPazDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new NotImplementedException();
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<AsignacionUsuarioOcadPazDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        public bool SGR_Obtener_Proyectos_TieneInstanciaActiva(String ObjetoNegocioId)
        {
            return _objetoPersistencia.SGR_Obtener_Proyectos_TieneInstanciaActiva(ObjetoNegocioId);
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_EliminarOperacionCreditoSGR(int proyectoid, string usuario)
        {
            return _objetoPersistencia.SGR_Viabilidad_EliminarOperacionCreditoSGR(proyectoid, usuario);
        }

        #endregion
    }
}
