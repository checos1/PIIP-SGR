using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Tramite
{
    public interface ITramiteSGPServicio
    {
        TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario);
        TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId);

        TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario);
        List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId);
        TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario);
        List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId);
        TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario);
        IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP);
        IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo);
        IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo);
        string ObtenerTiposValorPorEntidad(int IdEntidad, int IdTipoEntidad);

        RespuestaGeneralDto GuardarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario);

        RespuestaGeneralDto eliminarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario);

        string ObtenerDatosAdicionSgp(int TramiteId);
    }
}
