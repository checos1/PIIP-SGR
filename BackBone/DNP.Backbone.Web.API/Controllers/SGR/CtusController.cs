using System.Threading.Tasks;
using System.Web.Http;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.SGR;
using System.Net;
using System.Net.Http;
using DNP.Backbone.Dominio.Dto.SGR.CTUS;
using System;

namespace DNP.Backbone.Web.API.Controllers.SGR
{
    [Route("api/[controller]")]

    public class CtusController : ApiController
    {
        private readonly ISGRCtusServicios _sgrServicios;

        public CtusController(ISGRCtusServicios sgrServicios)
        {
            _sgrServicios = sgrServicios;
        }

        [Route("api/SGR/CTUS/LeerProyectoCtusConcepto")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_CTUS_LeerProyectoCtusConcepto(proyectoCtusId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/CTUS/GuardarProyectoCtusConcepto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_CTUS_GuardarProyectoCtusConcepto(obj, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/CTUS/GuardarAsignacionUsuarioEncargado")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_CTUS_GuardarAsignacionUsuarioEncargado(obj, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/CTUS/LeerProyectoCtusUsuarioEncargado")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_CTUS_LeerProyectoCtusUsuarioEncargado(proyectoCtusId, instanciaId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/CTUS/GuardarResultadoConceptoCtus")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_CTUS_GuardarResultadoConceptoCtus(obj, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Actualizar entidad adscrita CTUS
        /// </summary>     
        /// <param name="proyectoId"></param>       
        /// <param name="entityId"></param>         
        /// <returns>int</returns> 
        [Route("api/SGR/CTUS/ActualizarEntidadAdscritaCTUS")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_ActualizarEntidadAdscritaCTUS(proyectoId, entityId, tipo, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}