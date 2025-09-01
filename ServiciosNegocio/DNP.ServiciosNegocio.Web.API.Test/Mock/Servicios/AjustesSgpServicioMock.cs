using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Ajustes;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Ajustes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class AjustesSgpServicioMock : IAjustesSgpServicio
    {
        public RespuestaGeneralDto ActualizarHorizonteSgp(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
        {
            var resultado = new RespuestaGeneralDto();
            return resultado;
        }

        public IndicadorResponse ActualizarMetaAjusteIndicadorSgp(IndicadoresIndicadorProductoDto Indicador, string usuario)
        {
            var resultado = new IndicadorResponse();
            return resultado;
        }

        public void AgregarEntregableSgp(AgregarEntregable[] entregables, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public void EliminarEntregableSgp(EntregablesActividadesDto entregable)
        {
            throw new System.NotImplementedException();
        }

        public IndicadorResponse EliminarIndicadorProductoSgp(int indicadorId, string usuario)
        {
            var resultado = new IndicadorResponse();
            return resultado;
        }

        public string FuentesFinanciacionRecursosAjustesAgregarSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public Task<ReponseHttp> GuardarAjusteCostoActividadesSgp(ProductoAjusteDto producto, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public void GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public void GuardarBeneficiarioProductoLocalizacionSgp(BeneficiarioProductoLocalizacionDto beneficiario, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public void GuardarBeneficiarioProductoSgp(BeneficiarioProductoSgpDto beneficiario, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public void GuardarBeneficiarioTotalesSgp(BeneficiarioTotalesDto beneficiario, string usuario)
        {
            throw new System.NotImplementedException();
        }

        public IndicadorResponse GuardarIndicadoresSecundariosSgp(AgregarIndicadoresSecundariosDto parametrosGuardar, string usuario)
        {
            var resultado = new IndicadorResponse();
            return resultado;
        }

        public ResultadoProcedimientoDto GuardarLocalizacionSgp(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario)
        {
            var resultado = new ResultadoProcedimientoDto();
            return resultado;
        }

        public string ObtenerCambiosJustificacionHorizonteSgp(int IdProyecto)
        {
            return string.Empty;
        }

        public EncabezadoSGPDto ObtenerHorizonteSgp(ParametrosEncabezadoSGP parametros)
        {
            var resultado = new EncabezadoSGPDto();
            return resultado;
        }

        public IndicadorProductoDto ObtenerIndicadoresProductoSgp(string bpin)
        {
            var resultado = new IndicadorProductoDto();
            return resultado;
        }

        public string ObtenerProyectosBeneficiariosDetalleSgp(string Json)
        {
            return string.Empty;
        }

        public string ObtenerProyectosBeneficiariosSgp(string Bpin)
        {
            return string.Empty;
        }

        public ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividadesSgp(string bpin)
        {
            throw new System.NotImplementedException();
        }

        public Dominio.Dto.CadenaValor.RegionalizacionDto RegionalizacionGeneralSgp(string bpin)
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ConsultaTiposRecursosEntidadSgp(int entityTypeCatalogId, int entityType)
        {
        throw new System.NotImplementedException();
        }

        public string ObtenerCategoriasFocalizacionJustificacionSgp(string bpin)
        {
            throw new System.NotImplementedException();
        }
        public string ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string bpin)
        {
            throw new System.NotImplementedException();
        }

    }
}
