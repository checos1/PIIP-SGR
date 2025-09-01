namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesProyectos
{
    using DNP.ServiciosNegocio.Dominio.Dto.JustificacionCambios;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using Interfaces;
    using Interfaces.Proyectos;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CambiosRelacionPlanificacionPersistencia : Persistencia, ICambiosRelacionPlanificacionPersistencia
    {
        #region Constructor
        /// <summary>
        /// Constructor de SeccionCapituloPersistencia
        /// </summary>
        /// <param name="contextoFactory"></param>
        public CambiosRelacionPlanificacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion

        /// <summary>
        /// Obtiene los cambios realizados apartir del proyecto en firme - conpes
        /// </summary>
        /// <param name="IdProyecto">Identificación del proyecto</param>
        /// <returns></returns>
        public List<RelacionPlanificacionDto> ObtenerCambiosRelacionPlanificacion(int IdProyecto)
        {
            var resultSp = Contexto.upsGetEstadoProyectoConpes(IdProyecto);
            var result = resultSp.Select(est => new RelacionPlanificacionDto()
            {
                Id = est.ConpesID.Value,
                Estado = est.Estado,
                NombreConpes = est.NombreConpes,
                NumeroConpes = Convert.ToString(est.NumeroConpes)
            }).ToList();

            return result != null && result.Count() > 0 ? result : new List<RelacionPlanificacionDto>();

        }

    }
}
