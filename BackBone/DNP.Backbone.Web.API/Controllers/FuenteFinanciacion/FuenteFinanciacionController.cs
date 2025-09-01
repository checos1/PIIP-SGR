namespace DNP.Backbone.Web.API.Controllers.FuenteFinanciacion
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Comunes.Excepciones;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Servicios.Interfaces.Autorizacion;
    using DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class FuenteFinanciacionController : Base.BackboneBase
    {
        private readonly IFuenteFinanciacionServicios _fuenteFinanciacionServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;

        public FuenteFinanciacionController(IFuenteFinanciacionServicios fuenteFinanciacionServicios, IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _fuenteFinanciacionServicios = fuenteFinanciacionServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        [Route("api/FuenteFinanciacionObtener")]
        [HttpGet]
        public async Task<IHttpActionResult> FuenteFinanciacionObtener(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerFuenteFinanciacionAgregarN(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/FuenteFinanciacionAgregar")]
        [HttpPost]
        public async Task<IHttpActionResult> FuenteFinanciacionAgregar(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.AgregarFuenteFinanciacion(proyectoFuenteFinanciacionAgregarDto, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/FuenteFinanciacionEliminar")]
        [HttpPost]
        public async Task<IHttpActionResult> FuenteFinanciacionEliminar(string fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.EliminarFuenteFinanciacion(fuenteId, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/CostosVsSolicitado/Consultar")]
        [HttpGet]
        public async Task<IHttpActionResult> ResumenCostosVsSoliciatado(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerResumenCostosVsSolicitado(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ResumenFuenteFinanciacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ResumenFuenteFinanciacion(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ConsultarResumenFuentesFinanciacion(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ConsultarCostosPIIPvsFuentesPIIP")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarCostosPIIPvsFuentesPIIP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ConsultarCostosPIIPvsFuentesPIIP(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerFuenteFinanciacionVigencia")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuenteFinanciacionVigencia(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerFuenteFinanciacionVigencia(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerPoliticasTransversalesAjustes")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesAjustes(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ConsultarPoliticasTransversalesAjustes(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/AgregarPoliticasTransversalesAjustes")]
        [HttpPost]
        public async Task<IHttpActionResult> guardarPoliticasTransversalesAjustes(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.guardarPoliticasTransversalesAjustes(objPoliticaTransversalDto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/GuardarFocalizacionCategoriasPolitica")]
        [HttpPost]
        public async Task<IHttpActionResult> guardarFocalizacionCategoriasPolitica(FocalizacionCategoriasAjusteDto objCategoriaPoliticaDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.guardarFocalizacionCategoriasPolitica(objCategoriaPoliticaDto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerPoliticasTransversalesCategorias")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCategorias(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ConsultarPoliticasTransversalesCategorias(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Focalizacion/EliminarPoliticaProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPoliticasProyecto(int proyectoId, int politicaId, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.EliminarPoliticasProyecto(proyectoId, politicaId, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Focalizacion/EliminarCategoriaPoliticaProyecto")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId, int categoriaId, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.EliminarCategoriaPoliticasProyecto(proyectoId, politicaId, categoriaId, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/PoliticasCategoriaPorPadre")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasCategoriasPorPadre(int idPadre, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ConsultarPoliticasCategoriasPorPadre(idPadre, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerCategoriasSubcategorias")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCategoriasSubcategorias(int idPadre, int idEntidad, int esCategoria, int esGrupoEtnico, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerCategoriasSubcategorias(idPadre,idEntidad,esCategoria,esGrupoEtnico,usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerPoliticasTransversalesResumen")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesResumen(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPoliticasTransversalesResumen(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/Focalizacion/ConsultarPoliticasCategoriasIndicadores")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasCategoriasIndicadores(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ConsultarPoliticasCategoriasIndicadores(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/Focalizacion/ModificarCategoriasIndicadores")]
        [HttpPost]
        public async Task<IHttpActionResult> ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ModificarCategoriasIndicadores(parametrosGuardar, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerCrucePoliticasAjustes")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasAjustes(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                // var result1 = await Task.Run(() => _fuenteFinanciacionServicios.ConsultarPoliticasTransversalesAjustes(bpin, usuarioDNP, tokenAutorizacion));
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerCrucePoliticasAjustes(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/GuardarCrucePoliticasAjustes")]
        [HttpPost]
        public async Task<RespuestaGeneralDto> GuardarCrucePoliticasAjustes(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarCrucePoliticasAjustes(objListCruecePoliticasAjustesDto, usuarioDNP));
                return result;
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerPoliticasSolicitudConcepto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasSolicitudConcepto(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPoliticasSolicitudConcepto(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/FocalizacionSolicitarConceptoDT")]
        [HttpPost]
        public async Task<IHttpActionResult> FocalizacionSolicitarConceptoDT(List<FocalizacionSolicitarConceptoDto> objscDto)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.FocalizacionSolicitarConceptoDT(objscDto));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerDireccionesTecnicasPoliticasFocalizacion")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDireccionesTecnicasPoliticasFocalizacion(string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerDireccionesTecnicasPoliticasFocalizacion(usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerResumenSolicitudConcepto")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenSolicitudConcepto(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerResumenSolicitudConcepto(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/ObtenerPreguntasEnvioPoliticaSubDireccion")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasEnvioPoliticaSubDireccionDto PreguntasEnvioPoliticaSubDireccion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasEnvioPoliticaSubDireccion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/GuardarPreguntasEnvioPoliticaSubDireccionAjustes")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarPreguntasEnvioPoliticaSubDireccionAjustes(PreguntasEnvioPoliticaSubDireccionAjustes PreguntasEnvioPoliticaSubDireccion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarPreguntasEnvioPoliticaSubDireccionAjustes(PreguntasEnvioPoliticaSubDireccion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/GuardarRespuestaEnvioPoliticaSubDireccionAjustes")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarRespuestaEnvioPoliticaSubDireccionAjustes(RespuestaEnvioPoliticaSubDireccionAjustes PreguntasEnvioPoliticaSubDireccion)
        {
            try
            {
                var result = await Task.Run(() => _fuenteFinanciacionServicios.GuardarRespuestaEnvioPoliticaSubDireccionAjustes(PreguntasEnvioPoliticaSubDireccion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

    }
}