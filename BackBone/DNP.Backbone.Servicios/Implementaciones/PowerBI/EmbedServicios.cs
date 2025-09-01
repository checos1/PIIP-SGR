namespace DNP.Backbone.Servicios.Implementaciones.PowerBI
{
    using DNP.Backbone.Servicios.Interfaces.PowerBI;
    using DNP.Backbone.Dominio.Dto.PowerBI;
    using System.Threading.Tasks;
    using Microsoft.PowerBI.Api;
    using System;
    using Microsoft.PowerBI.Api.Models;

    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Rest;
    using System.Net;
    using DNP.Backbone.Comunes.Dto.PowerBI;
    using DNP.Backbone.Comunes;
    using System.Threading;
    using System.IO;
    using System.Net.Http;
    using DNP.Backbone.Comunes.Enums;

    public class EmbedServicios : EmbedConfigServicios, IEmbedServicios
    {
        public EmbedServicios()
            : base()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Devuelve el espacio de trabajo por filtro o configuración
        /// </summary>
        /// <param name="worksp">filtro</param>
        /// <returns>workspace ID</returns>
        private Guid volverIdentificacionWorkspace(string worksp) => Guid.TryParse(worksp, out Guid guidOutput) ? Guid.Parse(worksp) : base.m_embedConfig.WorkspaceId;

        /// <summary>
        /// Obtener lista de informes por espacio de trabajo power bi
        /// </summary>
        /// <param name="embedFiltroDto">filtrar de servicios</param>
        /// <returns>lista de informes</returns>
        public async Task<ParametrosReportesInfoDto> ObtenerListaReportes(EmbedFiltroDto embedFiltroDto)
        {
            var retorno = new ParametrosReportesInfoDto();
            try
            {
                if (!await base.GetTokenCredentials(TipoPowerBIEnum.Reporte))
                {
                    retorno.Mensaje = base.m_embedConfig.ErrorMessage;
                    return await Task.Run(() => retorno);
                }

                var workspaceId = volverIdentificacionWorkspace(embedFiltroDto.WorkspaceId);
                using (var client = new PowerBIClient(new Uri(ApiUrl), m_tokenCredentials))
                {

                    var reports = await client.Reports.GetReportsInGroupAsync(workspaceId);
                    if (!reports.Value.Any())
                    {
                        retorno.Mensaje = BackboneRecursos.PowerBI_NoTienesReportes;
                        return await Task.Run(() => retorno);
                    }
                    retorno.Reportes = (from powerbi in reports.Value
                                        select new ReportesInfoDto()
                                        {
                                            Id = powerbi.Id.ToString(),
                                            Name = powerbi.Name,
                                            EmbedUrl = powerbi.EmbedUrl,
                                            DatasetId = powerbi.DatasetId,
                                            WebUrl = powerbi.WebUrl
                                        }).ToList();

                    return await Task.Run(() => retorno);
                }
            }
            catch (HttpOperationException exc)
            {
                retorno.Mensaje = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
                return await Task.Run(() => retorno);
            }
        }

        /// <summary>
        /// Obtener lista de dashboards por espacio de trabajo power bi
        /// </summary>
        /// <param name="embedFiltroDto">filtrar de servicios</param>
        /// <returns>lista de dashboards</returns>
        public async Task<ParametrosDashBoardsInfoDto> ObtenerListaDashboard(EmbedFiltroDto embedFiltroDto)
        {
            var retorno = new ParametrosDashBoardsInfoDto();
            try
            {
                if (!await base.GetTokenCredentials(TipoPowerBIEnum.Dashboard))
                {
                    retorno.Mensaje = base.m_embedConfig.ErrorMessage;
                    return await Task.Run(() => retorno);
                }

                var workspaceId = volverIdentificacionWorkspace(embedFiltroDto.WorkspaceId);
                using (var client = new PowerBIClient(new Uri(ApiUrl), m_tokenCredentials))
                {
                    var dashboards = await client.Dashboards.GetDashboardsInGroupAsync(workspaceId);
                    if (!dashboards.Value.Any())
                    {
                        retorno.Mensaje = BackboneRecursos.PowerBI_NoTienesDashboards;
                        return await Task.Run(() => retorno);
                    }
                    retorno.Dashboards = (from powerbi in dashboards.Value
                                          select new DashboadrsInfoDto()
                                          {
                                              Id = powerbi.Id.ToString(),
                                              DisplayName = powerbi.DisplayName,
                                              EmbedUrl = powerbi.EmbedUrl,
                                              IsReadOnly = powerbi.IsReadOnly,
                                              Tiles = (from t in powerbi.Tiles
                                                       select new DashboardsTileInfoDto()
                                                       {
                                                           Id = t.Id.ToString(),
                                                           Title = t.Title,
                                                           ReportId = t.ReportId.ToString(),
                                                           EmbedData = t.EmbedData,
                                                           DatasetId = t.DatasetId,
                                                           ColSpan = t.ColSpan,
                                                           EmbedUrl = t.EmbedUrl,
                                                           RowSpan = t.RowSpan
                                                       }).ToList()
                                          }).ToList();

                    return await Task.Run(() => retorno);
                }
            }
            catch (HttpOperationException exc)
            {
                retorno.Mensaje = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
                return await Task.Run(() => retorno);
            }
        }

        /// <summary>
        /// Realiza autenticación y consulta en la api power bi para obtener el dashboards
        /// </summary>
        /// <param name="embedFiltroDto">filtrar de servicios</param>
        /// <returns>información necesaria para acceder al dashboards de Power Bi</returns>
        public async Task<EmbedConfig> ObtenerDashboard(EmbedFiltroDto embedFiltroDto)
        {
            try
            {
                if (!await base.GetTokenCredentials(TipoPowerBIEnum.Dashboard))
                    return await Task.Run(() => base.m_embedConfig);

                var workspaceId = volverIdentificacionWorkspace(embedFiltroDto.WorkspaceId);
                using (var client = new PowerBIClient(new Uri(ApiUrl), m_tokenCredentials))
                {
                    var dashboards = await client.Dashboards.GetDashboardsInGroupAsync(workspaceId);

                    var dashboard = dashboards.Value.FirstOrDefault();
                    if (dashboard == null)
                    {
                        m_embedConfig.ErrorMessage = BackboneRecursos.PowerBI_NoTienesDashboards;
                        return await Task.Run(() => base.m_embedConfig);
                    }

                    var dashboardId = Guid.TryParse(embedFiltroDto.DashboardId, out Guid gOutput) ? Guid.Parse(embedFiltroDto.DashboardId) : dashboard.Id;

                    var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                    var tokenResponse = await client.Dashboards.GenerateTokenInGroupWithHttpMessagesAsync(workspaceId, dashboardId, generateTokenRequestParameters);

                    if (tokenResponse == null)
                    {
                        m_embedConfig.ErrorMessage = BackboneRecursos.PowerBI_NoPudoGenerarToken;
                        return await Task.Run(() => base.m_embedConfig);
                    }

                    base.m_embedConfig = new EmbedConfig()
                    {
                        EmbedToken = tokenResponse.Body,
                        EmbedUrl = dashboard.EmbedUrl,
                        Id = dashboardId.ToString()
                    };

                    return await Task.Run(() => base.m_embedConfig);
                }
            }
            catch (HttpOperationException exc)
            {
                m_embedConfig.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
                return await Task.Run(() => base.m_embedConfig);
            }

        }

        /// <summary>
        /// Realiza autenticación y consulta en la api power bi para obtener el reporte
        /// </summary>
        /// <param name="embedFiltroDto">filtrar de servicios</param>
        /// <returns>información necesaria para acceder al reporte de Power Bi</returns>
        public async Task<EmbedConfig> ObtenerReportes(EmbedFiltroDto embedFiltroDto)
        {
            try
            {
                var tipo = string.IsNullOrEmpty(embedFiltroDto.DashboardId) ? TipoPowerBIEnum.Reporte : TipoPowerBIEnum.Dashboard;
                if (!await base.GetTokenCredentials(tipo))
                    return await Task.Run(() => base.m_embedConfig);

                using (var client = new PowerBIClient(new Uri(m_embedConfig.ApiUrl), base.m_tokenCredentials))
                {
                    var workspaceId = volverIdentificacionWorkspace(embedFiltroDto.WorkspaceId);

                    var reports = await client.Reports.GetReportsInGroupAsync(workspaceId);

                    if (reports.Value.Count() == 0)
                    {
                        m_embedConfig.ErrorMessage = BackboneRecursos.PowerBI_NoTienesReportes;
                        return await Task.Run(() => base.m_embedConfig);
                    }

                    var ReportId = Guid.TryParse(embedFiltroDto.ReportId, out Guid gOutput) ? Guid.Parse(embedFiltroDto.ReportId) : Guid.Empty;

                    Report report;
                    if (ReportId.Equals(Guid.Empty))
                    {
                        report = reports.Value.FirstOrDefault();
                        //m_embedConfig.ErrorMessage = BackboneRecursos.PowerBI.PowerBI_NoEncontroReporteID;
                        //return await Task.Run(() => base.m_embedConfig);
                    }
                    else
                    {
                        report = reports.Value.FirstOrDefault(r => r.Id.Equals(ReportId));
                    }

                    if (report == null)
                    {
                        m_embedConfig.ErrorMessage = BackboneRecursos.PowerBI_NoEncontroReporteID;
                        return await Task.Run(() => base.m_embedConfig);
                    }

                    var datasets = await client.Datasets.GetDatasetInGroupAsync(base.m_embedConfig.WorkspaceId == workspaceId ? base.m_embedConfig.WorkspaceId : workspaceId, report.DatasetId);
                    m_embedConfig.IsEffectiveIdentityRequired = datasets.IsEffectiveIdentityRequired;
                    m_embedConfig.IsEffectiveIdentityRolesRequired = datasets.IsEffectiveIdentityRolesRequired;
                    GenerateTokenRequest generateTokenRequestParameters;

                    //Así es como se crea un token de inserción con identidades efectivas
                    if (!string.IsNullOrWhiteSpace(embedFiltroDto.UserName))
                    {
                        var rls = new EffectiveIdentity(embedFiltroDto.UserName, new List<string> { report.DatasetId });
                        if (!string.IsNullOrWhiteSpace(embedFiltroDto.Roles))
                        {
                            var rolesList = new List<string>();
                            rolesList.AddRange(embedFiltroDto.Roles.Split(','));
                            rls.Roles = rolesList;
                        }
                        generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view", identities: new List<EffectiveIdentity> { rls });
                    }
                    else
                        generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");

                    var tokenResponse = await client.Reports.GenerateTokenInGroupWithHttpMessagesAsync(base.m_embedConfig.WorkspaceId == workspaceId ? base.m_embedConfig.WorkspaceId : workspaceId, report.Id, generateTokenRequestParameters);

                    if (tokenResponse == null)
                    {
                        m_embedConfig.ErrorMessage = BackboneRecursos.PowerBI_NoPudoGenerarToken;
                        return await Task.Run(() => base.m_embedConfig);
                    }

                    //Generar configuración de inserción.
                    m_embedConfig.EmbedToken = tokenResponse.Body;
                    m_embedConfig.EmbedUrl = report.EmbedUrl;
                    m_embedConfig.Id = report.Id.ToString();
                }

                return await Task.Run(() => base.m_embedConfig);
            }
            catch (HttpOperationException exc)
            {
                m_embedConfig.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
                return await Task.Run(() => base.m_embedConfig);
            }
        }

        public PowerBIClient clientTest { get; set; }

        #region EXPORT POWER BI

        private async Task<string> PostExportRequest(
          Guid reportId,
          FileFormat format,
          IList<string> pageNames = null /* Get the page names from the GetPages REST API */)
        {
            var powerBIReportExportConfiguration = new PowerBIReportExportConfiguration
            {
                Settings = new ExportReportSettings
                {
                    Locale = "en-us",
                },
                // Note that page names differ from the page display names
                // To get the page names use the GetPages REST API
                Pages = pageNames?.Select(pn => new ExportReportPage(pageName: pn)).ToList(),
            };

            var exportRequest = new ExportReportRequest
            {
                Format = format,
                PowerBIReportConfiguration = powerBIReportExportConfiguration,
            };
            // The 'Client' object is an instance of the Power BI .NET SDK
            var export = await clientTest.Reports.ExportToFileInGroupAsync(m_embedConfig.WorkspaceId, reportId, exportRequest);
            // Save the export ID, you'll need it for polling and getting the exported file
            return export.Id;
        }

        private async Task<HttpOperationResponse<Export>> PollExportRequest(
          Guid reportId,
          string exportId /* Get from the PostExportRequest response */,
          int timeOutInMinutes,
          CancellationToken token)
        {
            HttpOperationResponse<Export> httpMessage = null;
            Export exportStatus = null;
            DateTime startTime = DateTime.UtcNow;
            const int c_secToMillisec = 1000;
            do
            {
                if (DateTime.UtcNow.Subtract(startTime).TotalMinutes > timeOutInMinutes || token.IsCancellationRequested)
                {
                    // Error handling for timeout and cancellations 
                    return null;
                }
                // The 'Client' object is an instance of the Power BI .NET SDK
                httpMessage = await clientTest.Reports.GetExportToFileStatusInGroupWithHttpMessagesAsync(m_embedConfig.WorkspaceId, reportId, exportId);
                exportStatus = httpMessage.Body;

                // You can track the export progress using the PercentComplete that's part of the response
                //SomeTextBox.Text = string.Format("{0} (Percent Complete : {1}%)", exportStatus.Status.ToString(), exportStatus.PercentComplete);
                if (exportStatus.Status == ExportState.Running || exportStatus.Status == ExportState.NotStarted)
                {
                    // The recommended waiting time between polling requests can be found in the RetryAfter header
                    // Note that this header is not always populated
                    var retryAfter = httpMessage.Response.Headers.RetryAfter;
                    var retryAfterInSec = retryAfter.Delta.Value.Seconds;
                    await Task.Delay(retryAfterInSec * c_secToMillisec);
                }
            }

            // While not in a terminal state, keep polling
            while (exportStatus.Status != ExportState.Succeeded && exportStatus.Status != ExportState.Failed);

            return httpMessage;
        }

        private async Task<ExportedFileDto> GetExportedFile(
          Guid reportId,
          Export export /* Get from the PollExportRequest response */)
        {
            if (export.Status == ExportState.Succeeded)
            {
                // The 'Client' object is an instance of the Power BI .NET SDK
                var fileStream = await clientTest.Reports.GetFileOfExportToFileAsync(m_embedConfig.WorkspaceId, reportId, export.Id);
                return new ExportedFileDto
                {
                    FileStream = fileStream,
                    FileSuffix = export.ResourceFileExtension,
                };
            }
            return null;
        }

        public async Task<ByteArrayContent> ExportPowerBIReport(
          Guid reportId,
          int format,
          int pollingtimeOutInMinutes,
          CancellationToken token,
          IList<string> pageNames = null)  
        {
            const int c_maxNumberOfRetries = 3; /* Can be set to any desired number */
            const int c_secToMillisec = 1000;
            var retorno = new ByteArrayContent(new byte[] { });// byte[] { };
            try
            {
                Export export = null;
                int retryAttempt = 1;

                if (!await base.GetTokenCredentials(TipoPowerBIEnum.Reporte))
                    return retorno;

                using (clientTest = new PowerBIClient(new Uri(m_embedConfig.ApiUrl), base.m_tokenCredentials))
                {
                    do
                    {
                        var exportId = await PostExportRequest(reportId, (FileFormat)format, pageNames);
                        var httpMessage = await PollExportRequest(reportId, exportId, pollingtimeOutInMinutes, token);
                        export = httpMessage.Body;
                        if (export == null)
                        {
                            // Error, failure in exporting the report
                            return retorno;
                        }
                        if (export.Status == ExportState.Failed)
                        {
                            // Some failure cases indicate that the system is currently busy. The entire export operation can be retried after a certain delay
                            // In such cases the recommended waiting time before retrying the entire export operation can be found in the RetryAfter header
                            var retryAfter = httpMessage.Response.Headers.RetryAfter;
                            if (retryAfter == null)
                            {
                                // Failed state with no RetryAfter header indicates that the export failed permanently
                                return retorno;
                            }

                            var retryAfterInSec = retryAfter.Delta.Value.Seconds;
                            await Task.Delay(retryAfterInSec * c_secToMillisec);
                        }
                    }
                    while (export.Status != ExportState.Succeeded && retryAttempt++ < c_maxNumberOfRetries);

                    if (export.Status != ExportState.Succeeded)
                    {
                        // Error, failure in exporting the report
                        return retorno;
                    }

                    var exportedFile = await GetExportedFile(reportId, export);

                    // Now you have the exported file stream ready to be used according to your specific needs
                    // For example, saving the file can be done as follows:
                    //var stream = exportedFile.FileStream;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        exportedFile.FileStream.CopyTo(ms);
                        return new ByteArrayContent(ms.ToArray());
                    }
                }
            }
            catch(Exception ex)
            {
                // Error handling
                throw;
            }
        }

        public async Task<string> ObtenerMetiaType(int format)
        {
            return await Task.Run(() => {
                switch (format)
                {
                    case (int)FileFormat.PDF:
                        return "application/pdf";
                    case (int)FileFormat.CSV:
                        return "text/csv";
                    case (int)FileFormat.PNG:
                        return "image/png";
                    case (int)FileFormat.PPTX:
                        return "application/octet-stream";
                    case (int)FileFormat.XLSX:
                        return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    case (int)FileFormat.DOCX:
                        return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    default:
                        return "application/octet-stream";
                }
            });
        }

        #endregion EXPORT POWER BI


    }
}
