using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Flujos
{
	using DNP.Backbone.Comunes.Dto;
	using DNP.Backbone.Comunes.Excepciones;
	using DNP.Backbone.Comunes.Properties;
	using DNP.Backbone.Dominio;
	using DNP.Backbone.Dominio.Dto;
	using DNP.Backbone.Dominio.Dto.Flujos;
	using DNP.Flujos.Dominio.Dto.Flujos;
	using DNP.Backbone.Dominio.Dto.Proyecto;
	using Servicios.Interfaces.Autorizacion;
	using Servicios.Interfaces.ServiciosNegocio;
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;

	public class FlujoController : ApiController
	{
		public HttpResponseMessage RespuestaAutorizacion;
		private readonly IFlujoServicios _flujoServicios;
		private readonly IAutorizacionServicios _autorizacionUtilidades;

		public FlujoController(IFlujoServicios flujoServicios, IAutorizacionServicios autorizacionUtilidades)
		{
			_flujoServicios = flujoServicios;
			_autorizacionUtilidades = autorizacionUtilidades;
		}

		[Route("api/Flujos/GenerarInstancias")]
		[HttpPost]
		public async Task<IHttpActionResult> GenerarInstancias(ParametrosInstanciaDto parametrosInstanciaDto)
		{
			parametrosInstanciaDto.IdUsuarioDNP = User.Identity.Name;
			if (!ValidarParametrosRequest(parametrosInstanciaDto)) return Ok(HttpStatusCode.BadRequest);

			var result = await Task.Run(() => _flujoServicios.GenerarInstancias(parametrosInstanciaDto));
			return Ok(result);
		}

		[Route("api/Flujos/GenerarInstanciasMasivo")]
		[HttpPost]
		public async Task<IHttpActionResult> GenerarInstanciasMasivo(List<ParametrosInstanciaDto> parametrosInstanciaDto)
		{
			parametrosInstanciaDto[0].IdUsuarioDNP = User.Identity.Name;
			if (!ValidarParametrosRequest(parametrosInstanciaDto[0])) return Ok(HttpStatusCode.BadRequest);

			var result = await Task.Run(() => _flujoServicios.GenerarInstanciasMasivo(parametrosInstanciaDto));
			return Ok(result);
		}

		/// <summary>
		/// Obtiene el log de instancias creadas
		/// </summary>
		/// <param name="parametros"></param>
		/// <returns></returns>
		[Route("api/Flujos/ObtenerLogInstancia")]
		[HttpPost]
		public async Task<IHttpActionResult> ObtenerLogInstancia([FromBody] ParametrosLogsInstanciasDto parametros)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerLogInstancia(parametros, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}


		[Route("api/Flujos/ObtenerExcelLogInstancia")]
		[HttpPost]
		public async Task<HttpResponseMessage> ObtenerExcelLogInstancia(IList<LogsInstanciasDto> parametros)
		{

			try
			{
				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

				var _result = parametros;

				result.StatusCode = HttpStatusCode.OK;
				result.Content = ExcelUtilidades.ObtenerExcellLogInstancia(_result);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				//result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment; filename = Proyectos.xlsx");

				return result;
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}


		[Route("api/Flujos/CrearLogFlujo")]
		[HttpPost]
		public async Task<IHttpActionResult> CrearLogFlujo([FromBody] FlujosLogsInstanciasDto logs)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.CrearLogFlujo(logs, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}


		[Route("api/Flujos/ObtenerFlujosLogInstancia")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerFlujosLogInstancia([FromUri] Guid instanciaId, Guid nivelId)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerFlujoLogInstancia(instanciaId, nivelId, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

        [Route("api/Flujos/ObtenerHistoricoObservaciones")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerHistoricoObservaciones([FromUri] Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _flujoServicios.ObtenerHistoricoObservaciones(instanciaId, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Flujos/ObtenerInstanciasPermiso")]
		[HttpPost]
		public async Task<IHttpActionResult> ObtenerInstanciasPermiso([FromBody] ParametrosObjetosNegocioDto parametros)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerInstanciasPermiso(parametros, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/ValidarFlujoConInstanciaActiva")]
		[HttpPost]
		public async Task<IHttpActionResult> ValidarFlujoConInstanciaActiva([FromBody] ParametrosValidarFlujoDto parametros)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ValidarFlujoConInstanciaActiva(parametros, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/ObtenerInstanciasActivasProyectos")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerInstanciasActivasProyectos(string Bpins)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerInstanciasActivasProyectos(Bpins, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		private bool ValidarParametrosRequest(ParametrosInstanciaDto parametrosDto)
		{
			ValidarParametros(parametrosDto);

			RespuestaAutorizacion = _autorizacionUtilidades.
										ValidarUsuario(RequestContext.Principal.Identity.Name,
													   RequestContext.Principal.Identity.GetHashCode().ToString(),
													   ConfigurationManager.AppSettings["IdAplicacionBackbone"],
													   ConfigurationManager.AppSettings["idObtenerInbox"]).
										Result;

			return RespuestaAutorizacion.IsSuccessStatusCode;
		}

		private void ValidarParametros(ParametrosInstanciaDto parametrosDto)
		{
			if (parametrosDto == null)
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, Resources.ParametrosNoRecibidos));

			if (string.IsNullOrWhiteSpace(parametrosDto.IdUsuarioDNP))
				throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdUsuarioDNP"));

			if (parametrosDto.Proyectos == null || !parametrosDto.Proyectos.Any())
				throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "Proyectos"));

		}

		[Route("api/Flujos/ObtenerTramitesInstanciasEstadoCerrado")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerTramitesInstanciasEstadoCerrado(int proyectoId, int entidadId)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerTramitesInstanciasEstadoCerrado(proyectoId, entidadId, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/EliminarInstanciaProyectoTramite")]
		[HttpPost]
		public async Task<IHttpActionResult> EliminarInstanciaProyectoTramite(Guid instanciaTramite, string Bpin)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.EliminarInstanciaProyectoTramite(instanciaTramite, Bpin, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/ObtenerObservacionesPasoPadre")]
		[HttpPost]
		public async Task<IHttpActionResult> ObtenerObservacionesPasoPadre(Guid idInstancia, Guid idAccion)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerObservacionesPasoPadre(idInstancia, idAccion, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/RegistrarPermisosAccionPorUsuario")]
		[HttpPost]
		public async Task<IHttpActionResult> RegistrarPermisosAccionPorUsuario(RegistrarPermisosAccionDto permisosAccion)
		{
			if (!ValidarParametrosPermisosAccion(permisosAccion)) return Ok(HttpStatusCode.BadRequest);
			var result = await Task.Run(() => _flujoServicios.RegistrarPermisosAccionPorUsuario(permisosAccion));
			return Ok(result);
		}
		private bool ValidarParametrosPermisosAccion(RegistrarPermisosAccionDto permisosAccion)
		{
			if (string.IsNullOrWhiteSpace(permisosAccion.ObjetoNegocioId))
				throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "ObjetoNegocioId"));
			if (string.IsNullOrWhiteSpace(permisosAccion.IdAccion.ToString()) || permisosAccion.IdAccion.ToString() == "00000000-0000-0000-0000-000000000000")
				throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdAccion"));
			if (string.IsNullOrWhiteSpace(permisosAccion.IdInstancia.ToString()) || permisosAccion.IdInstancia.ToString() == "00000000-0000-0000-0000-000000000000")
				throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "IdInstancia"));
			if (permisosAccion.EntityTypeCatalogOptionId == 0)
				throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "EntityTypeCatalogOptionId"));
			if (permisosAccion.listadoUsuarios == null)
				throw new BackboneException(string.Format(Resources.ParametroNoRecibido, "listadoUsuarios"));
			return true;
		}
		[Route("api/Flujos/ConsultarAccionPorInstancia")]
		[HttpGet]
		public async Task<IHttpActionResult> ConsultarAccionPorInstancia([FromUri] Guid instanciaId, Guid idAccion)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ConsultarAccionPorInstancia(instanciaId, idAccion, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/Firmar")]
		[HttpPost]
		public async Task<IHttpActionResult> Firmar([FromUri] int tramiteId)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.CerrarInstancia(tramiteId, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/ObtenerDetalleTramite")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerDetallesTramite(string numerotramite)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerDetallesTramite(numerotramite, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/ObtenerProyectosPorTramite")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerProyectosPorTramite(Guid? instanciaId)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerProyectosPorTramite(instanciaId, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujo/ObtenerLog")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerLog(Guid instanciaId)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerLog(instanciaId, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujo/ObtenerLogSubpasos")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerLogSubpasos(Guid instanciaId)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerLogSubpasos(instanciaId, User.Identity.Name));
				
				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}


		[Route("api/Flujo/ObtenerTrazaInstancia")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerTrazaInstancia(Guid instanciaId)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerTrazaInstancia(instanciaId, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		/// <summary>
		/// Crea la instancia de tramite
		/// </summary>
		/// <param name="parametrosInstanciaDto">Parametros de creacion de instancia de tramite</param>
		/// <returns>Lista de instancias creadas</returns>
		[Route("api/Flujo/CrearInstancia")]
		[HttpPost]
		public async Task<IHttpActionResult> CrearInstancia(ParametrosInstanciaFlujoDto parametrosInstanciaDto)
		{
			try
			{
				var result = await _flujoServicios.CrearInstancia(parametrosInstanciaDto, User.Identity.Name);
				return Ok(result);


			}
			catch (Exception e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		/// <summary>
		/// Api para obtención de datos de flujos.
		/// </summary>
		/// <param name="filtroConsulta">Contiene filtro para a lista </param>
		/// <returns>Objeto con propiedades para realizar consulta de datos flujos.</returns>
		[Route("api/Flujo/ObtenerFlujosPorRoles")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerFlujosPorRoles([FromUri] FiltroConsultaOpcionesDto filtroConsulta)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.ObtenerPermisosFlujosPorAplicacionYRoles(filtroConsulta, User.Identity.Name));
				return Ok(result);

			}
			catch (Exception e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/Proyectos")]
		[HttpPost]
		public async Task<IHttpActionResult> ConsultarProyectosEntidadesSinInstanciasActivas(ParametrosProyectosFlujosDto entidadesyEstados)
		{

			try
			{

				var result = await Task.Run(() => _flujoServicios.ConsultarProyectosEntidadesSinInstanciasActivas(entidadesyEstados, User.Identity.Name));

				return Ok(result);

			}
			catch (Exception e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

        [Route("api/Flujos/ObtenerEstadoOcultarObservacionesGenerales")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEstadoOcultarObservacionesGenerales()
        {

            try
            {

                var result = await Task.Run(() => _flujoServicios.ObtenerEstadoOcultarObservacionesGenerales(User.Identity.Name));

                return Ok(result);

            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }



		[Route("api/Flujos/ObtenerValidacionVerAccion")]
		[HttpPost]
		public IHttpActionResult ObtenerValidacionVerAccion(ValidarRolAccionDto data)
		{
			var result = _flujoServicios.ObtenerValidacionVerAccion(data, User.Identity.Name);
			return Ok(result);
		}

		[Route("api/Flujo/ObtenerInstanciaProyecto")]
		[HttpGet]
		public IHttpActionResult ObtenerInstanciaProyecto(Guid idInstancia, string bpin)
		{
			var result = _flujoServicios.ObtenerInstanciaProyecto(idInstancia, bpin, User.Identity.Name);
			return Ok(result);
		}

		/// <summary>
		/// Crea traza de las observaciones 
		/// </summary>
		/// <param name="parametros"></param>
		/// <returns></returns>
		[Route("api/Flujo/CrearTrazaAccionesPorInstancia")]
		[HttpPost]
		public async Task<IHttpActionResult> CrearTrazaAccionesPorInstancia(TrazaAccionesPorInstanciaDto parametros)
		{
			try
			{
				if (string.IsNullOrEmpty(parametros.Observacion))
				{
					throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No contiene observación."));
				}
				parametros.CreadoPor = RequestContext.Principal.Identity.Name;
				var result = await Task.Run(() => _flujoServicios.CrearTrazaAccionesPorInstancia(parametros, User.Identity.Name));
				return Ok(result);
			}

			catch (Exception e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujo/ObtenerDevolucionesPorIdInstanciaYIdAccion")]
		[HttpGet]
		public async Task<IHttpActionResult> ObtenerDevolucionesPorIdInstanciaYIdAccion(Guid idInstancia, Guid idAccion)
		{
			var result = await Task.Run(() => _flujoServicios.ObtenerDevolucionesPorIdInstanciaYIdAccion(idInstancia, idAccion, User.Identity.Name));
			return Ok(result);
		}

		/// <summary>
		/// Se ejecuta una accion del flujo 
		/// </summary>
		/// <param name="parametrosEjecucionFlujo">Accion a ejecutar</param>
		/// <returns> objecto ResultadoEjecucionFlujoDto</returns>
		[Route("api/Flujos/EjecutarFlujo")]
		[HttpPost]
		public async Task<IHttpActionResult> EjecutarFlujo(ParametrosEjecucionFlujo parametrosEjecucionFlujo)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.EjecutarFlujo(parametrosEjecucionFlujo, User.Identity.Name));
				return Ok(result);
			}

			catch (Exception e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		/// <summary>
		/// Se ejecuta una accion del flujo 
		/// </summary>
		/// <param name="parametrosDevolucionFlujo">Accion a devolver</param>
		/// <returns> objecto parametrosDevolucionFlujo</returns>
		[Route("api/Flujos/DevolverFlujo")]
		[HttpPost]
		public async Task<IHttpActionResult> DevolverFlujo(ParametrosDevolverFlujoDto parametrosDevolucionFlujo)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.DevolverFlujo(parametrosDevolucionFlujo, User.Identity.Name));
				return Ok(result);
			}

			catch (Exception e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/EliminarInstanciaCerrada_AbiertaProyectoTramite")]
		[HttpPost]
		public async Task<IHttpActionResult> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.EliminarInstanciaCerrada_AbiertaProyectoTramite(instanciaTramite, Bpin, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/NotificarUsuariosPorInstanciaPadre")]
		[HttpPost]
		public async Task<IHttpActionResult> NotificarUsuariosPorInstanciaPadre(Guid instanciaId, string nombreNotificacion, string texto)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.NotificarUsuariosPorInstanciaPadre(instanciaId, nombreNotificacion, texto, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

        [Route("api/Flujos/ObtenerFlujosPorTipoObjeto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFlujosPorTipoObjeto(Guid tipoObjetoId)
        {
            try
            {
                var result = await Task.Run(() => _flujoServicios.ObtenerFlujosPorTipoObjeto(tipoObjetoId, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/Flujos/ObtenerAccionesFlujoPorFlujoId")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAccionesFlujoPorFlujoId(Guid flujoId)
        {
            try
            {
                var result = await Task.Run(() => _flujoServicios.ObtenerAccionesFlujoPorFlujoId(flujoId, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Flujos/ObtenerVigencias")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerVigencias(Guid tipoObjetoId)
        {
            try
            {
                var result = await Task.Run(() => _flujoServicios.ObtenerVigencias(tipoObjetoId, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Flujos/ExisteFlujoProgramacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ExisteFlujoProgramacion(int entidadId, Guid flujoId)
        {
            try
            {
                var result = await Task.Run(() => _flujoServicios.ExisteFlujoProgramacion(entidadId, flujoId, User.Identity.Name));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

		[Route("api/Flujos/SubPasoEjecutar")]
		[HttpPost]
		public async Task<IHttpActionResult> SubPasoEjecutar(ParametrosEjecucionSubPasoDto oParametrosEjecucionSubPasoDto)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.SubPasoEjecutar(oParametrosEjecucionSubPasoDto, User.Identity.Name));
				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}

		[Route("api/Flujos/SubPasosValidar")]
		[HttpGet]
		public async Task<IHttpActionResult> SubPasosValidar(Guid idInstancia, Guid idAccion)
		{
			try
			{
				var result = await Task.Run(() => _flujoServicios.SubPasosValidar(idInstancia, idAccion, User.Identity.Name));

				return Ok(result);
			}
			catch (BackboneException e)
			{
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
			}
		}
        //[Route("api/Flujos/ProyectosPriorizarSGR")]
        //[HttpGet]
        //public async Task<IHttpActionResult> ConsultarProyectosPriorizarSGR(String IdUsuarioDNP)
        //{
        //    try
        //    {
        //        var result = await Task.Run(() => _flujoServicios.ConsultarProyectosPriorizarSGR(User.Identity.Name, User.Identity.Name));

        //        return Ok(result);

        //    }
        //    catch (Exception e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
        //    }
        //}


    }

}