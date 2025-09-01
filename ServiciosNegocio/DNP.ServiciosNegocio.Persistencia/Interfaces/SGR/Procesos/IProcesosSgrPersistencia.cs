using DNP.ServiciosNegocio.Dominio.Dto.DesignacionEjecutor;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.GestionRecursos.Aprobacion;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Procesos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Procesos
{
    public interface IProcesosSgrPersistencia
    {
        IEnumerable<PriorizacionProyectoDto> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId);
        IEnumerable<AprobacionProyectoDto> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId);
        IEnumerable<ProyectoAprobacionInstanciasDto> ObtenerProyectoAprobacionInstanciasSGR(Nullable<Guid> instanciaId);
        ProyectoAprobacionInstanciasResultado GuardarProyectoAprobacionInstanciasSGR(ProyectoAprobacionInstanciasDto proyectoAprobacionInstanciasDto, string usuario);
        AprobacionProyectoCreditoDto ObtenerAprobacionProyectoCredito(Guid instancia, int entidad);
        ResultadoProcedimientoDto GuardarAprobacionProyectoCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto, string usuario);
        ProyectoProcesoResultado GuardarProyectoPermisosProcesoSGR(ProyectoProcesoDto proyectoProcesoDto, string usuario);
        IEnumerable<ProyectoPriorizacionDetalleDto> ObtenerPriorizionProyectoDetalleSGR(Nullable<Guid> instanciaId);
        ProyectoPriorizacionDetalleResultado GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario);
        string SGR_Proyectos_MostrarEstadosPriorizacion(int proyectoId);
        string SGR_Proyectos_MostrarEstadosAprobacion(int proyectoId);
        List<ProyectoResumenEstadoAprobacionCreditoDto> SGR_Proyectos_ResumenEstadoAprobacionCredito(int proyectoId);

        string SGR_Proyectos_LeerValoresAprobacion(int proyectoId);
        string SGR_Proyectos_MostrarEstadosAprobacionCreditoParcial(int proyectoId);
        List<EjecutorEntidadAsociado> SGR_Procesos_ConsultarEjecutorbyTipo(int proyectoId, int tipoEjecutorId);

        #region Designación Ejecutor

        /// <summary>
        /// Registrar valor de una columna dinamica del ejecutor por proyectoId.
        /// </summary>     
        /// <param name="valores"></param> 
        /// <param name="usuario"></param>
        /// <returns>bool</returns>   
        bool RegistrarRespuestaEjecutorSGR(RespuestaDesignacionEjecutorDto valores, string usuario);

        /// <summary>
        /// Obtener el valor de una columna dinámica del ejecutor por proyectoId.
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyectoId"></param>
        /// <returns>string</returns>
        string ObtenerRespuestaEjecutorSGR(string campo, int proyectoId);

        /// <summary>
        /// Registrar valor de dinamico aprobación valores.
        /// </summary>     
        /// <param name="valores"></param>        
        /// <param name="usuario"></param>      
        /// <returns>bool</returns>
        bool ActualizarValorEjecutorSGR(CampoItemValorDto valores, string usuario);


        /// <summary>
        /// Obtener valor de costos de estructuracion viabilidad.
        /// </summary>
        /// <param name="instanciaId"></param>     
        /// <returns>string</returns>
        string ObtenerValorCostosEstructuracionViabilidadSGR(Guid instanciaId);

        #endregion Designación Ejecutor
    }
}
