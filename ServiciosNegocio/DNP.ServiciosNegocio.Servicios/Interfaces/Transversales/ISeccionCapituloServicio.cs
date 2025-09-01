namespace DNP.ServiciosNegocio.Servicios.Interfaces.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISeccionCapituloServicio
    {
        Task<List<SeccionCapituloDto>> ConsultarSeccionCapitulos(string guiMacroproceso, int IdProyecto, string IdInstancia);
        Task<List<SeccionCapituloDto>> ConsultarSeccionCapitulosByMacroproceso(string guiMacroproceso,string NivelID, string FlujoId);
        Task<RespuestaGeneralDto> ValidarSeccionCapitulos(string guidMacroproceso, int IdProyecto, string IdInstancia);
        Task<CapituloModificado> ObtenerCapitulosModificados(string capitulo, string seccion, string guiMacroproceso, int idProyecto, string idInstancia);
        Task<List<ErroresProyectoDto>> ObtenerErroresProyecto(string GuidMacroproceso, int idProyecto, string GuidInstancia);
        Task<List<ErroresProyectoDto>> ObtenerErroresSeguimiento(string GuidMacroproceso, int idProyecto, string GuidInstancia);
        Task<List<ErroresTramiteDto>> ObtenerErroresTramite(string GuidMacroproceso, string GuidInstancia, string AccionId, string usuarioDNP, bool tieneCDP);
        Task<List<ErroresTramiteDto>> ObtenerErroresViabilidad(string GuiMacroproceso, int ProyectoId, string NivelId, string InstanciaId);
        Task<List<SeccionesTramiteDto>> ObtenerSeccionesTramite(string GuidMacroproceso, string GuidInstancia);
        Task<List<SeccionesTramiteDto>> ObtenerSeccionesPorFase(string GuidInstancia, string GuidFaseNivel);
        Task<SeccionesCapitulos> EliminarCapituloModificado(CapituloModificado capituloModificado);
        Task<List<ErroresPreguntasDto>> ObtenerErroresAprobacionRol(string GuiMacroproceso, int idProyecto, string GuidInstancia);
        Task<List<ErroresProyectoDto>> ObtenerErroresProgramacion(string GuidInstancia, string AccionId);
    }
}
