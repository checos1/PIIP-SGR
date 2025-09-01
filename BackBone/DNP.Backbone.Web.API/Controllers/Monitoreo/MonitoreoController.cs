using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Dto.PowerBI;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.PowerBI;
using DNP.Backbone.Servicios.Interfaces.Proyectos;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers
{
    public class MonitoreoController : Base.BackboneBase
    {
        private readonly IProyectoServicios _proyectoServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;
        private readonly IEmbedServicios _embedServicios;
        private readonly IFlujoServicios _flujoServicios;

        public MonitoreoController(IProyectoServicios proyectoServicios, IAutorizacionServicios autorizacionUtilidades,
            IServiciosNegocioServicios serviciosNegocioServicios, IEmbedServicios embedServicios,
            IFlujoServicios flujoServicios) : base(autorizacionUtilidades)
        {
            _proyectoServicios = proyectoServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
            _serviciosNegocioServicios = serviciosNegocioServicios;
            _embedServicios = embedServicios;
            _flujoServicios = flujoServicios;
        }



        /// <summary>
        /// Api para obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de proyectos.</returns>
        [Route("api/Monitoreo/ConsolaMonitoreo")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerMonitoreoProyectos(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.ObtenerMonitoreoProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos de monitoreo de proyectos.
        /// </summary>
        /// <param name="instanciaProyectoDto"></param>
        /// <returns>Objeto con propiedades para realizar consulta de datos de monitoreo de proyectos.</returns>
        [Route("api/Monitoreo/ObtenerInfoPDF")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerInfoPDF(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(instanciaProyectoDto.ProyectoParametrosDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _proyectoServicios.ObtenerMonitoreoProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto).ConfigureAwait(false);

                if (result != null)
                    result.ColumnasVisibles = instanciaProyectoDto.ColumnasVisibles;

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos Monitoreo de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion e filtro</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Monitoreo/ObtenerExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerExcel(InstanciaProyectoDto instanciaProyectoDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var _result = await _proyectoServicios.ObtenerMonitoreoProyectos(instanciaProyectoDto.ProyectoParametrosDto, Request.Headers.Authorization.Parameter, instanciaProyectoDto.ProyectoFiltroDto).ConfigureAwait(false);
                _result.ColumnasVisibles = instanciaProyectoDto.ColumnasVisibles;

                result.StatusCode = HttpStatusCode.OK;
                result.Content = ExcelUtilidades.ObtenerExcellConsolaMonitoreoProyectos(_result);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos Monitoreo de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion e filtro</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Monitoreo/ObtenerReportes")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerReportesPowerBI(EmbedParametrosDto embedParametrosDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(embedParametrosDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _embedServicios.ObtenerReportes(embedParametrosDto.EmbedFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para obtención de datos Monitoreo de proyectos en Excel.
        /// </summary>
        /// <param name="instanciaProyectoDto">Contiene informacion de autorizacion e filtro</param>
        /// <returns>Excel con datos generados</returns>
        [Route("api/Monitoreo/ObtenerDashboard")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerDashboardsPowerBI(EmbedParametrosDto embedParametrosDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(embedParametrosDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(RespuestaAutorizacion);
                var result = await _embedServicios.ObtenerDashboard(embedParametrosDto.EmbedFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Monitoreo/ObtenerListaDashboards")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaDashboardsPowerBI(EmbedParametrosDto embedParametrosDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(embedParametrosDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(RespuestaAutorizacion);
                var result = await _embedServicios.ObtenerListaDashboard(embedParametrosDto.EmbedFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Monitoreo/ObtenerListaReportes")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaReportesPowerBI(EmbedParametrosDto embedParametrosDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(embedParametrosDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(RespuestaAutorizacion);
                var result = await _embedServicios.ObtenerListaReportes(embedParametrosDto.EmbedFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Monitoreo/ObtenerReporteGantt")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerReporteGantt(EmbedParametrosDto embedParametrosDto)
        {
            try
            {
                var result = await _embedServicios.ObtenerReportes(embedParametrosDto.EmbedFiltroDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Monitoreo/DescargarReportes")]
        [HttpPost]
        public async Task<HttpResponseMessage> DescargarReportesPowerBI([FromBody] EmbedParametrosDto embedParametrosDto)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                CancellationToken ct = new CancellationTokenSource().Token;

                var idReporte = Guid.Parse(embedParametrosDto.EmbedFiltroDto.ReportId);
                //if (!ValidarParametrosRequest(embedParametrosDto.ParametrosInboxDto)) return ResponseMessage(base.RespuestaAutorizacion);
                var download = await  _embedServicios.ExportPowerBIReport(idReporte, embedParametrosDto.EmbedFiltroDto.FileFormat, 100000, ct).ConfigureAwait(false);
                var mediatype = await _embedServicios.ObtenerMetiaType(embedParametrosDto.EmbedFiltroDto.FileFormat).ConfigureAwait(false);
                result.Content = download;
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(mediatype);
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Monitoreo/ObtenerListaFileFormat")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaFileFormat(EmbedParametrosDto embedParametrosDto)
        {
            try
            {
                if (!await ValidarParametrosRequest(embedParametrosDto.ParametrosInboxDto).ConfigureAwait(false)) return ResponseMessage(base.RespuestaAutorizacion);

                List<dynamic> dynamics = new List<dynamic>();
                await Task.Run(() =>
                {
                    dynamics.Add(new { value = (int)FileFormat.PDF, text = nameof(FileFormat.PDF), type = ".pdf" });
                    //dynamics.Add(new { value = (int)FileFormat.PNG, text = FileFormat.PNG.ToString(), type = ".png" });
                    //dynamics.Add(new { value = (int)FileFormat.CSV, text = FileFormat.CSV.ToString(), type = ".csv" });
                    dynamics.Add(new { value = (int)FileFormat.PPTX, text = nameof(FileFormat.PPTX), type = ".pptx" });
                    //dynamics.Add(new { value = (int)FileFormat.XLSX, text = FileFormat.XLSX.ToString(), type = ".xlsx" });
                    //dynamics.Add(new { value = (int)FileFormat.DOCX, text = FileFormat.DOCX.ToString(), type = ".docx" });
                });

                return Ok(dynamics);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Monitoreo/ObtenerSituacaoAlertasProyectos")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerSituacaoAlertasProyectos(InstanciaProyectoDto proyectoDto)
        {
            try
            {
                var result = await _flujoServicios.ObtenerSituacaoAlertasProyectos(proyectoDto).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        public string MetiaType(int format)
        {
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
        }

    }

};
