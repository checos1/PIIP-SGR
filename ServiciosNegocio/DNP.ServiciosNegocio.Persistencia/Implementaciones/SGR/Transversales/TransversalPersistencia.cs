using AutoMapper;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Reportes;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.Transversales
{
    public class TransversalPersistencia : PersistenciaSGR, ITransversalPersistencia
    {
        private readonly IMapper _mapper;
        #region Constructor

        public TransversalPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
            _mapper = ConfigurarMapper();
        }

        #endregion

        #region "Métodos"

        /// <summary>
        /// Leer Parámetro Transversal.Parametros
        /// </summary>
        public string SGR_Transversal_LeerParametro(string Parametro)
        {
            string Json = Contexto.uspGetSGR_Transversal_LeerParametro(Parametro).FirstOrDefault();
            return Json;
        }

        public string SGR_Proyectos_LeerListaParametros()
        {
            string Json = Contexto.uspGetSGR_Transversal_LeerListaParametros().SingleOrDefault();
            return Json;
        }

        /// <summary>
        /// Leer información del encabezado de un proyecto
        /// </summary>
        /// <param name="proyectoId">Id del proyecto</param>
        /// <returns>Información del proyecto</returns>
        public EncabezadoSGRDto uspGetSGR_Encabezado_LeerEncabezado(ParametrosEncabezadoSGR parametros)
        {
            var resultado = Contexto.uspGetSGR_Encabezado_LeerEncabezado(parametros.IdInstancia, parametros.IdFlujo, parametros.IdNivel, parametros.IdProyectoStr, parametros.Tramite).FirstOrDefault();
            return _mapper.Map<EncabezadoSGRDto>(resultado);
        }

        /// <summary>
        /// Leer documentos soportes
        /// </summary>
        public IEnumerable<TipoDocumentoSoporteDto> SGR_Transversal_ObtenerTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            return _mapper.Map<IEnumerable<TipoDocumentoSoporteDto>>(Contexto.uspGetSGR_TipoDocumentosSoportePorRol(tipoTramiteId, roles, nivelId, instanciaId, accionId, tramiteId));
        }

        /// <summary>
        /// Leer documentos soportes para despliegue
        /// </summary>
        public IEnumerable<TipoDocumentoSoporteDto> SGR_Transversal_ObtenerListaTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            return _mapper.Map<IEnumerable<TipoDocumentoSoporteDto>>(Contexto.uspGetSGR_ListaTipoDocumentosSoportePorRol(tipoTramiteId, roles, nivelId, instanciaId, accionId, tramiteId));
        }

        /// <summary>
        /// Leer la configuración de los reportes
        /// </summary>
        public ConfiguracionReportesDto SGR_Transversal_ObtenerConfiguracionReportes(Guid instanciaId)
        {
            return _mapper.Map<ConfiguracionReportesDto>(Contexto.uspGetConfiguracionReportesPorInstancia(instanciaId).FirstOrDefault());
        }

        public Nullable<bool> AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(Guid instanciaId, Guid RolId, string usuario)
        {
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectResult<bool?> resultado =  Contexto.uspPostAutorizacionAccionesPorInstanciaSubFlujoOCADPaz(instanciaId, RolId, usuario);

                    if (Convert.ToBoolean( resultado))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return true;
                    }
                    else
                    {
                        var mensajeError = "Error al insertar los permisos para el rol especificado";
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception es)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<ValidacionOCADPazDto> SGR_Transversal_ValidacionOCADPaz(string proyectoId, Guid nivelId, Guid instanciaId, Guid flujoId) 
        {
            return _mapper.Map<IEnumerable<ValidacionOCADPazDto>>(Contexto.UspValidacionOCADPazSgr(proyectoId, nivelId, instanciaId, flujoId));
        }

        public IEnumerable<UsuariosProyectoDto> SGR_Transversal_ObtenerUsuariosNotificacionViabilidad(Guid instanciaId)
        {
            return _mapper.Map<IEnumerable<UsuariosProyectoDto>>(Contexto.UspObtenerUsuariosNotificacionViabilidad(instanciaId));
        }

        public IEnumerable<UsuariosProyectoDto> SGR_Transversal_ObtenerInformacionNotificacionInvolucrados(Guid instanciaId, string usuarioFirma)
        {
            return _mapper.Map<IEnumerable<UsuariosProyectoDto>>(Contexto.UspObtenerInformacionNotificacionInvolucrados(instanciaId, usuarioFirma));
        }

        #endregion

        private static IMapper ConfigurarMapper()
        {
            Mapper.Reset();
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<uspGetSGR_Encabezado_LeerEncabezado_Result, EncabezadoSGRDto>();
                cfg.CreateMap<uspGetSGR_TipoDocumentosSoportePorRol_Result, TipoDocumentoSoporteDto>();
                cfg.CreateMap<uspGetConfiguracionReportesPorInstancia_Result, ConfiguracionReportesDto>();
                cfg.CreateMap<UspObtenerInformacionNotificacionInvolucrados_Result, UsuariosProyectoDto>();
                cfg.CreateMap<UspObtenerUsuariosNotificacionViabilidad_Result, UsuariosProyectoDto>();
            }).CreateMapper();
        }
    }
}
