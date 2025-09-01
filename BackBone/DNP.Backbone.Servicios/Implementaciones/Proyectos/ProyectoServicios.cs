namespace DNP.Backbone.Servicios.Implementaciones.Proyectos
{
    using Comunes.Dto;
    using DNP.Backbone.Comunes.Enums;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Enums;
    using DNP.Backbone.Servicios.Interfaces;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using Dominio.Dto.Proyecto;
    using Interfaces.Autorizacion;
    using Interfaces.ServiciosNegocio;
    using Interfaces.Tramites;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using DNP.Backbone.Comunes.Utilidades;
    using Newtonsoft.Json.Linq;


    using System.Net.Http;
    using System.Net.Http.Headers;
    using Interfaces.Auditoria;
    using DNP.Backbone.Dominio.Dto.CadenaValor;
    using DNP.Backbone.Dominio.Dto.CostoActividades;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Beneficiarios;
    using DNP.Backbone.Dominio.Dto.Transversales;
    using DNP.Backbone.Dominio.Dto.SeguimientoControl;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using System.IO.Compression;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSharpCode.SharpZipLib.Core;
    using DNP.Backbone.Persistencia.MongoDB;
    using System.Net;
    using DNP.Backbone.Servicios.Interfaces.ManejadorArchivos;



    /// <summary>
    /// Clase responsable de la gestión de proyectos
    /// </summary>
    public class ProyectoServicios : IProyectoServicios
    {
        private readonly IFlujoServicios _flujoServicios;
        private readonly IAutorizacionServicios _autorizacionNegocioServicios;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly IManejadorArchivosServicios _manejadorArchivosServicios;
        private readonly string ENDPOINT = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        private readonly string ENDPOINTFLUJOS = ConfigurationManager.AppSettings["ApiMotorFlujos"];
        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="flujoServicios">Instancia de servicios de flujos</param>
        /// <param name="autorizacionNegocioServicios">Instancia de servicios de autorizacion de negocio servicios</param>
        public ProyectoServicios(IAutorizacionServicios autorizacionNegocioServicios, IFlujoServicios flujoServicios, IServiciosNegocioServicios serviciosNegocioServicios, IClienteHttpServicios clienteHttpServicios, IManejadorArchivosServicios manejadorArchivosServicios)
        {
            _autorizacionNegocioServicios = autorizacionNegocioServicios;
            _flujoServicios = flujoServicios;
            _serviciosNegocioServicios = serviciosNegocioServicios;
            _manejadorArchivosServicios = manejadorArchivosServicios;
            this._clienteHttpServicios = clienteHttpServicios;
        }

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Consulta de datos de proyectos.</returns>
        public async Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto, string usuarioDNP)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdTipoObjetoNegocio = datosConsulta.IdObjeto,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = proyectoFiltroDto
            };

            var listaObjetosNegocio = await _flujoServicios.ObtenerListaObjetosNegocioConInstanciasActivasYPausadas(parametros);

            //List<NegocioDto> listaObjetosNegocio ;
            //if (parametros.ProyectoFiltro.TipoEntidad == "Territorial")
            //{
            //    listaObjetosNegocio = await _flujoServicios.ObtenerListaObjetosNegocioConInstanciasActivasYPausadas(parametros);
            //}
            //else { listaObjetosNegocio =null; }
            var proyectoDto = new ProyectoDto();

                if (listaObjetosNegocio != null && listaObjetosNegocio.Count > 0)
                {
                    proyectoDto.GruposEntidades = CrearGrupoEntidades(listaObjetosNegocio.OrderBy(o => o.SectorNombre).ThenBy(o => o.NombreEntidad).ToList());
                }
                else
                {
                    proyectoDto.GruposEntidades = new List<GrupoEntidadProyectoDto>();
                }
            
            return proyectoDto;
        }

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <param name="usuarioDNP"></param>   
        /// <returns>string</returns> 
        public async Task<string> ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriValidacionDevolucionPaso"];
            uri = $"{uri}?instanciaId={instanciaId}&accionId={accionId}&accionDevolucionId={accionDevolucionId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);
            if (response != "")
                response = JsonConvert.DeserializeObject<string>(response);
            return response;
        }

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Consulta de datos de proyectos.</returns>
        public Task<ProyectoDto> ObtenerProyectosTodos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdTipoObjetoNegocio = datosConsulta.IdObjeto,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = proyectoFiltroDto
            };

            var listaObjetosNegocio = _flujoServicios.ObtenerListaObjetosNegocioConInstanciasTotales(parametros).Result;

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
        /// Obtención de datos de proyectos para PDF.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>        
        /// <returns>Consulta de datos de proyectos en PDF.</returns>
        public Task<ProyectoDto> ObtenerInfoPDF(InstanciaProyectoDto datosConsulta, string token)
        {
            var proyectos = ObtenerProyectos(datosConsulta.ProyectoParametrosDto, token, datosConsulta.ProyectoFiltroDto, null);

            return proyectos;
        }

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Resumen de los datos del proyecto.</returns>
        public Task<ProyectoResumenDto> ObtenerMonitoreoProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto)
        {
            var entidades = _autorizacionNegocioServicios.ObtenerEntidadesPorTipoEntidadYUsuario(proyectoFiltroDto.TipoEntidad, datosConsulta.IdUsuario).Result;

            var proyectos = _serviciosNegocioServicios.ObtenerProyectos(new ParametrosProyectosDto
            {
                IdsEntidades = entidades.Select(e => e.EntityTypeCatalogOptionId ?? 0).ToList(),
                NombresEstadosProyectos = new List<string>
                    {
                        ProyectoEstadoMga.Formulado.DescriptionAttr(),
                        ProyectoEstadoMga.Viable.DescriptionAttr(),
                        ProyectoEstadoMga.Aprobado.DescriptionAttr(),
                        ProyectoEstadoMga.EnEjecucion.DescriptionAttr()
                     }
            }, datosConsulta.IdUsuario).Result;

            var proyectoDto = new ProyectoResumenDto();
            IEnumerable<ProyectoResumenDto> resumenProyectosDto = new List<ProyectoResumenDto>();

            if (proyectos != null && proyectos.Count > 0)
            {
                var infoFinancieroProyectoFiltro = new InfoFinancieroProyectoFiltroDto
                {
                    ParametrosDto = new ParametrosDto
                    {
                        IdsRoles = datosConsulta.ListaIdsRoles,
                        IdUsuarioDNP = datosConsulta.IdUsuario,
                        TokenAutorizacion = token
                    },
                    AvanceFinanciero = proyectoFiltroDto.AvanceFinanciero,
                    AvanceFisico = proyectoFiltroDto.AvanceFisico,
                    AvanceProyecto = proyectoFiltroDto.AvanceProyecto,
                    Duracion = proyectoFiltroDto.Duracion,
                    PeriodoEjecucion = proyectoFiltroDto.PeriodoEjecucion
                };

                if (proyectoFiltroDto.ProyectosIds != null && proyectoFiltroDto.ProyectosIds.Any())
                    proyectos = proyectos.Where(p => proyectoFiltroDto.ProyectosIds.Contains(p.ProyectoId)).ToList();

                infoFinancieroProyectoFiltro.ProyectosIds = proyectos.Select(p => p.ProyectoId).ToList();
                var infoFinancieroProyectos = _flujoServicios.ObtenerInfoFinancieroProyectos(infoFinancieroProyectoFiltro).Result;
                var gruposEntidades = CrearGrupoEntidadesResumen(proyectos.OrderBy(p => p.SectorNombre).ThenBy(p => p.EntidadNombre).ToList(), infoFinancieroProyectos, proyectoFiltroDto);
                proyectoDto.GruposEntidades = AplicarFiltrosInfoFinancieroProyectos(gruposEntidades, infoFinancieroProyectoFiltro);

                // aplicar filtro por la entidad en el filtro seleccionado
                proyectoDto.GruposEntidades = proyectoDto.GruposEntidades.Select(p =>
                {
                    p.ListaEntidades = p.ListaEntidades.Where(q => q.IdEntidad == (proyectoFiltroDto.EntidadId ?? q.IdEntidad)).ToList();
                    return p;
                }).ToList();

                foreach (var g in proyectoDto.GruposEntidades)
                {
                    foreach (var ep in g.ListaEntidades)
                    {
                        var entidad = entidades.Find(e => e.EntityTypeCatalogOptionId == ep.IdEntidad);
                        if (entidad != null)
                            ep.SectorEntidad = entidad.AgrupadorEntidad;
                    }
                }
            }
            else
            {
                proyectoDto.GruposEntidades = new List<GrupoEntidadProyectoResumenDto>();
            }

            return Task.FromResult(proyectoDto);
        }

        public Task<InstanciaDto> ActivarInstancia(ProyectoParametrosDto datosConsulta, string token)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = new ProyectoFiltroDto(),
                InstanciaId = datosConsulta.InstanciaId
            };

            var instancia = _flujoServicios.ActivarInstancia(parametros).Result;

            return Task.FromResult(instancia);
        }

        public Task<InstanciaDto> PausarInstancia(ProyectoParametrosDto datosConsulta, string token)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = new ProyectoFiltroDto(),
                InstanciaId = datosConsulta.InstanciaId
            };

            var instancia = _flujoServicios.PausarInstancia(parametros).Result;

            return Task.FromResult(instancia);
        }

        public Task<InstanciaDto> DetenerInstancia(ProyectoParametrosDto datosConsulta, string token)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = new ProyectoFiltroDto(),
                InstanciaId = datosConsulta.InstanciaId
            };

            var instancia = _flujoServicios.DetenerInstancia(parametros).Result;

            return Task.FromResult(instancia);
        }

        public Task<InstanciaDto> CancelarInstanciaMisProcesos(ProyectoParametrosDto datosConsulta, string token)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = new ProyectoFiltroDto(),
                InstanciaId = datosConsulta.InstanciaId
            };

            var instancia = _flujoServicios.CancelarInstanciaMisProcesos(parametros).Result;

            return Task.FromResult(instancia);
        }

        async public Task<IEnumerable<ProyectoCreditoDto>> ObtenerContracreditos(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            var proyectosTramite = await _flujoServicios.ObtenerProyectosTramite(ObtenerInstanciaTramite(parametros, usuarioDnp));
            bool? grupoPermitidos = true;
            if (proyectosTramite != null && proyectosTramite.Count > 0)
            {
                grupoPermitidos = proyectosTramite.FirstOrDefault().GruposPermitidos;
            }

            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerContracredito"];

            var contraCredito = JsonConvert.DeserializeObject<IEnumerable<ProyectoCreditoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, parametros, usuarioDnp));
            var result = contraCredito.Where(p => !proyectosTramite.Select(c => c.IdObjetoNegocio).Contains(p.BPIN));
            //result.ToList().ForEach(p => p.GruposPermitidos = grupoPermitidos);
            return result;
        }

        async public Task<IEnumerable<ProyectoCreditoDto>> ObtenerCreditos(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            var proyectosTramite = await _flujoServicios.ObtenerProyectosTramite(ObtenerInstanciaTramite(parametros, usuarioDnp));
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerCredito"];

            var credito = JsonConvert.DeserializeObject<IEnumerable<ProyectoCreditoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, parametros, usuarioDnp));
            var result = credito.Where(p => !proyectosTramite.Select(c => c.IdObjetoNegocio).Contains(p.BPIN));
            return result;
        }

        async public Task<RespuestaGeneralDto> GuardarProyectos(ParametroProyectoTramiteDto parametros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarProyectos"];

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINTFLUJOS, uriMetodo, null, parametros, usuarioDnp));
        }

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Consulta de datos de proyectos.</returns>
        public async Task<ProyectoDto> ObtenerProyectosConsolaProcesos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto, string usuarioDNP)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdTipoObjetoNegocio = datosConsulta.IdObjeto,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = proyectoFiltroDto
            };

            if (!string.IsNullOrEmpty(usuarioDNP))
            {
                var entidadesVisualizador = await _autorizacionNegocioServicios.ObtenerEntidadesConRoleVisualizador(usuarioDNP);
                var entidadesVisualizadorIds = entidadesVisualizador.Where(x => x.EntityTypeCatalogOptionId.HasValue).Select(x => x.EntityTypeCatalogOptionId.Value);

                parametros.EntidadesVisualizador = entidadesVisualizadorIds.ToList();
            }

            var listaObjetosNegocio = await _flujoServicios.ObtenerListaObjetosNegocioConInstanciasActivasYPausadasConsolaProcesos(parametros);

            var proyectoDto = new ProyectoDto();

            if (listaObjetosNegocio != null && listaObjetosNegocio.Count > 0)
            {
                proyectoDto.GruposEntidades = CrearGrupoEntidades(listaObjetosNegocio.OrderBy(o => o.SectorNombre).ThenBy(o => o.NombreEntidad).ToList());
            }
            else
            {
                proyectoDto.GruposEntidades = new List<GrupoEntidadProyectoDto>();
            }

            return proyectoDto;
        }

        /// <summary>
        /// Obtención de token para acceder a la mga.
        /// </summary>
        /// <param name="bpin">codigo del proyecto</param>
        /// <param name="usuarioDNP">id del usuario</param>
        /// <returns>URl con token</returns>
        public async Task<string> ObtenerTokenMGA(string bpin, Dominio.Dto.UsuarioLogadoDto usuarioLogeado, string tipoUsuario, string tokenAutorizacion)
        {
            BPINsProyectosDto bpins = new BPINsProyectosDto()
            {
                BPINs = new List<string> { bpin },
                TokenAutorizacion = tokenAutorizacion
            };

            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarProyectosBPINs"];
            var proyecto = JsonConvert.DeserializeObject<IEnumerable<ProyectoEntidadDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, bpins, usuarioLogeado.IdUsuario));

            var proyectoId = proyecto == null ? 0 : proyecto.FirstOrDefault().ProyectoId;

            var message = string.Concat("ProjectId_", proyectoId, '|',
                              "ResquestTime_", DateTime.Now.ToShortTimeString(), '|',
                              "UserId_", ConvertirTipoDocumentoEquivalente(usuarioLogeado, tipoUsuario), '|',
                              "ReturnBankUrl_", "http://www.google.com.co");
            var token = EncryptStringWithTimeBasedSaltUrl(message);
            var uriMGA = ConfigurationManager.AppSettings["urlMGA"];
            var url = string.Concat(uriMGA, token);
            return url;
        }

        private string ConvertirTipoDocumentoEquivalente(Dominio.Dto.UsuarioLogadoDto usuarioLogeado, string tipoUsuario)
        {
            string tipoDocumento;
            string documentoConvertido = string.Empty;
            string usuarioDNP = usuarioLogeado.IdUsuario;

            if (tipoUsuario == "externo")
            {
                tipoDocumento = LeerTipoDocumentoEquivalente(usuarioDNP.Substring(0, 2)); //itcert|pasaporte|nit
                documentoConvertido = usuarioDNP.Replace(usuarioDNP.Substring(0, 2), tipoDocumento + ":#");
            }
            else
            {
                var cuentaUsuario = _autorizacionNegocioServicios.ObtenerCuentaUsuario(usuarioLogeado.IdUsuario, usuarioLogeado.IdUsuario);
                if (cuentaUsuario != null)
                {
                    if (cuentaUsuario.Result != null)
                    {
                        var cuenta = cuentaUsuario.Result.Cuenta.ToString().Split('@');
                        documentoConvertido = cuenta[0].ToString();
                    }
                }
            }
            return documentoConvertido;
        }

        private string LeerTipoDocumentoEquivalente(string Tipodocumento)
        {
            string TipoDocumentosInicial = "itcert:CC;pasaporte:PA;nit:NI";
            string resultado = string.Empty;
            string TipoDocumentos = TipoDocumentosInicial; // ConfigurationManager.AppSettings["TipoDocumentos"].ToString();
            string[] arregloTipoDocumentos = TipoDocumentos.Split(';');
            string[] homologacionTipoDocumento;

            foreach (string reg in arregloTipoDocumentos)
            {
                homologacionTipoDocumento = reg.Split(':');
                if (homologacionTipoDocumento[1].ToUpper() == Tipodocumento.ToUpper())
                {
                    resultado = homologacionTipoDocumento[0];
                    break;
                }
            }
            return resultado;
        }

        public async Task<string> ObtenerProyectosBpin(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            BPINsProyectosDto bpins = new BPINsProyectosDto()
            {
                BPINs = new List<string> { bpin },
                TokenAutorizacion = tokenAutorizacion
            };

            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarProyectosBPINs"];
            var proyecto = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, bpins, usuarioDNP);
            if (string.IsNullOrEmpty(proyecto)) return string.Empty;
            return proyecto;
        }

        #region Métodos Privados
        private List<GrupoEntidadProyectoResumenDto> AplicarFiltrosInfoFinancieroProyectos(List<GrupoEntidadProyectoResumenDto> grupoEntidades, InfoFinancieroProyectoFiltroDto infoFinancieroProyectoFiltro)
        {
            if (!string.IsNullOrEmpty(infoFinancieroProyectoFiltro.AvanceFinanciero))
                grupoEntidades?.ForEach(grupo =>
                    grupo.ListaEntidades?.ToList()?.ForEach(entidad =>
                        entidad.ObjetosNegocio = entidad.ObjetosNegocio?.Where(proyectoResumen => proyectoResumen.AvanceFinanciero != null && proyectoResumen.AvanceFinanciero.ToUpper().Contains(infoFinancieroProyectoFiltro.AvanceFinanciero.ToUpper())).ToList()));
            if (!string.IsNullOrEmpty(infoFinancieroProyectoFiltro.AvanceFisico))
                grupoEntidades?.ForEach(grupo =>
                    grupo.ListaEntidades?.ToList()?.ForEach(entidad =>
                        entidad.ObjetosNegocio = entidad.ObjetosNegocio?.Where(proyectoResumen => proyectoResumen.AvanceFisico != null && proyectoResumen.AvanceFisico.ToUpper().Contains(infoFinancieroProyectoFiltro.AvanceFisico.ToUpper())).ToList()));
            if (!string.IsNullOrEmpty(infoFinancieroProyectoFiltro.AvanceProyecto))
                grupoEntidades?.ForEach(grupo =>
                    grupo.ListaEntidades?.ToList()?.ForEach(entidad =>
                        entidad.ObjetosNegocio = entidad.ObjetosNegocio?.Where(proyectoResumen => proyectoResumen.AvanceProyecto != null && proyectoResumen.AvanceProyecto.ToUpper().Contains(infoFinancieroProyectoFiltro.AvanceProyecto.ToUpper())).ToList()));
            if (!string.IsNullOrEmpty(infoFinancieroProyectoFiltro.Duracion))
                grupoEntidades?.ForEach(grupo =>
                    grupo.ListaEntidades?.ToList()?.ForEach(entidad =>
                        entidad.ObjetosNegocio = entidad.ObjetosNegocio?.Where(proyectoResumen => proyectoResumen.Duracion != null && proyectoResumen.Duracion.ToUpper().Contains(infoFinancieroProyectoFiltro.Duracion.ToUpper())).ToList()));
            if (!string.IsNullOrEmpty(infoFinancieroProyectoFiltro.PeriodoEjecucion))
                grupoEntidades?.ForEach(grupo =>
                    grupo.ListaEntidades?.ToList()?.ForEach(entidad =>
                        entidad.ObjetosNegocio = entidad.ObjetosNegocio?.Where(proyectoResumen => proyectoResumen.PeriodoEjecucion != null && proyectoResumen.PeriodoEjecucion.ToUpper().Contains(infoFinancieroProyectoFiltro.PeriodoEjecucion.ToUpper())).ToList()));

            var filtrados = grupoEntidades.Where(grupo => grupo != null
                                                && grupo.ListaEntidades != null
                                                && grupo.ListaEntidades.Count > 0
                                                && grupo.ListaEntidades.Where(entidad => entidad.ObjetosNegocio != null && entidad.ObjetosNegocio.Count > 0).Count() > 0).ToList();
            filtrados.ForEach(grupo =>
                grupo.ListaEntidades = grupo.ListaEntidades.Where(entidad => entidad.ObjetosNegocio != null
                                                               && entidad.ObjetosNegocio.Count > 0).ToList());
            return filtrados;
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

        /// <summary>
        /// Obtención de grupo para datos de proyectos.
        /// </summary>
        /// <param name="listaObjetosNegocio">Lista de objeto negocio</param>        
        /// <returns>Consulta de grupos de datos de proyectos.</returns>
        private List<GrupoEntidadProyectoResumenDto> CrearGrupoEntidadesResumen(List<NegocioDto> listaObjetosNegocio, List<InfoFinancieroProyectoDto> infoFinancieroProyectos)
        {
            var lst = listaObjetosNegocio.GroupBy(x => x.IdObjetoNegocio).Select(x => x.First()).ToList();
            List<NegocioResumenDto> listaObjetosNegocioResumen = lst.Select(o => new NegocioResumenDto
            {
                CodigoBpin = o.IdObjetoNegocio,
                EstadoProyecto = o.EstadoProyecto,
                ProyectoId = o.ProyectoId.GetValueOrDefault(),
                ProyectoNombre = o.NombreObjetoNegocio,
                Horizonte = o.Horizonte,
                TieneAlertas = false,
                TieneRelatorios = false,
                FechaInicial = DateTime.MinValue,
                FechaFinal = DateTime.MinValue,
                IdEntidad = o.IdEntidad,
                TipoEntidad = o.TipoEntidad,
                NombreEntidad = o.NombreEntidad,
                DescripcionCR = o.DescripcionCR,
                SectorNombre = o.SectorNombre,
                AvanceFinanciero = infoFinancieroProyectos
                                   ?.Where(i => i.ProyectoId == o.ProyectoId.GetValueOrDefault())
                                   .FirstOrDefault()
                                   ?.AvanceFinanciero,
                AvanceFisico = infoFinancieroProyectos
                               ?.Where(i => i.ProyectoId == o.ProyectoId.GetValueOrDefault())
                               .FirstOrDefault()
                               ?.AvanceFisico,
                AvanceProyecto = infoFinancieroProyectos
                                 ?.Where(i => i.ProyectoId == o.ProyectoId.GetValueOrDefault())
                                 .FirstOrDefault()
                                 ?.AvanceProyecto,
                Duracion = infoFinancieroProyectos
                           ?.Where(i => i.ProyectoId == o.ProyectoId.GetValueOrDefault())
                           .FirstOrDefault()
                           ?.Duracion,
                PeriodoEjecucion = infoFinancieroProyectos
                                   ?.Where(i => i.ProyectoId == o.ProyectoId.GetValueOrDefault())
                                   .FirstOrDefault()
                                   ?.PeriodoEjecucion,
                SectorId = o.SectorId,
                EstadoId = o.EstadoProyectoId
            }).ToList();

            var grupoPorEntidades = from ge in listaObjetosNegocioResumen
                                    group ge by new { ge.IdEntidad, ge.NombreEntidad, ge.TipoEntidad }
                                    into g
                                    select new EntidadProyectoResumenDto()
                                    {
                                        IdEntidad = g.Key.IdEntidad,
                                        NombreEntidad = g.Key.NombreEntidad,
                                        TipoEntidad = g.Key.TipoEntidad,
                                        ObjetosNegocio = g.ToList()
                                    };

            var grupoPorTipoEntidad = (from gte in grupoPorEntidades
                                       group gte by new { gte.TipoEntidad }
                                       into g
                                       select new GrupoEntidadProyectoResumenDto()
                                       {
                                           TipoEntidad = g.Key.TipoEntidad,
                                           ListaEntidades = g.ToList()
                                       }).ToList();

            return grupoPorTipoEntidad;
        }

        private List<GrupoEntidadProyectoResumenDto> CrearGrupoEntidadesResumen(List<Dominio.Dto.ProyectosEntidadesDto> listaProyectos, List<InfoFinancieroProyectoDto> infoFinancieroProyectos, ProyectoFiltroDto proyectoFiltroDto)
        {
            var lst = listaProyectos.GroupBy(x => x.CodigoBpin).Select(x => x.First()).ToList();
            List<NegocioResumenDto> listaObjetosNegocioResumen = lst.Select(o => new NegocioResumenDto
            {
                CodigoBpin = o.CodigoBpin,
                EstadoProyecto = o.Estado,
                ProyectoId = o.ProyectoId,
                ProyectoNombre = o.ProyectoNombre,
                //Horizonte = o.Horizonte,
                TieneAlertas = false,
                TieneRelatorios = false,
                FechaInicial = DateTime.MinValue,
                FechaFinal = DateTime.MinValue,
                IdEntidad = o.EntidadId,
                //TipoEntidad = o.,
                NombreEntidad = o.EntidadNombre,
                //DescripcionCR = o.DescripcionCR,
                SectorNombre = o.SectorNombre,
                AvanceFinanciero = infoFinancieroProyectos
                                   ?.Where(i => i.ProyectoId == o.ProyectoId)
                                   .FirstOrDefault()
                                   ?.AvanceFinanciero,
                AvanceFisico = infoFinancieroProyectos
                               ?.Where(i => i.ProyectoId == o.ProyectoId)
                               .FirstOrDefault()
                               ?.AvanceFisico,
                AvanceProyecto = infoFinancieroProyectos
                                 ?.Where(i => i.ProyectoId == o.ProyectoId)
                                 .FirstOrDefault()
                                 ?.AvanceProyecto,
                Duracion = infoFinancieroProyectos
                           ?.Where(i => i.ProyectoId == o.ProyectoId)
                           .FirstOrDefault()
                           ?.Duracion,
                PeriodoEjecucion = infoFinancieroProyectos
                                   ?.Where(i => i.ProyectoId == o.ProyectoId)
                                   .FirstOrDefault()
                                   ?.PeriodoEjecucion,
                SectorId = o.SectorId,
                EstadoId = o.EstadoId
            }).ToList();

            Filtrar(ref listaObjetosNegocioResumen, proyectoFiltroDto);

            var grupoPorEntidades = from ge in listaObjetosNegocioResumen
                                    group ge by new { ge.IdEntidad, ge.NombreEntidad, ge.TipoEntidad }
                                    into g
                                    select new EntidadProyectoResumenDto()
                                    {
                                        IdEntidad = g.Key.IdEntidad,
                                        NombreEntidad = g.Key.NombreEntidad,
                                        TipoEntidad = g.Key.TipoEntidad,
                                        ObjetosNegocio = g.ToList()
                                    };

            var grupoPorTipoEntidad = (from gte in grupoPorEntidades
                                       group gte by new { gte.TipoEntidad }
                                       into g
                                       select new GrupoEntidadProyectoResumenDto()
                                       {
                                           TipoEntidad = g.Key.TipoEntidad,
                                           ListaEntidades = g.ToList()
                                       }).ToList();

            return grupoPorTipoEntidad;
        }

        private void Filtrar(ref List<NegocioResumenDto> listaObjetosNegocioResumen, ProyectoFiltroDto proyectoFiltroDto)
        {
            if (!string.IsNullOrEmpty(proyectoFiltroDto.Bpin))
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => !string.IsNullOrEmpty(p.CodigoBpin) && p.CodigoBpin.Contains(proyectoFiltroDto.Bpin)).ToList();
            }
            if (!string.IsNullOrEmpty(proyectoFiltroDto.Nombre))
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => !string.IsNullOrEmpty(p.ProyectoNombre) && p.ProyectoNombre.Contains(proyectoFiltroDto.Nombre)).ToList();
            }
            if (proyectoFiltroDto.EstadoProyectoId.HasValue)
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => p.EstadoId == proyectoFiltroDto.EstadoProyectoId).ToList();
            }
            if (!string.IsNullOrEmpty(proyectoFiltroDto.AvanceFinanciero))
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => !string.IsNullOrEmpty(p.AvanceFinanciero) && p.AvanceFinanciero.Contains(proyectoFiltroDto.AvanceFinanciero)).ToList();
            }
            if (!string.IsNullOrEmpty(proyectoFiltroDto.AvanceFisico))
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => !string.IsNullOrEmpty(p.AvanceFisico) && p.AvanceFisico.Contains(proyectoFiltroDto.AvanceFisico)).ToList();
            }
            if (!string.IsNullOrEmpty(proyectoFiltroDto.AvanceProyecto))
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => !string.IsNullOrEmpty(p.AvanceProyecto) && p.AvanceProyecto.Contains(proyectoFiltroDto.AvanceProyecto)).ToList();
            }
            if (!string.IsNullOrEmpty(proyectoFiltroDto.Duracion))
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => !string.IsNullOrEmpty(p.Duracion) && p.Duracion.Contains(proyectoFiltroDto.Duracion)).ToList();
            }
            if (!string.IsNullOrEmpty(proyectoFiltroDto.PeriodoEjecucion))
            {
                listaObjetosNegocioResumen = listaObjetosNegocioResumen.Where(p => !string.IsNullOrEmpty(p.PeriodoEjecucion) && p.PeriodoEjecucion.Contains(proyectoFiltroDto.PeriodoEjecucion)).ToList();
            }
        }
        private InstanciaTramiteDto ObtenerInstanciaTramite(ProyectoCreditoParametroDto parametros, string usuarioDnp)
        {
            var instanciaTramite = new InstanciaTramiteDto();
            instanciaTramite.InstanciaId = parametros.IdInstancia;
            var parametrosInbox = new ParametrosInboxDto();
            parametrosInbox.IdUsuario = usuarioDnp;
            var tramiteFiltro = new TramiteFiltroDto();
            tramiteFiltro.InstanciaId = parametros.IdInstancia;
            instanciaTramite.ParametrosInboxDto = parametrosInbox;
            instanciaTramite.TramiteFiltroDto = tramiteFiltro;
            return instanciaTramite;

        }

        #endregion
        #region Métodos Privados MGA
        private string EncryptStringWithTimeBasedSalt(string toEncrypt, DateTime currentDate)
        {

            byte[] toEncryptBytes = Encoding.UTF8.GetBytes(toEncrypt);
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = AddSalt(EncryptionKey, currentDate);
                provider.Mode = CipherMode.CBC;
                provider.Padding = PaddingMode.PKCS7;
                using (ICryptoTransform encryptor = provider.CreateEncryptor(provider.Key, provider.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(toEncryptBytes, 0, toEncryptBytes.Length);
                            cs.FlushFinalBlock();
                            var retVal = new byte[16 + ms.Length];
                            provider.IV.CopyTo(retVal, 0);
                            ms.ToArray().CopyTo(retVal, 16);
                            return Convert.ToBase64String(retVal);
                        }
                    }
                }
            }


        }

        private byte[] AddSalt(byte[] lowSodiumKey, DateTime currentDate)
        {
            int salt = TimeZoneInfo.ConvertTime(currentDate, MovistarTimeZone).DayOfYear;

            if (salt > 255)
            {
                lowSodiumKey[0] = 0xFE;
                lowSodiumKey[7] = (byte)(salt - 255);
            }
            else
            {
                lowSodiumKey[15] = (byte)(salt);
            }
            return lowSodiumKey;
        }


        private static readonly TimeZoneInfo MovistarTimeZone = TimeZoneInfo.CreateCustomTimeZone("Custom Time",
            new TimeSpan(-5, 0, 0), "Custom Timezone",
            "Custom Time");

        private static byte[] EncryptionKey = Encoding.ASCII.GetBytes("FBKmKC0sLb0IX5^796*6jNoYBqOtz0Q0");

        private string EncryptStringWithTimeBasedSaltUrl(string toEncrypt)
        {
            return Base64ForUrlEncode(EncryptStringWithTimeBasedSalt(toEncrypt, DateTime.UtcNow));
        }

        private string Base64ForUrlEncode(string str)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(str);
            return HttpServerUtility.UrlTokenEncode(encbuff);
        }


        #endregion


        async public Task<RespuestaGeneralDto> actualizarHorizonte(HorizonteProyectoDto parametrosHorizonte, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriActualizarHorizonte"];

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, parametrosHorizonte, usuarioDnp));
        }

        public async Task<IndicadorProductoDto> ObtenerIndicadoresProducto(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerIndicadoresProducto"] + "/" + bpin;

            var indicaadoresProducto = JsonConvert.DeserializeObject<IndicadorProductoDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, bpin, usuarioDNP));

            var indicadorPrd = new IndicadorProductoDto();

            if (indicaadoresProducto != null)
            {
                indicadorPrd = indicaadoresProducto;
            }
            return indicadorPrd;
        }

        public async Task<IndicadorResponse> GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriGuardarIndicadoresSecundarios"];

            return JsonConvert.DeserializeObject<IndicadorResponse>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<IndicadorResponse> EliminarIndicadorProducto(int indicadorId, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriEliminarIndicadorProducto"] + "/" + indicadorId;

            return JsonConvert.DeserializeObject<IndicadorResponse>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, indicadorId, usuarioDNP));
        }

        public async Task<ObjectivosAjusteDto> ObtenerResumenObjetivosProductosActividades(string bpin, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerResumenObjetivosProductosActividades"] + "?bpin=" + bpin;

            return JsonConvert.DeserializeObject<ObjectivosAjusteDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, bpin, usuarioDNP));
        }

        public async Task<ReponseHttp> GuardarCostoActividades(ProductoAjusteDto producto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarCostoActividades"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, producto, usuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }

        public async Task<IndicadorResponse> ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriActualizarMetaAjusteIndicador"];

            return JsonConvert.DeserializeObject<IndicadorResponse>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, Indicador, usuarioDNP));
        }

        public async Task<string> AgregarEntregable(AgregarEntregable[] entregables, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriAgregarEntregable"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, entregables, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarEntregable(EntregablesActividadesDto entregable, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarEntregable"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, entregable, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public Task<List<InstanciaDto>> DevolverInstanciasHijas(ProyectoParametrosDto datosConsulta, string token)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = new ProyectoFiltroDto(),
                InstanciaId = datosConsulta.InstanciaId
            };

            var instancia = _flujoServicios.DevolverInstanciasHijas(parametros).Result;

            return Task.FromResult(instancia);
        }

        public async Task<List<IndicadorCapituloModificadoDto>> IndicadoresValidarCapituloModificado(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriIndicadoresValidarCapituloModificado"] + "/" + bpin;

            var indicaadoresProducto = JsonConvert.DeserializeObject<List<IndicadorCapituloModificadoDto>>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, bpin, usuarioDNP));

            var indicadorPrd = new List<IndicadorCapituloModificadoDto>();

            if (indicaadoresProducto != null)
            {
                indicadorPrd = indicaadoresProducto;
            }
            return indicadorPrd;
        }

        public async Task<RegionalizacionDto> RegionalizacionGeneral(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriRegionalizacionGeneral"] + "/" + bpin;

            var regionalizacion = JsonConvert.DeserializeObject<RegionalizacionDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, bpin, usuarioDNP));

            var regionalizacionPrd = new RegionalizacionDto();

            if (regionalizacion != null)
            {
                regionalizacionPrd = regionalizacion;
            }
            return regionalizacionPrd;
        }

        public async Task<RespuestaGeneralDto> GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuarioDNP)
        {
            //var dato = JsonUtilidades.ACadenaJson(regionalizacionFuenteAjuste);            
            var uriMetodo = ConfigurationManager.AppSettings["UriGuardarRegionalizacionFuentesFinanciacionAjustes"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, regionalizacionFuenteAjuste, usuarioDNP));

        }

        public async Task<ObjectivosAjusteJustificacionDto> ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerResumenObjetivosProductosActividadesJustificacion"] + "?bpin=" + bpin;

            return JsonConvert.DeserializeObject<ObjectivosAjusteJustificacionDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, null, bpin, usuarioDNP));
        }

        public async Task<LocalizacionJustificacionProyectoDto> ObtenerJustificacionLocalizacionProyecto(int proyectoId, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerJustificacionLocalizacionProyecto"];
            uriMetodo += "?idProyecto=" + proyectoId;
            return JsonConvert.DeserializeObject<LocalizacionJustificacionProyectoDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, proyectoId, usuarioDNP));
        }

        public async Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuarioDNP)
        {
            //var dato = JsonUtilidades.ACadenaJson(regionalizacionFuenteAjuste);            
            var uriMetodo = ConfigurationManager.AppSettings["UriGuardarFocalizacionCategoriasAjustes"];
            var response = JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, null, focalizacionCategoriasAjuste, usuarioDNP));
            return response;

        }

        public async Task<string> ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriDetalleAjustesJustificaionRegionalizacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<List<ProyectoInstanciaDto>> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerInstanciaProyectoTramite"];
            uri += "?InstanciaId=" + InstanciaId + "&BPIN=" + BPIN + "&usuarioDNP=" + usuarioDNP;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);

            return JsonConvert.DeserializeObject<List<ProyectoInstanciaDto>>(respuesta);
        }

        public async Task<string> ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerSeccionOtrasPoliticasFacalizacionPT"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectosBeneficiarios"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerProyectosBeneficiariosDetalle(string json, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectosBeneficiariosDetalle"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uri, $"?json={json}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerJustificacionProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerJustificacionProyectosBeneficiarios"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarBeneficiarioTotales"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarBeneficiarioProducto"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarBeneficiarioProductoLocalizacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["GuardarBeneficiarioProductoLocalizacionCaracterizacion"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerSeccionPoliticaFocalizacionDT(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerSeccionPoliticaFocalizacionDT"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriGuardarReprogramacionPorProductoVigencia"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uri, null, reprogramacionValores, usuarioDNP));
        }
        public async Task<SoportesDto> ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos, string usuarioDNP)
        {
            SoportesDto soportes = new SoportesDto();
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerSoportesProyecto"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, filtroDocumentos, usuarioDNP, useJWTAuth: false);
            soportes = JsonConvert.DeserializeObject<SoportesDto>(respuesta);
            if (filtroDocumentos.proceso != "Migracion")
            {
                var urlBaseDoc = ConfigurationManager.AppSettings["ApiMotorFlujos"];
                var uriDoc = ConfigurationManager.AppSettings["uriDocumentosPIIP"] + "?proyectoId=" + filtroDocumentos.proyectoId;

                var respuestaPiip = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBaseDoc, uriDoc, null, null, usuarioDNP, useJWTAuth: false);
                var documentosPIIP = JsonConvert.DeserializeObject<List<DocumentosPIIPDto>>(respuestaPiip);
                soportes.Documentos = new List<DocumentosDto>();
                foreach (var item in documentosPIIP.Where(x=>x.NombreProceso.Equals(filtroDocumentos.proceso)))
                {
                    Dictionary<string, object> parametros = new Dictionary<string, object>
                    {
                        { "idinstancia", item.InstanciaId.ToString() },
                        { "idaccion", item.AccionId.ToString() },
                        { "idnivel", item.IdNivel.ToString() }
                    };
                    var resultDocMongo = _manejadorArchivosServicios.ObtenerListadoArchivos("tramites", parametros, usuarioDNP).Result.FirstOrDefault();
                    if (resultDocMongo != null)
                    {
                        soportes.Documentos.Add(new DocumentosDto()
                        {
                            Id = item.ProyectoId,
                            ProyectoId = item.ProyectoId,
                            Origen = item.Origen,
                            Vigencia = Convert.ToInt32(item.Vigencia),
                            Periodo = item.Periodo,
                            OrigenCompleto = item.OrigenCompleto,
                            NombreDocumento = resultDocMongo != null ? resultDocMongo.nombre : string.Empty,
                            FechaCreacion = resultDocMongo != null ? resultDocMongo.fecha.ToShortDateString() : string.Empty,
                            ProcesoOrigen = item.NombreProceso,
                            TipoDocumento = resultDocMongo.metadatos["tipodocumento"].ToString(),
                            UrlArchivo = resultDocMongo != null ? resultDocMongo.urlArchivo : string.Empty,
                            idArchivoBlob = resultDocMongo.metadatos["idarchivoblob"].ToString(),
                            ContenType = resultDocMongo.metadatos["contenttype"].ToString(),
                            Descripcion = resultDocMongo.metadatos["descripcion"].ToString(),
                            DocumentoDatos = resultDocMongo.metadatos["datosdocumento"].ToString(),
                            NumeroProceso = item.CodigoProceso
                        });
                    }
                }
                if (!string.IsNullOrWhiteSpace(filtroDocumentos.origen))
                    soportes.Documentos = soportes.Documentos.Where(x => x.Origen.Contains(filtroDocumentos.origen)).ToList();

                if (!string.IsNullOrWhiteSpace(filtroDocumentos.vigencia))
                    soportes.Documentos = soportes.Documentos.Where(x=>x.Vigencia.ToString().Contains(filtroDocumentos.vigencia)).ToList();

                if (!string.IsNullOrWhiteSpace(filtroDocumentos.periodo))
                    soportes.Documentos = soportes.Documentos.Where(x => x.Periodo.Contains(filtroDocumentos.periodo)).ToList();

                if (!string.IsNullOrWhiteSpace(filtroDocumentos.numeroProceso))
                    soportes.Documentos = soportes.Documentos.Where(x => x.NumeroProceso.Contains(filtroDocumentos.numeroProceso)).ToList();

                if(!string.IsNullOrWhiteSpace(filtroDocumentos.tipoDocumento))
                    soportes.Documentos = soportes.Documentos.Where(x=> x.TipoDocumento.Contains(filtroDocumentos.tipoDocumento)).ToList();

                if(!string.IsNullOrWhiteSpace(filtroDocumentos.NombreDocumento))
                    soportes.Documentos = soportes.Documentos.Where(x => x.NombreDocumento.Contains(filtroDocumentos.NombreDocumento)).ToList();

                
            }
            return soportes;
        }

        public async Task<PlanNacionalDesarrolloDto> ObtenerPND(int idProyecto, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriObtenerPlanNacionalDesarrollo"];
            uri += "?idProyecto=" + idProyecto;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, string.Empty, null, usuarioDNP, useJWTAuth: false);

            return JsonConvert.DeserializeObject<PlanNacionalDesarrolloDto>(respuesta);
        }

        /// <summary>
        /// Obtención de datos de proyectos por idUsuario, idObjeto y aplicación.
        /// </summary>
        /// <param name="datosConsulta">Datos de consulta</param>
        /// <param name="token">token</param>
        /// <param name="proyectoFiltroDto">filtro de lista</param>
        /// <returns>Consulta de datos de proyectos.</returns>
        public async Task<ProyectoVerificacionOcadPazDto> ObtenerProyectosVerificacionOcadPazSgr(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroVerificacionOcadPazSgrDto proyectoFiltroDto)
        {
            ParametrosObjetosNegocioDto parametros = new ParametrosObjetosNegocioDto()
            {
                IdsRoles = datosConsulta.ListaIdsRoles,
                IdTipoObjetoNegocio = datosConsulta.IdObjeto,
                IdUsuarioDNP = datosConsulta.IdUsuario,
                TokenAutorizacion = token,
                ProyectoFiltro = proyectoFiltroDto
            };

            var listaObjetosNegocio = await _flujoServicios.ObtenerListaObjetosNegocioConInstanciasActivasYPausadasVerificacionOcadPazSgr(parametros);

            var proyectoDto = new ProyectoVerificacionOcadPazDto();

            if (listaObjetosNegocio != null && listaObjetosNegocio.Count > 0)
            {
                proyectoDto.GruposEntidades = CrearGrupoEntidadesVerificacionOcadPaz(listaObjetosNegocio.OrderBy(o => o.SectorNombre).ThenBy(o => o.NombreEntidad).ToList());
            }
            else
            {
                proyectoDto.GruposEntidades = new List<GrupoEntidadProyectoVerificacionOcadPazDto>();
            }

            return proyectoDto;
        }

        /// <summary>
        /// Obtención de grupo para datos de proyectos.
        /// </summary>
        /// <param name="listaObjetosNegocio">Lista de objeto negocio</param>        
        /// <returns>Consulta de grupos de datos de proyectos.</returns>
        private List<GrupoEntidadProyectoVerificacionOcadPazDto> CrearGrupoEntidadesVerificacionOcadPaz(List<NegocioVerificacionOcadPazDto> listaObjetosNegocio)
        {
            var grupoPorEntidades = from ge in listaObjetosNegocio
                                    group ge by new { ge.IdEntidad, ge.NombreEntidad, ge.TipoEntidad }
                                    into g
                                    select new EntidadProyectoVerificacionOcadPazDto
                                    {
                                        IdEntidad = g.Key.IdEntidad,
                                        NombreEntidad = g.Key.NombreEntidad,
                                        TipoEntidad = g.Key.TipoEntidad,
                                        ObjetosNegocio = g.ToList()
                                    };

            var grupoPorTipoEntidad = (from gte in grupoPorEntidades
                                       group gte by new { gte.TipoEntidad }
                                       into g
                                       select new GrupoEntidadProyectoVerificacionOcadPazDto
                                       {
                                           TipoEntidad = g.Key.TipoEntidad,
                                           ListaEntidades = g.ToList()
                                       }).ToList();

            return grupoPorTipoEntidad;
        }
    }
}
