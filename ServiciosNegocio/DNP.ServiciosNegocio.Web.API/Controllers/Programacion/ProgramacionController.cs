using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Comunes.Dto.Programacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;
using DNP.ServiciosNegocio.Servicios.Interfaces.Programacion;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Web.API.Controllers.Programacion
{
    public class ProgramacionController : ApiController
    {
        private readonly IProgramacionServicio _programacionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ProgramacionController(IProgramacionServicio programacionServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _programacionServicio = programacionServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }        

        [Route("api/Programacion/ValidarCalendarioProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Validar calendario programacion", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ValidarCalendarioProgramacion(int? entityTypeCatalogOptionId, Nullable<Guid> nivelId, Nullable<int> seccionCapituloId)
        {
            var result = await Task.Run(() => _programacionServicio.ValidarCalendarioProgramacion(entityTypeCatalogOptionId, nivelId, seccionCapituloId));

            return Ok(result);
        }

        [Route("api/Programacion/ObtenerCargaMasivaCreditos")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener carga masiva creditos", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCargaMasivaCreditos()
        {
            var result = await Task.Run(() => _programacionServicio.ObtenerCargaMasivaCreditos());

            return Ok(result);
        }

        [Route("api/Programacion/ObtenerProgramacionProyectosSinPresupuestal")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener programación proyectos sin presupuestal", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProgramacionProyectosSinPresupuestal(int? sectorId, int? entidadId, string proyectoId)
        {
            var result = await Task.Run(() => _programacionServicio.ObtenerProgramacionProyectosSinPresupuestal(sectorId, entidadId, proyectoId));

            return Ok(result);
        }

        [Route("api/Programacion/ObtenerCargaMasivaCuotas")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener carga masiva cuotas", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId)
        {
            var result = await Task.Run(() => _programacionServicio.ObtenerCargaMasivaCuotas(Vigencia, EntityTypeCatalogOptionId));

            return Ok(result);
        }

        [Route("api/Programacion/ObtenerProgramacionSectores")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener programación sectores", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProgramacionSectores(int? sectorId)
        {
            var result = await Task.Run(() => _programacionServicio.ObtenerProgramacionSectores(sectorId));

            return Ok(result);
        }

        [Route("api/Programacion/ObtenerProgramacionEntidadesSector")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener programación entidades por sector", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProgramacionEntidadesSector(int? sectorId)
        {
            var result = await Task.Run(() => _programacionServicio.ObtenerProgramacionEntidadesSector(sectorId));

            return Ok(result);
        }

        [Route("api/Programacion/ObtenerCalendarioProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener programación calendario", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCalendarioProgramacion(Guid FlujoId)
        {
            var result = await Task.Run(() => _programacionServicio.ObtenerCalendarioProgramacion(FlujoId));

            return Ok(result);
        }

        [Route("api/Programacion/RegistrarCargaMasivaCreditos")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar cargue masivo creditos", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarCargaMasivaCreditos([FromBody] List<CargueCreditoDto> json)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.RegistrarCargaMasivaCreditos(json, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ObtenerDatosProgramacionEncabezado")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos Encabezado Programacion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int TramiteId, string origen)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, 
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(), 
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], 
                                                                                    ConfigurationManager.AppSettings["ObtenerDatosProgramacionEncabezado"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                var result = await Task.Run(() => _programacionServicio.ObtenerDatosProgramacionEncabezado(EntidadDestinoId, TramiteId, origen));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Programacion/GuardarDatosProgramacionDistribucion")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Programacion Distribucion", typeof(ProgramacionDistribucionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto objProgramacionDistribucionDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["GuardarDatosProgramacionDistribucion"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.GuardarDatosProgramacionDistribucion(objProgramacionDistribucionDto, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ObtenerDatosProgramacionDetalle")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos deatelle de la Programacion", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosProgramacionDetalle(int tramiteidProyectoId, string origen)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["ObtenerDatosProgramacionDetalle"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _programacionServicio.ObtenerDatosProgramacionDetalle(tramiteidProyectoId, origen));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Programacion/ValidarCargaMasivaCreditos")]
        [SwaggerResponse(HttpStatusCode.OK, "Validar cargue masivo creditos", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarCargaMasivaCreditos([FromBody] List<CargueCreditoDto> json)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicio.ValidarCargaMasivaCreditos(json));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }

        [Route("api/Programacion/RegistrarCargaMasivaCuota")]
        [SwaggerResponse(HttpStatusCode.OK, " Registrar cargue masivo de cuotas", typeof(ProgramacionDistribucionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarCargaMasivaCuota([FromBody] List<CargueCuotaDto> json)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.RegistrarCargaMasivaCuota(json, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/RegistrarProyectosSinPresupuestal")]
        [SwaggerResponse(HttpStatusCode.OK, " Registrar proyectos sin presupuestal", typeof(ProyectoSinPresupuestalDto))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarProyectosSinPresupuestal([FromBody] List<ProyectoSinPresupuestalDto> json)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.RegistrarProyectosSinPresupuestal(json, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/RegistrarCalendarioProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, " Registrar calendario programacion", typeof(CalendarioProgramacionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarCalendarioProgramacion([FromBody] List<CalendarioProgramacionDto> json)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.RegistrarCalendarioProgramacion(json, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/GuardarDatosProgramacionFuentes")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Programacion Distribucion", typeof(ProgramacionFuenteDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosProgramacionFuentes(ProgramacionFuenteDto objProgramacionFuenteDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["GuardarDatosProgramacionFuente"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.GuardarDatosProgramacionFuente(objProgramacionFuenteDto, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ValidarConsecutivoPresupuestal")]
        [SwaggerResponse(HttpStatusCode.OK, "Validar consecutivo presupuestal", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarConsecutivoPresupuestal([FromBody] List<ProyectoSinPresupuestalDto> json)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicio.ValidarConsecutivoPresupuestal(json));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }

        [Route("api/Programacion/ValidarCargaMasivaCuotas")]
        [SwaggerResponse(HttpStatusCode.OK, "Validar cargue masivo cuotas", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarCargaMasivaCuotas([FromBody] List<CargueCuotaDto> json)
        {
            try
            {
                var result = await Task.Run(() => _programacionServicio.ValidarCargaMasivaCuotas(json));
                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                return CrearRespuestaError(e.Message);
            }
        }

        [Route("api/Programacion/ObtenerDatostProgramacionProducto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos ProgramacionProducto", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatostProgramacionProducto(int tramiteId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["ObtenerDatostProgramacionProducto"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _programacionServicio.ObtenerDatostProgramacionProducto(tramiteId));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Programacion/GuardarDatosProgramacionProducto")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Programacion Distribucion", typeof(ProgramacionFuenteDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosProgramacionProducto(ProgramacionProductoDto ProgramacionProductoDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["GuardarDatosProgramacionProducto"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.GuardarDatosProgramacionProducto(ProgramacionProductoDto, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/GuardarDatosProgramacionIniciativa")]
        [SwaggerResponse(HttpStatusCode.OK, " Funcion para Registrar los Datos Programacion Iniciativa", typeof(ProgramacionIniciativaDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto ProgramacionIniciativa)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["GuardarDatosProgramacionIniciativa"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.GuardarDatosProgramacionIniciativa(ProgramacionIniciativa, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/GuardarProgramacionRegionalizacion")]
        [SwaggerResponse(HttpStatusCode.OK, " Permite registrar la regionalización del POAI", typeof(ProgramacionRegionalizacionDto))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto programacionRegionalizacionDto)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["GuardarProgramacionRegionalizacion"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.GuardarProgramacionRegionalizacion(programacionRegionalizacionDto, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ConsultarPoliticasTransversalesProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesProgramacion(string Bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ConsultarPoliticasTransversalesProgramacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _programacionServicio.ConsultarPoliticasTransversalesProgramacion(Bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/AgregarPoliticasTransversalesProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta al agregar una nueva politica en programacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto objIncluirPoliticasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["AgregarPoliticasTransversalesProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.AgregarPoliticasTransversalesProgramacion(objIncluirPoliticasDto, RequestContext.Principal.Identity.Name));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/ConsultarPoliticasTransversalesCategoriasProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin)
        {

            var result = await Task.Run(() => _programacionServicio.ConsultarPoliticasTransversalesCategoriasProgramacion(Bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/EliminarPoliticasProyectoProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Elimina la politica tranversal de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["EliminarPoliticasProyectoProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                await Task.Run(() => _programacionServicio.EliminarPoliticasProyectoProgramacion(tramiteidProyectoId, politicaId));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };
                return Ok(respuesta);

            }
            catch (ServiciosNegocioException e)
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    ReasonPhrase = e.Message,
                };
                return Ok(respuesta);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/AgregarCategoriasPoliticaTransversalesProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["AgregarCategoriasPoliticaTransversalesProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.AgregarCategoriasPoliticaTransversalesProgramacion(objIncluirPoliticasDto, usuario));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/GuardarPoliticasTransversalesCategoriasProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.GuardarPoliticasTransversalesCategoriasProgramacion(objIncluirPoliticasDto, usuario));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/EliminarCategoriasProyectoProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado definitivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto)
        {
            try
            {
                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.EliminarCategoriasProyectoProgramacion(objIncluirPoliticasDto, usuario));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/EliminarCategoriaPoliticasProyectoProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Eliminar politica de un proyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name, RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"], ConfigurationManager.AppSettings["EliminarCategoriaPoliticasProyectoProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                await Task.Run(() => _programacionServicio.EliminarCategoriaPoliticasProyectoProgramacion(proyectoId, politicaId, categoriaId));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };
                return Ok(respuesta);

            }
            catch (ServiciosNegocioException e)
            {
                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    ReasonPhrase = e.Message,
                };
                return Ok(respuesta);
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/ObtenerCrucePoliticasProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales que se pueden actualizar con otras políticas por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasProgramacion(string Bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerCrucePoliticasProgramacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _programacionServicio.ObtenerCrucePoliticasProgramacion(Bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/PoliticasSolicitudConceptoProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales que solicitan concepto técnico para asociar en programacion.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> PoliticasSolicitudConceptoProgramacion(string Bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["PoliticasSolicitudConceptoProgramacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _programacionServicio.PoliticasSolicitudConceptoProgramacion(Bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/GuardarCrucePoliticasProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por guardado del cruce de políticas", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["GuardarCrucePoliticasProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.GuardarCrucePoliticasProgramacion(parametrosGuardar, usuario));

                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/SolicitarConceptoDTProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Permite solicitar concepto para políticas transversales en programacion", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["SolicitarConceptoDTProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.SolicitarConceptoDTProgramacion(parametrosGuardar, usuario));

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    ReasonPhrase = ServiciosNegocioRecursos.PostExitoso
                };

                return Ok(result);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/ObtenerResumenSolicitudConceptoProgramacion")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con los datos de la solicitud de concepto de una política de programación por proyecto.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenSolicitudConceptoProgramacion(string Bpin)
        {
            var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                ConfigurationManager.AppSettings["ObtenerResumenSolicitudConceptoProgramacion"]).Result;

            if (!respuestaAutorizacion.IsSuccessStatusCode)
                return ResponseMessage(respuestaAutorizacion);

            var result = await Task.Run(() => _programacionServicio.ObtenerResumenSolicitudConceptoProgramacion(Bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ObtenerProgramacionBuscarProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna listado de Datos ObtenerProgramacionBuscarProyecto", typeof(string))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProgramacionBuscarProyecto(int EntidadDestinoId, int tramiteid, string bpin, string NombreProyecto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                    RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                    ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                    ConfigurationManager.AppSettings["ObtenerProgramacionBuscarProyecto"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                var result = await Task.Run(() => _programacionServicio.ObtenerProgramacionBuscarProyecto(EntidadDestinoId, tramiteid, bpin, NombreProyecto));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Programacion/BorrarTramiteProyecto")]
        [SwaggerResponse(HttpStatusCode.OK, "Informacion devuelta por Borrar TramiteProyecto", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> BorrarTramiteProyecto(ProgramacionDistribucionDto ProgramacionDistribucion)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["BorrarTramiteProyecto"]).Result;
            if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.BorrarTramiteProyecto(ProgramacionDistribucion, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        

        

        [Route("api/Programacion/GuardarDatosInclusion")]
        [SwaggerResponse(HttpStatusCode.OK, "Guarda el proyecto incluido", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosInclusion(ProgramacionDistribucionDto ProgramacionDistribucion)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                                                                                   RequestContext.Principal.Identity.GetHashCode().ToString(),
                                                                                   ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                                                                                   ConfigurationManager.AppSettings["GuardarDatosInclusion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);
                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.GuardarDatosInclusion(ProgramacionDistribucion, usuario));
                if (result != null) return Ok(result);

                var respuestaHttp = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuestaHttp);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/ConsultarPoliticasTransversalesCategoriasModificaciones")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto para modificaciones.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin)
        {
            var result = await Task.Run(() => _programacionServicio.ConsultarPoliticasTransversalesCategoriasModificaciones(Bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/GuardarPoliticasTransversalesCategoriasModificaciones")]
        [SwaggerResponse(HttpStatusCode.OK, "Registro de politicas y categorias para modificaciones", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["SolicitarConceptoDTProgramacion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.GuardarPoliticasTransversalesCategoriasModificaciones(objIncluirPoliticasDto, usuario));

                if (result != null) return Ok(result);

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }

        [Route("api/Programacion/ConsultarPoliticasTransversalesAprobacionesModificaciones")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de politicas transversales por proyecto para modificaciones.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin)
        {
            var result = await Task.Run(() => _programacionServicio.ConsultarPoliticasTransversalesAprobacionesModificaciones(Bpin));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }


        #region carga masiva saldos

        [Route("api/Programacion/RegistrarCargaMasivaSaldos")]
        [SwaggerResponse(HttpStatusCode.OK, "Registrar cargue masivo saldos", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarCargaMasivaSaldos([FromBody] int TipoCargueId)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.RegistrarCargaMasivaSaldos(TipoCargueId, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ObtenerLogErrorCargaMasivaSaldos")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener log errores cargue masivo saldos", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.ObtenerLogErrorCargaMasivaSaldos(TipoCargueDetalleId, CarguesIntegracionId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ObtenerCargaMasivaSaldos")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener cargue masivo saldos", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCargaMasivaSaldos(string TipoCargue)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.ObtenerCargaMasivaSaldos(TipoCargue));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ObtenerTipoCargaMasiva")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener tipo cargue masivo", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTipoCargaMasiva(string TipoCargue)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.ObtenerTipoCargaMasiva(TipoCargue));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ValidarCargaMasiva")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener tipo cargue masivo", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarCargaMasiva(dynamic jsonListaRegistros)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.ValidarCargaMasiva(jsonListaRegistros, usuario));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ObtenerDetalleCargaMasivaSaldos")]
        [SwaggerResponse(HttpStatusCode.OK, "Obtener detalle cargue masivo saldos", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleCargaMasivaSaldos(int? CargueId)
        {
            string usuario = RequestContext.Principal.Identity.Name;

            var result = await Task.Run(() => _programacionServicio.ObtenerDetalleCargaMasivaSaldos(CargueId));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };
            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/ConsultarCatalogoIndicadoresPolitica")]
        [SwaggerResponse(HttpStatusCode.OK, "Retorna string con listado de indicadores asociados a una politica transversal.", typeof(HttpResponseMessage))]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio)
        {
            var result = await Task.Run(() => _programacionServicio.ConsultarCatalogoIndicadoresPolitica(PoliticaId, Criterio));
            if (result != null) return Ok(result);

            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        [Route("api/Programacion/GuardarModificacionesAsociarIndicadorPolitica")]
        [SwaggerResponse(HttpStatusCode.OK, "Registro de indicadores asociados a categorias de politicas tranversales para modificaciones", typeof(HttpResponseMessage))]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion)
        {
            try
            {
                var respuestaAutorizacion = _autorizacionUtilidades.ValidarUsuario(RequestContext.Principal.Identity.Name,
                    RequestContext.Principal.Identity.GetHashCode().ToString(), ConfigurationManager.AppSettings["idAdministracionEnAutorizaciones"],
                    ConfigurationManager.AppSettings["GuardarDatosInclusion"]).Result;
                if (!respuestaAutorizacion.IsSuccessStatusCode) return ResponseMessage(respuestaAutorizacion);

                string usuario = RequestContext.Principal.Identity.Name;

                var result = await Task.Run(() => _programacionServicio.GuardarModificacionesAsociarIndicadorPolitica(proyectoId, politicaId, categoriaId, indicadorId,accion, usuario));

                if (result != null) return Ok(result);

                var respuesta = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = ServiciosNegocioRecursos.SinResultados
                };

                return ResponseMessage(respuesta);
            }
            catch (ServiciosNegocioException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (AutorizacionException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, e));
            }
        }


        #endregion  carga masiva saldos
        #region Respuestas Servicio        
        private IHttpActionResult Responder(object listaProyecto)
        {
            return listaProyecto != null ? Ok(listaProyecto) : CrearRespuestaNoFound();
        }

        private IHttpActionResult CrearRespuestaNoFound()
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound,
                ReasonPhrase = ServiciosNegocioRecursos.SinResultados
            };

            return ResponseMessage(respuestaHttp);
        }

        private IHttpActionResult CrearRespuestaError(string message)
        {
            var respuestaHttp = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = message
            };

            return ResponseMessage(respuestaHttp);
        }
        #endregion
    }
}