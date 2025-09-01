
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using System.Collections.Generic;
using System;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Reportes;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Transversales
{
    public interface ITransversalServicio
    {
        string SGR_Transversal_LeerParametro(string parametro);
        string SGR_Proyectos_LeerListaParametros();

        /// <summary>
        /// Leer información del encabezado de un proyecto
        /// </summary>
        /// <param name="proyectoId">Id del proyecto</param>
        /// <returns>Información del proyecto</returns>
        EncabezadoSGRDto uspGetSGR_Encabezado_LeerEncabezado(ParametrosEncabezadoSGR parametros);

        /// <summary>
        /// Leer documentos soporte
        /// </summary>
        IEnumerable<TipoDocumentoSoporteDto> SGR_Transversal_ObtenerTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId);

        /// <summary>
        /// Leer documentos soporte para despliegue
        /// </summary>
        IEnumerable<TipoDocumentoSoporteDto> SGR_Transversal_ObtenerListaTipoDocumentoSoporte(int tipoTramiteId, string roles, int? tramiteId, Guid nivelId, Guid instanciaId, Guid accionId);

        /// <summary>
        /// Leer la configuración de los reportes
        /// </summary>
        ConfiguracionReportesDto SGR_Transversal_ObtenerConfiguracionReportes(Guid instanciaId);

        // <summary>
        /// Se da permiso a los usuarios del subflujo ocadpaz
        /// </summary>
        Nullable<bool> AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(Guid instanciaId, Guid RolId, string usuario);

        /// <summary>
        /// Obtiene los mensajes de validación de OCAD Paz
        /// </summary>
        /// 
        IEnumerable<ValidacionOCADPazDto> SGR_Transversal_ValidacionOCADPaz(string proyectoId, Guid nivelId, Guid instanciaId, Guid flujoId);
        IEnumerable<UsuariosProyectoDto> SGR_Transversal_ObtenerUsuariosNotificacionViabilidad(Guid instanciaId);
        IEnumerable<UsuariosProyectoDto> SGR_Transversal_ObtenerInformacionNotificacionInvolucrados(Guid instanciaId, string usuarioFirma);
    }
}
