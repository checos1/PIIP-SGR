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

    public class CambiosJustificacionHorizontePersistencia : Persistencia, ICambiosJustificacionHorizontePersistencia
    {
        #region Constructor
        /// <summary>
        /// Constructor de SeccionCapituloPersistencia
        /// </summary>
        /// <param name="contextoFactory"></param>
        public CambiosJustificacionHorizontePersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion

        /// <summary>
        /// Obtiene los cambios realizados apartir del proyecto en firme - conpes
        /// </summary>
        /// <param name="IdProyecto">Identificación del proyecto</param>
        /// <returns></returns>
        public List<JustificaccionHorizonteDto> ObtenerCambiosJustificacionHorizonte(int IdProyecto)
        {
            var resultSp = Contexto.upsGetEstadoProyectoHorizonte(IdProyecto);
            var result = resultSp.Select(est => new JustificaccionHorizonteDto()
            {
                HorizonteId = est.Horizonteid,
                Periodo = est.Periodo,
                Vigencia = est.Vigencia,
                Estado = est.Estado,
                VigenciaFirme = est.VigenciaFirme
            }).ToList();

            return result != null && result.Count() > 0 ? result : new List<JustificaccionHorizonteDto>();

        }

    }
}
