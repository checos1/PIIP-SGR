using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.FuenteFinanciacion
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Comunes.Dto;
    using Comunes.Dto.Formulario;

    public interface IFuenteFinanciacionAgregarServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregar(ParametrosConsultaDto parametrosConsulta);
        string ObtenerFuenteFinanciacionAgregarN(string bpin);
        string ObtenerFuenteFinanciacionVigencia(string bpin);
        ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregarPreview();
        ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> ConstruirParametrosGuardado(HttpRequestMessage request, ProyectoFuenteFinanciacionAgregarDto contenido);
        void Guardar(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, ParametrosAuditoriaDto parametrosAuditoria, bool guardarTemporalmente);
        void GuardarFuenteFinanciacion(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario);
        FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId);
        string ObtenerResumenCostosVsSolicitado(string bpin) ;
        //ResumenFuenteFinanciacionDTO ConsultarResumenFteFinanciacion(string bpin);   
        string ConsultarResumenFteFinanciacion(string bpin);
        string ConsultarCostosPIIPvsFuentesPIIP(string bpin);
        string FuentesFinanciacionRecursosAjustesAgregar(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario);
        string ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string name);
        string ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string name);
        OperacionCreditoDatosGeneralesDto ObtenerOperacionCreditoDatosGenerales(string bpin, Guid? instanciaId);
        FuenteFinanciacionResultado GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto, string usuario);
        OperacionCreditoDetallesDto ObtenerOperacionCreditoDetalles(string bpin, Guid? instanciaId);
        FuenteFinanciacionResultado GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuario);
    }
}
