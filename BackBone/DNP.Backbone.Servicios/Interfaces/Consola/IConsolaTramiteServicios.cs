namespace DNP.Backbone.Servicios.Interfaces.Consola
{
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Consola;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IConsolaTramiteServicios
    {
        Task<ConsolaTramiteDto> ObtenerConsolaTramites(InstanciaTramiteDto instanciaTramiteDto);
        Task<ProyectosTramitesDTO> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto);
        Task<List<DocumentoDto>> ObtenerDocumentos(string idInstacia, string idUsuarioDNP);
        Task<Guid?> ObtenerIdAplicacionPorInstancia(Guid idInstancia, string usuarioDnp);
        Task<List<MacroProcesosCantidadDto>> ObtenerMacroProcesosCantidad(string usuarioDnp);
        Task<List<MacroProcesosDto>> ObtenerMacroProcesos(string usuarioDnp);
        Task<List<MacroProcesosDto>> ObtenerProcesos(string usuarioDnp);
    }
}
