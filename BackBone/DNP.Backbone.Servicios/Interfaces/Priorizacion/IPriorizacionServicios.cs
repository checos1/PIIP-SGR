using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Priorizacion;
using DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Dominio.Dto.SGR.GestionRecursos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace DNP.Backbone.Servicios.Interfaces.Priorizacion
{
    public interface IPriorizacionServicios
    {
        Task<List<PriorizacionDatosBasicosDto>> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins, string idUsuarioDNP);
        Task<string> ObtenerFuentesSGR(string bpin, Guid? instanciaId, string usuarioDNP);
        Task<ReponseHttp> RegistrarFuentesSGR(List<EtapaSGRDto> jsonEtapa, string UsuarioDNP);
        Task<string> ObtenerFuentesNoSGR(string bpin, Guid? instanciaId, string usuarioDNP);
        Task<ReponseHttp> RegistrarFuentesNoSGR(List<EtapaNoSGRDto> jsonEtapa, string UsuarioDNP);
        Task<string> ObtenerResumenFuentesCostos(string bpin, Guid? instanciaId, string usuarioDNP);
        Task<string> ObtenerTiposCofinanciaciones(string usuarioDNP);
        Task<ReponseHttp> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto jsonVigencias, string UsuarioDNP);
        Task<string> ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, int? vigencia, int? vigenciaFuente, string usuarioDNP);
        Task<string> ObtenerPriorizacionProyecto(Guid? instanciaId, string usuarioDNP);
        Task<string> ObtenerAprobacionProyecto(Guid? instanciaId, string usuarioDNP);
        Task<IEnumerable<ProyectoPriorizacionDetalleDto>> ObtenerPriorizionProyectoDetalleSGR(Nullable<Guid> instanciaId, string usuarioDNP);
        Task<ProyectoPriorizacionDetalleResultado> GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string UsuarioDNP);
        Task<ProyectoPriorizacionDetalleResultado> GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string UsuarioDNP);
        Task<AprobacionProyectoCreditoDto> ObtenerAprobacionProyectoCredito(Guid instancia, int entidad, string usuarioDNP);
        Task<ReponseHttp> GuardarAprobacionProyectoCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto, string usuarioDNP);
    }
}
