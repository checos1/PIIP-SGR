using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Comunes.Excepciones;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.Beneficiarios;
using DNP.Backbone.Dominio.Dto.CadenaValor;
using DNP.Backbone.Dominio.Dto.CostoActividades;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.SGP.Ajustes;
using DNP.Backbone.Servicios.Interfaces.Autorizacion;
using DNP.Backbone.Servicios.Interfaces.Focalizacion;
using DNP.Backbone.Servicios.Interfaces.ServiciosNegocio;
using DNP.Backbone.Servicios.Interfaces.SGP;
using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using Microsoft.ProjectServer.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Task = System.Threading.Tasks.Task;


namespace DNP.Backbone.Web.API.Controllers.SGP
{
    public class SGPController : Base.BackboneBase
    {
        private readonly ISGPServicios _sgpServicios;
        private readonly IAutorizacionServicios _autorizacionUtilidades;
        private readonly IServiciosNegocioServicios _serviciosNegocioServicios;

        public SGPController(ISGPServicios sgpServicios, IServiciosNegocioServicios serviciosNegocioServicios,IAutorizacionServicios autorizacionUtilidades)
            : base(autorizacionUtilidades)
        {
            _sgpServicios = sgpServicios;
            _serviciosNegocioServicios = serviciosNegocioServicios;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        /// <summary>
        /// Api para lista de localizaciones de proyecto SGP.
        /// </summary>
        /// <param name="bpin">bpin</param>
        /// <returns>Lista de estado del proyecto</returns>
        [Route("api/SGP/ObtenerProyectoListaLocalizacionesSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectoListaLocalizacionesSGP(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerProyectoListaLocalizacionesSGP(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerDesagregarRegionalizacionSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDesagregarRegionalizacionSGP(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerDesagregarRegionalizacionSGP(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerPoliticasTransversalesProyectoSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesProyectoSGP(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerPoliticasTransversalesProyectoSGP(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/EliminarPoliticasProyectoSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarPoliticasProyectoSGP(int proyectoId, int politicaId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.EliminarPoliticasProyectoSGP(proyectoId, politicaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/AgregarPoliticasTransversalesAjustesSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarPoliticasTransversalesAjustesSGP(CategoriaProductoPoliticaDto objPoliticaTransversalDto)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.AgregarPoliticasTransversalesAjustesSGP(objPoliticaTransversalDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerListaTiposRecursosSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTiposRecursos(ProyectoParametrosDto peticionObtenerProyecto)
        {
            try
            {
                if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false))
                    return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _serviciosNegocioServicios.ObtenerListaCatalogoEntidades(peticionObtenerProyecto, CatalogoEnum.TiposRecursos).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ConsultarPoliticasCategoriasIndicadoresSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarPoliticasCategoriasIndicadoresSGP(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ConsultarPoliticasCategoriasIndicadoresSGP(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ModificarPoliticasCategoriasIndicadoresSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> ModificarPoliticasCategoriasIndicadoresSGP(CategoriasIndicadoresDto parametrosGuardar)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ModificarPoliticasCategoriasIndicadoresSGP(parametrosGuardar, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerPoliticasTransversalesCategoriasSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesCategoriasSGP(string instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerPoliticasTransversalesCategoriasSGP(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/EliminarCategoriasPoliticasProyectoSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarCategoriasPoliticasProyectoSGP(int proyectoId, int politicaId, int categoriaId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.EliminarCategoriasPoliticasProyectoSGP(proyectoId, politicaId, categoriaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarFocalizacionCategoriasAjustesSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFocalizacionCategoriasAjustesSGP(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarFocalizacionCategoriasAjustesSGP(focalizacionCategoriasAjuste, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerCategoriasSubcategoriasSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCategoriasSubcategoriasSGP(int idPadre, int idEntidad, int esCategoria, int esGrupoEtnico)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerCategoriasSubcategoriasSGP(idPadre, idEntidad, esCategoria, esGrupoEtnico, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarFocalizacionCategoriasPoliticaSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFocalizacionCategoriasPoliticaSGP(FocalizacionCategoriasAjusteDto objCategoriaPoliticaDto)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarFocalizacionCategoriasPoliticaSGP(objCategoriaPoliticaDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerCrucePoliticasAjustesSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCrucePoliticasAjustesSGP(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerCrucePoliticasAjustesSGP(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerPoliticasTransversalesResumenSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerPoliticasTransversalesResumenSGP(Guid instanciaId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerPoliticasTransversalesResumenSGP(instanciaId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarCrucePoliticasAjustesSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCrucePoliticasAjustesSGP(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarCrucePoliticasAjustesSGP(objListCruecePoliticasAjustesDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerFuenteFinanciacionVigenciaSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerFuenteFinanciacionVigenciaSGP(string bpin, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerFuenteFinanciacionVigenciaSGP(bpin, UsuarioLogadoDto.IdUsuario, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/FuenteFinanciacionEliminarSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> FuenteFinanciacionEliminarSGP(string fuenteId, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.EliminarFuenteFinanciacionSGP(fuenteId, UsuarioLogadoDto.IdUsuario, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ConsultarFuentesProgramarSolicitadoSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ConsultarFuentesProgramarSolicitadoSGP(string bpin, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ConsultarFuentesProgramarSolicitadoSGP(bpin, UsuarioLogadoDto.IdUsuario, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarFuentesProgramarSolicitadoSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarFuentesProgramarSolicitadoSGP(ProgramacionValorFuenteDto objProgramacionValorFuenteDto)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarFuentesProgramarSolicitadoSGP(objProgramacionValorFuenteDto, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/DatosAdicionalesObtenerSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> DatosAdicionalesObtenerSGP(int fuenteId, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerDatosAdicionalesSGP(fuenteId, UsuarioLogadoDto.IdUsuario, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }


        [Route("api/SGP/DatosAdicionalesAgregarSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> DatosAdicionalesAgregarSGP(DatosAdicionalesDto objDatosAdicionalesDto, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.AgregarDatosAdicionalesSGP(objDatosAdicionalesDto, UsuarioLogadoDto.IdUsuario, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/DatosAdicionalesEliminarSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> DatosAdicionalesEliminarSGP(int cofinanciadorId, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.EliminarDatosAdicionalesSGP(cofinanciadorId, UsuarioLogadoDto.IdUsuario, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/FuenteFinanciacionAgregarSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> FuenteFinanciacionAgregarSGP(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.AgregarFuenteFinanciacionSGP(proyectoFuenteFinanciacionAgregarDto, UsuarioLogadoDto.IdUsuario, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerCategoriaProductosPoliticaSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCategoriaProductosPoliticaSGP(string bpin, int fuenteId, int politicaId)
        {
            try
            {
                var result = await Task.Run(() =>
                _sgpServicios.ObtenerCategoriaProductosPoliticaSGP(bpin, fuenteId, politicaId, UsuarioLogadoDto.IdUsuario,
                Request.Headers.Authorization.Parameter));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/guardarDatosSolicitudRecursosSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarDatosSolicitudRecursosSGP(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto)
        {
            try
            {
                var result = await Task.Run(() =>
                _sgpServicios.GuardarDatosSolicitudRecursosSGP(categoriaProductoPoliticaDto, UsuarioLogadoDto.IdUsuario,
                Request.Headers.Authorization.Parameter));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerIndicadoresPoliticaSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIndicadoresPoliticaSGP(string bpin)
        {
            try
            {
                var result = await Task.Run(() =>
                _sgpServicios.ObtenerIndicadoresPoliticaSGP(bpin, UsuarioLogadoDto.IdUsuario));

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerJustificacionHorizonteSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerJustificacionHorizonteSGP(int IdProyecto)
        {
            try
            {
                var seccionesCapitulos = await _sgpServicios.ObtenerJustificacionHorizonteSGP(IdProyecto, UsuarioLogadoDto.IdUsuario);
                var result = new JustificacionHorizonteCambiosDto();
                if (seccionesCapitulos != null)
                {

                    result = new JustificacionHorizonteCambiosDto()
                    {
                        Vigencia = seccionesCapitulos,
                        VigenciaFirme = seccionesCapitulos
                    };
                }
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/actualizarHorizonteSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> actualizarHorizonteSGP(HorizonteProyectoDto parametrosHorizonte)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.actualizarHorizonteSGP(parametrosHorizonte, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerIndicadoresProductoSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerIndicadoresProductoSGP(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerIndicadoresProductoSGP(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarIndicadoresSecundariosSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarIndicadoresSecundariosSGP(AgregarIndicadoresSecundariosDto parametros)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarIndicadoresSecundariosSGP(parametros, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/EliminarIndicadorProductoSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> EliminarIndicadorProductoSGP(int indicadorId)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.EliminarIndicadorProductoSGP(indicadorId, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ActualizarMetaAjusteIndicadorSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> ActualizarMetaAjusteIndicadorSGP(IndicadoresIndicadorProductoDto Indicador)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ActualizarMetaAjusteIndicadorSGP(Indicador, User.Identity.Name));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerProyectosBeneficiariosSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiariosSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerProyectosBeneficiariosSGP(bpin, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerProyectosBeneficiariosDetalleSGP")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerProyectosBeneficiariosDetalleSGP(string json, string usuarioDNP, string tokenAutorizacion)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerProyectosBeneficiariosDetalleSGP(json, usuarioDNP, tokenAutorizacion));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarBeneficiarioTotalesSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioTotalesSGP(BeneficiarioTotalesDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarBeneficiarioTotalesSGP(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarBeneficiarioProductoSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoSGP(BeneficiarioProductoSgpDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarBeneficiarioProductoSGP(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarBeneficiarioProductoLocalizacionSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacionSGP(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarBeneficiarioProductoLocalizacionSGP(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarBeneficiarioProductoLocalizacionCaracterizacionSGP")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarBeneficiarioProductoLocalizacionCaracterizacionSGP(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarBeneficiarioProductoLocalizacionCaracterizacionSGP(beneficiario, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/GuardarLocalizacionSGP")]
        [HttpPost]
        public async Task<ResultadoProcedimientoDto> GuardarLocalizacionSGP(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP)
        {
            try
            {
                var result = await _sgpServicios.guardarLocalizacionSGP(objLocalizacion, usuarioDNP, Request.Headers.Authorization.Parameter).ConfigureAwait(false);

                return (result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/ObtenerHorizonteSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerHorizonteSgp(ProyectoParametrosEncabezadoDto ppt)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerHorizonteSgp(ppt, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        //Migracion servicios fuentes de financiacion y costos 
        //Fuenets de finanaciacion
        [Route("api/SGP/guardarFuentesFinanciacionRecursosAjustesSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> guardarFuentesFinanciacionRecursosAjustesSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.guardarFuentesFinanciacionRecursosAjustesSgp(objFuenteFinanciacionAgregarAjusteDto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/SGP/proyecto/ObtenerResumenObjetivosProductosActividadesSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerResumenObjetivosProductosActividadesSgp(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerResumenObjetivosProductosActividadesSgp(bpin, UsuarioLogadoDto.IdUsuario));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/SGP/proyecto/GuardarCostoActividadesSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> GuardarCostoActividadesSgp(ProductoAjusteDto producto, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.GuardarCostoActividadesSgp(producto, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/proyecto/AgregarEntregableSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> AgregarEntregableSgp(AgregarEntregable[] entregables, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.AgregarEntregableSgp(entregables, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/proyecto/EliminarEntregableSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> EliminarEntregableSgp(EntregablesActividadesDto entregable, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.EliminarEntregableSgp(entregable, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/proyecto/RegionalizacionGeneralSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> RegionalizacionGeneralSgp(string bpin)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.RegionalizacionGeneralSgp(bpin, UsuarioLogadoDto.IdUsuario, Request.Headers.Authorization.Parameter));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
        [Route("api/SGP/Proyecto/ObtenerListaTiposRecursosxEntidadSgp")]
        [HttpPost]
        public async Task<IHttpActionResult> ObtenerListaTiposRecursosxEntidadSgp(ProyectoParametrosDto peticionObtenerProyecto, int entityTypeCatalogId, int entityType)
        {
            try
            {
               if (!await ValidarParametrosRequest(peticionObtenerProyecto).ConfigureAwait(false))
                    return ResponseMessage(base.RespuestaAutorizacion);
                var result = await _sgpServicios.ObtenerListaTiposRecursosxEntidadSgp(peticionObtenerProyecto, entityTypeCatalogId, entityType).ConfigureAwait(false);

                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/Ajustes/ObtenerCategoriasFocalizacionJustificacionSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerCategoriasFocalizacionJustificacionSgp(string Bpin, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerCategoriasFocalizacionJustificacionSgp(Bpin, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }

        [Route("api/SGP/Ajustes/ObtenerDetalleCategoriasFocalizacionJustificacionSgp")]
        [HttpGet]
        public async Task<IHttpActionResult> ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string Bpin, string usuarioDNP)
        {
            try
            {
                var result = await Task.Run(() => _sgpServicios.ObtenerDetalleCategoriasFocalizacionJustificacionSgp(Bpin, usuarioDNP));
                return Ok(result);
            }
            catch (BackboneException e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e));
            }
        }
    }
}