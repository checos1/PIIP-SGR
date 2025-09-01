using DNP.ServiciosNegocio.Comunes.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosTransaccional.Persistencia.Interfaces.Tramites
{
    public interface ITramitePersistencia
    {
        DetalleCartaConceptoDto GetRadicadoEntradaORFEO(int? tramiteId);

        string GetUsuarioDestinoORFEO(int? tramiteId, string idUsuarioDNP);

        int PostActualizarCartaRadicado(int tramiteId, string usuarioDnp, string radicadoEntrada = "", string radicadoSalida = "", string expedienteId = "");

        List<ServiciosNegocio.Comunes.Dto.Tramites.TramiteProyectoDto> GetTramiteProyectos(int tramiteId);

        List<AnalistaResponsableDto> ObtenerAnalistaResponsablePorSector(int sectorId);
        string EliminarMarcaPrevioProyectoVigencia(string bpin, string vigencia);
        ResponseDto<bool> ActualizarCargueMasivo(string numeroProceso, string usuario);

        string ConsultarCargueExcel(string numeroProceso);
        List<DatosUsuarioDto> ObtenerUsuariosPorInstanciaPadre(Guid InstanciaId);
        List<CodigoPresupuestal_Proyecto> ObtenerDatosMarcaPrevioVigencia_Proyectos(string Bpin);
        ResponseDto<bool> ActualizaCampoRemitenteConcepto(int tramiteId, string usuarioDNP);
        DetalleTramiteDto ObtenerDetalleTramiteRadicado(string numeroTramite);

        int ObtenerDependenciaByEntidadOrfeoId(int EntidadOrfeoId);
    }
}
