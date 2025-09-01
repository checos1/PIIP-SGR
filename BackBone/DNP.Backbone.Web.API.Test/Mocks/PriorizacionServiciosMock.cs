using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto.Priorizacion;
using DNP.Backbone.Dominio.Dto.Priorizacion.Viabilidad;
using DNP.Backbone.Dominio.Dto.SeguimientoControl;
using DNP.Backbone.Dominio.Dto.SGR.GestionRecursos;
using DNP.Backbone.Servicios.Interfaces.Priorizacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Web.API.Test.Mocks
{
    public class PriorizacionServiciosMock : IPriorizacionServicios
    {
        public Task<string> ObtenerFuentesSGR(string bpin, Guid? instanciaId, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<List<PriorizacionDatosBasicosDto>> ObtenerProyectosPorBPINs(BPINsProyectosDto bpins, string usuarioDNP)
        {
            return Task.FromResult(new List<PriorizacionDatosBasicosDto>());
        }

        public Task<ReponseHttp> RegistrarFuentesSGR(List<EtapaSGRDto> jsonEtapa, string UsuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerFuentesNoSGR(string bpin, Guid? instanciaId, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<ReponseHttp> RegistrarFuentesNoSGR(List<EtapaNoSGRDto> jsonEtapa, string UsuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerResumenFuentesCostos(string bpin, Guid? instanciaId, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> ObtenerTiposCofinanciaciones(string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public Task<ReponseHttp> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto jsonVigencias, string UsuarioDNP)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, int? vigencia, int? vigenciaFuente, string usuarioDNP)
        {
            return Task.FromResult(string.Empty);
        }

        public async Task<IEnumerable<ProyectoPriorizacionDetalleDto>> ObtenerPriorizionProyectoDetalleSGR(Nullable<Guid> instanciaId, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public async Task<ProyectoPriorizacionDetalleResultado> GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string UsuarioDNP)
        {  
            throw new NotImplementedException(); 
        }

        Task<string> IPriorizacionServicios.ObtenerPriorizacionProyecto(Guid? instanciaId, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        Task<string> IPriorizacionServicios.ObtenerAprobacionProyecto(Guid? instanciaId, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        public async Task<ProyectoPriorizacionDetalleResultado> GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string UsuarioDNP)
        {
            throw new NotImplementedException();
        }

        public async Task<AprobacionProyectoCreditoDto> ObtenerAprobacionProyectoCredito(Guid instancia, int entidad, string usuarioDNP)
        {
            throw new NotImplementedException();
        }

        Task<ReponseHttp> IPriorizacionServicios.GuardarAprobacionProyectoCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto, string usuarioDNP)
        {
            throw new NotImplementedException();
        }
    }
}
