using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos
{
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
    using DNP.ServiciosNegocio.Dominio.Dto.Conpes;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProyectoPersistencia
    {
        ProyectoDto ObtenerProyectoPreview();
        List<ProyectoEntidadDto> ObtenerProyectosEntidad(List<int> idEntidades, List<string> estados);
        List<ProyectoPriorizarDto> ObtenerProyectosPriorizar(String IdUsuarioDNP);
        List<ProyectoEntidadDto> ObtenerProyectosPorEstados(List<int> idsEntidades,
                                                            List<string> nombresEstadosProyectos);
        List<ProyectoEntidadDto> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins);
        List<EntidadDto> ObtenerEntidadesPorIds(List<string> idsEntidades);
        List<ProyectoEntidadDto> ObtenerProyectosPorIds(List<int> ids);
        List<CrTypeDto> ObtenerCRType();
        List<FaseDto> ObtenerFase();
        RespuestaGeneralDto MantenimientoMatrizFlujo(List<MatrizEntidadDestinoAccionDto> flujos);
        List<MatrizEntidadDestinoAccionDto> ObtenerMatrizFlujo(int EntidadResponsableId);

        short InsertarAuditoriaEntidad(AuditoriaEntidadDto auditoriaEntidad);
        List<AuditoriaEntidadDto> ObtenerAuditoriaEntidad(int proyectoId);

        IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo, int? idEntidadFiltro, string bpin, string nombreProyecto);

        IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo, string bpin, string nombreProyecto);
        CapituloConpes ObtenerProyectoConpes(int proyectoId, Guid InstanciaId, string GuiMacroproceso, Guid NivelId, Guid FlujoId);
        RespuestaGeneralDto ActualizarHorizonte(HorizonteProyectoDto datosHorizonteProyecto, string usuario);
        RespuestaGeneralDto AdicionarProyectoConpes(CapituloConpes Conpes, string usuario);
        List<DocumentoCONPESDto> EliminarProyectoConpes(int proyectoId, int conpesId);
        ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividades(string bpin);
        void GuardarAjusteCostoActividades(ProductoAjusteDto producto, string usuario);
        void AgregarEntregable(AgregarEntregable[] entregables, string usuario);
        void EliminarEntregable(EntregablesActividadesDto entregable);
        ObjectivosAjusteJustificacionDto ObtenerResumenObjetivosProductosActividadesJustificacion(string bpin);
        LocalizacionJustificacionProyectoDto ObtenerJustificacionLocalizacionProyecto(int idProyecto);

        List<ProyectoInstanciaDto> ObtenerInstanciaProyectoTramite(Guid InstanciaId, string BPIN);
        List<ConfiguracionUnidadMatrizDTO> ObtenerMatrizEntidadDestino(ListMatrizEntidadDestinoDto dto, string usuario);
        RespuestaGeneralDto ActualizarMatrizEntidadDestino(ListaMatrizEntidadUnidadDto dto, string usuario);

        /// <summary>
        /// Validacion previa a la devolución de un paso
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <param name="accionId"></param>   
        /// <param name="accionDevolucionId"></param>   
        /// <param name="usuario"></param> 
        /// <returns>string</returns> 
        string ValidacionDevolucionPaso(Guid instanciaId, Guid accionId, Guid accionDevolucionId, string usuario);

        string ObtenerProyectosBeneficiarios(string Bpin);
        string ObtenerProyectosBeneficiariosDetalle(string Json);
        string ObtenerJustificacionProyectosBeneficiarios(string Bpin);
        void GuardarBeneficiarioTotales(BeneficiarioTotalesDto beneficiario, string usuario);
        void GuardarBeneficiarioProducto(BeneficiarioProductoDto beneficiario, string usuario);
        void GuardarBeneficiarioProductoLocalizacion(BeneficiarioProductoLocalizacionDto beneficiario, string usuario);
        void GuardarBeneficiarioProductoLocalizacionCaracterizacion(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario);
        string GetCategoriasSubcategorias_JSON(int padreId, Nullable<int> entidadId, int esCategoria, int esGruposEtnicos);
        List<ProyectoEntidadDto> ConsultarProyectosASeleccionar(ParametrosProyectosDto parametros);
        RespuestaGeneralDto GuardarReprogramacionPorProductoVigencia(List<ReprogramacionValores> reprogramacionValores, string usuario);
        SoportesDto ObtenerDocumentosProyecto(FiltroDocumentosDto filtroDocumentos);
        PlanNacionalDesarrolloDto ObtenerPND(int idProyecto);
    }
}
