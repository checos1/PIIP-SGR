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
    public class CambiosJustificacionHorizonteServicio : ICambiosJustificacionHorizonServicio
    {
        private readonly ICambiosJustificacionHorizontePersistencia _justificacionHorizontePersistencia;
        private readonly ISeccionCapituloPersistencia _seccionCapituloPersistencia;

        public CambiosJustificacionHorizonteServicio(ICambiosJustificacionHorizontePersistencia justificacionHorizontePersistencia,
        ISeccionCapituloPersistencia seccionCapituloPersistencia)
        {
            _justificacionHorizontePersistencia = justificacionHorizontePersistencia;
            _seccionCapituloPersistencia = seccionCapituloPersistencia;
        }

        /// <summary>
        /// Guarda la justificación de horizonte
        /// </summary>
        /// <param name="capituloModificado">Objeto con de capitulo modificado</param>
        /// <param name="usuario">Nombre de usuario</param>
        /// <returns></returns>
        public Task<bool> GuardarJustificacionCambios(CapituloModificado capituloModificado, string usuario)
        {
            capituloModificado.Usuario = usuario;
            return Task.FromResult(_seccionCapituloPersistencia.GuardarJustificacionCambios(capituloModificado));
        }

		public Task<List<JustificaccionHorizonteDto>> ObtenerCambiosJustificacionHorizonte(int IdProyecto)
		{
            var seccionCapitulos = _justificacionHorizontePersistencia.ObtenerCambiosJustificacionHorizonte(IdProyecto);
            if (seccionCapitulos.Count == 0) return Task.FromResult<List<JustificaccionHorizonteDto>>(null);
            return Task.FromResult(seccionCapitulos);
        }
	}
}