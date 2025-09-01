using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto;
using DNP.Backbone.Dominio.Dto.Proyecto;
using DNP.Backbone.Dominio.Dto.Tramites;
using DNP.Backbone.Dominio.Dto.Tramites.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.SGP
{
    public interface ISGPTramiteServicios
    {
        Task<RespuestaGeneralDto> ActualizarEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuarioDnp);
        Task<RespuestaGeneralDto> EliminarProyectoTramiteNegocio(InstanciaTramiteDto instanciaTramiteDto);
        Task<InstanciaResultado> EliminarInstanciaCerrada_AbiertaProyectoTramite(Guid instanciaTramite, string Bpin, string usuarioDnp);

        //Información Presupuestal
        Task<RespuestaGeneralDto> GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametros, string usuarioDnp);
        Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentes(int tramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> parametros, string usuarioDnp);
        Task<List<ProyectoTramiteFuenteDto>> ObtenerListaProyectosFuentesAprobado(int tramiteId, string usuarioDnp);
        Task<RespuestaGeneralDto> GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametros, string usuarioDnp);
        Task<List<ProyectoRequisitoDto>> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, string usuarioDnp, bool isCDP);
        Task<IEnumerable<ProyectoCreditoDto>> ObtenerContracreditosSgp(ProyectoCreditoParametroDto parametros, string usuarioDnp);
        Task<IEnumerable<ProyectoCreditoDto>> ObtenerCreditosSgp(ProyectoCreditoParametroDto parametros, string usuarioDnp);
        Task<string> ObtenerTiposValorPorEntidadSgp(int IdEntidad, int IdTipoEntidad, string usuarioDnp);

        Task<string> ObtenerDatosAdicionSgp(int tramiteId, string usuarioDnp);

        Task<string> GuardarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario);

        Task<string> EiliminarDatosAdicionSgp(ConvenioDonanteDto objConvenioDonanteDto, string usuario);

    }
}
