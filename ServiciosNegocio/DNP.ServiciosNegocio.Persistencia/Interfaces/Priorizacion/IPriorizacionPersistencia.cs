using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Priorizacion
{
    public interface IPriorizacionPersistencia
    {
        List<PriorizacionDatosBasicosDto> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins);
        InstanciaPriorizacionDto ObtenerRegistroPriorizacion(ObjetoNegocio objetoNegocio);
        string ObtenerFuentesSGR(string bpin, Nullable<Guid> instanciaId);
        void RegistrarViabilidadFuentesSGR(List<EtapaSGRDto> json, string usuario);
        string ObtenerFuentesNoSGR(string bpin, Nullable<Guid> instanciaId);
        void RegistrarViabilidadFuentesNoSGR(List<EtapaNoSGRDto> json, string usuario);
        string ObtenerResumenFuentesCostos(string bpin, Nullable<Guid> instanciaId);
        void RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto json, string usuario);
        string ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, Nullable<int> vigencia, Nullable<int> vigenciaFuente);
        //IEnumerable<PriorizacionProyectoDto> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId);
        //IEnumerable<PriorizacionProyectoDto> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId);
        ProyectoPriorizacionDetalleResultado GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario);
    }
}
