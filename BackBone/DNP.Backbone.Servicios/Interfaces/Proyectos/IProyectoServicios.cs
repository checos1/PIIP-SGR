namespace DNP.Backbone.Servicios.Interfaces.Proyectos
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Beneficiarios;
    using DNP.Backbone.Dominio.Dto.CadenaValor;
    using DNP.Backbone.Dominio.Dto.Consola;
    using DNP.Backbone.Dominio.Dto.CostoActividades;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Transversal;
    using DNP.Backbone.Dominio.Dto.Transversales;
	using Dominio.Dto.Proyecto;

    public interface IProyectoServicios
	{
        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <param name="usuarioDNP"></param>   
        /// <returns>string</returns> 
        Task<string> ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuarioDNP);

        Task<ProyectoDto> ObtenerInfoPDF(InstanciaProyectoDto datosConsulta, string token);
		Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token);
		Task<ProyectoDto> ObtenerProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto, string usuarioDNP);
		Task<ProyectoDto> ObtenerProyectosTodos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto);
		Task<string> ObtenerProyectosBpin(string bpin, string usuarioDNP, string tokenAutorizacion);
		Task<ProyectoResumenDto> ObtenerMonitoreoProyectos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto);
		Task<InstanciaDto> ActivarInstancia(ProyectoParametrosDto datosConsulta, string token);
		Task<InstanciaDto> PausarInstancia(ProyectoParametrosDto datosConsulta, string token);
		Task<InstanciaDto> DetenerInstancia(ProyectoParametrosDto datosConsulta, string token);
		Task<InstanciaDto> CancelarInstanciaMisProcesos(ProyectoParametrosDto datosConsulta, string token);
		Task<IEnumerable<ProyectoCreditoDto>> ObtenerContracreditos(ProyectoCreditoParametroDto parametros, string usuarioDnp);
		Task<IEnumerable<ProyectoCreditoDto>> ObtenerCreditos(ProyectoCreditoParametroDto parametros, string usuarioDnp);
		Task<RespuestaGeneralDto> GuardarProyectos(ParametroProyectoTramiteDto parametros, string usuarioDnp);
		Task<ProyectoDto> ObtenerProyectosConsolaProcesos(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroDto proyectoFiltroDto, string usuarioDNP);
		Task<string> ObtenerTokenMGA(string bpin, Dominio.Dto.UsuarioLogadoDto usuarioDNP, string tipoUsuario, string tokenAutorizacion);
		Task<RespuestaGeneralDto> actualizarHorizonte(HorizonteProyectoDto parametrosHorizonte, string usuarioDnp);
		Task<IndicadorProductoDto> ObtenerIndicadoresProducto(string bpin, string usuarioDNP, string tokenAutorizacion);
		Task<IndicadorResponse> GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametros, string usuarioDnp);
		Task<IndicadorResponse> EliminarIndicadorProducto(int indicadorId, string usuarioDNP);
        Task<ObjectivosAjusteDto> ObtenerResumenObjetivosProductosActividades(string bpin, string usuarioDNP);
        Task<Dominio.Dto.SeguimientoControl.ReponseHttp> GuardarCostoActividades(ProductoAjusteDto producto, string usuarioDNP);
		Task<IndicadorResponse> ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador, string usuarioDNP);
        Task<string> AgregarEntregable(AgregarEntregable[] entregables, string usuarioDNP);
        Task<string> EliminarEntregable(EntregablesActividadesDto entregable, string usuarioDNP);
		Task<List<InstanciaDto>> DevolverInstanciasHijas(ProyectoParametrosDto datosConsulta, string token);
		Task<List<IndicadorCapituloModificadoDto>> IndicadoresValidarCapituloModificado(string bpin, string usuarioDNP, string tokenAutorizacion);
		Task<RegionalizacionDto> RegionalizacionGeneral(string bpin, string usuarioDNP, string tokenAutorizacion);
		Task<RespuestaGeneralDto> GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuarioDNP);
        Task<ObjectivosAjusteJustificacionDto> ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin, string usuarioDNP);
		Task<LocalizacionJustificacionProyectoDto> ObtenerJustificacionLocalizacionProyecto(int proyectoId, string usuarioDNP);
		Task<RespuestaGeneralDto> GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuarioDNP);
        Task<string> ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin, string usuarioDNP, string tokenAutorizacion);
		Task<List<ProyectoInstanciaDto>> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN, string usuarioDNP);
		Task<string> ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion);
		Task<string> ObtenerProyectosBeneficiariosDetalle(string json, string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerJustificacionProyectosBeneficiarios(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuarioDNP);
        Task<string> GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuarioDNP);
        Task<string> GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuarioDNP);
        Task<string> GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuarioDNP);
		Task<string> ObtenerSeccionPoliticaFocalizacionDT(string bpin, string usuarioDNP, string tokenAutorizacion);
		Task<RespuestaGeneralDto> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuarioDNP);
        Task<SoportesDto> ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos, string usuarioDNP);
		Task<PlanNacionalDesarrolloDto> ObtenerPND(int idProyecto, string usuarioPND);
		Task<ProyectoVerificacionOcadPazDto> ObtenerProyectosVerificacionOcadPazSgr(ProyectoParametrosDto datosConsulta, string token, ProyectoFiltroVerificacionOcadPazSgrDto proyectoFiltroDto);
    }
}
