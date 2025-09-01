using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Comunes.Enum;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites;
using DNP.ServiciosTransaccional.Servicios.Dto;
using DNP.ServiciosTransaccional.Servicios.Interfaces;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Fichas;
using DNP.ServiciosTransaccional.Servicios.Interfaces.Tramites;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DNP.ServiciosTransaccional.Servicios.Implementaciones.Tramites
{
    public class TramiteServicios : ITramiteServicio
    {
        private readonly ITramitePersistencia _tramitePersistencia;
        private readonly IClienteHttpServicios _clienteHttpServicios;
        private readonly IFichaServicios _fichaServicios;
        private readonly string ENDPOINT = ConfigurationManager.AppSettings["ApiInteroperabilidadOrfeo"];
        private readonly string MOTOR_FLUJO_ENDPOINT = ConfigurationManager.AppSettings["ApiMotorFlujos"];
        private readonly string SERVICIO_NEGOCIO = ConfigurationManager.AppSettings["ApiServicioNegocio"];
        

        public TramiteServicios(IClienteHttpServicios clienteHttpServicios, ITramitePersistencia tramitePersistencia, IFichaServicios fichaServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
            _tramitePersistencia = tramitePersistencia;
            _fichaServicios = fichaServicios;
        }

        public async Task<ResponseDto<bool>> ReasignarRadicadoORFEO(ReasignacionRadicadoDto parametros, string usuario)
        {
            var detalleRadicado = int.TryParse(parametros.TramiteId, out int TramiteId) ? _tramitePersistencia.GetRadicadoEntradaORFEO(TramiteId) : null;

            parametros.NoRadicado = detalleRadicado == null ? null : detalleRadicado.RadicadoEntrada;

            if (parametros.UsuarioDestino != null && parametros.UsuarioDestino.Login == "-" && int.TryParse(parametros.TramiteId, out TramiteId))
            {
                var documentoUsuarioDestino = _tramitePersistencia.GetUsuarioDestinoORFEO(TramiteId, parametros.UsuarioOrigen.Login);
                parametros.UsuarioDestino.Login = (await ObtenerUsuarioCuentas(documentoUsuarioDestino, usuario)).First().Cuenta.Split('@').First();//FirstOrDefault(cuenta => cuenta.Cuenta.Contains("@dnp.gov.co"))?.Cuenta.Split('@').First();
            }
            else
            {
                var loginUsuarioDestino = (await ObtenerUsuarioCuentas(parametros.UsuarioDestino.Login, usuario)).First().Cuenta.Split('@').First();//FirstOrDefault(cuenta => cuenta.Cuenta.Contains("@dnp.gov.co"))?.Cuenta.Split('@').First();
                if (string.IsNullOrWhiteSpace(loginUsuarioDestino)) throw new Exception("No fue posible obtener el usuario Destino del radicado");
                parametros.UsuarioDestino.Login = loginUsuarioDestino;

            }

            var loginUsuarioOrigen = (await ObtenerUsuarioCuentas(parametros.UsuarioOrigen.Login, usuario)).First().Cuenta.Split('@').First();//.FirstOrDefault(cuenta => cuenta.Cuenta.Contains("@dnp.gov.co"))?.Cuenta.Split('@').First();
            if (string.IsNullOrWhiteSpace(loginUsuarioOrigen)) throw new Exception("No fue posible obtener el usuario Origen del radicado");
            parametros.UsuarioOrigen.Login = loginUsuarioOrigen;

            var uriMetodo = ConfigurationManager.AppSettings["uriReasignarRadicadoORFEO"];

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, parametros, usuario, useJWTAuth: false);

            return JsonConvert.DeserializeObject<ResponseDto<bool>>(respuesta);
        }

        public async Task<ResponseDto<bool>> CargarDocumentoElectronicoORFEO(DatosDocumentoElectronicoDSDto datosDocumentoElectronicoDSDto, string usuarioDNP, string usuarioRadica = "")
        {
            ResponseDto<bool> respuesta = new ResponseDto<bool>();
            string mensajeError = "No es posible el usuario Origen para la carga del documento electrónico en el radicado";

            if (!string.IsNullOrEmpty(usuarioRadica)) {
                datosDocumentoElectronicoDSDto.usuarioRadica.Login = usuarioRadica;
            } else {
                var usuariocuenta = await ObtenerUsuarioCuentas(usuarioDNP);
                if (!usuariocuenta.Estado)
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = mensajeError;
                    return respuesta;
                }
                var loginUsuarioOrigen = usuariocuenta.Data.First().Cuenta.Split('@').First(); 
                if (string.IsNullOrWhiteSpace(loginUsuarioOrigen))
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = mensajeError;
                    return respuesta;
                }
                datosDocumentoElectronicoDSDto.usuarioRadica.Login = loginUsuarioOrigen;
            }
            
            var uriMetodo = ConfigurationManager.AppSettings["uriCargarDocumentoElectronicoORFEO"];


            var respuestacarga = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, datosDocumentoElectronicoDSDto, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResponseDto<bool>>(respuestacarga);
        }

        public async Task<ResponseDto<bool>> ConsultarRadicado(string radicadoSalida, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarRadicado"];
            var x = decimal.TryParse(radicadoSalida, out decimal radicado) ? radicado : 0;
            uriMetodo += radicado;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, ENDPOINT, uriMetodo, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResponseDto<bool>>(respuesta);
        }

        public async Task<ResponseDto<bool>> CerrarRadicado(CerrarRadicadoDto parametros, string usuarioDNP, bool hasUserSelected = false)
        {
            ResponseDto<bool> respuesta = new ResponseDto<bool>();

            if(!hasUserSelected) { 
                var usuariocuenta = await ObtenerUsuarioCuentas(usuarioDNP);
                if (!usuariocuenta.Estado)
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = "No es posible encontrar el usuario para el cierre del radicado";
                    return respuesta;
                }
                parametros.UsuarioArchiva.Login = usuariocuenta.Data.First().Cuenta.Split('@').First();
            }
            var uriMetodo = ConfigurationManager.AppSettings["uriCerrarRadicado"];

            var respuestacerrar = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, ENDPOINT, uriMetodo, string.Empty, parametros, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<ResponseDto<bool>>(respuestacerrar);
        }

        public async Task<ResponseDto<bool>> CerrarRadicadosTramite(string numeroTramite, string usuarioDNP)
        {
            var response = new ResponseDto<bool>();
            CerrarRadicadoDto modelRequest = new CerrarRadicadoDto {
                UsuarioArchiva = new UsuarioRadicacionDto()
            };

            try
            {
                var tramiteDetail = _tramitePersistencia.ObtenerDetalleTramiteRadicado(numeroTramite);
                var radicadosTramite = _tramitePersistencia.GetRadicadoEntradaORFEO(tramiteDetail.TramiteId);
                var analistaDestino = await ObtenerAnalistaResponsableSector(tramiteDetail.SectorId);

               
                modelRequest = new CerrarRadicadoDto {
                    UsuarioArchiva = new UsuarioRadicacionDto  {
                        Login = analistaDestino
                    },
                    NoRadicado = Convert.ToDecimal(radicadosTramite.RadicadoEntrada),
                    Observacion = "Cierre de radicado de entrada"
                };

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var cierreEntradaResponse = await CerrarRadicado(modelRequest, usuarioDNP, true);
                if (!cierreEntradaResponse.Estado) {
                    return cierreEntradaResponse;
                }
                
                modelRequest.NoRadicado = Convert.ToDecimal(radicadosTramite.RadicadoSalida);
                modelRequest.Observacion = "Cierre de radicado de salida";
                var cierreSalidaResponse = await CerrarRadicado(modelRequest, usuarioDNP);
                if (!cierreSalidaResponse.Estado) {
                    return cierreSalidaResponse;
                }
                
                response.Estado = true;
                response.Data = true;
            }
            catch (Exception ex) {
                //await ResetAccionesFlujoPorTramite(numeroTramite, usuarioDNP);
                response.Mensaje = ex.Message;
            }

            return response;
            
        }

        public async Task<ResponseDto<bool>> CerrarRadicadosTramiteDummy(string numeroTramite, string usuarioDNP)
        {
            return new ResponseDto<bool>
            {
                Estado = true,
                Data = true
            };
        }

        /// <summary>
        /// Crea radicado de entrada para los trámites
        /// </summary>
        /// <param name="numeroTramite">Número de trámite</param>
        /// <param name="usuarioDnp">Usuario de DNP que realiza el proceso</param>
        /// <returns></returns>
        public async Task<CommonResponseDto<dynamic>> GenerarRadicadoEntrada(string numeroTramite, string usuarioDnp)
        {
            var response = new CommonResponseDto<dynamic>();
            var uriRegistrarRadicadoEntrada = ConfigurationManager.AppSettings["uriRadicadoEntradaOrfeo"];
            var tipoReporteRadicadoAnexo = ConfigurationManager.AppSettings["nombreAnexoRadicadoReporte"];

            try
            {
                var tramiteDetail = _tramitePersistencia.ObtenerDetalleTramiteRadicado(numeroTramite);
                var radicadoEntrada = _tramitePersistencia.GetRadicadoEntradaORFEO(tramiteDetail.TramiteId);
                var proyectosTramite = _tramitePersistencia.GetTramiteProyectos(tramiteDetail.TramiteId);
                var analistaDestino = await ObtenerAnalistaResponsableSector(tramiteDetail.SectorId);
                var dependenciaOrfeo = _tramitePersistencia.ObtenerDependenciaByEntidadOrfeoId(tramiteDetail.EntidadId);

                if (radicadoEntrada != null && !string.IsNullOrEmpty(radicadoEntrada.RadicadoEntrada)) {
                    response.Estado = true;
                    return response;
                }

                var model = CrearRadicadoRequestDto.CrearRadicadoEntrada(
                    numeroTramite,
                    tramiteDetail, 
                    proyectosTramite, 
                    analistaDestino,
                    dependenciaOrfeo
                );

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Post,
                    ENDPOINT,
                    uriRegistrarRadicadoEntrada,
                    string.Empty,
                    model,
                    usuarioDnp,
                    useJWTAuth: false
                );

                if (string.IsNullOrWhiteSpace(jsonResponse)) {
                    throw new Exception("No fue posible crear el radicado de entrada");
                }

                var modelResponse = JsonConvert.DeserializeObject<CommonResponseDto<CreacionRadicadoEntradaDto>>(jsonResponse);
                if (!modelResponse.Estado) {
                    throw new Exception(modelResponse.Mensaje);
                }

                //Carga de anexo 
                var tipoAnexoRadicado = await _fichaServicios.ObtenerPlantillaReporteAnexo(tipoReporteRadicadoAnexo, usuarioDnp);
                var anexoResponse = await _fichaServicios.ObtenerAnexoRadicadoTramite(
                    tramiteDetail.TramiteId,
                    tipoAnexoRadicado.Nombre,
                    tipoAnexoRadicado.ID,
                    usuarioDnp
                );
                var datosDocumento = DatosDocumentoElectronicoDSDto.GenerarAnexoRadicadoEntrada(modelResponse.Data.RadicadoId, usuarioDnp, anexoResponse);

                var resultFile = await CargarDocumentoElectronicoORFEO(datosDocumento, usuarioDnp, analistaDestino);
                if (!resultFile.Estado) {
                    throw new Exception(resultFile.Mensaje);
                }

                _tramitePersistencia.PostActualizarCartaRadicado(
                    tramiteDetail.TramiteId,
                    usuarioDnp,
                    modelResponse.Data.RadicadoId,
                    "",
                    modelResponse.Data.ExpedienteId
                );

                response.Estado = true;
            }
            catch (Exception e)
            {
                await ResetAccionesFlujoPorTramite(numeroTramite, usuarioDnp);
                response.Mensaje = e.Message;
            }
            return response;
        }

        /// <summary>
        /// Crea radicado de entrada para los trámites
        /// </summary>
        /// <param name="numeroTramite">Número de trámite</param>
        /// <param name="usuarioDnp">Usuario de DNP que realiza el proceso</param>
        /// <returns></returns>
        public async Task<CommonResponseDto<dynamic>> GenerarRadicadoEntradaDummy(string numeroTramite, string usuarioDnp)
        {
            var response = new CommonResponseDto<dynamic>();

            try
            {
                var tramiteDetail = _tramitePersistencia.ObtenerDetalleTramiteRadicado(numeroTramite);
                var radicadoEntrada = _tramitePersistencia.GetRadicadoEntradaORFEO(tramiteDetail.TramiteId);
                var analistaDestino = await ObtenerAnalistaResponsableSector(tramiteDetail.SectorId);

                if (radicadoEntrada != null && !string.IsNullOrEmpty(radicadoEntrada.RadicadoEntrada))
                {
                    response.Estado = true;
                    return response;
                }

                _tramitePersistencia.PostActualizarCartaRadicado(
                    tramiteDetail.TramiteId,
                    usuarioDnp,
                    "202204300440842",
                    "",
                    "2022043200804200110E"
                );

                response.Estado = true;
            }
            catch (Exception e)
            {
                response.Mensaje = e.Message;
            }
            return response;
        }

        public async Task<CommonResponseDto<CreacionRadicadoEntradaDto>> GenerarRadicadoSalidaDummy(string numeroTramite, string usuarioDnp)
        {
            var response = new CommonResponseDto<CreacionRadicadoEntradaDto>();

            try
            {
                var tramiteDetail = _tramitePersistencia.ObtenerDetalleTramiteRadicado(numeroTramite);
                var analistaDestino = (await ObtenerUsuarioCuentas(usuarioDnp, usuarioDnp)).First().Cuenta.Split('@').First();//.FirstOrDefault(cuenta => cuenta.Cuenta.Contains("@dnp.gov.co"))?.Cuenta.Split('@').First();

                
                _tramitePersistencia.PostActualizarCartaRadicado(
                    tramiteDetail.TramiteId,
                    usuarioDnp,
                    "202204300440842",
                    "202204300440842",
                    "2022043200804200110E"
                );

                response.Estado = true;
                response.Data = new CreacionRadicadoEntradaDto
                {
                    RadicadoId = "202204300440842",
                    ExpedienteId = "2022043200804200110E"
                };
            }
            catch (Exception e)
            {
                response.Mensaje = e.Message;
            }
            return response;
        }

        public async Task<CommonResponseDto<bool>> GenerarDocumentoFirmado(string numeroTramite, string usuarioDnp)
        {
            var response = new CommonResponseDto<bool>();
            var uriRegistrarRadicadoEntrada = ConfigurationManager.AppSettings["uriRadicadoEntradaOrfeo"];
            var tipoReporteRadicadoAnexo = "";

            try
            {
                var tramiteDetail = _tramitePersistencia.ObtenerDetalleTramiteRadicado(numeroTramite);
                var radicadosTramite = _tramitePersistencia.GetRadicadoEntradaORFEO(tramiteDetail.TramiteId);
                var analistaDestino = (await ObtenerUsuarioCuentas(usuarioDnp, usuarioDnp)).First().Cuenta.Split('@').First(); ;
                _tramitePersistencia.ActualizaCampoRemitenteConcepto(tramiteDetail.TramiteId, usuarioDnp);
                if (radicadosTramite == null || radicadosTramite.RadicadoSalida == null) {
                    response.Estado = false;
                    response.Mensaje = "No se puede vincular el archivo firmado ";
                    return response;
                }
                if (tramiteDetail.PDF == null)
                {
                    tipoReporteRadicadoAnexo = ObtenerNombreDocumentoCartaConceptoPorTipoTramite(tramiteDetail.CodigoTipoTramite);
                }
                else
                {
                    tipoReporteRadicadoAnexo = tramiteDetail.PDF;
                }

                // tipoReporteRadicadoAnexo = ObtenerNombreDocumentoCartaConceptoPorTipoTramite(tramiteDetail.CodigoTipoTramite);                

                // Carga de anexo
                var tipoAnexoRadicado = await _fichaServicios.ObtenerPlantillaReporteAnexo(tipoReporteRadicadoAnexo, usuarioDnp);
                var anexoResponse = await _fichaServicios.ObtenerAnexoRadicadoTramite(
                    tramiteDetail.TramiteId,
                    tipoAnexoRadicado.Nombre,
                    tipoAnexoRadicado.ID,
                    usuarioDnp
                );
                var datosDocumento = DatosDocumentoElectronicoDSDto.GenerarAnexoRadicadoEntrada(radicadosTramite.RadicadoSalida, usuarioDnp, anexoResponse);

                var resultFile = await CargarDocumentoElectronicoORFEO(datosDocumento, usuarioDnp, analistaDestino);
                if (!resultFile.Estado) {
                    response.Estado = false;
                    response.Mensaje = "No se posible asignar el documento electrónico al radicado de salida";
                    return response;
                }


                response.Estado = true;
                response.Data = true;
            }
            catch (Exception e) {
                await ResetAccionesFlujoPorTramite(numeroTramite, usuarioDnp);
                response.Mensaje = e.Message;
            }
            return response;
        }

        public async Task<CommonResponseDto<CreacionRadicadoEntradaDto>> GenerarRadicadoSalida(string numeroTramite, string usuarioDnp)
        {
            var response = new CommonResponseDto<CreacionRadicadoEntradaDto>();
            var uriRegistrarRadicadoEntrada = ConfigurationManager.AppSettings["uriRadicadoEntradaOrfeo"];

            try
            {
                var tramiteDetail = _tramitePersistencia.ObtenerDetalleTramiteRadicado(numeroTramite);
                var radicadoEntrada = _tramitePersistencia.GetRadicadoEntradaORFEO(tramiteDetail.TramiteId);
                _tramitePersistencia.ActualizaCampoRemitenteConcepto(tramiteDetail.TramiteId, usuarioDnp);
                var analistaDestino = (await ObtenerUsuarioCuentas(usuarioDnp, usuarioDnp)).First().Cuenta.Split('@').First();

                if (radicadoEntrada == null || radicadoEntrada.RadicadoEntrada == null)
                {
                    response.Estado = false;
                    response.Mensaje = "No se puede generar un radicado de salida si el trámite no tiene radicado de entrada asignado";
                    return response;
                }

                var model = CrearRadicadoRequestDto.CrearRadicadoSalida(
                    numeroTramite,
                    tramiteDetail,
                    radicadoEntrada.RadicadoEntrada,
                    analistaDestino,
                    radicadoEntrada.ExpedienteId
                );

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Post,
                    ENDPOINT,
                    uriRegistrarRadicadoEntrada,
                    string.Empty,
                    model,
                    usuarioDnp,
                    useJWTAuth: false
                );

                if (string.IsNullOrWhiteSpace(jsonResponse))
                {
                    response.Estado = false;
                    response.Mensaje = "No fue posible crear el radicado de salida";
                    await ResetAccionesFlujoPorTramite(numeroTramite, usuarioDnp);
                    //response.Mensaje = e.Message;
                    return response;
                }

                var modelResponse = JsonConvert.DeserializeObject<CommonResponseDto<CreacionRadicadoEntradaDto>>(jsonResponse);
                if (!modelResponse.Estado)
                {
                    response.Estado = false;
                    response.Mensaje = modelResponse.Mensaje;
                }

                _tramitePersistencia.PostActualizarCartaRadicado(
                    tramiteDetail.TramiteId,
                    usuarioDnp,
                    radicadoEntrada.RadicadoEntrada,
                    modelResponse.Data.RadicadoId,
                    radicadoEntrada.ExpedienteId
                );

                response.Estado = true;
                response.Data = modelResponse.Data;
            }
            catch (Exception e)
            {
                await ResetAccionesFlujoPorTramite(numeroTramite, usuarioDnp);
                response.Mensaje = e.Message;
            }
            return response;
        }

        /// <summary>
        /// Obtiene el detalle del tramite a partir del número del tramite
        /// </summary>
        /// <param name="numeroTramite">Número único del trámite</param>
        /// <param name="usuarioDNP">Usuario DNP que realiza el proceso</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DetalleTramiteDto> ObtenerDetalleTramite(string numeroTramite, string usuarioDNP)
        {
            var detalleTramiteUrl = string.Format(
                ConfigurationManager.AppSettings["uriConsultaDetalleTramiteFlujos"],
                numeroTramite
            );

            try
            {
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Get,
                    MOTOR_FLUJO_ENDPOINT,
                    detalleTramiteUrl,
                    string.Empty,
                    null,
                    usuarioDNP,
                    useJWTAuth: false
                );

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    throw new Exception("No fue posible obtener el detalle del tramite");
                }

                var resultModel = JsonConvert.DeserializeObject<DetalleTramiteDto>(jsonResponse);

                return resultModel;
            }
            
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Obtiene el analista responsable asociado al sector actual
        /// </summary>
        /// <param name="idSector">Identificador del sector</param>
        /// <param name="usuarioDnp">Usuario DNP que realzia el proceso</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> ObtenerAnalistaResponsableSector(int idSector)
        {
            try
            {
                var analistasResponsables = _tramitePersistencia.ObtenerAnalistaResponsablePorSector(idSector);
                return analistasResponsables.First().Cuenta.Split('@').First();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
       

        public async Task<CommonResponseDto<List<UsuarioCuentaDto>>> ObtenerUsuarioCuentas(string idUsuarioDnp)
        {
            var response = new CommonResponseDto<List<UsuarioCuentaDto>> ();
            var detalleTramiteUrl = string.Format(
               "{0}?idUsuarioDnp={1}",
               ConfigurationManager.AppSettings["urObtenerUsuarioCuentas"],
               idUsuarioDnp
           );

            try
            {
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Get,
                    MOTOR_FLUJO_ENDPOINT,
                    detalleTramiteUrl,
                    string.Empty,
                    null,
                    idUsuarioDnp,
                    useJWTAuth: false
                );

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    response.Estado = false;
                    response.Mensaje = "No fue posible obtener el usuario";
                    return response;
                }

                response.Estado = true;
                response.Data = JsonConvert.DeserializeObject<List<UsuarioCuentaDto>>(jsonResponse);

                return response;
            }
            catch (Exception e)
            {
                response.Estado = false;
                response.Mensaje = e.Message;
                return response;
               
            }
        }

        /// <summary>
        /// Obtiene el detalle del tramite a partir del número del tramite
        /// </summary>
        /// <param name="numeroTramite">Número único del trámite</param>
        /// <param name="usuarioDNP">Usuario DNP que realiza el proceso</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<UsuarioCuentaDto>> ObtenerUsuarioCuentas(string idUsuarioDnp, string usuarioDNP)
        {
            var detalleTramiteUrl = string.Format(
                "{0}?idUsuarioDnp={1}",
                ConfigurationManager.AppSettings["urObtenerUsuarioCuentas"],
                idUsuarioDnp
            );

            try
            {
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Get,
                    MOTOR_FLUJO_ENDPOINT,
                    detalleTramiteUrl,
                    string.Empty,
                    null,
                    usuarioDNP,
                    useJWTAuth: false
                );

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    throw new Exception("No fue posible obtener el usuario destino del radicado");
                }

                var resultModel = JsonConvert.DeserializeObject<List<UsuarioCuentaDto>>(jsonResponse);

                return resultModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto<bool>> CerrarInstancias(string objetoNegocio, string usuarioDnp)
        {
            var response = new CommonResponseDto<bool>();
            try
            {
                var tramiteDetail = await ObtenerDetalleTramite(objetoNegocio, usuarioDnp);
                var rta = await CerrarInstanciasTramite(tramiteDetail.TramiteId, usuarioDnp);
                return rta;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
                return response;
            }

            
        }

        public async Task<CommonResponseDto<bool>> CerrarInstanciasTramite(int tramiteId, string usuarioDNP)
        {
            CommonResponseDto<bool> resultModel = new CommonResponseDto<bool>();
            var detalleTramiteUrl = string.Format(
               "{0}?tramiteId={1}",
               ConfigurationManager.AppSettings["uriCerrarInstancia"],
               tramiteId
           );

            try
            {
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Get,
                    MOTOR_FLUJO_ENDPOINT,
                    detalleTramiteUrl,
                    string.Empty,
                    null,
                    usuarioDNP,
                    useJWTAuth: false
                );

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    throw new Exception("No fue posible cerrar las instancias asociadas al trámite.");
                }

                var resultado = JsonConvert.DeserializeObject<InstanciaResultado>(jsonResponse);
                if (resultado != null && resultado.Exitoso)
                {
                    resultModel.Estado = resultado.Exitoso;
                }
                else
                {
                    resultModel.Estado = false;
                    resultModel.Mensaje = resultado.MensajeOperacion;
                }

                return resultModel;
            }
            catch (Exception e)
            {
                resultModel.Estado = false;
                resultModel.Mensaje = e.Message;
                return resultModel;
            }
        }
    
        public async Task<Carta> ConsultarCarta(int tramiteId, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarCarta"];
            uriMetodo += "?tramiteId=" + tramiteId;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, SERVICIO_NEGOCIO, uriMetodo, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<Carta>(respuesta);
            
        }

        public async Task<TramitesResultado> FirmarCarta(int tramiteId, string radicadoSalida, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriFirmarCarta"];
            uriMetodo += "?tramiteId=" + tramiteId  + "&radicadoSalida="  + radicadoSalida;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, SERVICIO_NEGOCIO, uriMetodo, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<TramitesResultado>(respuesta);
        }

        public async Task<string> ObtenerPDF(int tramiteId, int TipoTramiteId, string usuarioDNP)
        {
            string mensaje = "Error";
            var tipoReporteRadicadoAnexo = "";
            switch (TipoTramiteId) {
                case 1:///Traslado Ordinario
                    tipoReporteRadicadoAnexo=ConfigurationManager.AppSettings["nombrePDFReporte"];
                break;
                case 4:///Vigencia Futura
                    tipoReporteRadicadoAnexo = ConfigurationManager.AppSettings["NombrePDFCartaVf"];
                break;
            }
            var tipoAnexoRadicado = await _fichaServicios.ObtenerPlantillaReporteAnexo(tipoReporteRadicadoAnexo, usuarioDNP);
            var anexoResponse = await _fichaServicios.ObtenerAnexoRadicadoTramite(
                    tramiteId,
                    tipoAnexoRadicado.Nombre,
                    tipoAnexoRadicado.ID,
                    usuarioDNP
                );
            if (string.IsNullOrEmpty(anexoResponse))
                return mensaje;
            else
                return anexoResponse;
        }

        public async Task<bool> NotificarUsuarios(Guid idInstancia, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriNotificarUsuarios"];
            uriMetodo += "?idInstancia=" + idInstancia ;

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, MOTOR_FLUJO_ENDPOINT, uriMetodo, string.Empty, null, usuarioDNP, useJWTAuth: false);
            return JsonConvert.DeserializeObject<bool>(respuesta);
        }

        /// <summary>
        /// Devuelve al estado anterior del flujo del trámite seleccionado
        /// </summary>
        /// <param name="numeroTramite">número de trámite</param>
        /// <param name="usuarioDnp">Usuario actual logueado</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ResetAccionesFlujoPorTramite(string numeroTramite, string usuarioDnp) {
            var detalleTramiteUrl = string.Format(
                ConfigurationManager.AppSettings["uriResetAccionesFlujoPorTramite"],
                numeroTramite
            );

            try {
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Get,
                    MOTOR_FLUJO_ENDPOINT,
                    detalleTramiteUrl,
                    string.Empty,
                    null,
                    usuarioDnp,
                    useJWTAuth: false
                );

                if (string.IsNullOrEmpty(jsonResponse)) {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> EliminarMarcaPrevioProyectoVigencia(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP)
        {
            ServiciosNegocio.Comunes.Dto.Tramites.ResponseDto<string> response = new ServiciosNegocio.Comunes.Dto.Tramites.ResponseDto<string>();
            string salida = string.Empty;
            try
            {
                string  rta = await Task.FromResult<string>(_tramitePersistencia.EliminarMarcaPrevioProyectoVigencia(bpin, vigencia));

                if (string.IsNullOrEmpty(rta) || !rta.Equals("Exitoso"))
                {
                    await ResetAccionesFlujoPorProyecto(bpin, usuarioDNP);
                    response.Mensaje = "No se puede eliminar marca previo concpeto";
                    throw new Exception(response.Mensaje);
                }
                else
                {
                    salida = "Exitoso";
                }

                return salida;
               
            }
            catch (Exception e)
            {
                await ResetAccionesFlujoPorProyecto(bpin, usuarioDNP);
                response.Mensaje = e.Message;
                throw new Exception(e.Message);
            }
        }

        public async Task<string> EnviarCorreoMarcaPrevio(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP)
        {
            ServiciosNegocio.Comunes.Dto.Tramites.ResponseDto<string> response = new ServiciosNegocio.Comunes.Dto.Tramites.ResponseDto<string>();
            string salida = string.Empty;
            try
            {
                var uriMetodo = ConfigurationManager.AppSettings["uriEnviarCorreoMarcaPrevio"];
                var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, MOTOR_FLUJO_ENDPOINT, uriMetodo, string.Empty, lista, usuarioDNP, useJWTAuth: false);
                if (string.IsNullOrEmpty(respuesta))
                {
                    salida = "No se puede enviar el correo";
                }
                else
                    salida = "Exitoso";
             
                return salida;

            }
            catch (Exception e)
            {
                await ResetAccionesFlujoPorProyecto(bpin, usuarioDNP);
                response.Mensaje = e.Message;
                throw new Exception(e.Message);
            }
        }



        public ResponseDto<bool> ActualizarCargueMasivo(string numeroProceso, string usuario)
        {
            var resultado = _tramitePersistencia.ActualizarCargueMasivo(numeroProceso, usuario);            
            return resultado;
        }

        public string ConsultarCargueExcel(string numeroProceso)
        {
            return _tramitePersistencia.ConsultarCargueExcel(numeroProceso);
        }

       
        public async Task<string> NotificarMarcaPrevio(string bpin, string vigencia, List<CodigoPresupuestal_Proyecto> lista, string usuarioDNP)
        {
            string salida = string.Empty;
            var uriMetodo = ConfigurationManager.AppSettings["uriNotificarMarcaPrevio"];
            lista[0].ListaUsuarios = _tramitePersistencia.ObtenerUsuariosPorInstanciaPadre(lista[0].InstanciaId);

            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, MOTOR_FLUJO_ENDPOINT, uriMetodo, string.Empty, lista, usuarioDNP, useJWTAuth: false);

            if (string.IsNullOrEmpty(respuesta))
            {
                salida = "No se puede enviar el correo";
            }
            else
                salida = "Exitoso";

            return salida;
        }

        public List<CodigoPresupuestal_Proyecto> ObtenerDatosMarcaPrevioVigencia_Proyectos(string Bpin) {

            return _tramitePersistencia.ObtenerDatosMarcaPrevioVigencia_Proyectos(Bpin);
        }


        /// <summary>
        /// Devuelve al estado anterior del flujo del trámite seleccionado
        /// </summary>
        /// <param name="numeroTramite">número de trámite</param>
        /// <param name="usuarioDnp">Usuario actual logueado</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ResetAccionesFlujoPorProyecto(string bpin, string usuarioDnp)
        {
            var detalleTramiteUrls = ConfigurationManager.AppSettings["uriResetAccionesFlujoPorProyecto"].Split('/');
            var detalleTramiteUrl = detalleTramiteUrls[0] + "/" + detalleTramiteUrls[1] + "/" + detalleTramiteUrls[2];
            detalleTramiteUrl += "?bin=" + bpin;
            try
            {
                var jsonResponse = await _clienteHttpServicios.ConsumirServicio(
                    MetodosServiciosWeb.Get,
                    MOTOR_FLUJO_ENDPOINT,
                    detalleTramiteUrl,
                    string.Empty,
                    null,
                    usuarioDnp,
                    useJWTAuth: false
                );

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

      
        #region Métodos privados
        /// <summary>
        /// Retorna el nombre del reporte a consultar de acuerdo con el codigo del tipo de trámite
        /// </summary>
        /// <param name="codigoTipoTramite">Codigo de tipo de trámite</param>
        /// <returns></returns>
        private string ObtenerNombreDocumentoCartaConceptoPorTipoTramite(string codigoTipoTramite)
            {
                string response = "";

                switch (codigoTipoTramite)
                {
                    case "VFO":
                    case "VFE":
                    case "VF":
                        response = ConfigurationManager.AppSettings["nombreDocumentoFirmadoVF"];
                        break;
                    case "AL":
                        response = ConfigurationManager.AppSettings["documentoCartaConceptoAclaracionLeyenda"];
                        break;
                case "I":
                    response = ConfigurationManager.AppSettings["nombrePDFCartaIncorporacion"];
                    break;
                case "AD":
                    response = ConfigurationManager.AppSettings["nombrePDFCartaAdicion"];
                    break;
                case "RVF":
                    response = ConfigurationManager.AppSettings["NombrePDFCartaReprogramacionVF"];
                    break;
                case "TO":
                case "TL":
                case "DPC":
                    response = ConfigurationManager.AppSettings["nombrePDFReporte"];
                        break;
                        default:
                        response = "";
                        break;
                }

                if(string.IsNullOrEmpty(response)) {
                    throw new Exception("No fue posible identificar el formato de la carta concepto por tipo de trámite");
                }

                return response;
            }
        #endregion Métodos privados
    }
}
