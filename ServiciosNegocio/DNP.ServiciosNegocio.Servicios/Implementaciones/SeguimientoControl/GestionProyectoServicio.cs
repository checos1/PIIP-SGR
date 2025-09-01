namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SeguimientoControl
{
    using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.SeguimientoControl;
    using DNP.ServiciosNegocio.Servicios.Interfaces.SeguimientoControl;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Clase responsable de la estrucutra de sección capitulos de los macroprocesos
    /// </summary>
    public class GestionProyectoServicio : IGestionSeguimientoServicio
    {
        private readonly IGestionSeguimientoPersistencia _seccionCapituloPersistencia;

        /// <summary>
        /// Constructor SeccionCapituloServicio
        /// </summary>
        /// <param name="secccionCapituloPersistencia"></param>
        /// <param name="fasePersistencia"></param>
        public GestionProyectoServicio(IGestionSeguimientoPersistencia secccionCapituloPersistencia)
        {
            _seccionCapituloPersistencia = secccionCapituloPersistencia;
        }

        public Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(GestionSeguimientoDto proyecto)
        {
            var erroresProyecto = _seccionCapituloPersistencia.ObtenerErroresProyecto(proyecto);
            if (erroresProyecto == null) return Task.FromResult<List<ErroresProyectoDto>>(null);
            return Task.FromResult(erroresProyecto);
        }


        public Task<List<TransversalSeguimientoDto>> ObtenerListadoUnidadesMedida()
        {
            var unidadesMedida = _seccionCapituloPersistencia.ObtenerListadoUnidadesMedida();
            if (unidadesMedida == null) return Task.FromResult<List<TransversalSeguimientoDto>>(null);
            return Task.FromResult(unidadesMedida);
        }

    }
}