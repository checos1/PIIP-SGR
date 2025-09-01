
namespace DNP.Backbone.Servicios.Interfaces.FuenteFinanciacion
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using Dominio.Dto.Proyecto;

    public interface IFuenteFinanciacionServicios
    {
        /// <summary>
        /// llamado al servicio para consultar fuente de financiacion
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ObtenerFuenteFinanciacionAgregarN(string bpin, string usuarioDNP, string tokenAutorizacion);

        /// <summary>
        /// llamado al servicio para agregar fuente de financiacion
        /// </summary>
        /// <param name="proyectoFuenteFinanciacionAgregarDto"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> AgregarFuenteFinanciacion(ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto, string usuarioDNP, string tokenAutorizacion);

        Task<string> EliminarFuenteFinanciacion(string fuenteId, string usuarioDNP, string tokenAutorizacion);

        Task<string> ObtenerResumenCostosVsSolicitado(string bpin, string usuarioDNP, string tokenAutorizacion);

        /// <summary>
        /// llamado al servicio para consultar el resumen de las fuentes de financiacion
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ConsultarResumenFuentesFinanciacion(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> ConsultarCostosPIIPvsFuentesPIIP(string bpin, string usuarioDNP, string tokenAutorizacion);

        /// <summary>
        /// nuevo metodo para consultar la fuente vigencia y cofinanciador
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ObtenerFuenteFinanciacionVigencia(string bpin, string usuarioDNP, string tokenAutorizacion);

        /// <summary>
        /// nuevo metodo para consultar la fuente vigencia y cofinanciador
        /// </summary>
        /// <param name="bpin"></param>
        /// <param name="usuarioDNP"></param>
        /// <param name="tokenAutorizacion"></param>
        /// <returns></returns>
        Task<string> ConsultarPoliticasTransversalesAjustes(string bpin, string usuarioDNP, string tokenAutorizacion);

        Task<string> guardarPoliticasTransversalesAjustes(CategoriaProductoPoliticaDto objPoliticaTransversalDto, string usuarioDNP);
        Task<string> guardarFocalizacionCategoriasPolitica(FocalizacionCategoriasAjusteDto objCategoriaPoliticaDto, string usuarioDNP);
        Task<string> ConsultarPoliticasTransversalesCategorias(string bpin, string usuarioDNP, string tokenAutorizacion);

        Task<string> EliminarPoliticasProyecto(int proyectoId, int politicaId, string usuarioDNP, string tokenAutorizacion);

        Task<string> ConsultarPoliticasCategoriasPorPadre(int idPadre, string usuarioDNP);
        Task<string> ObtenerCategoriasSubcategorias(int idPadre, int idEntidad, int esCategoria, int esGrupoEtnico, string usuarioDNP);
        Task<string> ObtenerPoliticasTransversalesResumen(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> ConsultarPoliticasCategoriasIndicadores(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario, string tokenAutorizacion);

        Task<string> EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId, int categoriaId, string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerCrucePoliticasAjustes(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<RespuestaGeneralDto> GuardarCrucePoliticasAjustes(List<CrucePoliticasAjustesDto> objListCruecePoliticasAjustesDto, string usuarioDNP);
        Task<string> ObtenerPoliticasSolicitudConcepto(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> FocalizacionSolicitarConceptoDT(List<FocalizacionSolicitarConceptoDto> objscDto);
        Task<string> ObtenerDireccionesTecnicasPoliticasFocalizacion(string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerResumenSolicitudConcepto(string bpin, string usuarioDNP, string tokenAutorizacion);
        Task<string> ObtenerPreguntasEnvioPoliticaSubDireccion(PreguntasEnvioPoliticaSubDireccionDto PreguntasEnvioPoliticaSubDireccion);
        Task<string> GuardarPreguntasEnvioPoliticaSubDireccionAjustes(PreguntasEnvioPoliticaSubDireccionAjustes PreguntasEnvioPoliticaSubDireccion);
        Task<string> GuardarRespuestaEnvioPoliticaSubDireccionAjustes(RespuestaEnvioPoliticaSubDireccionAjustes PreguntasEnvioPoliticaSubDireccion);
    }
}
