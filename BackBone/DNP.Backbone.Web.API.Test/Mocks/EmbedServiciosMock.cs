namespace DNP.Backbone.Web.API.Test.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Dto.PowerBI;
    using DNP.Backbone.Dominio.Dto.PowerBI;
    using DNP.Backbone.Servicios.Interfaces.PowerBI;

    public class EmbedServiciosMock : IEmbedServicios
    {
        public Task<ByteArrayContent> ExportPowerBIReport(Guid reportId, int format, int pollingtimeOutInMinutes, CancellationToken token, IList<string> pageNames = null)
        {
            return Task.FromResult(new ByteArrayContent(new List<byte>().ToArray()));
        }

        public Task<EmbedConfig> ObtenerDashboard(EmbedFiltroDto EmbedFiltroDto)
        {
            return Task.FromResult(new EmbedConfig());
        }

        public Task<ParametrosDashBoardsInfoDto> ObtenerListaDashboard(EmbedFiltroDto embedFiltroDto)
        {
            return Task.FromResult(new ParametrosDashBoardsInfoDto());
        }

        public Task<ParametrosReportesInfoDto> ObtenerListaReportes(EmbedFiltroDto embedFiltroDto)
        {
            return Task.FromResult(new ParametrosReportesInfoDto());
        }

        public Task<string> ObtenerMetiaType(int format)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<EmbedConfig> ObtenerReportes(EmbedFiltroDto EmbedFiltroDto)
        {
            return Task.FromResult(new EmbedConfig());
        }
    }
}
