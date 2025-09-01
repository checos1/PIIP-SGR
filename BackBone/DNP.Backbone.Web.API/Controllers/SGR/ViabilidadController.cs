using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.SGR
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.SGR;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using DNP.Backbone.Dominio.Dto.SGR;
    using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
    using Newtonsoft.Json.Linq;
    using DNP.Backbone.Comunes.Extensiones;

    [Route("api/[controller]")]

    public class ViabilidadController : ApiController
    {
        private readonly ISGRViabilidadServicios _sgrServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public ViabilidadController(ISGRViabilidadServicios sgrServicios, IAutorizacionServicios autorizacionUtilidades)
        {
            _sgrServicios = sgrServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGR/Transversal/LeerParametro")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_LeerParametro(string parametro)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Transversal_LeerParametro(parametro, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Transversal/LeerListaParametros")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_LeerListaParametros()
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Transversal_LeerListaParametros(User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Acuerdo/LeerProyecto")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Acuerdo_LeerProyecto(proyectoId, nivelId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Acuerdo/GuardarProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Acuerdo_GuardarProyecto(AcuerdoSectorClasificadorDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Acuerdo_GuardarProyecto(obj,  User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Proyectos/LeerListas")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_LeerListas(nivelId, proyectoId,  nombreLista, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Viabilidad/LeerInformacionGeneral")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, User.Identity.Name, tipoConceptoViabilidadCode));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Viabilidad/LeerParametricas")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Viabilidad_LeerParameticas(int proyectoId, Guid nivelId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Viabilidad_LeerParametricas(proyectoId, nivelId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Viabilidad/GuardarInformacionBasica")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Viabilidad_GuardarInformacionBasica(InformacionBasicaViabilidadDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Viabilidad_GuardarInformacionBasica(obj, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Viabilidad/ObtenerPuntajeProyecto")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Viabilidad_ObtenerPuntajeProyecto(System.Guid instanciaId, int entidadId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Viabilidad_ObtenerPuntajeProyecto(instanciaId, entidadId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Viabilidad/GuardarPuntajeProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Viabilidad_GuardarPuntajeProyecto([FromBody] JObject puntajesProyecto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Viabilidad_GuardarPuntajeProyecto(puntajesProyecto.Serialize(), User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}