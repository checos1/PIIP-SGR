using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.Beneficiarios;
using DNP.Backbone.Dominio.Dto.CadenaValor;
using DNP.Backbone.Dominio.Dto.CostoActividades;
using DNP.Backbone.Dominio.Dto.Focalizacion;
using DNP.Backbone.Dominio.Dto.FuenteFinanciacion;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Dominio.Dto.SGP.Ajustes;
using DNP.Backbone.Servicios.Interfaces.SGP;
using DNP.ServiciosNegocio.Dominio.Dto.DatosAdicionales;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class SGPServiciosMock : ISGPServicios
    {
        public Task<string> ObtenerProyectoListaLocalizacionesSGP(string bpin, string IdUsuario, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }
        public Task<string> ObtenerDesagregarRegionalizacionSGP(string bpin, string usuarioDnp, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerPoliticasTransversalesProyectoSGP(string bpin, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }
        public Task<string> EliminarPoliticasProyectoSGP(int proyectoId, int politicaId, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }
        public Task<string> AgregarPoliticasTransversalesAjustesSGP(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ConsultarPoliticasCategoriasIndicadoresSGP(Guid instanciaId, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ModificarPoliticasCategoriasIndicadoresSGP(CategoriasIndicadoresDto parametrosGuardar, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerPoliticasTransversalesCategoriasSGP(string instanciaId, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> EliminarCategoriasPoliticasProyectoSGP(int proyectoId, int politicaId, int categoriaId, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustesSGP(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerCategoriasSubcategoriasSGP(int idPadre, int idEntidad, int esCategoria, int esGrupoEtnico, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarFocalizacionCategoriasPoliticaSGP(FocalizacionCategoriasAjusteDto objCategoriaPoliticaDto, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerCrucePoliticasAjustesSGP(Guid instanciaId, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerPoliticasTransversalesResumenSGP(Guid instanciaId, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarCrucePoliticasAjustesSGP(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerFuenteFinanciacionVigenciaSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> EliminarFuenteFinanciacionSGP(string fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ConsultarFuentesProgramarSolicitadoSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarFuentesProgramarSolicitadoSGP(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerDatosAdicionalesSGP(int fuenteId, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<RespuestaGeneralDto> AgregarDatosAdicionalesSGP(DatosAdicionalesDto objDatosAdicionalesDto, string usuarioDNP, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        public Task<string> EliminarDatosAdicionalesSGP(int cofinanciadorId, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> AgregarFuenteFinanciacionSGP(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerCategoriaProductosPoliticaSGP(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarDatosSolicitudRecursosSGP(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto, string usuarioDnp, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerIndicadoresPoliticaSGP(string bpin, string usuarioDnp)
        {
            return Task.FromResult(string.Empty);
        }



        public Task<RespuestaGeneralDto> actualizarHorizonteSGP(HorizonteProyectoDto parametrosHorizonte, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }
        public Task<List<JustificacionHorizontenDto>> ObtenerJustificacionHorizonteSGP(int IdProyecto, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }
        public Task<IndicadorProductoDto> ObtenerIndicadoresProductoSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            throw new System.NotImplementedException();
        }

        public Task<IndicadorResponse> GuardarIndicadoresSecundariosSGP(AgregarIndicadoresSecundariosDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<IndicadorResponse> EliminarIndicadorProductoSGP(int indicadorId, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }
        public Task<IndicadorResponse> ActualizarMetaAjusteIndicadorSGP(IndicadoresIndicadorProductoDto Indicador, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ObtenerProyectosBeneficiariosSGP(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerProyectosBeneficiariosDetalleSGP(string json, string usuarioDNP, string tokenAutorizacion)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarBeneficiarioTotalesSGP(BeneficiarioTotalesDto beneficiario, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarBeneficiarioProductoSGP(BeneficiarioProductoSgpDto beneficiario, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarBeneficiarioProductoLocalizacionSGP(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> GuardarBeneficiarioProductoLocalizacionCaracterizacionSGP(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResultadoProcedimientoDto> guardarLocalizacionSGP(LocalizacionProyectoAjusteDto objLocalizacion, string usuarioDNP, string tokenAutorizacion)
        {
            throw new NotImplementedException();
        }

        public Task<EncabezadoSGPDto> ObtenerHorizonteSgp(ProyectoParametrosEncabezadoDto parametros, string usuarioDnp)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> guardarFuentesFinanciacionRecursosAjustesSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<ObjectivosAjusteDto> ObtenerResumenObjetivosProductosActividadesSgp(string bpin, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<ReponseHttp> GuardarCostoActividadesSgp(ProductoAjusteDto producto, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<string> AgregarEntregableSgp(AgregarEntregable[] entregables, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<string> EliminarEntregableSgp(EntregablesActividadesDto entregable, string usuarioDNP)
        {
            throw new NotImplementedException();
        }
        public Task<RegionalizacionDto> RegionalizacionGeneralSgp(string bpin, string usuarioDNP, string tokenAutorizacion)
        {
            throw new NotImplementedException();

        }
        public Task<string> ObtenerListaTiposRecursosxEntidadSgp(ProyectoParametrosDto peticion, int entityTypeCatalogId, int entityType)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerCategoriasFocalizacionJustificacionSgp(string bpin, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }
        public Task<string> ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string bpin, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

    }
}