using DNP.ServiciosNegocio.Comunes.Dto;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class FuenteFinanciacionAgregarServicioMock : IFuenteFinanciacionAgregarServicio
    {
        public string Usuario { get; set; }
        public string Ip { get; set; }

        public ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> ConstruirParametrosGuardado(HttpRequestMessage request, ProyectoFuenteFinanciacionAgregarDto contenido)
        {
            throw new NotImplementedException();
        }

        public string ConsultarCostosPIIPvsFuentesPIIP(string bpin)
        {
            throw new NotImplementedException();
        }

        public string ConsultarResumenFteFinanciacion(string bpin)
        {
            throw new NotImplementedException();
        }

        public FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId)
        {
            throw new NotImplementedException();
        }

        public string FuentesFinanciacionRecursosAjustesAgregar(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario)
        {
            throw new NotImplementedException();
        }

        public void Guardar(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente)
        {
            throw new NotImplementedException();
        }

        public void GuardarFuenteFinanciacion(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario)
        {
            throw new NotImplementedException();
        }

        public FuenteFinanciacionResultado GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto, string usuario)
        {
            throw new NotImplementedException();
        }

        public FuenteFinanciacionResultado GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuario)
        {
            throw new NotImplementedException();
        }

        public string ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string name)
        {
            throw new NotImplementedException();
        }

        public string ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string name)
        {
            throw new NotImplementedException();
        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregar(ParametrosConsultaDto parametrosConsulta)
        {
            throw new NotImplementedException();
        }

        public string ObtenerFuenteFinanciacionAgregarN(string bpin)
        {
            throw new NotImplementedException();
        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregarPreview()
        {
            throw new NotImplementedException();
        }

        public string ObtenerFuenteFinanciacionVigencia(string bpin)
        {
            throw new NotImplementedException();
        }

        public OperacionCreditoDatosGeneralesDto ObtenerOperacionCreditoDatosGenerales(string bpin, Guid? instanciaId)
        {
            OperacionCreditoDatosGeneralesDto dto = new OperacionCreditoDatosGeneralesDto();
            CriteriosDto criterio = new CriteriosDto();
            criterio.Habilita = true;
            criterio.NombreTipoValor = "Valor";
            criterio.Valor = 1000000;
            dto.Criterios = new List<CriteriosDto> { criterio };
            return dto;
        }

        public OperacionCreditoDetallesDto ObtenerOperacionCreditoDetalles(string bpin, Guid? instanciaId)
        {
            OperacionCreditoDetallesDto dto = new OperacionCreditoDetallesDto();
            dto.CostoFinanciero = 1000000;
            dto.CostoPatrimonio = 2000000;
            dto.ValorTotalCredito = 3000000;
            return dto;
        }

        public string ObtenerResumenCostosVsSolicitado(string bpin)
        {
            throw new NotImplementedException();
        }
    }
}
