using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.Beneficiarios;
using DNP.Backbone.Dominio.Dto.CadenaValor;
using DNP.Backbone.Dominio.Dto.CostoActividades;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
using DNP.Backbone.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.SGP.Ajustes;
using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DNP.Backbone.Servicios.Interfaces.SGP
{
    public interface ISGPServicios
    {
        Task<string> ObtenerProyectoListaLocalizacionesSGP(string bpin, string IdUsuario, string tokenAutorizacion);
        Task<string> ObtenerDesagregarRegionalizacionSGP(string bpin, string usuarioDnp, string tokenAutorizacion);
        Task<string> ObtenerPoliticasTransversalesProyectoSGP(string bpin, string usuarioDnp);
        Task<string> EliminarPoliticasProyectoSGP(int proyectoId, int politicaId, string usuarioDnp);
        Task<string> AgregarPoliticasTransversalesAjustesSGP(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP);
        Task<string> ConsultarPoliticasCategoriasIndicadoresSGP(Guid instanciaId, string usuarioDnp);
        Task<string> ModificarPoliticasCategoriasIndicadoresSGP(CategoriasIndicadoresDto parametrosGuardar, string usuarioDnp);
        Task<string> ObtenerPoliticasTransversalesCategoriasSGP(string instanciaId, string usuarioDnp);
        Task<string> EliminarCategoriasPoliticasProyectoSGP(int proyectoId, int politicaId, int categoriaId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustesSGP(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario);
        Task<string> ObtenerCategoriasSubcategoriasSGP(int idPadre, int idEntidad, int esCategoria, int esGrupoEtnico, string usuarioDNP);
        Task<string> GuardarFocalizacionCategoriasPoliticaSGP(FocalizacionCategoriasAjusteDto objCategoriaPoliticaDto, string usuarioDNP);
        Task<string> ObtenerCrucePoliticasAjustesSGP(Guid instanciaId, string usuarioDnp);
        Task<string> ObtenerPoliticasTransversalesResumenSGP(System.Guid instanciaId, string usuarioDnp);
        Task<string> GuardarCrucePoliticasAjustesSGP(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP);
        Task<string> ObtenerFuenteFinanciacionVigenciaSGP(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> EliminarFuenteFinanciacionSGP(string fuenteId, string usuarioDNP, string tokenAutorizacion);
        Task<string> ConsultarFuentesProgramarSolicitadoSGP(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> GuardarFuentesProgramarSolicitadoSGP(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuarioDNP);        
        Task<string> ObtenerDatosAdicionalesSGP(int fuenteId, string usuarioDNP, string tokenAutorizacion);
        Task<RespuestaGeneralDto> AgregarDatosAdicionalesSGP(DatosAdicionalesDto objDatosAdicionalesDto, string usuarioDNP, string tokenAutorizacion);
        Task<string> EliminarDatosAdicionalesSGP(int cofinanciadorId, string usuarioDNP, string tokenAutorizacion);
        Task<string> AgregarFuenteFinanciacionSGP(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerCategoriaProductosPoliticaSGP(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion);
        Task<string> GuardarDatosSolicitudRecursosSGP(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto, string usuarioDnp, string tokenAutorizacion);
        Task<string> ObtenerIndicadoresPoliticaSGP(string bpin, string usuarioDnp);

        Task<RespuestaGeneralDto> actualizarHorizonteSGP(HorizonteProyectoDto parametrosHorizonte, string usuarioDnp);

        Task<List<JustificacionHorizontenDto>> ObtenerJustificacionHorizonteSGP(int IdProyecto, string usuarioDnp);

        Task<IndicadorProductoDto> ObtenerIndicadoresProductoSGP(string bpin, string usuarioDNP, string tokenAutorizacion);

        Task<IndicadorResponse> GuardarIndicadoresSecundariosSGP(AgregarIndicadoresSecundariosDto parametros, string usuarioDnp);

        Task<IndicadorResponse> EliminarIndicadorProductoSGP(int indicadorId, string usuarioDNP);

        Task<IndicadorResponse> ActualizarMetaAjusteIndicadorSGP(IndicadoresIndicadorProductoDto Indicador, string usuarioDNP);

        Task<string> ObtenerProyectosBeneficiariosSGP(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerProyectosBeneficiariosDetalleSGP(string json, string usuarioDNP, string tokenAutorizacion);

        Task<string> GuardarBeneficiarioTotalesSGP(BeneficiarioTotalesDto beneficiario, string usuarioDNP);
        Task<string> GuardarBeneficiarioProductoSGP(BeneficiarioProductoSgpDto beneficiario, string usuarioDNP);
        Task<string> GuardarBeneficiarioProductoLocalizacionSGP(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP);
        Task<string> GuardarBeneficiarioProductoLocalizacionCaracterizacionSGP(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP);

        Task<ResultadoProcedimientoDto> guardarLocalizacionSGP(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP, string tokenAutorizacion);
        Task<EncabezadoSGPDto> ObtenerHorizonteSgp(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp);
        //Servicios migrados Fuentes de finanaciacion y costos
        Task<string> guardarFuentesFinanciacionRecursosAjustesSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuarioDNP);
        Task<ObjectivosAjusteDto> ObtenerResumenObjetivosProductosActividadesSgp(string bpin, string usuarioDNP);
        Task<Dominio.Dto.SeguimientoControl.ReponseHttp> GuardarCostoActividadesSgp(ProductoAjusteDto producto, string usuarioDNP);
        Task<string> AgregarEntregableSgp(AgregarEntregable[] entregables, string usuarioDNP);
        Task<string> EliminarEntregableSgp(EntregablesActividadesDto entregable, string usuarioDNP);
        Task<RegionalizacionDto> RegionalizacionGeneralSgp(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerListaTiposRecursosxEntidadSgp(ProyectoParametrosDto peticion, int entityTypeCatalogId, int entityType);
        Task<string> ObtenerCategoriasFocalizacionJustificacionSgp(string bpin, string usuarioDNP);
        Task<string> ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string bpin, string usuarioDNP);
    }
}
