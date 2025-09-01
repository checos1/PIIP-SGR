using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Web.API.Controllers.SGP
{
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.SGP;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using DNP.Backbone.Dominio.Dto.SGP;
    using DNP.Backbone.Dominio.Dto.SGP.Viabilidad;
    using DNP.Backbone.Dominio.Dto.SGP.Transversal;
    using DNP.Backbone.Dominio.Dto;

    [Route("api/[controller]")]

    public class SGPViabilidadController : ApiController
    {
        private readonly ISGPViabilidadServicios _sgpServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public SGPViabilidadController(ISGPViabilidadServicios sgpServicios, IAutorizacionServicios autorizacionUtilidades)
        {
            _sgpServicios = sgpServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("SGP/Transversal/LeerParametro")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPTransversalLeerParametro(string parametro)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPTransversalLeerParametro(parametro, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Acuerdo/LeerProyecto")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPAcuerdoLeerProyecto(proyectoId, nivelId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Acuerdo/GuardarProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> SGPAcuerdoGuardarProyecto(AcuerdoSectorClasificadorSGPDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPAcuerdoGuardarProyecto(obj,  User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/LeerListas")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPProyectosLeerListas(nivelId, proyectoId,  nombreLista, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/Viabilidad/LeerInformacionGeneral")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPViabilidadLeerInformacionGeneral(int proyectoId, Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPViabilidadLeerInformacionGeneral(proyectoId, instanciaId, User.Identity.Name, tipoConceptoViabilidadCode));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/Viabilidad/LeerParametricas")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPViabilidadLeerParameticas(int proyectoId, Guid nivelId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPViabilidadLeerParametricas(proyectoId, nivelId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/Viabilidad/GuardarInformacionBasica")]
        [HttpPost]
        public async Task<IHttpActionResult> SGPViabilidadGuardarInformacionBasica(InformacionBasicaViabilidadSGPDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPViabilidadGuardarInformacionBasica(obj, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/GuardarProyectoViabilidadInvolucradosSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoViabilidadInvolucrados(ProyectoViabilidadInvolucradosSGPDto objProyectoViabilidadInvolucradosDto)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarProyectoViabilidadInvolucradosSGP(objProyectoViabilidadInvolucradosDto, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/EliminarProyectoViabilidadInvolucradosSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoViabilidadInvolucradoso(int id)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.EliminarProyectoViabilidadInvolucradosSGP(id, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/LeerProyectoViabilidadInvolucrados")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPProyectosLeerProyectoViabilidadInvolucrados(proyectoId, instanciaId, tipoConceptoViabilidadId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/ObtenerEntidadDestinoResponsableFlujo")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPProyectosObtenerEntidadDestinoResponsableFlujo(rolId, crTypeId, entidadResponsableId, proyectoId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/ObtenerEntidadDestinoResponsableFlujoTramite")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(rolId, entidadResponsableId, tramiteId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGP/Proyectos/LeerProyectoViabilidadInvolucradosFirma")]
        [HttpGet]
        public async Task<IHttpActionResult> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.SGPProyectosLeerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}