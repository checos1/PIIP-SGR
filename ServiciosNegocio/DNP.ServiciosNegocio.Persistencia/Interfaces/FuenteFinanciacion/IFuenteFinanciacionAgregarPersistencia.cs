
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion
{
    using System;
    using System.Collections.Generic;
    using Comunes.Dto.Formulario;

    public interface IFuenteFinanciacionAgregarPersistencia
    {
        ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregar(string bpin);
        string ObtenerFuenteFinanciacionAgregarN(string bpin);
        string ObtenerFuenteFinanciacionVigencia(string bpin);
        ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregarPreview();
        void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario);
        void GuardarFuenteFinanciacion(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar, string usuario);
        FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId);
        string ObtenerResumenCostosVsSolicitado(string bpin) ;
        //ResumenFuenteFinanciacionDTO ConsultarResumenFteFinanciacion(string bpin);
        string ConsultarResumenFteFinanciacion(string bpin);
        string ConsultarCostosPIIPvsFuentesPIIP(string bpin);
        string FuentesFinanciacionRecursosAjustesAgregar(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario);
        string ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string usuario);
        string ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string usuario);
        OperacionCreditoDatosGeneralesDto ObtenerOperacionCreditoDatosGenerales(string bpin, Guid? instanciaId);
        FuenteFinanciacionResultado GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto, string usuario);
        OperacionCreditoDetallesDto ObtenerOperacionCreditoDetalles(string bpin, Guid? instanciaId);
        FuenteFinanciacionResultado GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto, string usuario);
    }
}
