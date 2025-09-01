namespace DNP.Backbone.Web.API.Controllers.SGR
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto;
    using DNP.Backbone.Dominio.Dto.DesignacionEjecutor;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.SGR;
    using DNP.Backbone.Dominio.Dto.SGR.AvalUso;
    using DNP.Backbone.Dominio.Dto.SGR.CTEI;
    using DNP.Backbone.Dominio.Dto.SGR.CTUS;
    using DNP.Backbone.Dominio.Dto.SGR.GestionRecursos;
    using DNP.Backbone.Dominio.Dto.SGR.OcadPaz;
    using DNP.Backbone.Dominio.Dto.SGR.Transversal;
    using DNP.Backbone.Dominio.Dto.SGR.Viabilidad;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.SGR;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Linq;
    using Newtonsoft.Json;

    public class SGRController : Base.BackboneBase
    {
        private readonly ISGRServicios _sgrServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public SGRController(ISGRServicios sgrServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _sgrServicios = sgrServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/SGR/ObtenerOperacionCreditoDatosGenerales")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerOperacionCreditoDatosGenerales(string bpin, Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerOperacionCreditoDatosGenerales(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/GuardarOperacionCreditoDatosGenerales")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/EliminarOperacionCredito")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarOperacionCredito(int proyectoid)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.EliminarOperacionCreditoSGR(proyectoid, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/ObtenerOperacionCreditoDetalles")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerOperacionCreditoDetalles(string bpin, Guid? instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerOperacionCreditoDetalles(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/GuardarOperacionCreditoDetalles")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/ObtenerEjecutorByTipoEntidad")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEjecutorByTipoEntidad(int idTipoEntidad)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerEjecutorByTipoEntidad(idTipoEntidad, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/ObtenerEjecutores")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEjecutores(string nit, int? tipoEntidadId, int? entidadId)
        {
            try
            {
                nit = nit == null ? string.Empty : nit;
                tipoEntidadId = tipoEntidadId == null ? 0 : tipoEntidadId;
                var result = await Task.Run(() => _sgrServicios.ObtenerListadoEjecutores(nit, tipoEntidadId, entidadId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/ObtenerEjecutoresAsociados")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerEjecutoresAsociados(int proyectoId)
        {
            try
            {

                var result = await Task.Run(() => _sgrServicios.ObtenerListadoEjecutoresAsociados(proyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/CrearEjecutorAsociado")]
        [HttpPost]
        public async Task<IHttpActionResult> CrearEjecutorAsociado(int proyectoId, int ejecutorId, int tipoEjecutorId)
        {
            try
            {

                var result = await Task.Run(() => _sgrServicios.CrearEjecutorAsociado(proyectoId, ejecutorId, UsuarioLogadoDto.IdUsuario, tipoEjecutorId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/EliminarEjecutorAsociado")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarEjecutorAsociado(int EjecutorAsociadoId)
        {
            try
            {

                var result = await Task.Run(() => _sgrServicios.EliminarEjecutorAsociado(EjecutorAsociadoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Transversales/ObtenerDesagregarRegionalizacionSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDesagregarRegionalizacionSgr(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerDesagregarRegionalizacionSgr(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        //[Route("SGR/Transversales/ObtenerDatosGeneralesProyectoSgr")]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerDatosGeneralesProyectoSgr(int? pProyectoId, Guid pNivelId)
        //{
        //    try
        //    {
        //        var result = await Task.Run(() => _sgrServicios.ObtenerDatosGeneralesProyectoSgr(pProyectoId, pNivelId, UsuarioLogadoDto.IdUsuario));
        //        return Ok(result);
        //    }
        //    catch (BackboneException e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
        //    }
        //}

        [Route("SGR/Transversales/ObtenerFocalizacionPoliticasTransversalesFuentesSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFocalizacionPoliticasTransversalesFuentesSgr(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerFocalizacionPoliticasTransversalesFuentesSgr(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Transversales/GuardarFocalizacionCategoriasAjustesSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFocalizacionCategoriasAjustesSgr(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarFocalizacionCategoriasAjustesSgr(focalizacionCategoriasAjuste, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        //[Route("SGR/Transversales/ObtenerPoliticasTransversalesCrucePoliticasSgr")]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCrucePoliticasSgr(string bpin, int IdFuente)
        //{
        //    try
        //    {
        //        var result = await Task.Run(() => _sgrServicios.ObtenerPoliticasTransversalesCrucePoliticasSgr(bpin, IdFuente, UsuarioLogadoDto.IdUsuario));
        //        return Ok(result);
        //    }
        //    catch (BackboneException e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
        //    }
        //}
        //[Route("SGR/Transversales/ObtenerDatosIndicadoresPoliticaSgr")]
        //[HttpGet]
        //public async Task<IHttpActionResult> ObtenerDatosIndicadoresPoliticaSgr(string bpin)
        //{
        //    try
        //    {
        //        var result = await Task.Run(() => _sgrServicios.ObtenerDatosIndicadoresPoliticaSgr(bpin, UsuarioLogadoDto.IdUsuario));
        //        return Ok(result);
        //    }
        //    catch (BackboneException e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
        //    }
        //}
        [Route("SGR/Transversales/ObtenerDatosCategoriaProductosPoliticaSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCategoriaProductosPoliticaSgr(string bpin, int fuenteId, int politicaId)
        {
            try
            {
                var result = await Task.Run(() =>
                _sgrServicios.ObtenerCategoriaProductosPoliticaSgr(bpin, fuenteId, politicaId, UsuarioLogadoDto.IdUsuario,
                Request.Headers.Authorization.Parameter));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }



        [Route("SGR/Transversales/ObtenerPoliticasTransversalesProyectoSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesProyectoSgr(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerPoliticasTransversalesProyectoSgr(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/EliminarPoliticasProyectoSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPoliticasProyectoSgr(int proyectoId, int politicaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.EliminarPoliticasProyectoSgr(proyectoId, politicaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/AgregarPoliticasTransversalesAjustesSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarPoliticasTransversalesAjustesSgr(CategoriaProductoPoliticaDto objPoliticaTransversalDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.AgregarPoliticasTransversalesAjustesSgr(objPoliticaTransversalDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/ConsultarPoliticasCategoriasIndicadoresSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasCategoriasIndicadoresSgr(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ConsultarPoliticasCategoriasIndicadoresSgr(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/ObtenerPoliticasTransversalesCategoriasSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCategoriasSgr(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerPoliticasTransversalesCategoriasSgr(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/EliminarCategoriasPoliticasProyectoSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriasPoliticasProyectoSgr(int proyectoId, int politicaId, int categoriaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.EliminarCategoriasPoliticasProyectoSgr(proyectoId, politicaId, categoriaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/ModificarPoliticasCategoriasIndicadoresSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> ModificarPoliticasCategoriasIndicadoresSgr(CategoriasIndicadoresDto parametrosGuardar)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ModificarPoliticasCategoriasIndicadoresSgr(parametrosGuardar, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/ObtenerCrucePoliticasAjustesSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasAjustesSgr(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerCrucePoliticasAjustesSgr(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/GuardarCrucePoliticasAjustesSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCrucePoliticasAjustesSgr(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarCrucePoliticasAjustesSgr(objListCruecePoliticasAjustesDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("SGR/Transversales/ObtenerPoliticasTransversalesResumenSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesResumenSgr(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerPoliticasTransversalesResumenSgr(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Transversal/ObtenerTipoDocumentoSoporte")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTipoDocumentoTramite(int tipoTramiteId, int? tramiteId, string roles, string nivelId, string instanciaId, string accionId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerTipoDocumentoSoporte(tipoTramiteId, roles, tramiteId, User.Identity.Name, nivelId, instanciaId, accionId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Transversal/ObtenerListaTipoDocumentoSoporte")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerListaTipoDocumentoTramite(int tipoTramiteId, int? tramiteId, string roles, string nivelId, string instanciaId, string accionId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerListaTipoDocumentoSoporte(tipoTramiteId, roles, tramiteId, User.Identity.Name, nivelId, instanciaId, accionId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/LeerProyectoViabilidadInvolucrados")]
        [HttpGet]
        public async Task<IHttpActionResult> LeerProyectoViabilidadInvolucrados(int proyectoId, int tipoConceptoViabilidadId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.LeerProyectoViabilidadInvolucrados(proyectoId, tipoConceptoViabilidadId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/ProyectoViabilidadInvolucrados/Agregar")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoViabilidadInvolucrados(ProyectoViabilidadInvolucradosDto objProyectoViabilidadInvolucradosDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarProyectoViabilidadInvolucrados(objProyectoViabilidadInvolucradosDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/ProyectoViabilidadInvolucrados/Eliminar")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarProyectoViabilidadInvolucradoso(int id)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.EliminarProyectoViabilidadInvolucradoso(id, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/TiposRecursosEntidadPorGrupoRecursos")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ConsultarTiposRecursosEntidadPorGrupoRecursos(entityTypeCatalogId, resourceGroupId, incluir, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/LeerProyectoViabilidadInvolucradosFirma")]
        [HttpGet]
        public async Task<IHttpActionResult> LeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.LeerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [HttpPost]
        [Route("api/SGR/CargarFirma")]
        public async Task<IHttpActionResult> CargarFirma(FileToUploadDto parametro)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.CargarFirma(parametro.FileAsBase64, parametro.RolId.ToString(), parametro.UsuarioId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [HttpPost]
        [Route("api/SGR/BorrarFirma")]
        public async Task<IHttpActionResult> BorrarFirma(FileToUploadDto parametro)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.BorrarFirma(parametro.UsuarioId));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/validarSiExisteFirmaUsuario")]
        [HttpPost]
        public async Task<IHttpActionResult> ValidarSiExisteFirmaUsuario(string usuarioDnp)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ValidarSiExisteFirmaUsuario(usuarioDnp));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Firmar")]
        [HttpPost]
        public async Task<IHttpActionResult> Firmar(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp, int entidadId)
        {
            RespuestaGeneralDto respuestasalida = new RespuestaGeneralDto();
            try
            {

                respuestasalida = await Task.Run(() => _sgrServicios.Firmar(instanciaId, tipoConceptoViabilidadId, usuarioDnp, entidadId));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/EliminarFirma")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarFirma(Guid instanciaId, int tipoConceptoViabilidadId, string usuarioDnp, int entidadId)
        {
            RespuestaGeneralDto respuestasalida = new RespuestaGeneralDto();
            try
            {

                respuestasalida = await Task.Run(() => _sgrServicios.EliminarFirma(instanciaId, tipoConceptoViabilidadId, usuarioDnp, entidadId));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/LeerProyectoCtus")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerProyectoCtus(int ProyectoId, Guid instanciaId)
        {
            try
            {

                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_LeerProyectoCtus(ProyectoId, instanciaId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/LeerEntidadesSolicitarCtus")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerEntidadesSolicitarCtus(int ProyectoId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_LeerEntidadesSolicitarCtus(ProyectoId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/GuardarProyectoCtus")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoSolicitarCTUS(ProyectoCtusDto objProyectoCtusDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarProyectoSolicitarCTUS(objProyectoCtusDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Transversal/LeerConfiguracionReportes")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ObtenerConfiguracionReportes(Guid instanciaId)
        {
            try
            {

                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Transversal_ObtenerConfiguracionReportes(instanciaId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/ObtenerUsuariosVerificacionOcadPaz")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(EntidadRolDto entidadRol)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(entidadRol.RolId, entidadRol.EntidadId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/OCADPaz/GuardarAsignacionUsuarioEncargado")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioOcadPazDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(obj, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Transversal/AutorizacionAccionesPorInstanciaSubFlujoOCADPaz")]
        [HttpPost]
        public async Task<IHttpActionResult> AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(Guid instanciaId, Guid RolId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.AutorizacionAccionesPorInstanciaSubFlujoOCADPaz(instanciaId, RolId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("SGR/Proyectos/validarTecnicoOcadpaz")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_validarTecnicoOcadpaz(Guid instanciaId, Guid accionId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_validarTecnicoOcadpaz(instanciaId, accionId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Transversal/ValidacionOCADPaz")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Transversal_ValidacionOCADPaz(string proyectoId, Guid nivelId, Guid instanciaId, Guid flujoId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Transversal_ValidacionOCADPaz(proyectoId, nivelId, instanciaId, flujoId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/ValidarInstanciaCTUSNoFinalizada")]
        [HttpGet]
        public async Task<IHttpActionResult> VerificarInstanciaCTUSNoFinalizada(int idProyecto)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.ValidarInstanciaCTUSNoFinalizada(idProyecto, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/CumplimentoFlujoSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidarViavilidadCumplimentoFlujoSGR(Guid instanciaId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.ValidarViavilidadCumplimentoFlujoSGR(instanciaId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/TieneInstanciaActiva")]
        [HttpGet]
        public async Task<IHttpActionResult> TieneInstanciaActiva(String ObjetoNegocioId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.TieneInstanciaActiva(ObjetoNegocioId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #region Entidad Nacional SGR

        /// <summary>
        /// Leer entidades por id del proyecto
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipoEntidad"></param>  
        /// <returns>List<EntidadesAdscritasDto></returns> 
        [Route("api/SGR/Proyectos/LeerEntidadesAdscritas")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerEntidadesAdscritas(int proyectoId, string tipoEntidad)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_LeerEntidadesAdscritas(proyectoId, tipoEntidad, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Actualizar entidad adscrita
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="delegado"></param> 
        /// <param name="entityId"></param>         
        /// <returns>int</returns> 
        [Route("api/SGR/Proyectos/ActualizarEntidadAdscrita")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_ActualizarEntidadAdscrita(int proyectoId, bool delegado, int entityId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_ActualizarEntidadAdscrita(proyectoId, entityId, delegado, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Guardar asignacion usuario encargado
        /// </summary> 
        /// <param name="json"></param>  
        /// <returns>ResultadoProcedimientoDto</returns> 
        [Route("api/SGR/Proyectos/GuardarAsignacionUsuarioEncargado")]
        [HttpPost]
        public async Task<IHttpActionResult> SGR_Proyectos_GuardarAsignacionUsuarioEncargado(UsuarioEncargadoDto json)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_GuardarAsignacionUsuarioEncargado(json, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Validar entidad delegada
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipo"></param>  
        /// <returns>Json</returns> 
        [Route("api/SGR/Proyectos/ValidarEntidadDelegada")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_ValidarEntidadDelegada(int proyectoId, string tipo)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_ValidarEntidadDelegada(proyectoId, tipo, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Validar usuario encargado
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="instanciaId"></param>  
        /// <returns>Json</returns> 
        [Route("api/SGR/Proyectos/LeerAsignacionUsuarioEncargado")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_LeerAsignacionUsuarioEncargado(int proyectoId, Guid instanciaId)
        {
            try
            {
                var respuestasalida = await Task.Run(() => _sgrServicios.SGR_Proyectos_LeerAsignacionUsuarioEncargado(proyectoId, instanciaId, User.Identity.Name));
                return Ok(respuestasalida);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion Entidad Nacional SGR

        #region CTEI

        [Route("api/SGR/Proyectos/LeerDatosAdicionalesCTEI")]
        [HttpGet]
        public async Task<IHttpActionResult> LeerDatosAdicionalesCTEI(int proyectoId, Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_LeerDatosAdicionalesCTEI(proyectoId, instanciaId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGR/Proyectos/GuardarDatosAdicionalesCTEI")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosAdicionalesCTEI(DatosAdicionalesCTEIDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_GuardarDatosAdicionalesCTEI(obj, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }

        }

        #endregion CTEI

        #region Aval de Uso
        [Route("api/SGR/Proyectos/RegistrarAvalUsoSgr")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarAvalUsoSgr(DatosAvalUsoDto obj)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_RegistrarAvalUsoSgr(obj, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/SGR/Proyectos/LeerAvalUsoSgr")]
        [HttpGet]
        public async Task<IHttpActionResult> LeerAvalUsoSgr(int proyectoId, Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_LeerAvalUsoSgr(proyectoId, instanciaId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion

        #region Priorizacion
        [Route("SGR/Proyectos/MostrarEstadosPriorizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Proyectos_MostrarEstadosPriorizacion(int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.SGR_Proyectos_MostrarEstadosPriorizacion(proyectoId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        #endregion  

        #region Aprobación

        [Route("api/Aprobacion/ObtenerProyectoAprobacionInstanciasSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoAprobacionInstanciasSGR(Nullable<Guid> instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerProyectoAprobacionInstanciasSGR(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Aprobacion/GuardarProyectoAprobacionInstanciasSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoAprobacionInstanciasSGR(ProyectoAprobacionInstanciasDto proyectoAprobacionInstanciasDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarProyectoAprobacionInstanciasSGR(proyectoAprobacionInstanciasDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Aprobacion/ObtenerProyectoResumenAprobacionSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoResumenAprobacionSGR(string proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerProyectoResumenAprobacionSGR(proyectoId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Aprobacion/ObtenerProyectoResumenAprobacionCreditoParcialSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoResumenAprobacionCreditoParcialSGR(string proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerProyectoResumenAprobacionCreditoParcialSGR(proyectoId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Aprobacion/ObtenerProyectoResumenEstadoAprobacionCreditoSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoResumenEstadoAprobacionCreditoSGR(string proyectoId)
        {
            try
            {

                var resultado = await Task.Run(() => _sgrServicios.ObtenerProyectoResumenEstadoAprobacionCreditoSGR(proyectoId, User.Identity.Name));
                return Ok(resultado);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("api/SGR/Proyectos/GuardarProyectoPermisosProcesoSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarProyectoPermisosProcesoSGR(ProyectoProcesoDto proyectoProcesoDto)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.GuardarProyectoPermisosProcesoSGR(proyectoProcesoDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion

        #region Designacion Ejecutor

        /// <summary>
        /// Registrar valor de una columna dinamica del ejecutor por proyectoId.
        /// </summary>     
        /// <param name="valores"></param> 
        /// <returns>bool</returns> 
        [Route("api/SGR/Procesos/RegistrarRespuestaEjecutorSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> RegistrarRespuestaEjecutorSGR(RespuestaDesignacionEjecutorDto valores)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.RegistrarRespuestaEjecutorSGR(valores, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener el valor de una columna dinámica del ejecutor por proyectoId.
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyectoId"></param>
        /// <returns>string</returns>
        [Route("api/SGR/Procesos/ObtenerRespuestaEjecutorSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerRespuestaEjecutorSGR(string campo, int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerRespuestaEjecutorSGR(campo, proyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtiene los valores de la aprobación por proyectoId.
        /// </summary>    
        /// <param name="proyectoId"></param>    
        /// <returns>string</returns>
        [Route("api/SGR/Procesos/LeerValoresAprobacionSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> LeerValoresAprobacionSGR(int proyectoId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.LeerValoresAprobacionSGR(proyectoId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Registrar valor de dinamico aprobación valores.
        /// </summary>     
        /// <param name="valores"></param>        
        /// <returns>bool</returns>
        [Route("api/SGR/Procesos/ActualizarValorEjecutorSGR")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarValorEjecutorSGR(CampoItemValorDto valores)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ActualizarValorEjecutorSGR(valores, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        /// <summary>
        /// Obtener valor de costos de estructuracion viabilidad.
        /// </summary>
        /// <param name="instanciaId"></param>     
        /// <returns>string</returns>
        [Route("api/SGR/Procesos/ObtenerValorCostosEstructuracionViabilidadSGR")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerValorCostosEstructuracionViabilidadSGR(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgrServicios.ObtenerValorCostosEstructuracionViabilidadSGR(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        #endregion Designacion Ejecutor

        [Route("api/SGR/Procesos/ConsultarEjecutorbyTipo")]
        [HttpGet]
        public async Task<IHttpActionResult> SGR_Procesos_ConsultarEjecutorbyTipo(int proyectoId, int tipoEjecutorId)
        {
            try
            {

                var result = await Task.Run(() => _sgrServicios.SGR_Procesos_ConsultarEjecutorbyTipo(proyectoId, tipoEjecutorId, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}