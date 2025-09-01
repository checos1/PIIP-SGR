namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Consola;
    using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ConsolaTramiteServiciosMock : IConsolaTramiteServicios
    {
        public Task<List<DocumentoDto>> ObtenerDocumentos(string idInstacia, string idUsuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<Guid?> ObtenerIdAplicacionPorInstancia(Guid idInstancia, string usuarioDnp)
        {
            return Task.FromResult((Guid?)Guid.NewGuid());
        }

        public Task<ProyectosTramitesDTO> ObtenerProyectosTramite(InstanciaTramiteDto instanciaTramiteDto)
        {
            return instanciaTramiteDto.ParametrosInboxDto?.IdUsuario == "jdelgado" ? Task.FromResult(new ProyectosTramitesDTO()) :
                Task.FromResult<ProyectosTramitesDTO>(null);
        }

        Task<ConsolaTramiteDto> IConsolaTramiteServicios.ObtenerConsolaTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            return instanciaTramiteDto.ParametrosInboxDto?.IdUsuario == "jdelgado" ? Task.FromResult(new ConsolaTramiteDto()) :
                Task.FromResult<ConsolaTramiteDto>(null);
        }

        public Task<ConsolaTramiteDto> ObtenerConsolaTramites(InstanciaTramiteDto instanciaTramiteDto)
        {
            return Task.FromResult(new ConsolaTramiteDto());
        }

        public Task<List<MacroProcesosCantidadDto>> ObtenerMacroProcesosCantidad(string usuarioDnp)
        {
            return Task.FromResult(new List<MacroProcesosCantidadDto>());
        }

        public Task<List<MacroProcesosDto>> ObtenerMacroProcesos(string usuarioDnp)
        {
            return Task.FromResult(new List<MacroProcesosDto>());
        }

        public Task<List<MacroProcesosDto>> ObtenerProcesos(string usuarioDnp)
        {
            return Task.FromResult(new List<MacroProcesosDto>());
        }
    }
}
