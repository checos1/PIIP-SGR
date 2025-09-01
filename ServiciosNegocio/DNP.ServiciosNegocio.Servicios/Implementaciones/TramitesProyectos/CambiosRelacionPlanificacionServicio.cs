namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Dominio.Dto.JustificacionCambios;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesProyectos;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Interfaces.Transversales;

    /// <summary>
    /// Clase responsable de la estrucutra de sección capitulos de los macroprocesos
    /// </summary>
    public class CambiosRelacionPlanificacionServicio : ICambiosRelacionPlanificacionServicio
    {
        private readonly ICambiosRelacionPlanificacionPersistencia _relacionPlanificacionPersistencia;
        private readonly ISeccionCapituloPersistencia _seccionCapituloPersistencia;

        /// <summary>
        /// Constructor SeccionCapituloServicio
        /// </summary>
        /// <param name="secccionCapituloPersistencia"></param>
        /// <param name="fasePersistencia"></param>
        public CambiosRelacionPlanificacionServicio(ICambiosRelacionPlanificacionPersistencia relacionPlanificacionPersistencia,
            ISeccionCapituloPersistencia seccionCapituloPersistencia)
        {
            _relacionPlanificacionPersistencia = relacionPlanificacionPersistencia;
            _seccionCapituloPersistencia = seccionCapituloPersistencia;
        }

        public Task<List<RelacionPlanificacionDto>> ConsultarCambiosConpes(int IdProyecto)
        {
            var seccionCapitulos = _relacionPlanificacionPersistencia.ObtenerCambiosRelacionPlanificacion(IdProyecto);
            if (seccionCapitulos.Count == 0) return Task.FromResult<List<RelacionPlanificacionDto>>(null);
            return Task.FromResult(seccionCapitulos);
        }

        /// <summary>
        /// Guarda la justificación de cambio - conpes
        /// </summary>
        /// <param name="capituloModificado">Objeto con de capitulo modificado</param>
        /// <param name="usuario">Nombre de usuario</param>
        /// <returns></returns>
        public Task<bool> GuardarJustificacionCambios(CapituloModificado capituloModificado, string usuario)
        {
            capituloModificado.Usuario = usuario;
            return Task.FromResult(_seccionCapituloPersistencia.GuardarJustificacionCambios(capituloModificado));
        }

        public Task<bool> FocalizacionActualizaPoliticasModificadas(JustificacionPoliticaModificada capituloModificado, string usuario)
        {
            capituloModificado.Usuario = usuario;
            return Task.FromResult(_seccionCapituloPersistencia.FocalizacionActualizaPoliticasModificadas(capituloModificado));
        }
    }
}