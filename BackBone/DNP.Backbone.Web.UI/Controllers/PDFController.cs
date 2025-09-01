using System.Web.Mvc;
using System.Diagnostics.CodeAnalysis;
using Rotativa;
using Rotativa.Options;
using DNP.Backbone.Dominio.Dto.Inbox;
using System;
using DNP.Backbone.Dominio.Dto.Tramites;
using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
using System.Collections.Generic;
using DNP.Backbone.Dominio.Dto.Monitoreo;
using DNP.Backbone.Dominio.Dto;
using DNP.Autorizacion.Dominio.Dto;
using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Consola;
using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
using DNP.Backbone.Dominio.Dto.UsuarioNotificacion;
using DNP.Backbone.Dominio.Dto.Flujos;

namespace DNP.Backbone.Web.UI.Controllers
{
    [ExcludeFromCodeCoverage]
    public class PDFController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        public ActionResult ProyectosPDF(InboxDto inbox)
        {
            var archivo = ResponseViewAsPdf(inbox, "Proyectos", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("Proyectos");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        public ActionResult ProyectosPDFConsola(InboxDto inbox)
        {
            var archivo = ResponseViewAsPdf(inbox, "ProyectosConsola", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("ProyectosConsola");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        public ActionResult TramitesPDF(InboxTramite inbox)
        {
            var archivo = ResponseViewAsPdf(inbox, "Tramites", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("Tramites");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        ///     Obitiene un archivo binario PDF con los datos enviados.
        /// </summary>
        /// <param name="datos">Proyectos en la consulta actual de la consola</param>
        /// <returns></returns>
        public ActionResult ConsolaProyectosPDF(DNP.Backbone.Dominio.Dto.Proyecto.ProyectoDto datos)
        {
            try
            {
                var archivo = ResponseViewAsPdf(datos, "ConsolaProyectos", Orientation.Portrait);
                var nombre  = nombreDelArchivoPDF("Proyectos");

                return Json(new
                {
                    Datos       = File(archivo, System.Net.Mime.MediaTypeNames.Application.Pdf, nombre),
                    EsExcepcion = false
                }); ;
            }
            catch (Exception exception) {
                return Json(new { 
                    EsExcepcion      = true,
                    ExcepcionMensaje = $"PDF.ConsolaProyectosPDF: {exception.Message}\\n{exception.InnerException?.Message??String.Empty}"
                });
            } 
        }

        /// <summary>
        ///  Obtiene un archivo binario PDF con los datos enviados en el parámetro de la petición
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NotificacionesMantenimiento(List<Dominio.Dto.UsuarioNotificacion.UsuarioNotificacionConfigDto> datos) {
            try
            {
                var archivo = ResponseViewAsPdf(datos, "NotificacionesMantenimiento", Orientation.Portrait);
                var nombre  = nombreDelArchivoPDF("NotificacionesUsuarioMantenimiento");

                return Json(new
                {
                    Datos       = File(archivo, System.Net.Mime.MediaTypeNames.Application.Pdf, nombre),
                    EsExcepcion = false
                });
            }
            catch (Exception exception) {
                return Json(new { 
                    EsExcepcion = true,
                    ExcepcionMensaje = $"PDF.NotificacionesMantenimiento: {exception.Message}\\n{exception.InnerException?.Message ?? String.Empty}"
                });
            }
        }

        /// <summary>
        ///  Obtiene un archivo binario de la lista de temas proporcionados como parámetro de la petición HTTP request.
        /// </summary>
        /// <param name="datos">Lista de datos a mostrar en el archivo PDF como un objeto <see cref="List{T}"/> donde <c>T</c> es una instancia de la clase
        ///                     <see cref="Dominio.Dto.CentroAyuda.AyudaTemaListaItemDto"/>
        ///  </param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TemarioCentroAyuda(List<Dominio.Dto.CentroAyuda.AyudaTemaListaItemDto> datos) {
            try
            {
                var archivo = ResponseViewAsPdf(datos, "TemarioCentroAyuda", Orientation.Portrait);
                var nombre  = nombreDelArchivoPDF("ListaTemasAyuda");

                return Json(new
                {
                    Datos = File(archivo, System.Net.Mime.MediaTypeNames.Application.Pdf, nombre),
                    EsExcepcion = false
                });
            }
            catch (Exception exception) {
                return Json(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = $"PDF.TemarioCentroAyuda: {exception.Message}\\n{exception.InnerException?.Message ?? String.Empty}"
                });
            }
        }


        /// <summary>
        ///  Obtiene un archivo binario de la lista de videos proporcionados como parámetro de la petición HTTP request.
        /// </summary>
        /// <param name="datos">Lista de datos a mostrar en el archivo PDF como un objeto <see cref="List{T}"/> donde <c>T</c> es una instancia de la clase
        ///                     <see cref="Dominio.Dto.CentroAyuda.AyudaTemaListaItemDto"/>
        ///  </param>
        /// <returns></returns>
        public ActionResult VideosCentroAyuda(List<Dominio.Dto.CentroAyuda.AyudaTemaListaItemDto> datos) {
            try
            {
                var archivo = ResponseViewAsPdf(datos, "VideosCentroAyuda", Orientation.Portrait);
                var nombre = nombreDelArchivoPDF("ListaVideosAyuda");

                return Json(new
                {
                    Datos = File(archivo, System.Net.Mime.MediaTypeNames.Application.Pdf, nombre),
                    EsExcepcion = false
                });
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    EsExcepcion = true,
                    ExcepcionMensaje = $"PDF.VideosCentroAyuda: {exception.Message}\\n{exception.InnerException?.Message ?? String.Empty}"
                });
            }
        }

        //public actionresult tramitespdf()
        //{
        //    var inbox = new inboxtramite()
        //    {
        //        columnasvisibles = new string[] { "codigo", "descripcion", "fecha", "valorproprio", "valorsgr", "tipotramite", "entidad", "identificadorcr", "estadotramite", "sector" },
        //        listagrupotramiteentidad = new list<grupotramiteentidad>()
        //        {
        //            new grupotramiteentidad()
        //           {
        //               nombreentidad = "chiá",
        //               sector = "desarollo",
        //               grupotramites = new list<grupotramites>()
        //               {
        //                   new grupotramites()
        //                   {
        //                       nombretipoentidad = "ministerio",
        //                       nombreentidad = "chiá",
        //                       nombresector = "desarollo",
        //                       listatramites =  new list<tramitedto>()
        //                       {
        //                          new tramitedto()
        //                          {
        //                            id = 1,
        //                            nombresector = "desarollo",
        //                            descripcion = "primeiro teste",
        //                            identificadorcr = "90001",
        //                            nombreentidad = "chiá",
        //                            valorproprio = 3,
        //                            valorsgp = 1,
        //                            tipotramite = new tipotramitedto() { id = 1, nombre = "transporte" },
        //                            descestado = "vigente",
        //                            fechacreacion = datetime.now
        //                          },
        //                                                            new tramitedto()
        //                          {
        //                            id = 2,
        //                            nombresector = "desarollo",
        //                            descripcion = "primeiro teste",
        //                            identificadorcr = "90001",
        //                            nombreentidad = "chiá",
        //                            valorproprio = 3,
        //                            valorsgp = 1,
        //                            tipotramite = new tipotramitedto() { id = 1, nombre = "transporte" },
        //                            descestado = "vigente",
        //                            fechacreacion = datetime.now
        //                          },
        //                          new tramitedto()
        //                          {
        //                            id = 3,
        //                            nombresector = "desarollo",
        //                            descripcion = "primeiro teste 2",
        //                            identificadorcr = "90001",
        //                            nombreentidad = "chiá",
        //                            valorproprio = 5,
        //                            valorsgp = 6,
        //                            tipotramite = new tipotramitedto() { id = 1, nombre = "transporte" },
        //                            descestado = "vigente",
        //                            fechacreacion = datetime.now
        //                          }
        //                       }
        //                   }
        //               }
        //           },
        //            new grupotramiteentidad()
        //           {
        //               nombreentidad = "boyaca",
        //               sector = "transporte",
        //               grupotramites = new list<grupotramites>()
        //               {
        //                   new grupotramites()
        //                   {
        //                       nombretipoentidad = "ministerio",
        //                       nombreentidad = "boyaca",
        //                       nombresector = "transporte",
        //                       listatramites =  new list<tramitedto>()
        //                       {
        //                          new tramitedto()
        //                          {
        //                            id = 1,
        //                            nombresector = "transporte",
        //                            descripcion = "primeiro teste 3",
        //                            identificadorcr = "90001",
        //                            nombreentidad = "chiá",
        //                            valorproprio = 5,
        //                            valorsgp = 5,
        //                            tipotramite = new tipotramitedto() { id = 1, nombre = "transporte" },
        //                            descestado = "empacado",
        //                            fechacreacion = datetime.now
        //                          },
        //                          new tramitedto()
        //                          {
        //                            id = 2,
        //                            nombresector = "transporte",
        //                            descripcion = "primeiro teste 4",
        //                            identificadorcr = "90001",
        //                            nombreentidad = "chiá",
        //                            valorproprio = 6,
        //                            valorsgp = 7,
        //                            tipotramite = new tipotramitedto() { id = 1, nombre = "transporte" },
        //                            descestado = "empacado",
        //                            fechacreacion = datetime.now
        //                          }
        //                       }
        //                   }
        //               }
        //           }
        //        }
        //    };

        //    return view("tramites", inbox);
        //}

        public ActionResult TramitesProyectosPDF(ProyectosTramitesDTO inbox)
        {
            var archivo = ResponseViewAsPdf(inbox, "TramitesProyectos", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("TramitesProyectos");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        public ActionResult ConsolaMonitoreoProyectosPDF(ProyectoResumenDto proyecto)
        {
            var archivo = ResponseViewAsPdf(proyecto, "ConsolaMonitoreoProyectos", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("ConsolaMonitoreoProyectos");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        public ActionResult ConsolaTramitesPDF(ConsolaTramiteDto consolaTramiteDto)
        {
            var archivo = ResponseViewAsPdf(consolaTramiteDto, "ConsolaTramites", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("ConsolaTramites");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        public ActionResult ConsolaTramitesProyectosPDF(ProyectosTramitesDTO inbox)
        {
            var archivo = ResponseViewAsPdf(inbox, "ConsolaTramitesProyectos", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("ConsolaTramitesProyectos");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// Pdf Perfiles
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        public ActionResult PerfilesPDF(List<PerfilDto> dto)
        {
            var archivo = ResponseViewAsPdf(dto, "Perfiles", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("Perfiles");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// Pdf Entidades
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        public ActionResult EntidadesPDF(List<EntidadFiltroDto> dto)
        {
            var archivo = ResponseViewAsPdf(dto, "Entidades", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("Entidades");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// Pdf Roles
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        public ActionResult RolesPDF(List<RolDto> dto)
        {
            var archivo = ResponseViewAsPdf(dto, "Roles", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("Roles");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// Pdf Inflexibilidad
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        public ActionResult InflexibilidadesPDF(List<InflexibilidadDto> dto)
        {
            var archivo = ResponseViewAsPdf(dto, "Inflexibilidades", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("Inflexibilidades");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }
        

        /// <summary>
        /// Pdf Comum
        /// </summary>
        /// <param name="inbox"></param>
        /// <returns></returns>
        public ActionResult PdfComum(PdfDto dto)
        {
            var archivo = ResponseViewAsPdf(dto.Data, dto.Nombre, Orientation.Portrait);
            var nombre = nombreDelArchivoPDF(dto.Nombre);
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// Método que envía los datos de alertas para su finalización y generación de pdf.
        /// </summary>
        /// <param name="alertasConfigs">Dados a serem processados pelo pdf</param>
        /// <returns>Archivo pdf generado</returns>
        public ActionResult ConsolaAlertaConfigPDF(List<AlertasConfigDto> alertasConfigs)
        {
            var archivo = ResponseViewAsPdf(alertasConfigs, "ConsolaAlertasConfig", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("ConsolaAlertasConfig");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        public ActionResult UsuariosPDF(List<UsuarioReportesDto> entidadUsuarioDtos)
        {
            var archivo = ResponseViewAsPdf(entidadUsuarioDtos, "Usuarios", Orientation.Portrait);
            
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            DateTime serverTime = DateTime.Now;
            DateTimeOffset colombiaTime = TimeZoneInfo.ConvertTime(serverTime, timeZone);
            var nombre = "Usuarios" + colombiaTime.ToString("dd MM yyyy HH mm ss");
            nombre =nombre.Replace(" ", ""); 
            nombre =nombre+ ".pdf";
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// Método que devuelve el nombre del archivo concatenando fecha y hora
        /// </summary>
        /// <param name="fuente"></param>
        /// <returns>String: Devuelve el nombre del archivo</returns>
        private string nombreDelArchivoPDF(string fuente)
        {
            var data = $"{ DateTime.Now.Year }-{ DateTime.Now.Month.ToString().PadLeft(2, '0')}-{ DateTime.Now.Day.ToString().PadLeft(2, '0')}";
            var hora = $"{ DateTime.Now.TimeOfDay.Hours}h{ DateTime.Now.TimeOfDay.Minutes}m{DateTime.Now.TimeOfDay.Seconds.ToString().PadLeft(2, '0')}";
            return $"{fuente}_{data}_{hora}.pdf";
        }

        /// <summary>
        /// Método que compila os dados e o template html para arquivo em pdf.
        /// </summary>
        /// <typeparam name="T">Um tipo qualquer de objeto</typeparam>
        /// <param name="dados">Dados a serem processados pelo pdf</param>
        /// <param name="viewName">Template do pdf</param>
        /// <param name="pageOrientation"></param>
        /// <returns>Array de byte do PDF gerado</returns>
        protected byte[] ResponseViewAsPdf<T>(T dados, string viewName, Orientation pageOrientation)
        {
            var pageMargins = pageOrientation == Orientation.Portrait
                ? new Margins { Left = 10, Bottom = 20, Right = 10, Top = 20 }
                : new Margins { Left = 20, Bottom = 10, Right = 20, Top = 10 };

            var pdf = new ViewAsPdf(viewName, dados)
            {
                PageSize = Size.A4,
                PageOrientation = pageOrientation,
                PageMargins = pageMargins,
                PageWidth = 210,
                PageHeight = 297,
                CustomSwitches = "--header-right \"" + "  Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "    Pag.: [page]/[toPage]\"" +
                                 " --header-font-size \"9\" --header-spacing 5 --header-font-name \"calibri light\"" +
                                 " --footer-center \"" + "  Pag.: [page]/[toPage]\"" +
                                 " --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"",
                IsGrayScale = false
            };
            return pdf.BuildFile(this.ControllerContext);
        }

        /// <summary>
        /// Pdf log de instancias 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ActionResult InstanciasLogPDF(List<LogsInstanciasDto> dto)
        {
            var archivo = ResponseViewAsPdf(dto, "InstanciasLog", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("InstanciasLog");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

        /// <summary>
        /// Pdf mensajes de notificaciones  
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ActionResult NotificacionesMensajesPDF(List<UsuarioNotificacionMensajesPDFDto> dto)
        {
            var archivo = ResponseViewAsPdf(dto, "NotificacionesMensajes", Orientation.Portrait);
            var nombre = nombreDelArchivoPDF("NotificacionesMensajes");
            return File(archivo, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
        }

    }
}