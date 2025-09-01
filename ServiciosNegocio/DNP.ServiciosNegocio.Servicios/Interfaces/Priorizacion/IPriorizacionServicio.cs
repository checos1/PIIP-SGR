using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Priorizacion
{
    public interface IPriorizacionServicio
    {
        Task<List<PriorizacionDatosBasicosDto>> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins);
        Task<InstanciaPriorizacionDto> ObtenerRegistroPriorizacion(ObjetoNegocio bpins);
        string ObtenerFuentesSGR(string bpin, Nullable<Guid> instanciaId);
        Task<ReponseHttp> RegistrarViabilidadFuentesSGR(List<EtapaSGRDto> json, string usuario);
        string ObtenerFuentesNoSGR(string bpin, Nullable<Guid> instanciaId);
        Task<ReponseHttp> RegistrarViabilidadFuentesNoSGR(List<EtapaNoSGRDto> json, string usuario);
        string ObtenerResumenFuentesCostos(string bpin, Nullable<Guid> instanciaId);
        Task<ReponseHttp> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto json, string usuario);
        string ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, Nullable<int> vigencia, Nullable<int> vigenciaFuente);
        //Task<IEnumerable<PriorizacionProyectoDto>> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId);
        //Task<IEnumerable<PriorizacionProyectoDto>> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId);
        ProyectoPriorizacionDetalleResultado GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario);
    }
}