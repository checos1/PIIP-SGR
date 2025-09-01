namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Proyectos
{
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.Reportes;
    using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosTransaccional.Persistencia.Interfaces.Transversales;
    using DNP.ServiciosTransaccional.Servicios.Dto;
    using DNP.ServiciosTransaccional.Servicios.Interfaces;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas;
    using DNP.ServiciosTransaccional.Servicios.Interfaces.ManejadorArchivos;
    using Interfaces.Proyectos;
    using Interfaces.Transversales;
    using Newtonsoft.Json;
    using Persistencia.Interfaces.Proyecto;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Enum;
    using ServiciosNegocio.Dominio.Dto.Transferencias;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class ProyectoServicio : ServicioBase<ObjetoNegocio>, IProyectoServicio
    {
        private readonly IProyectoPersistencia _proyectoPersistencia;
        private readonly IAuditoriaServicios _auditoriaServicios;
        private readonly IProyectoSGRPersistencia _proyectoSGRPersistencia;
        private readonly IFichaServicios _fichaServicio;
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly IManejadorArchivosServicio _manejadorArchivosServicios;
        private readonly IParametrosPersistencia _parametrosPersistencia;
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public ProyectoServicio(IProyectoPersistencia proyectoPersistencia,
            IAuditoriaServicios auditoriaServicios,
            IProyectoSGRPersistencia proyectoSGRPersistencia, IFichaServicios fichaServicio, IClienteHttpServicios clienteHttpServicios
            , IManejadorArchivosServicio manejadorArchivosServicios
            , IParametrosPersistencia parametrosPersistencia
            ) : base(auditoriaServicios)
        {
            _proyectoPersistencia = proyectoPersistencia;
            _auditoriaServicios = auditoriaServicios;
            _proyectoSGRPersistencia = proyectoSGRPersistencia;
            _fichaServicio = fichaServicio;
            _clienteHttpServicios = clienteHttpServicios;
            _manejadorArchivosServicios = manejadorArchivosServicios;
            _parametrosPersistencia = parametrosPersistencia;
        }

        public object ActualizarEstado(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _proyectoPersistencia.ActualizarEstado(parametrosActualizar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaEstadoProyecto, parametrosActualizar.Contenido);
            GenerarAuditoria(parametrosActualizar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Modificacion,
                             mensajeAccion);
            return resultado;
        }

        public object ActualizarEstadoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _proyectoSGRPersistencia.ActualizarEstadoSGR(parametrosActualizar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaEstadoProyecto, parametrosActualizar.Contenido);
            GenerarAuditoria(parametrosActualizar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Modificacion,
                             mensajeAccion);
            return resultado;
        }

        public object IniciarFlujoSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _proyectoSGRPersistencia.IniciarFlujoSGR(parametrosActualizar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaEstadoProyecto, parametrosActualizar.Contenido);
            GenerarAuditoria(parametrosActualizar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Modificacion,
                             mensajeAccion);
            return resultado;
        }

        protected override object GuardadoDefinitivo(ParametrosGuardarDto<ObjetoNegocio> parametrosGuardar, string usuario) { throw new System.NotImplementedException(); }

        public object ActualizarNombre(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            var resultado = _proyectoPersistencia.ActualizarNombre(parametrosActualizar, parametrosAuditoria.Usuario);
            var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaEstadoProyecto, parametrosActualizar.Contenido);
            GenerarAuditoria(parametrosActualizar,
                             parametrosAuditoria,
                             parametrosAuditoria.Ip,
                             parametrosAuditoria.Usuario,
                             TipoMensajeEnum.Modificacion,
                             mensajeAccion);
            return resultado;
        }

        public async Task<object> GenerarFichaViabilidadSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            try
            {
                string _generarFicha = _parametrosPersistencia.ConsultarParametro("GenerarFichaViabilidadSGR");
                string flujosNoGenerarFichaAplicarSGR = _parametrosPersistencia.ConsultarParametro("FlujosNoGenerarFichaAplicarSGR");

                if (_generarFicha.Equals("S", StringComparison.OrdinalIgnoreCase) && !flujosNoGenerarFichaAplicarSGR.Contains(parametrosActualizar.Contenido.FlujoId))
                {
                    //await GenerarFichaViabilidad(parametrosActualizar, parametrosAuditoria);
                    await GenerarFichaGenerico(parametrosActualizar, parametrosAuditoria, 0);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<object> GenerarFichaViabilidadSGP(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            try
            {
                string _generarFicha = _parametrosPersistencia.ConsultarParametro("GenerarFichaViabilidadSGP");

                if (_generarFicha.Equals("S", StringComparison.OrdinalIgnoreCase))
                {
                    await GenerarFichaViabilidad(parametrosActualizar, parametrosAuditoria);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Generacion de fichas metodo generico
        /// </summary>     
        /// <param name="parametrosActualizar"></param>   
        /// <param name="parametrosAuditoria"></param>   
        /// <param name="tipoFicha"></param>     
        /// <returns>Task<object></returns> 
        public async Task<object> GenerarFichaGenerico(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria, int tipoFicha)
        {
            #region Transversal

            ObjetoNegocio contenido = parametrosActualizar.Contenido;

            string fechaGeneracion = DateTime.Now.ToString("ddMMyyyy");
            string nombreArchivo = "";
            string urlServicio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            string uriMetodo = ConfigurationManager.AppSettings["uriObtenerConfiguracionReporteSgr"];
            string uri = "";
            string nombreFicha = "";
            string nombrePaso = "";
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            #endregion Transversal        

            #region Ficha Viabilidad

            if (tipoFicha == 0)
            {
                nombreArchivo = nombreFicha + "_" + contenido.ObjetoNegocioId + "_" + fechaGeneracion;
                uri = $"{uriMetodo}?ProyectoId={contenido.ObjetoNegocioId}&instanciaId={contenido.InstanciaId}";

                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                        MetodosServiciosWeb.GetAsync,
                        urlServicio,
                        uri,
                        string.Empty,
                        null,
                        parametrosAuditoria.Usuario,
                        useJWTAuth: false
                );

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    var configuracionReporte = JsonConvert.DeserializeObject<ConfiguracionReportesDto>(jsonResponse);

                    nombreFicha = configuracionReporte.NombreDocumento;
                    nombrePaso = configuracionReporte.NombrePaso;
                    string coleccionBD = configuracionReporte.Coleccion;
                    var _nombreReporte = configuracionReporte.NombreRdl;
                    var _tipoDocumento = configuracionReporte.NombreTipoDocumento;
                    var _tipoDocumentoId = configuracionReporte.TipoDocumentoId;
                    var _descripcion = configuracionReporte.Descripcion;
                    var _nombre = nombreArchivo + ".pdf";
                    var _tramiteId = contenido.ObjetoNegocioId;
                    var _idRolParametro = _parametrosPersistencia.ConsultarParametro("RolViabilidadSGR");
                    var _idRolPorDefecto = Guid.Empty.ToString();
                    var _idRolCliente = contenido.IdRol;
                    var _idRol = (_idRolCliente == null || _idRolCliente == "" || _idRolCliente == _idRolPorDefecto) ? _idRolParametro : _idRolCliente;

                    var idReporte = await Task.Run(() => _fichaServicio.ObtenerPlantillaReporteAnexo(_nombreReporte, parametrosAuditoria.Usuario));

                    // Obtener ficha
                    var ficha = await Task.Run(() => _fichaServicio.ObtenerFichaFisicaSGR(contenido.InstanciaId, contenido.NivelId, _tramiteId, _nombreReporte, idReporte.ID, false, parametrosAuditoria.Usuario));

                    // Guardar ficha
                    parametros = new Dictionary<string, object>
                    {
                        { "_nombre", _nombre },
                        { "_tipoDocumento", _tipoDocumento },
                        { "_tipoDocumentoId", _tipoDocumentoId },
                        { "_idRol", _idRol },
                        { "_descripcion", _descripcion },
                    };

                    GuardarFichaGenerico(contenido, parametrosAuditoria, ficha, nombreFicha, parametros, coleccionBD, nombrePaso);

                    GenerarAuditoriaGlobal(parametrosActualizar, parametrosAuditoria, TipoMensajeEnum.Modificacion, "GenerarFichaViabilidadSGR");
                }

                return true;
            }

            #endregion Ficha Viabilidad

            #region Ficha CTUS SGR

            else if (tipoFicha == 1)
            {
                string _generarFicha = _parametrosPersistencia.ConsultarParametro("GenerarFichaCTUSSGR");

                if (_generarFicha.Equals("S", StringComparison.OrdinalIgnoreCase))
                {
                    nombreArchivo = nombreFicha + "_" + contenido.ObjetoNegocioId + "_" + fechaGeneracion;
                    uri = $"{uriMetodo}?instanciaId={contenido.InstanciaId}";

                    var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                            MetodosServiciosWeb.GetAsync,
                            urlServicio,
                            uri,
                            string.Empty,
                            null,
                            parametrosAuditoria.Usuario,
                            useJWTAuth: false
                    );

                    var configuracionReporte = JsonConvert.DeserializeObject<ConfiguracionReportesDto>(jsonResponse);

                    nombreFicha = configuracionReporte.NombreDocumento;
                    nombrePaso = configuracionReporte.NombrePaso;
                    string coleccionBD = configuracionReporte.Coleccion;
                    var _nombreReporte = configuracionReporte.NombreRdl;
                    var _tipoDocumento = configuracionReporte.NombreTipoDocumento;
                    var _tipoDocumentoId = configuracionReporte.TipoDocumentoId;
                    var _descripcion = configuracionReporte.Descripcion;
                    var _nombre = nombreArchivo + ".pdf";
                    var _tramiteId = contenido.ObjetoNegocioId;

                    uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolAprobadorCtusSgr"];
                    uri = $"{uriMetodo}?ProyectoId={contenido.ObjetoNegocioId}&instanciaId={contenido.InstanciaId}";

                    jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                            MetodosServiciosWeb.GetAsync,
                            urlServicio,
                            uri,
                            string.Empty,
                            null,
                            parametrosAuditoria.Usuario,
                            useJWTAuth: false
                    );

                    var proyectoCtus = JsonConvert.DeserializeObject<ServiciosNegocio.Dominio.Dto.SGR.CTUS.RolApruebaCTUSDto>(jsonResponse);

                    var _idRolParametro = proyectoCtus != null ? proyectoCtus.RolDirectorId.Value.ToString() : _parametrosPersistencia.ConsultarParametro("RolFirmaConceptoViabilidadSGR"); ;
                    var _idRolPorDefecto = Guid.Empty.ToString();
                    var _idRolCliente = contenido.IdRol;
                    var _idRol = (_idRolCliente == null || _idRolCliente == "" || _idRolCliente == _idRolPorDefecto) ? _idRolParametro : _idRolCliente;

                    var idReporte = await Task.Run(() => _fichaServicio.ObtenerPlantillaReporteAnexo(_nombreReporte, parametrosAuditoria.Usuario));

                    // Obtener ficha
                    var ficha = await Task.Run(() => _fichaServicio.ObtenerFichaFisicaSGR(contenido.InstanciaId, contenido.NivelId, _tramiteId, _nombreReporte, idReporte.ID, false, parametrosAuditoria.Usuario));

                    // Guardar ficha
                    parametros = new Dictionary<string, object>
                    {
                        { "_nombre", _nombre },
                        { "_tipoDocumento", _tipoDocumento },
                        { "_tipoDocumentoId", _tipoDocumentoId },
                        { "_idRol", _idRol },
                        { "_descripcion", _descripcion },
                    };

                    GuardarFichaGenerico(contenido, parametrosAuditoria, ficha, nombreFicha, parametros, coleccionBD, nombrePaso);
                }

                return true;
            }

            #endregion Ficha CTUS SGR

            #region Ficha manual

            else if (tipoFicha == 2)
            {
                string ficha = "";
                uri = $"{uriMetodo}?instanciaId={contenido.InstanciaId}";

                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                        MetodosServiciosWeb.GetAsync,
                        urlServicio,
                        uri,
                        string.Empty,
                        null,
                        parametrosAuditoria.Usuario,
                        useJWTAuth: false
                );

                var configuracionReporte = JsonConvert.DeserializeObject<ConfiguracionReportesDto>(jsonResponse);

                nombreFicha = configuracionReporte.NombreDocumento;
                nombrePaso = configuracionReporte.NombrePaso;
                string coleccionBD = configuracionReporte.Coleccion;
                var _nombreReporte = configuracionReporte.NombreRdl;
                var _tipoDocumento = configuracionReporte.NombreTipoDocumento;
                var _tipoDocumentoId = configuracionReporte.TipoDocumentoId;
                var _descripcion = configuracionReporte.Descripcion;
                nombreArchivo = nombreFicha + "_" + _tipoDocumento + contenido.ObjetoNegocioId + "_" + fechaGeneracion;
                var _nombre = nombreArchivo + ".pdf";
                var _tramiteId = contenido.ObjetoNegocioId;
                var _idRolParametro = _parametrosPersistencia.ConsultarParametro("RolFirmaConceptoViabilidadSGR"); ;
                var _idRolPorDefecto = Guid.Empty.ToString();
                var _idRolCliente = contenido.IdRol;
                var _idRol = (string.IsNullOrEmpty(_idRolCliente) || _idRolCliente == _idRolPorDefecto) ? _idRolParametro : _idRolCliente;

                var idReporte = await Task.Run(() => _fichaServicio.ObtenerPlantillaReporteAnexo(_nombreReporte, parametrosAuditoria.Usuario));

                // Obtener ficha
                ficha = await Task.Run(() => _fichaServicio.ObtenerFichaFisicaSGR(contenido.InstanciaId, contenido.NivelId, _tramiteId, _nombreReporte, idReporte.ID, false, parametrosAuditoria.Usuario));

                // Guardar ficha
                parametros = new Dictionary<string, object>
                    {
                        { "_nombre", _nombre },
                        { "_tipoDocumento", _tipoDocumento },
                        { "_tipoDocumentoId", _tipoDocumentoId },
                        { "_idRol", _idRol },
                        { "_descripcion", _descripcion },
                    };

                GuardarFichaGenerico(contenido, parametrosAuditoria, ficha, nombreFicha, parametros, coleccionBD, nombrePaso);

                return ficha;
            }

            #endregion Ficha manual

            else
                return Task.CompletedTask;
        }

        /// <summary>
        /// Metodo generico que guarda las fichas
        /// </summary>     
        /// <param name="contenido"></param>   
        /// <param name="parametrosAuditoria"></param>   
        /// <param name="ficha"></param>  
        /// <param name="nombreFicha"></param>  
        /// <param name="parametros"></param>  
        /// <param name="coleccionBD"></param>      
        /// <param name="nombrePaso"></param> 
        /// <returns>void</returns> 
        private async void GuardarFichaGenerico(ObjetoNegocio contenido, ParametrosAuditoriaDto parametrosAuditoria, string ficha, string nombreFicha, Dictionary<string, object> parametros, string coleccionBD, string nombrePaso)
        {
            string _nombre = (string)parametros["_nombre"];
            string _tipoDocumento = (string)parametros["_tipoDocumento"];
            int _tipoDocumentoId = (int)parametros["_tipoDocumentoId"];
            string _idRol = (string)parametros["_idRol"];
            string _descripcion = (string)parametros["_descripcion"];

            List<Dictionary<string, object>> lstFile = new List<Dictionary<string, object>>();

            Dictionary<string, object> file = new Dictionary<string, object>();
            file.Add("FileName", _nombre);
            file.Add("ContentType", "application/pdf");
            file.Add("ContentLength", ficha.Length);
            file.Add("InputStream", ficha);
            lstFile.Add(file);

            var bytes = Convert.FromBase64String(ficha);
            var fileStream = new MemoryStream(bytes);

            // Version documento
            int _versionDocumentoSoporte = VersionamientoDocumento(contenido.InstanciaId, parametrosAuditoria.Usuario, nombreFicha);

            var metadatos = new
            {
                extension = "pdf",
                idInstancia = contenido.InstanciaId,
                idAccion = contenido.IdAccion,
                tipoDocumento = _tipoDocumento,
                tipoDocumentoId = _tipoDocumentoId,
                idNivel = contenido.NivelId,
                idRol = _idRol,
                descripcion = _descripcion,
                versionDocumentoSoporte = _versionDocumentoSoporte.ToString(),
                pasoDocumento = nombrePaso.ToString(),
                proyectoId = contenido.ObjetoNegocioId.ToString()
            };

            ArchivoViewModelDto archivo = new ArchivoViewModelDto
            {
                FormFile = lstFile,
                Nombre = _nombre,
                Coleccion = coleccionBD,
                Status = "Creado",
                Metadatos = JsonConvert.SerializeObject(metadatos)
            };

            var result = await Task.Run(() => _manejadorArchivosServicios.CargarArchivos(archivo, fileStream, parametrosAuditoria.Usuario));
        }

        private async Task GenerarFichaViabilidad(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            ObjetoNegocio contenido = parametrosActualizar.Contenido;

            var fechaGeneracion = DateTime.Now.ToString("ddMMyyyy");
            var nombreArchivo = "FichaAutomaticaDocumentodeViabilidad_" + contenido.ObjetoNegocioId + "_" + fechaGeneracion;

            string urlServicio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerConfiguracionReporteSgr"];
            var uri = $"{uriMetodo}?ProyectoId={contenido.ObjetoNegocioId}&instanciaId={contenido.InstanciaId}";

            var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.GetAsync,
                    urlServicio,
                    uri,
                    string.Empty,
                    null,
                    parametrosAuditoria.Usuario,
                    useJWTAuth: false
            );

            if (!string.IsNullOrEmpty(jsonResponse))
            {

                var configuracionReporte = JsonConvert.DeserializeObject<ConfiguracionReportesDto>(jsonResponse);

                var _nombreReporte = configuracionReporte.NombreRdl;
                var _tipoDocumento = configuracionReporte.NombreTipoDocumento;
                var _tipoDocumentoId = configuracionReporte.TipoDocumentoId;
                var _descripcion = configuracionReporte.Descripcion;
                var _nombre = nombreArchivo + ".pdf";
                var _tramiteId = contenido.ObjetoNegocioId;
                var _idRolParametro = _parametrosPersistencia.ConsultarParametro("RolViabilidadSGR");
                var _idRolPorDefecto = Guid.Empty.ToString();
                var _idRolCliente = contenido.IdRol;
                var _idRol = (_idRolCliente == null || _idRolCliente == "" || _idRolCliente == _idRolPorDefecto) ? _idRolParametro : _idRolCliente;

                var idReporte = await Task.Run(() => _fichaServicio.ObtenerPlantillaReporteAnexo(_nombreReporte, parametrosAuditoria.Usuario));

                // obtener ficha
                var ficha = await Task.Run(() => _fichaServicio.ObtenerFichaFisicaSGR(contenido.InstanciaId, contenido.NivelId, _tramiteId, _nombreReporte, idReporte.ID, false, parametrosAuditoria.Usuario));

                // guardar ficha
                List<Dictionary<string, object>> lstFile = new List<Dictionary<string, object>>();

                Dictionary<string, object> file = new Dictionary<string, object>();
                file.Add("FileName", _nombre);
                file.Add("ContentType", "application/pdf");
                file.Add("ContentLength", ficha.Length);
                file.Add("InputStream", ficha);
                lstFile.Add(file);

                var bytes = Convert.FromBase64String(ficha);
                var fileStream = new MemoryStream(bytes);

                // Version documento
                int _versionDocumentoSoporte = VersionamientoDocumento(contenido.InstanciaId, parametrosAuditoria.Usuario, "FichaAutomaticaDocumentodeViabilidad");

                var metadatos = new
                {
                    extension = "pdf",
                    idInstancia = contenido.InstanciaId,
                    idAccion = contenido.IdAccion,
                    tipoDocumento = _tipoDocumento,
                    tipoDocumentoId = _tipoDocumentoId,
                    idNivel = contenido.NivelId,
                    idRol = _idRol,
                    descripcion = _descripcion,
                    versionDocumentoSoporte = _versionDocumentoSoporte.ToString()
                };

                ArchivoViewModelDto archivo = new ArchivoViewModelDto
                {
                    FormFile = lstFile,
                    Nombre = _nombre,
                    Coleccion = "tramites",
                    Status = "Creado",
                    Metadatos = JsonConvert.SerializeObject(metadatos)
                };

                var result = await Task.Run(() => _manejadorArchivosServicios.CargarArchivos(archivo, fileStream, parametrosAuditoria.Usuario));

                GenerarAuditoriaGlobal(parametrosActualizar, parametrosAuditoria, TipoMensajeEnum.Modificacion, "GenerarFichaViabilidadSGR");
            }
        }

        /// <summary>
        /// Genera versionamiento de los documentos automaticos
        /// </summary>     
        /// <param name="InstanciaId"></param>   
        /// <param name="usuarioDNP"></param>   
        /// <param name="nombreDoc"></param>     
        /// <returns>int</returns> 
        private int VersionamientoDocumento(string InstanciaId, string usuarioDNP, string nombreDoc)
        {
            Dictionary<string, object> parametrosArchivosMGA = new Dictionary<string, object>();
            parametrosArchivosMGA.Add("idInstancia", InstanciaId);

            var coleccion = "tramites";
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriObtenerListadoArchivosPIIP = ConfigurationManager.AppSettings["uriObtenerListadoArchivosPIIP"] + coleccion;

            string response = _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Post,
                    urlBase,
                    uriObtenerListadoArchivosPIIP,
                    string.Empty,
                    parametrosArchivosMGA,
                    usuarioDNP,
                    useJWTAuth: false).Result;

            var archivos = JsonConvert.DeserializeObject<List<ArchivoInfoDto>>(response);

            var documentoEncontrado = archivos?
                .Where(x => x.nombre.Contains(nombreDoc))
                .OrderByDescending(x => x.fecha)
                .FirstOrDefault();

            var _versionDocumentoSoporte = 1;

            if (documentoEncontrado != null) // Verificar si no es null
            {
                // Asegurarse de que la clave existe y convertir el valor correctamente
                _versionDocumentoSoporte = documentoEncontrado.metadatos != null &&
                                           documentoEncontrado.metadatos.ContainsKey("versiondocumentosoporte")
                    ? Convert.ToInt32(documentoEncontrado.metadatos["versiondocumentosoporte"]) + 1
                    : 1;
            }
            else
                _versionDocumentoSoporte = 1;

            return _versionDocumentoSoporte;
        }

        public async Task<string> SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid instanciaId, string usuarioDnp)
        {
            string urlServiciosNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMensajeProyecto = ConfigurationManager.AppSettings["uriGenerarMensajeEstadoProyectoSgr"];

            var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.GetAsync,
                    urlServiciosNegocio,
                    string.Format("{0}{1}", uriMensajeProyecto, "?instanciaId="),
                    instanciaId.ToString(),
                    null,
                    usuarioDnp,
                    useJWTAuth: false
            );
            var mensajeRetorno = JsonConvert.DeserializeObject<string>(jsonResponse);

            return mensajeRetorno;
        }

        public async Task<string> SGR_Proyectos_PostAplicarFlujoSGR(string FlujoId, string ObjetoNegocioId, Guid instanciaId, string usuarioDnp)
        {
            string urlServiciosNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMensajeProyecto = ConfigurationManager.AppSettings["uriPostAplicarFlujoSGR"];

            var parametros = new DNP.ServiciosTransaccional.Servicios.Dto.AplicarFlujoSGRDto
            {
                ObjetoNegocioId = ObjetoNegocioId,
                instanciaId = instanciaId,
                Usuario = usuarioDnp,
            };

            var response = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.PostAsync,
                    urlServiciosNegocio,
                    uriMensajeProyecto,
                    null,
                    parametros,
                    usuarioDnp,
                    useJWTAuth: false
            );

            return response;
        }

        public async Task<string> SGR_Proyectos_PostDevolverFlujoSGR(string FlujoId, string ObjetoNegocioId, Guid instanciaId, string usuarioDnp)
        {
            string urlServiciosNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMensajeProyecto = ConfigurationManager.AppSettings["uriPostDevolverFlujoSGR"];

            var parametros = new DNP.ServiciosTransaccional.Servicios.Dto.DevolverFlujoSGRDto
            {
                ObjetoNegocioId = ObjetoNegocioId,
                instanciaId = instanciaId,
                Usuario = usuarioDnp
            };

            var response = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.PostAsync,
                    urlServiciosNegocio,
                    uriMensajeProyecto,
                    null,
                    parametros,
                    usuarioDnp,
                    useJWTAuth: false
            );

            return response;
        }

        public async Task<string> SGR_CTUS_CrearInstanciaCtusSGR(ObjetoNegocio objetoNegocio, string usuarioDnp)
        {
            string urlServicio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerProyectoCtusSgr"];
            var uri = $"{uriMetodo}?ProyectoId={objetoNegocio.ObjetoNegocioId}&instanciaId={objetoNegocio.InstanciaId}";

            var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.GetAsync,
                    urlServicio,
                    uri,
                    string.Empty,
                    null,
                    usuarioDnp,
                    useJWTAuth: false
            );

            var proyectoCtus = JsonConvert.DeserializeObject<ProyectoCtusDto>(jsonResponse);

            if (proyectoCtus != null && !proyectoCtus.SolicitaCtus)
            {
                return string.Empty;
            }

            urlServicio = ConfigurationManager.AppSettings["ApiMotorFlujos"];
            uriMetodo = ConfigurationManager.AppSettings["uriRegistroInstancia"];

            string flujoCtus = _parametrosPersistencia.ConsultarParametro("FlujoCTUS");

            var crearInstanciaCtus = new CrearInstanciaDto
            {
                FlujoId = flujoCtus,
                UsuarioId = usuarioDnp,
                RolId = proyectoCtus.RolDirectorId.ToString(),
                ObjetoId = objetoNegocio.ObjetoNegocioId,
                IdInstancia = objetoNegocio.InstanciaId,
                ListaEntidades = new List<int> { proyectoCtus.EntidadDestino.Value },
                Proyectos = new List<ProyectoInstanciaDto> {
                    new ProyectoInstanciaDto
                    {
                        FlujoId = flujoCtus,
                        IdObjetoNegocio = objetoNegocio.ObjetoNegocioId
                    }
                }
            };

            jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.PostAsync,
                    urlServicio,
                    uriMetodo,
                    null,
                    crearInstanciaCtus,
                    usuarioDnp,
                    useJWTAuth: false
            );
            string mensajeRetorno = string.Empty;

            var retorno = JsonConvert.DeserializeObject<List<InstanciaResultado>>(jsonResponse).FirstOrDefault();

            if (retorno.Exitoso)
            {
                mensajeRetorno = $"{_parametrosPersistencia.ConsultarParametro("MensajeCreacionInstanciaCTUS")} {proyectoCtus.NombreEntidadDestino}";
            }

            return mensajeRetorno;
        }

        public async Task<object> GenerarFichaCTUSSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            try
            {
                string _generarFicha = _parametrosPersistencia.ConsultarParametro("GenerarFichaCTUSSGR");

                if (_generarFicha.Equals("S", StringComparison.OrdinalIgnoreCase))
                {
                    ObjetoNegocio contenido = parametrosActualizar.Contenido;

                    var fechaGeneracion = DateTime.Now.ToString("ddMMyyyy");
                    var nombreArchivo = "FichaAutomaticaDocumentoCTUS_" + contenido.ObjetoNegocioId + "_" + fechaGeneracion;

                    string urlServicio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
                    var uriMetodo = ConfigurationManager.AppSettings["uriObtenerConfiguracionReporteSgr"];
                    var uri = $"{uriMetodo}?instanciaId={contenido.InstanciaId}";

                    var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                            MetodosServiciosWeb.GetAsync,
                            urlServicio,
                            uri,
                            string.Empty,
                            null,
                            parametrosAuditoria.Usuario,
                            useJWTAuth: false
                    );

                    var configuracionReporte = JsonConvert.DeserializeObject<ConfiguracionReportesDto>(jsonResponse);

                    var _nombreReporte = configuracionReporte.NombreRdl;
                    var _tipoDocumento = configuracionReporte.NombreTipoDocumento;
                    var _tipoDocumentoId = configuracionReporte.TipoDocumentoId;
                    var _descripcion = configuracionReporte.Descripcion;
                    var _nombre = nombreArchivo + ".pdf";
                    var _tramiteId = contenido.ObjetoNegocioId;

                    uriMetodo = ConfigurationManager.AppSettings["uriObtenerRolAprobadorCtusSgr"];
                    uri = $"{uriMetodo}?ProyectoId={contenido.ObjetoNegocioId}&instanciaId={contenido.InstanciaId}";

                    jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                            MetodosServiciosWeb.GetAsync,
                            urlServicio,
                            uri,
                            string.Empty,
                            null,
                            parametrosAuditoria.Usuario,
                            useJWTAuth: false
                    );

                    var proyectoCtus = JsonConvert.DeserializeObject<ServiciosNegocio.Dominio.Dto.SGR.CTUS.RolApruebaCTUSDto>(jsonResponse);

                    var _idRolParametro = proyectoCtus != null ? proyectoCtus.RolDirectorId.Value.ToString() : _parametrosPersistencia.ConsultarParametro("RolFirmaConceptoViabilidadSGR"); ;
                    var _idRolPorDefecto = Guid.Empty.ToString();
                    var _idRolCliente = contenido.IdRol;
                    var _idRol = (_idRolCliente == null || _idRolCliente == "" || _idRolCliente == _idRolPorDefecto) ? _idRolParametro : _idRolCliente;

                    var idReporte = await Task.Run(() => _fichaServicio.ObtenerPlantillaReporteAnexo(_nombreReporte, parametrosAuditoria.Usuario));

                    // obtener ficha
                    var ficha = await Task.Run(() => _fichaServicio.ObtenerFichaFisicaSGR(contenido.InstanciaId, contenido.NivelId, _tramiteId, _nombreReporte, idReporte.ID, false, parametrosAuditoria.Usuario));

                    // guardar ficha
                    List<Dictionary<string, object>> lstFile = new List<Dictionary<string, object>>();

                    Dictionary<string, object> file = new Dictionary<string, object>();
                    file.Add("FileName", _nombre);
                    file.Add("ContentType", "application/pdf");
                    file.Add("ContentLength", ficha.Length);
                    file.Add("InputStream", ficha);
                    lstFile.Add(file);

                    var bytes = Convert.FromBase64String(ficha);
                    var fileStream = new MemoryStream(bytes);

                    // Version documento
                    int _versionDocumentoSoporte = VersionamientoDocumento(contenido.InstanciaId, parametrosAuditoria.Usuario, "FichaAutomaticaDocumentoCTUS");

                    var metadatos = new
                    {
                        extension = "pdf",
                        idInstancia = contenido.InstanciaId,
                        idAccion = contenido.IdAccion,
                        tipoDocumento = _tipoDocumento,
                        tipoDocumentoId = _tipoDocumentoId,
                        idNivel = contenido.NivelId,
                        idRol = _idRol,
                        descripcion = _descripcion,
                        versionDocumentoSoporte = _versionDocumentoSoporte.ToString()
                    };

                    ArchivoViewModelDto archivo = new ArchivoViewModelDto
                    {
                        FormFile = lstFile,
                        Nombre = _nombre,
                        Coleccion = "tramites",
                        Status = "Creado",
                        Metadatos = JsonConvert.SerializeObject(metadatos)
                    };

                    var result = await Task.Run(() => _manejadorArchivosServicios.CargarArchivos(archivo, fileStream, parametrosAuditoria.Usuario));
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> GenerarAdjuntarFichaManualSGR(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            string ficha = "";
            try
            {
                ObjetoNegocio contenido = parametrosActualizar.Contenido;

                var fechaGeneracion = DateTime.Now.ToString("ddMMyyyy");
                string urlServicio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
                var uriMetodo = ConfigurationManager.AppSettings["uriObtenerConfiguracionReporteSgr"];
                var uri = $"{uriMetodo}?instanciaId={contenido.InstanciaId}";

                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                        MetodosServiciosWeb.GetAsync,
                        urlServicio,
                        uri,
                        string.Empty,
                        null,
                        parametrosAuditoria.Usuario,
                        useJWTAuth: false
                );

                var configuracionReporte = JsonConvert.DeserializeObject<ConfiguracionReportesDto>(jsonResponse);

                var _nombreReporte = configuracionReporte.NombreRdl;
                var _tipoDocumento = configuracionReporte.NombreTipoDocumento;
                var _tipoDocumentoId = configuracionReporte.TipoDocumentoId;
                var _descripcion = configuracionReporte.Descripcion;
                var nombreArchivo = "FichaAutomaticaDocumento" + _tipoDocumento + contenido.ObjetoNegocioId + "_" + fechaGeneracion;
                var _nombre = nombreArchivo + ".pdf";
                var _tramiteId = contenido.ObjetoNegocioId;

                var _idRolParametro = _parametrosPersistencia.ConsultarParametro("RolFirmaConceptoViabilidadSGR"); ;
                var _idRolPorDefecto = Guid.Empty.ToString();
                var _idRolCliente = contenido.IdRol;
                var _idRol = (string.IsNullOrEmpty(_idRolCliente) || _idRolCliente == _idRolPorDefecto) ? _idRolParametro : _idRolCliente;

                var idReporte = await Task.Run(() => _fichaServicio.ObtenerPlantillaReporteAnexo(_nombreReporte, parametrosAuditoria.Usuario));

                // obtener ficha
                ficha = await Task.Run(() => _fichaServicio.ObtenerFichaFisicaSGR(contenido.InstanciaId, contenido.NivelId, _tramiteId, _nombreReporte, idReporte.ID, false, parametrosAuditoria.Usuario));

                // guardar ficha
                List<Dictionary<string, object>> lstFile = new List<Dictionary<string, object>>();

                Dictionary<string, object> file = new Dictionary<string, object>();
                file.Add("FileName", _nombre);
                file.Add("ContentType", "application/pdf");
                file.Add("ContentLength", ficha.Length);
                file.Add("InputStream", ficha);
                lstFile.Add(file);

                var bytes = Convert.FromBase64String(ficha);
                var fileStream = new MemoryStream(bytes);

                int _versionDocumentoSoporte = VersionamientoDocumento(contenido.InstanciaId, parametrosAuditoria.Usuario, "FichaAutomaticaDocumento");

                var metadatos = new
                {
                    extension = "pdf",
                    idInstancia = contenido.InstanciaId,
                    idAccion = contenido.IdAccion,
                    tipoDocumento = _tipoDocumento,
                    tipoDocumentoId = _tipoDocumentoId,
                    idNivel = contenido.NivelId,
                    idRol = _idRol,
                    descripcion = _descripcion,
                    versionDocumentoSoporte = _versionDocumentoSoporte.ToString()
                };

                ArchivoViewModelDto archivo = new ArchivoViewModelDto
                {
                    FormFile = lstFile,
                    Nombre = _nombre,
                    Coleccion = "tramites",
                    Status = "Creado",
                    Metadatos = JsonConvert.SerializeObject(metadatos)
                };

                var result = await Task.Run(() => _manejadorArchivosServicios.CargarArchivos(archivo, fileStream, parametrosAuditoria.Usuario));

                return ficha;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<bool> SGR_Proyectos_NotificarUsuariosViabilidad(Guid instanciaId, string proyectoId, string usuarioDnp)
        {
            string urlServiciosNegocio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMensajeProyecto = ConfigurationManager.AppSettings["uriObtenerUsuariosNotificacionViabilidadSgr"];

            var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.GetAsync,
                    urlServiciosNegocio,
                    string.Format("{0}{1}", uriMensajeProyecto, "?instanciaId="),
                    instanciaId.ToString(),
                    null,
                    usuarioDnp,
                    useJWTAuth: false
            );

            if (!string.IsNullOrEmpty(jsonResponse))
            {

                var usuarios = JsonConvert.DeserializeObject<IEnumerable<UsuariosProyectoDto>>(jsonResponse);

                List<ParametrosCrearNotificacionFlujoDto> data = new List<ParametrosCrearNotificacionFlujoDto>();

                data = usuarios.Select(u => new ParametrosCrearNotificacionFlujoDto()
                {
                    IdUsuarioDNP = u.Usuario,
                    NombreNotificacion = "Concepto de viabilidad",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(3),
                    ContenidoNotificacion = u.Notificacion
                }).ToList();

                try
                {
                    var uriServicioNotificacion = ConfigurationManager.AppSettings["ApiNotificacion"] + ConfigurationManager.AppSettings["uriCrearNotificacionFlujo"];
                    jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                        MetodosServiciosWeb.PostAsync,
                        uriServicioNotificacion,
                        "",
                        null,
                        data,
                        usuarioDnp,
                        useJWTAuth: false);

                }
                catch { }
            }

            return true;
        }

        public async Task<string> SGR_CTUS_CrearInstanciaCtusAutomaticaSGR(ObjetoNegocio objetoNegocio, string usuarioDnp)
        {
            string entidadConcepto = _parametrosPersistencia.ConsultarParametro("EntidadDNPCTUSCTEI");

            string urlServicio = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriGuardarProyectoCtusSgr"];

            var crearCtus = new CrearProyectoCtusDto
            {
                ProyectoId = Convert.ToInt32(objetoNegocio.ObjetoNegocioId),
                Instancia = objetoNegocio.InstanciaId,
                SolicitaCtus = true,
                EntidadConcepto = Convert.ToInt32(entidadConcepto),
                RolSolicita = objetoNegocio.IdRol
            };

            var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.PostAsync,
                    urlServicio,
                    uriMetodo,
                    string.Empty,
                    crearCtus,
                    usuarioDnp,
                    useJWTAuth: false
            );

            var mensajeRetorno = await SGR_CTUS_CrearInstanciaCtusSGR(objetoNegocio, usuarioDnp);

            return mensajeRetorno;
        }


        public async Task<bool> IniciarFlujoSGP(ParametrosGuardarDto<ObjetoNegocio> parametrosActualizar, ParametrosAuditoriaDto parametrosAuditoria)
        {
            string ObjetoNegocioId = parametrosActualizar.Contenido.ObjetoNegocioId;

            //1. Obtener el Id de la instancia anterior. Revisar bien
            //string idInstanciaAnterior = string.Empty;
            //idInstanciaAnterior = "F6B6630B-2497-4BDF-B4A5-3FE4B9B90961";
            string idInstanciaAnterior = _proyectoPersistencia.GetInstaciasProyectoSGP(ObjetoNegocioId);

            if (!string.IsNullOrEmpty(idInstanciaAnterior))
            {
                string idInstanciaDestino = parametrosActualizar.Contenido.InstanciaId.ToString();

                //2. Recupera las observaciones de la instancia anterior
                _proyectoPersistencia.PostRecuperaDatosSGP(idInstanciaAnterior, idInstanciaDestino);

                //3. armar el objeto de petición con los datos anteriores.
                Dictionary<string, object> parametros = new Dictionary<string, object>();

                parametros.Add("collectionMetadata", "ArchivosPIIP");
                parametros.Add("idInstancia", idInstanciaAnterior.ToLower());
                parametros.Add("idInstanciaDestino", idInstanciaDestino.ToLower());
                parametros.Add("coleccion", "tramites");

                //4. Hacer el llamado a la api del manejador de archivos para clonar documentos, faltan definir las uris del manejador
                var result = await Task.Run(() => _manejadorArchivosServicios.Clonar(parametros, parametrosAuditoria.Usuario));

                var mensajeAccion = string.Format(ServiciosNegocioRecursos.AuditoriaEstadoProyecto, parametrosActualizar.Contenido);
                GenerarAuditoria(parametrosActualizar,
                                 parametrosAuditoria,
                                 parametrosAuditoria.Ip,
                                 parametrosAuditoria.Usuario,
                                 TipoMensajeEnum.Modificacion,
                                 mensajeAccion);
            }

            return true;
        }
    }
}
