namespace DNP.ServiciosNegocio.Servicios.Interfaces.Proyectos
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Comunes.Dto.ObjetosNegocio;
	using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
	using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
	using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
    using Dominio.Dto.Proyectos;

	public interface IProyectoServicio
	{
        string Usuario { get; set; }
        string Ip { get; set; }
        Task<ProyectoDto> ObtenerProyecto(string bPin, string tokenAutorizacion);
		Task<ProyectoDto> ObtenerProyectoPreview();
		Task<List<ProyectoEntidadDto>> ConsultarProyectosPorEntidadesYEstados(ParametrosProyectosDto parametros);
		Task<List<ProyectoPriorizarDto>> ObtenerProyectosPriorizar(String IdUsuarioDNP);

        Task<List<ProyectoEntidadDto>> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins);
		Task<List<EntidadDto>> ConsultarEntidadesPorIds(List<string> idsEntidades);
		Task<List<ProyectoEntidadDto>> ConsultarProyectosPorIds(List<int> ids);
		Task<List<CrTypeDto>> ObtenerCRType();
		Task<List<FaseDto>> ObtenerFase();
		Task<RespuestaGeneralDto> MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos);

		Task<List<MatrizEntidadDestinoAccionDto>> ObtenerMatrizFlujo(int entidadResponsableId);

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param> 
		/// <param name="usuario"></param>   
        /// <returns>string</returns> 
		string ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuario);

        Task<RespuestaGeneralDto> InsertarAuditoriaEntidad(AuditoriaEntidadDto auditoriaEntidad);
		Task<RespuestaGeneralDto> ObtenerAuditoriaEntidad(int proyectoId);
		IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo, int? idEntidadFiltro, string bpin, string nombreProyecto);
		IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo, string bpin, string nombreProyecto);
		CapituloConpes ObtenerProyectoConpes(int proyectoId, Guid InstanciaId, string GuiMacroproceso,string NivelId,string FlujoId);
		Task<RespuestaGeneralDto> ActualizarHorizonte(HorizonteProyectoDto datosHorizonteProyecto, string usuario);
		Task<RespuestaGeneralDto> AdicionarProyectoConpes(CapituloConpes Conpes, string usuario);

		List<DocumentoCONPESDto> EliminarProyectoConpes(int proyectoId, int conpesId);
        ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividades(string bpin);
        Task<ReponseHttp> GuardarAjusteCostoActividades(ProductoAjusteDto producto, string usuario);
        void AgregarEntregable(AgregarEntregable[] entregables, string usuario);
        void EliminarEntregable(EntregablesActividadesDto entregable);
        ObjectivosAjusteJustificacionDto ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin);
		LocalizacionJustificacionProyectoDto ObtenerJustificacionLocalizacionProyecto(int idProyecto);

		List<ProyectoInstanciaDto> ObtenerInstanciaProyectoTramite(string InstanciaId, string BPIN);
        string ObtenerProyectosBeneficiarios(string bpin);
        string ObtenerJustificacionProyectosBeneficiarios(string bpin);
        void GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuario);
        List<ConfiguracionUnidadMatrizDTO> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario);
        Task<RespuestaGeneralDto> ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario);
        void GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuario);
        void GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuario);
        void GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario);
		string GetCategoriasSubcategorias_JSON(int padreId, Nullable<int> entidadId, int esCategoria, int esGruposEtnicos);
		Task<List<ProyectoEntidadDto>> ConsultarProyectosASeleccionar(ParametrosProyectosDto parametros);
		Task<RespuestaGeneralDto> GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuario);
	
        SoportesDto ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos);
		string ObtenerProyectosBeneficiariosDetalle(string json);
		PlanNacionalDesarrolloDto ObtenerPND(int idProyecto);
    }

}
