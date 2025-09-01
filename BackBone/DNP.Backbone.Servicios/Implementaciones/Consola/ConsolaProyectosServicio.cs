namespace DNP.Backbone.Servicios.Implementaciones.Consola
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.AutorizacionNegocio;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.Consola;
    using Dominio.Dto.Proyecto;
    using Interfaces.Autorizacion;
    using Interfaces.ServiciosNegocio;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using SPClient = Microsoft.SharePoint.Client;

    /// <summary>
    /// Clase API responsable de la gestión de servicio de proyectos
    /// </summary>
    public class ConsolaProyectosServicio : IConsolaProyectosServicio
    {
        #region Atributos

        private readonly IFlujoServicios _flujoServicios;
        private readonly IAutorizacionServicios _autorizacionNegocioServicios;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;
        private IClienteHttpServicios _clienteHttpServicios;

        #endregion Atributos

        #region Constructor

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>
        public ConsolaProyectosServicio(IFlujoServicios flujoServicios, IAutorizacionServicios autorizacionNegocioServicios, IServiciosNegocioServicios serviciosNegocioServicios, IClienteHttpServicios clienteHttpServicios)
        {
            _flujoServicios = flujoServicios;
            _autorizacionNegocioServicios = autorizacionNegocioServicios;
            _serviciosNegocioServicios = serviciosNegocioServicios;
            _clienteHttpServicios = clienteHttpServicios;
        }

        #endregion Constructor

        #region Métodos

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Consulta de datos de proyectos.</returns>
        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            var entidades = _autorizacionNegocioServicios.ObtenerEntidadesPorTipoEntidadYUsuario(proyectoFiltroDto.TipoEntidad, datosConsulta.IdUsuario).Result;
            FiltrarEntidades(ref entidades, proyectoFiltroDto);
            //alterar entidades por 


            var proyectos = _serviciosNegocioServicios.ObtenerProyectos(new ParametrosProyectosDto
            {
                IdsEntidades = entidades.Select(e => e.EntityTypeCatalogOptionId ?? 0).ToList(),
                NombresEstadosProyectos = new List<string>
                    {
                        ProyectoEstadoMga.Formulado.DescriptionAttr(),
                        ProyectoEstadoMga.Viable.DescriptionAttr(),
                        ProyectoEstadoMga.Aprobado.DescriptionAttr(),
                        ProyectoEstadoMga.EnEjecucion.DescriptionAttr(),
                        ProyectoEstadoMga.Cerrado.DescriptionAttr(),
                        ProyectoEstadoMga.NoAprobado.DescriptionAttr(),
                        ProyectoEstadoMga.NoViable.DescriptionAttr()
                     }
            }, datosConsulta.IdUsuario).Result;
            FiltrarProyectos(ref proyectos, proyectoFiltroDto);


            var proyectoDto = new ProyectoDto();

            if (proyectos != null && proyectos.Count > 0)
            {
                proyectoDto.GruposEntidades = CrearGrupoEntidades(proyectos.OrderBy(p => p.SectorNombre).ThenBy(p => p.EntidadNombre).ToList());

                proyectoDto.GruposEntidades.ForEach(negocioDto =>
                {
                    var negocio = negocioDto.ListaEntidades.ToList();

                    negocio.ForEach(grupoEntidadesNegocio =>
                    {
                        grupoEntidadesNegocio.ObjetosNegocio.ForEach(entidadeNegocioDto =>
                        {
                            var sector = entidades.FirstOrDefault(ent => ent.EntityTypeCatalogOptionId == entidadeNegocioDto.IdEntidad);
                            if (sector != null)
                            {
                                entidadeNegocioDto.AgrupadorEntidad = sector.AgrupadorEntidad;
                                entidadeNegocioDto.NombreEntidad = sector.Entidad;
                            }
                        });
                    });
                });
            }
            else
            {
                proyectoDto.GruposEntidades = new List<GrupoEntidadProyectoDto>();
            }

            return Task.FromResult(proyectoDto);
        }

        private void FiltrarProyectos(ref List<Dominio.Dto.ProyectosEntidadesDto> proyectos, ProyectoFiltroDto proyectoFiltroDto)
        {
            if (proyectos != null && proyectos.Count > 0)
            {
                if (!string.IsNullOrEmpty(proyectoFiltroDto.Identificador))
                {
                    proyectos = proyectos.Where(p => !string.IsNullOrEmpty(p.ProyectoId.ToString()) && p.ProyectoId.ToString().Contains(proyectoFiltroDto.Identificador)).ToList();
                }
                if (!string.IsNullOrEmpty(proyectoFiltroDto.Bpin))
                {
                    proyectos = proyectos.Where(p => !string.IsNullOrEmpty(p.CodigoBpin) && p.CodigoBpin.Contains(proyectoFiltroDto.Bpin)).ToList();
                }
                if (!string.IsNullOrEmpty(proyectoFiltroDto.Nombre))
                {
                    proyectos = proyectos.Where(p => !string.IsNullOrEmpty(p.ProyectoNombre) && p.ProyectoNombre.ToLower().Contains(proyectoFiltroDto.Nombre.ToLower())).ToList();
                }
                if (proyectoFiltroDto.EstadoProyectoId.HasValue)
                {
                    proyectos = proyectos.Where(p => p.EstadoId == proyectoFiltroDto.EstadoProyectoId).ToList();
                }
                if (proyectoFiltroDto.SectorId.HasValue)
                {
                    proyectos = proyectos.Where(p => p.SectorId == proyectoFiltroDto.SectorId).ToList();
                }
                if (proyectoFiltroDto.VigenciaProyectoId.HasValue)
                    proyectos = proyectos.Where(x => x.HorizonteInicio <= proyectoFiltroDto.VigenciaProyectoId && x.HorizonteFin >= proyectoFiltroDto.VigenciaProyectoId).ToList();

            }
        }

        private void FiltrarEntidades(ref List<EntidadFiltroDto> entidades, ProyectoFiltroDto proyectoFiltroDto)
        {
            if (entidades.Count > 0 && proyectoFiltroDto.EntidadId.HasValue)
            {
                entidades = entidades.Where(e => e.EntityTypeCatalogOptionId == proyectoFiltroDto.EntidadId).ToList();
            }
            if (!string.IsNullOrEmpty(proyectoFiltroDto.TipoEntidad))
            {
                entidades = entidades.Where(e => e.TipoEntidad == proyectoFiltroDto.TipoEntidad).ToList();
            }
        }

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>        
        /// <returns>Consulta de datos de proyectos.</returns>
        public Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Obtención de grupo para datos de proyectos.
        /// </summary>
        /// <param name="listaProyectos">Lista de proyectos</param>        
        /// <returns>Consulta de grupos de datos de proyectos.</returns>
        private List<GrupoEntidadProyectoDto> CrearGrupoEntidades(List<Dominio.Dto.ProyectosEntidadesDto> listaProyectos)
        {
            List<NegocioDto> listaObjetosNegocio = new List<NegocioDto>();

            listaObjetosNegocio = listaProyectos.Select(p => new NegocioDto
            {
                EstadoProyecto = p.Estado,
                EstadoProyectoId = p.EstadoId,
                IdEntidad = p.EntidadId,
                NombreEntidad = p.EntidadNombre,
                ProyectoId = p.ProyectoId,
                SectorId = p.SectorId,
                SectorNombre = p.SectorNombre,
                NombreObjetoNegocio = p.ProyectoNombre,
                IdObjetoNegocio = p.CodigoBpin,
                HorizonteInicio = p.HorizonteInicio,
                HorizonteFin = p.HorizonteFin
            }).ToList();

            var grupoPorEntidades = from ge in listaObjetosNegocio
                                    group ge by new { ge.IdEntidad, ge.NombreEntidad, ge.TipoEntidad }
                                    into g
                                    select new EntidadProyectoDto()
                                    {
                                        IdEntidad = g.Key.IdEntidad,
                                        NombreEntidad = g.Key.NombreEntidad,
                                        TipoEntidad = g.Key.TipoEntidad,
                                        ObjetosNegocio = g.ToList()
                                    };

            var grupoPorTipoEntidad = (from gte in grupoPorEntidades
                                       group gte by new { gte.TipoEntidad }
                                       into g
                                       select new GrupoEntidadProyectoDto()
                                       {
                                           TipoEntidad = g.Key.TipoEntidad,
                                           ListaEntidades = g.ToList()
                                       }).ToList();

            return grupoPorTipoEntidad;
        }

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Consulta de datos de proyectos.</returns>
        public Task<ProyectoDto> ObtenerInstanciasProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdTipoObjetoNegocio = datosConsulta.IdObjeto,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = proyectoFiltroDto
            };

            var listaObjetosNegocio = _flujoServicios.ObtenerListaObjetosNegocioConInstanciasActivas(parametros).Result;

            var proyectoDto = new ProyectoDto();

            if (listaObjetosNegocio != null && listaObjetosNegocio.Count > 0)
            {
                proyectoDto.GruposEntidades = CrearGrupoEntidades(listaObjetosNegocio);
            }
            else
            {
                proyectoDto.GruposEntidades = new List<GrupoEntidadProyectoDto>();
            }

            return Task.FromResult(proyectoDto);
        }

        public Task<object> InsertarAuditoria(Dominio.Dto.Proyecto.AuditoriaEntidadDto auditoriaEntidad, String usuarioDNP)
        {

            var result = _serviciosNegocioServicios.InsertAuditoriaEntidadProyecto(auditoriaEntidad, usuarioDNP).Result;

            return Task.FromResult(result);
        }

        public Task<object> ObtenerAuditoriaEntidadProyecto(int proyectoId, String usuarioDNP)
        {
            var result = _serviciosNegocioServicios.ObtenerAuditoriaEntidadProyecto(proyectoId, usuarioDNP).Result;
            return Task.FromResult(result);
        }

        /// <summary>
        /// Obtención de grupo para datos de proyectos.
        /// </summary>
        /// <param name="listaObjetosNegocio">Lista de objeto negocio</param>        
        /// <returns>Consulta de grupos de datos de proyectos.</returns>
        private List<GrupoEntidadProyectoDto> CrearGrupoEntidades(List<NegocioDto> listaObjetosNegocio)
        {
            var grupoPorEntidades = from ge in listaObjetosNegocio
                                    group ge by new { ge.IdEntidad, ge.NombreEntidad, ge.TipoEntidad }
                                    into g
                                    select new EntidadProyectoDto()
                                    {
                                        IdEntidad = g.Key.IdEntidad,
                                        NombreEntidad = g.Key.NombreEntidad,
                                        TipoEntidad = g.Key.TipoEntidad,
                                        ObjetosNegocio = g.ToList()
                                    };

            var grupoPorTipoEntidad = (from gte in grupoPorEntidades
                                       group gte by new { gte.TipoEntidad }
                                       into g
                                       select new GrupoEntidadProyectoDto()
                                       {
                                           TipoEntidad = g.Key.TipoEntidad,
                                           ListaEntidades = g.ToList()
                                       }).ToList();

            return grupoPorTipoEntidad;
        }


        public List<ProjectDocumentSimpleDto> ObtenerDocumentosAdjuntos(string idProyecto)
        {
            var allDocumentSimple = new List<ProjectDocumentSimpleDto>();
            var allDocument = new List<ProjectDocumentDto>();

            try
            {
                var sharePointBaseUrl = "http://intramga.dnp.gov.co";
                var sharePointExternalBaseUrl = "https://portalmga.dnp.gov.co";
                var documentLibraryName = "Documentos";
                var comment = "Comments";
                var category = "_Category";

                string fullSiteUrl = new Uri(new Uri(sharePointBaseUrl), idProyecto).ToString();

                using (System.Web.Hosting.HostingEnvironment.Impersonate())
                {
                    // Cambiar el sitio url de abajo
                    using (var clientContext = new SPClient.ClientContext(new Uri(fullSiteUrl)))
                    {
                        // Establecer credenciales
                        clientContext.Credentials = GetSharePointAdminUserCredentials();

                        SPClient.Web site = clientContext.Web;

                        // Cambiar el nombre de biblioteca de documentos
                        SPClient.List docLib = site.Lists.GetByTitle(documentLibraryName);
                        clientContext.Load(docLib);

                        SPClient.CamlQuery caml = new SPClient.CamlQuery();

                        //caml.ViewXml = "<View><Query><Where><Eq><FieldRef Name=\"FileLeafRef\"/><Value Type=\"Text\">Fachada lateral derecha.pdf</Value></Eq></Where></Query></View>";

                        SPClient.ListItemCollection items = docLib.GetItems(caml);

                        clientContext.Load(items);
                        clientContext.ExecuteQuery();
                        int counter = 1;

                        // Iterar a través de todos los elementos de la biblioteca de documentos
                        foreach (SPClient.ListItem item in items)
                        {

                            if (item.FileSystemObjectType == Microsoft.SharePoint.Client.FileSystemObjectType.File)
                            {
                                var documentEntity = new ProjectDocumentDto();
                                var documentEntitySimple = new ProjectDocumentSimpleDto();
                                int size = 0;

                                Microsoft.SharePoint.Client.File file = item.File;
                                documentEntity.FileStream = file.OpenBinaryStream();

                                //Load the Stream data for the file
                                clientContext.Load(file);
                                clientContext.ExecuteQuery();

                                documentEntity.DocumentName = item.File.Name;
                                documentEntity.DocumentPath = new Uri(new Uri(sharePointExternalBaseUrl), item.File.ServerRelativeUrl).ToString();
                                documentEntity.Created = ((DateTime)item["Created"]).ToLocalTime();
                                documentEntity.Comment = item[comment] as string;
                                documentEntity.Category = item[category] as string;


                                documentEntitySimple.DocumentName = item.File.Name;
                                documentEntitySimple.Created = ((DateTime)item["Created"]).ToLocalTime();
                                documentEntitySimple.Category = item[category] as string;


                                if (string.IsNullOrEmpty(documentEntity.Category) == false)
                                {
                                    documentEntity.Category = documentEntity.Category;
                                    documentEntitySimple.Category = documentEntity.Category;
                                }

                                documentEntity.Size = string.Empty;
                                documentEntitySimple.Size = string.Empty;
                                if (int.TryParse(item["File_x0020_Size"] as string, out size))
                                {
                                    if (size > 0)
                                    {
                                        if (size > 1024)
                                        {
                                            size = size / 1024;
                                        }
                                        else
                                        {
                                            size = 1;
                                        }

                                        documentEntity.Size = size.ToString(System.Globalization.CultureInfo.CurrentCulture) + " KB";
                                        documentEntitySimple.Size = size.ToString(System.Globalization.CultureInfo.CurrentCulture) + " KB";
                                    }
                                }

                                int extensionIndex = documentEntity.DocumentName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
                                if (extensionIndex > 0)
                                {
                                    documentEntity.Extension = documentEntity.DocumentName.Substring(extensionIndex + 1).ToUpperInvariant();
                                    documentEntitySimple.Extension = documentEntity.DocumentName.Substring(extensionIndex + 1).ToUpperInvariant();
                                }
                                else
                                {
                                    documentEntity.Extension = string.Empty;
                                    documentEntitySimple.Extension = string.Empty;

                                }

                                documentEntity.Order = counter;
                                counter++;

                                allDocument.Add(documentEntity);
                                allDocumentSimple.Add(documentEntitySimple);
                            }
                        }
                        //   return allDocument;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return allDocumentSimple;
            //   return allDocument;


        }

        public HttpContent ObtenerDocumentoAdjunto(string idProyecto, string nombreArchivo)
        {
            var allDocument = new List<ProjectDocumentDto>();

            try
            {
                var sharePointBaseUrl = "http://intramga.dnp.gov.co";
                var documentLibraryName = "Documentos";

                string fullSiteUrl = new Uri(new Uri(sharePointBaseUrl), idProyecto).ToString();

                using (System.Web.Hosting.HostingEnvironment.Impersonate())
                {
                    using (var clientContext = new SPClient.ClientContext(new Uri(fullSiteUrl)))
                    {
                        clientContext.Credentials = GetSharePointAdminUserCredentials();

                        SPClient.Web site = clientContext.Web;

                        SPClient.List docLib = site.Lists.GetByTitle(documentLibraryName);
                        clientContext.Load(docLib);

                        SPClient.CamlQuery caml = new SPClient.CamlQuery();
                        caml.ViewXml = $"<View><Query><Where><Eq><FieldRef Name=\"FileLeafRef\"/><Value Type=\"Text\">{nombreArchivo}</Value></Eq></Where></Query></View>";

                        SPClient.ListItemCollection items = docLib.GetItems(caml);

                        clientContext.Load(items);
                        clientContext.ExecuteQuery();

                        foreach (SPClient.ListItem item in items)
                        {

                            if (item.FileSystemObjectType == Microsoft.SharePoint.Client.FileSystemObjectType.File)
                            {
                                var documentEntity = new ProjectDocumentDto();

                                Microsoft.SharePoint.Client.File file = item.File;
                                documentEntity.FileStream = file.OpenBinaryStream();

                                clientContext.Load(file);
                                clientContext.ExecuteQuery();

                                using (HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK))
                                {
                                    using (System.IO.MemoryStream mStream = new System.IO.MemoryStream())
                                    {
                                        if (documentEntity.FileStream != null)
                                        {
                                            documentEntity.FileStream.Value.CopyTo(mStream);
                                            mStream.Seek(0, SeekOrigin.Begin);
                                            byte[] buf = new byte[mStream.Length];
                                            mStream.Read(buf, 0, buf.Length);
                                            documentEntity.BytesArray = buf;
                                            var result = new HttpResponseMessage(HttpStatusCode.OK)
                                            {
                                                Content = new ByteArrayContent(mStream.ToArray())
                                            };
                                            return result.Content;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private static ICredentials GetSharePointAdminUserCredentials()
        {
            var completeUserName = @"DNPAZURE\dnpadmin";
            var password = "DNPAZURE2016+";

            if (!string.IsNullOrEmpty(completeUserName) && !string.IsNullOrEmpty(password))
            {
                var usernamecomponets = completeUserName.Split('\\');
                if (usernamecomponets != null && usernamecomponets.Length == 2)
                    return new System.Net.NetworkCredential(usernamecomponets[1], password, usernamecomponets[0]);

                else throw new ApplicationException("La configuracion del usuario de administracion de Sharepoint esta incorrecta.");
            }
            return System.Net.CredentialCache.DefaultNetworkCredentials;
        }

        public async Task<Guid?> ObtenerIdAplicacionPorBpin(string idObjetoNegocio, string usuarioDnp)
        {
            var endPoint = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerIdAplicacionPorBpin"];
            var parametros = $"?idObjetoNegocio={idObjetoNegocio}";

            var idAplicacion = JsonConvert.DeserializeObject<Guid?>(
                await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, endPoint, uriMetodo, parametros, null, usuarioDnp));

            return idAplicacion;
        }
        #endregion Métodos
    }
}
