using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Reportes;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.Transversales 
{
    public class TransversalServicio : ITransversalServicio
    {
        #region Constructor

        private readonly ITransversalPersistencia _objetoPersistencia;

        public TransversalServicio(ITransversalPersistencia TransversalSGRPersistencia)
        {
            _objetoPersistencia = TransversalSGRPersistencia;
        }



        #endregion

        #region "Métodos"

        /// <summary>
        /// Leer Parámetro Transversal.Parametros
        /// </summary>
        public string SGR_Transversal_LeerParametro(string parametro)
        {
            return _objetoPersistencia.SGR_Transversal_LeerParametro(parametro);
        }

        public string SGR_Proyectos_LeerListaParametros()
        {
            return _objetoPersistencia.SGR_Proyectos_LeerListaParametros();
        }

        /// <summary>
        /// Leer información del encabezado de un proyecto
        /// </summary>
        /// <param name="proyectoId">Id del proyecto</param>
        /// <returns>Información del proyecto</returns>
        public EncabezadoSGRDto uspGetSGR_Encabezado_LeerEncabezado(ParametrosEncabezadoSGR parametros)
        {
            return _objetoPersistencia.uspGetSGR_Encabezado_LeerEncabezado(parametros);
        }

        /// <summary>
        /// Leer documentos soporte
        /// </summary>
        public IEnumerable<TipoDocumentoSoporteDto> SGR_Transversal_ObtenerTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            return _objetoPersistencia.SGR_Transversal_ObtenerTipoDocumentoSoporte(tipoTramiteId, roles, tramiteId, nivelId, instanciaId, accionId);
        }

        /// <summary>
        /// Leer documentos soporte para despliegue
        /// </summary>
        public IEnumerable<TipoDocumentoSoporteDto> SGR_Transversal_ObtenerListaTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId)
        {
            return _objetoPersistencia.SGR_Transversal_ObtenerListaTipoDocumentoSoporte(tipoTramiteId, roles, tramiteId, nivelId, instanciaId, accionId);
        }

        /// <summary>
        /// Leer la configuración de los reportes
        /// </summary>
        public ConfiguracionReportesDto SGR_Transversal_ObtenerConfiguracionReportes(Guid instanciaId)
        {
            return _objetoPersistencia.SGR_Transversal_ObtenerConfiguracionReportes(instanciaId);
        }

        /// <summary>
        /// Se da permiso a los usuarios del subflujo ocadpaz
        /// </summary>
        /// 
        public Nullable<bool> AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(Guid instanciaId, Guid RolId, string usuario)
        {
            return _objetoPersistencia.AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(instanciaId, RolId, usuario);
        }

        /// <summary>
        /// Obtiene los mensajes de validación de OCAD Paz
        /// </summary>
        /// 
        public IEnumerable<ValidacionOCADPazDto> SGR_Transversal_ValidacionOCADPaz(string proyectoId, Guid nivelId, Guid instanciaId, Guid flujoId)
        {
            return _objetoPersistencia.SGR_Transversal_ValidacionOCADPaz(proyectoId, nivelId, instanciaId, flujoId);
        }

        public IEnumerable<UsuariosProyectoDto> SGR_Transversal_ObtenerUsuariosNotificacionViabilidad(Guid instanciaId)
        {
            return _objetoPersistencia.SGR_Transversal_ObtenerUsuariosNotificacionViabilidad(instanciaId);
        }

        public IEnumerable<UsuariosProyectoDto> SGR_Transversal_ObtenerInformacionNotificacionInvolucrados(Guid instanciaId, string usuarioFirma)
        {
            return _objetoPersistencia.SGR_Transversal_ObtenerInformacionNotificacionInvolucrados(instanciaId, usuarioFirma);
        }

        #endregion
    }
}
