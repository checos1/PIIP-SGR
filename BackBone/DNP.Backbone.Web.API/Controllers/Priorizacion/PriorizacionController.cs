using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto.Priorizacion;
using DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad;
using DNP.Backbone.Dominio.Dto.SGR.GestionRecursos;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Priorizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.Priorizacion
{
    public class PriorizacionController : Base.BackboneBase
    {
        private readonly IPriorizacionServicios _priorizacionServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        /// <summary>
        /// Constructor de clases
        /// </summary>
        /// <param name="priorizacionServicios">Instancia de servicios de priorizacion</param>
        /// <param name="autorizacionUtilidades">Instancia de servicios de autorizacion</param>
        public PriorizacionController(IPriorizacionServicios priorizacionServicios,
            IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _priorizacionServicios = priorizacionServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/priorizacion/ConsultarPriorizacionPorBPINs")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerProyectosPorBPINs([FromBody] BPINsProyectosDto bpins)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerProyectosPorBPINs(bpins, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/priorizacion/ConsultarFuentesSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesSGR(string bpin, Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerFuentesSGR(bpin, instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/priorizacion/RegistrarFuentesSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarFuentesSGR(List<EtapaSGRDto> jsonEtapa)
        {
            try
            {
                var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _priorizacionServicios.RegistrarFuentesSGR(jsonEtapa, UsuarioDNP));

            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/priorizacion/ConsultarFuentesNoSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuentesNoSGR(string bpin, Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerFuentesNoSGR(bpin, instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/priorizacion/RegistrarFuentesNoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarFuentesNoSGR(List<EtapaNoSGRDto> jsonEtapa)
        {
            try
            {
                var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _priorizacionServicios.RegistrarFuentesNoSGR(jsonEtapa, UsuarioDNP));

            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/priorizacion/ConsultarResumenFuentesCostos")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenFuentesCostos(string bpin, Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerResumenFuentesCostos(bpin, instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Api para lista de Departamentos.
        /// </summary>
        /// <param name="peticion">Contiene informacion de autorizacion</param>
        /// <returns>Lista de sectores</returns>
        [Route("api/priorizacion/TiposCofinanciaciones")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarTiposCofinanciaciones()
        {
            try
            {
                var result = await _priorizacionServicios.ObtenerTiposCofinanciaciones(UsuarioLogadoDto.IdUsuario).ConfigureAwait(false);
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/priorizacion/RegistrarDatosCofinanciadorFuentesNoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto jsonVigencias)
        {
            try
            {
                var UsuarioDNP = UsuarioLogadoDto.IdUsuario;
                return Ok(await _priorizacionServicios.RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(jsonVigencias, UsuarioDNP));

            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Priorizacion/ConsultarDatosCofinanciadorFuentesNoSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, int? vigencia, int? vigenciaFuente)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerDatosAdicionalesCofinanciadorNoSGR(bpin, vigencia, vigenciaFuente, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Priorizacion/ObtenerPriorizacionProyecto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPriorizacionProyecto(Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerPriorizacionProyecto(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Priorizacion/ObtenerAprobacionProyecto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAprobacionProyecto(Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerAprobacionProyecto(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Priorizacion/ObtenerPriorizionProyectoDetalle")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPriorizionProyectoDetalle(Nullable<Guid> instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerPriorizionProyectoDetalleSGR(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Priorizacion/GuardarPriorizionProyectoDetalleSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.GuardarPriorizionProyectoDetalleSGR(proyectoPriorizacionDetalleDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Priorizacion/GuardarPermisosPriorizionProyectoDetalleSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.GuardarPermisosPriorizionProyectoDetalleSGR(proyectoPriorizacionDetalleDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Aprobacion/ObtenerAprobacionProyectoCredito")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerAprobacionProyectoCredito(Guid instancia, int entidad)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.ObtenerAprobacionProyectoCredito(instancia, entidad, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Aprobacion/GuardarAprobacionProyectoCredito")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarAprobacionProyectoCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto)
        {
            try
            {
                var result = await Task.Run(() => _priorizacionServicios.GuardarAprobacionProyectoCredito(aprobacionProyectoCreditoDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}