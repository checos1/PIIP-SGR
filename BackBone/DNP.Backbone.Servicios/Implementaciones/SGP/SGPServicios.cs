using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Comunes.Enums;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.Beneficiarios;
using DNP.Backbone.Dominio.Dto.CadenaValor;
using DNP.Backbone.Dominio.Dto.CostoActividades;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Dominio.Dto.SGP.Ajustes;
using DNP.Backbone.Servicios.Interfaces;
using DNP.Backbone.Servicios.Interfaces.SGP;
using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;


namespace DNP.Backbone.Servicios.Implementaciones.SGP
{
    public class SGPServicios : ISGPServicios
    {
        private IClienteHttpServicios _clienteHttpServicios;
        private readonly string urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];

        public SGPServicios(IClienteHttpServicios clienteHttpServicios)
        {
            _clienteHttpServicios = clienteHttpServicios;
        }

        public async Task<string> ObtenerProyectoListaLocalizacionesSGP(string bpin, string usuarioDnp, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriConsultarProyectosLocalizacionesSGP"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> ObtenerDesagregarRegionalizacionSGP(string bpin, string usuarioDnp, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerDesagregarRegionalizacionSGP"];

            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, $"?bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> ObtenerPoliticasTransversalesProyectoSGP(string bpin, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesProyectoSGP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> EliminarPoliticasProyectoSGP(int proyectoId, int politicaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarPoliticasProyectoSGP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?proyectoId={proyectoId}&politicaId={politicaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> AgregarPoliticasTransversalesAjustesSGP(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriAgregarPoliticasTransversalesAjustesSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objPoliticaTransversalDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ConsultarPoliticasCategoriasIndicadoresSGP(Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarPoliticasCategoriasIndicadoresSGP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> ModificarPoliticasCategoriasIndicadoresSGP(CategoriasIndicadoresDto parametrosGuardar, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriModificarPoliticasCategoriasIndicadoresSGP"] + "?usuario=" + usuarioDNP;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametrosGuardar, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerPoliticasTransversalesCategoriasSGP(string instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesCategoriasSGP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> EliminarCategoriasPoliticasProyectoSGP(int proyectoId, int politicaId, int categoriaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriEliminarCategoriasPoliticasProyectoSGP"] + $"?proyectoId={proyectoId}&politicaId={politicaId}&categoriaId={categoriaId}";
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustesSGP(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarFocalizacionCategoriasAjustesSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, focalizacionCategoriasAjuste, usuario);
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(respuesta);
        }

        public async Task<string> ObtenerCategoriasSubcategoriasSGP(int idPadre, int idEntidad, int esCategoria, int esGrupoEtnico, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerCategoriasSubcategoriasSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?padreId={idPadre}&entidadId={idEntidad}&esCategoria={esCategoria}&esGruposEtnicos={esGrupoEtnico}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarFocalizacionCategoriasPoliticaSGP(FocalizacionCategoriasAjusteDto objCategoriaPoliticaDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriPoliticasTransversalesCategoriasPoliticasAgregarSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objCategoriaPoliticaDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerCrucePoliticasAjustesSGP(Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerCrucePoliticasAjustesSGP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> ObtenerPoliticasTransversalesResumenSGP(Guid instanciaId, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerPoliticasTransversalesResumenSGP"];
            var response = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?instanciaId={instanciaId}", null, usuarioDnp, useJWTAuth: false);
            if (string.IsNullOrEmpty(response)) return string.Empty;
            return response;
        }

        public async Task<string> GuardarCrucePoliticasAjustesSGP(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarCrucePoliticasAjustesSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objListCruecePoliticasAjustesDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerFuenteFinanciacionVigenciaSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuenteFinanciacionVigenciaSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarFuenteFinanciacionSGP(string fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuenteFinanciacionEliminarSGP"] + $"?fuenteFinanciacionId={fuenteId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ConsultarFuentesProgramarSolicitadoSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriConsultarProgramaSolicitadoSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarFuentesProgramarSolicitadoSGP(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarProgramaSolicitadoSGP"] + $"?usuario={usuarioDNP}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objProgramacionValorFuenteDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerDatosAdicionalesSGP(int fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriDatosAdicionalesConsultarSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?fuenteId={fuenteId}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<RespuestaGeneralDto> AgregarDatosAdicionalesSGP(DatosAdicionalesDto objDatosAdicionalesDto, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriDatosAdicionalesAgregarSGP"];
            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objDatosAdicionalesDto, usuarioDNP, useJWTAuth: false));
        }

        public async Task<string> EliminarDatosAdicionalesSGP(int cofinanciadorId, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriDatosAdicionalesEliminarSGP"] + $"?cofinanciadorId={cofinanciadorId}";
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> AgregarFuenteFinanciacionSGP(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriFuenteFinanciacionAgregarSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, proyectoFuenteFinanciacionAgregarDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerCategoriaProductosPoliticaSGP(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriCatProductosPoliticaConsultarSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?Bpin={bpin}&fuenteId={fuenteId}&politicaId={politicaId}", null, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }

        public async Task<string> GuardarDatosSolicitudRecursosSGP(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto, string usuarioDnp, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarDatosSolicitudRecursosSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, categoriaProductoPoliticaDto, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }

        public async Task<string> ObtenerIndicadoresPoliticaSGP(string bpin, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriIndicadorPoliticaConsultarSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?Bpin={bpin}", null, usuarioDnp, useJWTAuth: false);
            return respuesta;
        }

        async public Task<RespuestaGeneralDto> actualizarHorizonteSGP(HorizonteProyectoDto parametrosHorizonte, string usuarioDnp)
        {
            var uri = ConfigurationManager.AppSettings["uriActualizarHorizonteSGP"];

            return JsonConvert.DeserializeObject<RespuestaGeneralDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, parametrosHorizonte, usuarioDnp));
        }

        public async Task<List<JustificacionHorizontenDto>> ObtenerJustificacionHorizonteSGP(int IdProyecto, string usuarioDnp)
        {
     
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerJustificacionHorizonteSGP"];
            uriMetodo += "?proyectoId=" + IdProyecto;
            var result = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, null, usuarioDnp);
            return JsonConvert.DeserializeObject<List<JustificacionHorizontenDto>>(result);
        }

        public async Task<IndicadorProductoDto> ObtenerIndicadoresProductoSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriObtenerIndicadoresProductoSGP"] + "/" + bpin;

            var indicaadoresProducto = JsonConvert.DeserializeObject<IndicadorProductoDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, bpin, usuarioDNP));

            var indicadorPrd = new IndicadorProductoDto();

            if (indicaadoresProducto != null)
            {
                indicadorPrd = indicaadoresProducto;
            }
            return indicadorPrd;
        }

        public async Task<IndicadorResponse> GuardarIndicadoresSecundariosSGP(AgregarIndicadoresSecundariosDto parametros, string usuarioDnp)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriGuardarIndicadoresSecundariosSGP"];

            return JsonConvert.DeserializeObject<IndicadorResponse>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }

        public async Task<IndicadorResponse> EliminarIndicadorProductoSGP(int indicadorId, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriEliminarIndicadorProductoSGP"] + "/" + indicadorId;

            return JsonConvert.DeserializeObject<IndicadorResponse>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, indicadorId, usuarioDNP));
        }

        public async Task<IndicadorResponse> ActualizarMetaAjusteIndicadorSGP(IndicadoresIndicadorProductoDto Indicador, string usuarioDNP)
        {
            var uriMetodo = ConfigurationManager.AppSettings["UriActualizarMetaAjusteIndicadorSGP"];

            return JsonConvert.DeserializeObject<IndicadorResponse>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, Indicador, usuarioDNP));
        }

        public async Task<string> ObtenerProyectosBeneficiariosSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectosBeneficiariosSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerProyectosBeneficiariosDetalleSGP(string json, string usuarioDNP, string tokenAutorizacion)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerProyectosBeneficiariosDetalleSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?json={json}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioTotalesSGP(BeneficiarioTotalesDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarBeneficiarioTotalesSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioProductoSGP(BeneficiarioProductoSgpDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarBeneficiarioProductoSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioProductoLocalizacionSGP(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriGuardarBeneficiarioProductoLocalizacionSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> GuardarBeneficiarioProductoLocalizacionCaracterizacionSGP(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["GuardarBeneficiarioProductoLocalizacionCaracterizacionSGP"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, beneficiario, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<ResultadoProcedimientoDto> guardarLocalizacionSGP(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP, string tokenAutorizacion)
        {
           
            var uriMetodo = ConfigurationManager.AppSettings["uriBackboneGuardarDefLocalizacionesSGP"];
            uriMetodo += "?usuario=" + usuarioDNP;
            var response = JsonConvert.DeserializeObject<ResultadoProcedimientoDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, objLocalizacion, usuarioDNP, useJWTAuth: false));
            return response;
        }

        public async Task<EncabezadoSGPDto> ObtenerHorizonteSgp(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerHorizonteSgp"];
            return JsonConvert.DeserializeObject<EncabezadoSGPDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uriMetodo, null, parametros, usuarioDnp));
        }
        //Servicios migrados fuentes de finanaciacion y costos
        //Fuentes de financiamiento
        public async Task<string> guardarFuentesFinanciacionRecursosAjustesSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuarioDNP)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriFuentesFinanciacionRecursosAjustesSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, objFuenteFinanciacionAgregarAjusteDto, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        //Costos
        public async Task<ObjectivosAjusteDto> ObtenerResumenObjetivosProductosActividadesSgp(string bpin, string usuarioDNP)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerResumenObjetivosProductosActividadesSgp"] + "?bpin=" + bpin;
            return JsonConvert.DeserializeObject<ObjectivosAjusteDto>(await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, null, bpin, usuarioDNP));
        }
        public async Task<ReponseHttp> GuardarCostoActividadesSgp(ProductoAjusteDto producto, string usuarioDNP)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriGuardarCostoActividadesSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, producto, usuarioDNP, useJWTAuth: false);
            var json = JsonConvert.DeserializeObject<ReponseHttp>(respuesta);
            return json;
        }
        public async Task<string> AgregarEntregableSgp(AgregarEntregable[] entregables, string usuarioDNP)
        {

            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriAgregarEntregableSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, entregables, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> EliminarEntregableSgp(EntregablesActividadesDto entregable, string usuarioDNP)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["uriEliminarEntregableSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Post, urlBase, uri, null, entregable, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
        public async Task<RegionalizacionDto> RegionalizacionGeneralSgp(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uri = ConfigurationManager.AppSettings["UriRegionalizacionGeneralSgp"] + "?bpin=" + bpin;

            //var regionalizacion = JsonConvert.DeserializeObject<RegionalizacionDto>(await  _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, null, bpin, usuarioDNP, useJWTAuth: false));
            var regionalizacion = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, null, bpin, usuarioDNP, useJWTAuth: false);

            var regionalizacionPrd = new RegionalizacionDto();

           if (regionalizacion != null)
            {
               regionalizacionPrd = JsonConvert.DeserializeObject<RegionalizacionDto>(regionalizacion);
            }

            //return regionalizacionPrd;
            return regionalizacionPrd;
        }
        public async Task<string> ObtenerListaTiposRecursosxEntidadSgp(ProyectoParametrosDto peticion, int entityTypeCatalogId, int entityType)
        {
            //ConsultaTiposRecursosEntidad(int entityTypeCatalogId)
            var urlBase = ConfigurationManager.AppSettings["ApiServicioNegocio"];
            var uriMetodo = ConfigurationManager.AppSettings["uriObtenerTiposRecursosEntidadSgp"];
            uriMetodo += "?entityTypeCatalogId=" + entityTypeCatalogId + "&entityType=" + entityType;
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uriMetodo, string.Empty, peticion, peticion.IdUsuario, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerCategoriasFocalizacionJustificacionSgp(string bpin, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerCategoriasFocalizacionJustificacionSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }

        public async Task<string> ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string bpin, string usuarioDNP)
        {
            var uri = ConfigurationManager.AppSettings["uriObtenerDetalleCategoriasFocalizacionJustificacionSgp"];
            var respuesta = await _clienteHttpServicios.ConsumirServicio(MetodosServiciosWeb.Get, urlBase, uri, $"?bpin={bpin}", null, usuarioDNP, useJWTAuth: false);
            if (string.IsNullOrEmpty(respuesta)) return string.Empty;
            return respuesta;
        }
    }
}