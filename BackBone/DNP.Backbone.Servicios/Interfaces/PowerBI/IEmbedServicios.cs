namespace DNP.Backbone.Servicios.Interfaces.PowerBI
{
    using DNP.Backbone.Comunes.Dto.PowerBI;
    using DNP.Backbone.Dominio.Dto.PowerBI;
    using Microsoft.PowerBI.Api.Models;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEmbedServicios
    {
        Task<EmbedConfig>  ObtenerReportes(EmbedFiltroDto EmbedFiltroDto);
        Task<EmbedConfig> ObtenerDashboard(EmbedFiltroDto EmbedFiltroDto);
        Task<ParametrosReportesInfoDto> ObtenerListaReportes(EmbedFiltroDto embedFiltroDto);
        Task<ParametrosDashBoardsInfoDto> ObtenerListaDashboard(EmbedFiltroDto embedFiltroDto);
        Task<string> ObtenerMetiaType(int format);
        Task<ByteArrayContent> ExportPowerBIReport(
          Guid reportId,
          int format,
          int pollingtimeOutInMinutes,
          CancellationToken token,
          IList<string> pageNames = null);
    }
}
