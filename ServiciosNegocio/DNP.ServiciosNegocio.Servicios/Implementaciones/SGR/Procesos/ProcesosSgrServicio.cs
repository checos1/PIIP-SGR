using DNP.ServiciosNegocio.Dominio.Dto.DesignacionEjecutor;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.GestionRecursos.Aprobacion;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Procesos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Procesos;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Procesos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGR.Procesos
{
    public class ProcesosSgrServicio : IProcesosSgrServicio
    {

        private readonly IProcesosSgrPersistencia _procesosSgrPersistencia;

        public Task<IEnumerable<PriorizacionProyectoDto>> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId)
        {
            var priorizacionProyecto = _procesosSgrPersistencia.ObtenerPriorizacionProyecto(instanciaId);

            return Task.FromResult(priorizacionProyecto);
        }


        public Task<IEnumerable<AprobacionProyectoDto>> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId)
        {
            var aprobacionProyecto = _procesosSgrPersistencia.ObtenerAprobacionProyecto(instanciaId);

            return Task.FromResult(aprobacionProyecto);
        }

        public ProcesosSgrServicio(IProcesosSgrPersistencia aprobacionSgrPersistencia)
        {
            _procesosSgrPersistencia = aprobacionSgrPersistencia;
        }
        public ProyectoAprobacionInstanciasResultado GuardarProyectoAprobacionInstanciasSGR(ProyectoAprobacionInstanciasDto proyectoAprobacionInstanciasDto, string usuario)
        {
            return _procesosSgrPersistencia.GuardarProyectoAprobacionInstanciasSGR(proyectoAprobacionInstanciasDto, usuario);
        }

        public AprobacionProyectoCreditoDto ObtenerAprobacionProyectoCredito(Guid instancia, int entidad)
        {
            return _procesosSgrPersistencia.ObtenerAprobacionProyectoCredito(instancia, entidad);
        }

        public ResultadoProcedimientoDto GuardarAprobacionProyectoCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto, string usuario)
        {
            return _procesosSgrPersistencia.GuardarAprobacionProyectoCredito(aprobacionProyectoCreditoDto, usuario);
        }

        public IEnumerable<ProyectoAprobacionInstanciasDto> ObtenerProyectoAprobacionInstanciasSGR(Guid? instanciaId)
        {
            return _procesosSgrPersistencia.ObtenerProyectoAprobacionInstanciasSGR(instanciaId);
        }

        public ProyectoProcesoResultado GuardarProyectoPermisosProcesoSGR(ProyectoProcesoDto proyectoProcesoDto, string usuario)
        {
            return _procesosSgrPersistencia.GuardarProyectoPermisosProcesoSGR(proyectoProcesoDto, usuario);
        }

        public IEnumerable<ProyectoPriorizacionDetalleDto> ObtenerPriorizionProyectoDetalleSGR(Nullable<Guid> instanciaId)
        {
            return _procesosSgrPersistencia.ObtenerPriorizionProyectoDetalleSGR(instanciaId);
        }

        public ProyectoPriorizacionDetalleResultado GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario)
        {
            return _procesosSgrPersistencia.GuardarPriorizionProyectoDetalleSGR(proyectoPriorizacionDetalleDto, usuario);
        }

        public string SGR_Proyectos_MostrarEstadosPriorizacion(int proyectoId)
        {
            return _procesosSgrPersistencia.SGR_Proyectos_MostrarEstadosPriorizacion(proyectoId);
        }
        public string SGR_Proyectos_MostrarEstadosAprobacion(int proyectoId)
        {
            return _procesosSgrPersistencia.SGR_Proyectos_MostrarEstadosAprobacion(proyectoId);            
        }
        public Task<List<ProyectoResumenEstadoAprobacionCreditoDto>> SGR_Proyectos_ResumenEstadoAprobacionCredito(int proyectoId)
        {
            var list = _procesosSgrPersistencia.SGR_Proyectos_ResumenEstadoAprobacionCredito(proyectoId);
            if (list.Count == 0) return Task.FromResult<List<ProyectoResumenEstadoAprobacionCreditoDto>>(null);
            return Task.FromResult(list);
        }

        public string SGR_Proyectos_LeerValoresAprobacion(int proyectoId)
        {
            return _procesosSgrPersistencia.SGR_Proyectos_LeerValoresAprobacion(proyectoId);
        }
        public string SGR_Proyectos_MostrarEstadosAprobacionCreditoParcial(int proyectoId)
        {
            return _procesosSgrPersistencia.SGR_Proyectos_MostrarEstadosAprobacionCreditoParcial(proyectoId);
        }

        public Task<List<EjecutorEntidadAsociado>> SGR_Procesos_ConsultarEjecutorbyTipo(int proyectoId, int tipoEjecutorId)
        {
            var ejecutorList = _procesosSgrPersistencia.SGR_Procesos_ConsultarEjecutorbyTipo(proyectoId, tipoEjecutorId);

            if (ejecutorList.Count == 0) return Task.FromResult<List<EjecutorEntidadAsociado>>(null);
            return Task.FromResult(ejecutorList);
        }

        #region Designación Ejecutor

        /// <summary>
        /// Registrar valor de una columna dinamica del ejecutor por proyectoId.
        /// </summary>     
        /// <param name="valores"></param> 
        /// <param name="usuario"></param>
        /// <returns>bool</returns> 
        public bool RegistrarRespuestaEjecutorSGR(RespuestaDesignacionEjecutorDto valores, string usuario)
        {
            return _procesosSgrPersistencia.RegistrarRespuestaEjecutorSGR(valores, usuario);
        }

        /// <summary>
        /// Obtener el valor de una columna dinámica del ejecutor por proyectoId.
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyectoId"></param>
        /// <returns>string</returns>
        public string ObtenerRespuestaEjecutorSGR(string campo, int proyectoId)
        {
            return _procesosSgrPersistencia.ObtenerRespuestaEjecutorSGR(campo, proyectoId);
        }

        /// <summary>
        /// Registrar valor de dinamico aprobación valores.
        /// </summary>     
        /// <param name="valores"></param> 
        /// <param name="usuario"></param>
        /// <returns>bool</returns>
        public bool ActualizarValorEjecutorSGR(CampoItemValorDto valores, string usuario)
        {
            return _procesosSgrPersistencia.ActualizarValorEjecutorSGR(valores, usuario);
        }

        /// <summary>
        /// Obtener valor de costos de estructuracion viabilidad.
        /// </summary>
        /// <param name="instanciaId"></param>     
        /// <returns>string</returns>
        public string ObtenerValorCostosEstructuracionViabilidadSGR(Guid instanciaId)
        {
            return _procesosSgrPersistencia.ObtenerValorCostosEstructuracionViabilidadSGR(instanciaId);
        }

        #endregion Designación Ejecutor
    }
}
