using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Ajustes;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Ajustes;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Ajustes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RegionalizacionDto = DNP.ServiciosNegocio.Dominio.Dto.CadenaValor.RegionalizacionDto;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.SGP.Ajustes
{
    public class AjustesSgpServicio : IAjustesSgpServicio
    {
        private readonly IAjustesSgpPersistencia _objetoPersistencia;

        public AjustesSgpServicio(IAjustesSgpPersistencia ajustesSgpPersistencia)
        {
            _objetoPersistencia = ajustesSgpPersistencia;
        }

        #region Horizonte

        public EncabezadoSGPDto ObtenerHorizonteSgp(ParametrosEncabezadoSGP parametros)
        {
            return _objetoPersistencia.ObtenerHorizonteSgp(parametros);
        }
        public RespuestaGeneralDto ActualizarHorizonteSgp(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
        {
            return _objetoPersistencia.ActualizarHorizonteSgp(datosHorizonteProyecto, usuario);
        }

        public string ObtenerCambiosJustificacionHorizonteSgp(int IdProyecto)
        {
            return _objetoPersistencia.ObtenerCambiosJustificacionHorizonteSgp(IdProyecto);
        }

        #endregion

        #region Indicadores

        public IndicadorProductoDto ObtenerIndicadoresProductoSgp(string bpin)
        {
            return _objetoPersistencia.ObtenerIndicadoresProductoSgp(bpin);
        }
        public IndicadorResponse GuardarIndicadoresSecundariosSgp(AgregarIndicadoresSecundariosDto parametrosGuardar, string usuario)
        {
            return _objetoPersistencia.GuardarIndicadoresSecundariosSgp(parametrosGuardar, usuario);
        }
        public IndicadorResponse EliminarIndicadorProductoSgp(int indicadorId, string usuario)
        {
            return _objetoPersistencia.EliminarIndicadorProductoSgp(indicadorId, usuario);
        }
        public IndicadorResponse ActualizarMetaAjusteIndicadorSgp(IndicadoresIndicadorProductoDto Indicador, string usuario)
        {
            return _objetoPersistencia.ActualizarMetaAjusteIndicadorSgp(Indicador, usuario);
        }

        #endregion

        #region Beneficiarios

        public string ObtenerProyectosBeneficiariosSgp(string Bpin)
        {
            return _objetoPersistencia.ObtenerProyectosBeneficiariosSgp(Bpin);
        }
        public string ObtenerProyectosBeneficiariosDetalleSgp(string Json)
        {
            return _objetoPersistencia.ObtenerProyectosBeneficiariosDetalleSgp(Json);
        }
        public void GuardarBeneficiarioTotalesSgp(BeneficiarioTotalesDto beneficiario, string usuario)
        {
            _objetoPersistencia.GuardarBeneficiarioTotalesSgp(beneficiario, usuario);
        }
        public void GuardarBeneficiarioProductoSgp(BeneficiarioProductoSgpDto beneficiario, string usuario)
        {
            _objetoPersistencia.GuardarBeneficiarioProductoSgp(beneficiario, usuario);
        }
        public void GuardarBeneficiarioProductoLocalizacionSgp(BeneficiarioProductoLocalizacionDto beneficiario, string usuario)
        {
            _objetoPersistencia.GuardarBeneficiarioProductoLocalizacionSgp(beneficiario, usuario);
        }
        public void GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario)
        {
            _objetoPersistencia.GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(beneficiario, usuario);
        }

        #endregion

        #region Localizaciones

        public ResultadoProcedimientoDto GuardarLocalizacionSgp(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario)
        {
            return _objetoPersistencia.GuardarLocalizacionSgp(localizacionProyecto, usuario);
        }

        #endregion
        #region fuentefinanciacion
        public string FuentesFinanciacionRecursosAjustesAgregarSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario)
        {
            return _objetoPersistencia.FuentesFinanciacionRecursosAjustesAgregarSgp(objFuenteFinanciacionAgregarAjusteDto, usuario);
        }
        #endregion
        #region costos
        public ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividadesSgp(string bpin)
        {
            return _objetoPersistencia.ObtenerResumenObjetivosProductosActividadesSgp(bpin);
        }
        public Task<ReponseHttp> GuardarAjusteCostoActividadesSgp(ProductoAjusteDto producto, string usuario)
        {
            try
            {
                _objetoPersistencia.GuardarAjusteCostoActividadesSgp(producto, usuario);
                return Task.FromResult<ReponseHttp>(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult<ReponseHttp>(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        public void AgregarEntregableSgp(AgregarEntregable[] entregables, string usuario)
        {
            _objetoPersistencia.AgregarEntregableSgp(entregables, usuario);
        }

        public void EliminarEntregableSgp(EntregablesActividadesDto entregable)
        {
            _objetoPersistencia.EliminarEntregableSgp(entregable);
        }
        #endregion
        #region regionalizacion
        public RegionalizacionDto RegionalizacionGeneralSgp(string bpin)
        {
            return _objetoPersistencia.RegionalizacionGeneralSgp(bpin);
        }
        #endregion
        #region recursos
        public List<CatalogoDto> ConsultaTiposRecursosEntidadSgp(int entityTypeCatalogId, int entityType) {
            return _objetoPersistencia.ConsultarTiposRecursosEntidadSgp(entityTypeCatalogId, entityType);
        }
        #endregion
        public string ObtenerCategoriasFocalizacionJustificacionSgp(string bpin)
        {
            return _objetoPersistencia.ObtenerCategoriasFocalizacionJustificacionSgp(bpin);
        }
        public string ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string bpin)
        {
            return _objetoPersistencia.ObtenerDetalleCategoriasFocalizacionJustificacionSgp(bpin);
        }
    }
}
