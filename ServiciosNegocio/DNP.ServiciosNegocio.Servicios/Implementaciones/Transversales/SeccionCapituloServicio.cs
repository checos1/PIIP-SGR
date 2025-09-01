namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Transversales
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales;
    using Interfaces.Transversales;

    /// <summary>
    /// Clase responsable de la estrucutra de sección capitulos de los macroprocesos
    /// </summary>
    public class SeccionCapituloServicio : ISeccionCapituloServicio
    {
        private readonly ISeccionCapituloPersistencia _seccionCapituloPersistencia;
        private readonly ICambiosRelacionPlanificacionServicio _relacionPlanificacionServicio;
        private readonly IFasePersistencia _fasePersistencia;

        /// <summary>
        /// Constructor SeccionCapituloServicio
        /// </summary>
        /// <param name="secccionCapituloPersistencia"></param>
        /// <param name="fasePersistencia"></param>
        public SeccionCapituloServicio(ISeccionCapituloPersistencia secccionCapituloPersistencia, 
            IFasePersistencia fasePersistencia,
            ICambiosRelacionPlanificacionServicio relacionPlanificacionServicio)
        {
            _seccionCapituloPersistencia = secccionCapituloPersistencia;
            _fasePersistencia = fasePersistencia;
            _relacionPlanificacionServicio = relacionPlanificacionServicio;
        }

        /// <summary>
        /// Método que obtiene el listado de secciones y capitulos consultando por GUID fase - macroproceso
        /// </summary>
        /// <param name="guidMacroproceso">GUID de la tabla [Transversal].[fase]</param>
        ///<param name="IdProyecto">Id proyecto</param>
        ///<param name="IdInstancia">Id Instancia</param>
        /// <returns></returns>
        public Task<List<SeccionCapituloDto>> ConsultarSeccionCapitulos(string guidMacroproceso, int IdProyecto, string IdInstancia)
        {
            var infoMacroproceso = _fasePersistencia.ObtenerFaseByGuid(guidMacroproceso);
            var guidInstancia = Guid.Parse(IdInstancia);
            var seccionCapitulos = _seccionCapituloPersistencia.ObtenerListaCapitulosModificadosByMacroproceso(infoMacroproceso.Id, IdProyecto, guidInstancia);
            if (seccionCapitulos.Count == 0) return Task.FromResult<List<SeccionCapituloDto>>(null);
            return Task.FromResult(seccionCapitulos);
        }

        /// <summary>
        /// Método que obtiene el listado de secciones y capitulos consultando por GUID fase - macroproceso
        /// </summary>
        /// <param name="guidMacroproceso">GUID de la tabla [Transversal].[fase]</param>
        /// <returns></returns>
        public Task<List<SeccionCapituloDto>> ConsultarSeccionCapitulosByMacroproceso(string guidMacroproceso,string NivelID, string FlujoId)
        {
            var infoMacroproceso = _fasePersistencia.ObtenerFaseByGuid(guidMacroproceso);
            Guid guidNivelId = Guid.Parse(NivelID);
            Guid guidFlujoID = Guid.Parse(FlujoId);
            var seccionCapitulos = _seccionCapituloPersistencia.ObtenerListaCapitulosByMacroproceso(infoMacroproceso.Id, guidNivelId, guidFlujoID);
            if (seccionCapitulos.Count == 0) return Task.FromResult<List<SeccionCapituloDto>>(null);
            return Task.FromResult(seccionCapitulos);
        }

        public Task<RespuestaGeneralDto> ValidarSeccionCapitulos(string guidMacroproceso, int IdProyecto, string IdInstancia)
        {
            var infoMacroproceso = _fasePersistencia.ObtenerFaseByGuid(guidMacroproceso);
            var guidInstancia = Guid.Parse(IdInstancia);
            return Task.FromResult(_seccionCapituloPersistencia.ValidarSeccionCapitulos(infoMacroproceso.Id, IdProyecto, guidInstancia));
        }

        public Task<CapituloModificado> ObtenerCapitulosModificados(string capitulo, string seccion, string guiMacroproceso, int idProyecto, string idInstancia)
        {
            var guidInstancia = Guid.Parse(idInstancia);
            var capitulosModificado = _seccionCapituloPersistencia.ObtenerCapitulosModificados(capitulo, seccion, guiMacroproceso, idProyecto, guidInstancia);
            
            if (capitulosModificado == null) return Task.FromResult<CapituloModificado>(null);
            return Task.FromResult(capitulosModificado);
        }

        public Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(string GuidMacroproceso, int idProyecto, string GuidInstancia)
        {
            var guidInstancia = Guid.Parse(GuidInstancia);
            var guidMacroproceso = Guid.Parse(GuidMacroproceso);

            var erroresProyecto = _seccionCapituloPersistencia.ObtenerErroresProyecto(guidMacroproceso, idProyecto, guidInstancia);
            if (erroresProyecto == null) return Task.FromResult<List<ErroresProyectoDto>>(null);
            return Task.FromResult(erroresProyecto);
        }

        public Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(string GuidMacroproceso, int idProyecto, string GuidInstancia)
        {
            var guidInstancia = Guid.Parse(GuidInstancia);
            var guidMacroproceso = Guid.Parse(GuidMacroproceso);

            var erroresProyecto = _seccionCapituloPersistencia.ObtenerErroresSeguimiento(guidMacroproceso, idProyecto, guidInstancia);
            if (erroresProyecto == null) return Task.FromResult<List<ErroresProyectoDto>>(null);
            return Task.FromResult(erroresProyecto);
        }

        public Task<List<ErroresTramiteDto>> ObtenerErroresTramite(string GuidMacroproceso, string GuidInstancia, string AccionId, string usuarioDNP, bool tieneCDP)
        {
            var guidInstancia = Guid.Parse(GuidInstancia);
            var guidMacroproceso = Guid.Parse(GuidMacroproceso);
            var accionID = Guid.Parse(AccionId);

            var erroresTramite = _seccionCapituloPersistencia.ObtenerErroresTramite(guidMacroproceso, guidInstancia, accionID, usuarioDNP, tieneCDP);
            if (erroresTramite == null) return Task.FromResult<List<ErroresTramiteDto>>(null);
            return Task.FromResult(erroresTramite);
        }

        public Task<List<ErroresTramiteDto>> ObtenerErroresViabilidad(string GuiMacroproceso, int ProyectoId, string NivelId, string InstanciaId)
        {
            var guidMacroproceso = Guid.Parse(GuiMacroproceso);
            var nivelId = Guid.Parse(NivelId);
            var instanciaId = Guid.Parse(InstanciaId);

            var erroresTramite = _seccionCapituloPersistencia.ObtenerErroresViabilidad(guidMacroproceso, ProyectoId, nivelId, instanciaId);
            if (erroresTramite == null) return Task.FromResult<List<ErroresTramiteDto>>(null);
            return Task.FromResult(erroresTramite);
        }

        public Task<List<SeccionesTramiteDto>> ObtenerSeccionesTramite(string GuidMacroproceso, string GuidInstancia)
        {
            var guidMacroproceso = Guid.Parse(GuidMacroproceso);
            var guidInstancia = Guid.Parse(GuidInstancia);

            var seccionesTramite = _seccionCapituloPersistencia.ObtenerSeccionesTramite(guidMacroproceso, guidInstancia);
            if (seccionesTramite == null) return Task.FromResult<List<SeccionesTramiteDto>>(null);
            return Task.FromResult(seccionesTramite);
        }

        public Task<List<SeccionesTramiteDto>> ObtenerSeccionesPorFase(string GuidInstancia, string GuidFaseNivel)
        {
            var guidInstancia = Guid.Parse(GuidInstancia);
            var guidFaseNivel = Guid.Parse(GuidFaseNivel);

            var seccionesTramite = _seccionCapituloPersistencia.ObtenerSeccionesPorFase(guidInstancia, guidFaseNivel);
            if (seccionesTramite == null) return Task.FromResult<List<SeccionesTramiteDto>>(null);
            return Task.FromResult(seccionesTramite);
        }
        public Task<SeccionesCapitulos> EliminarCapituloModificado(CapituloModificado capituloModificado)
        {
            return Task.FromResult(_seccionCapituloPersistencia.EliminarCapituloModificado(capituloModificado));
        }

        public Task<List<ErroresPreguntasDto>> ObtenerErroresAprobacionRol(string GuiMacroproceso, int idProyecto, string GuidInstancia)
        {
            var guidInstancia = Guid.Parse(GuidInstancia);
            var guidMacroproceso = Guid.Parse(GuiMacroproceso);

            var erroresPreguntas = _seccionCapituloPersistencia.ObtenerErroresAprobacionRol(guidMacroproceso, idProyecto, guidInstancia);
            if (erroresPreguntas == null) return Task.FromResult<List<ErroresPreguntasDto>>(null);
            return Task.FromResult(erroresPreguntas);
        }

        public Task<List<ErroresProyectoDto>> ObtenerErroresProgramacion(string GuidInstancia, string AccionId)
        {
            var guidInstancia = Guid.Parse(GuidInstancia);
            var accionID = Guid.Parse(AccionId);

            var erroresTramite = _seccionCapituloPersistencia.ObtenerErroresProgramacion(guidInstancia, accionID);
            if (erroresTramite == null) return Task.FromResult<List<ErroresProyectoDto>>(null);
            return Task.FromResult(erroresTramite);
        }

    }
}