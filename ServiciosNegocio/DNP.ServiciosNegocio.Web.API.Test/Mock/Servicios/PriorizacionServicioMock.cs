using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Servicios.Interfaces.Priorizacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    public class PriorizacionServicioMock : IPriorizacionServicio
    {
        public Task<List<PriorizacionDatosBasicosDto>> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            //return Task.FromResult(new List<PriorizacionDatosBasicosDto>() { BPIN = 97954, ProyectoId  = 97954 });
            return Task.FromResult(new List<PriorizacionDatosBasicosDto>() { new PriorizacionDatosBasicosDto()
                { 
                    BPIN = "97902",
                    ProyectoId = 97902,
                    NombreProyecto = "Construcción Ampliación de Infraestructura para Generación de Cupos en Los Establecimientos de Reclusión del Orden - Nacional",
                    Recurso = "",
                    Fase = "",
                    ValorProyecto = 0
                }
            });
        }

        Task<InstanciaPriorizacionDto> IPriorizacionServicio.ObtenerRegistroPriorizacion(ObjetoNegocio bpins)
        {
            throw new System.NotImplementedException();
        }

        public string ObtenerFuentesSGR(string bpin, Guid? instanciaId)
        {
            return string.Empty;
        }

        public Task<ReponseHttp> RegistrarViabilidadFuentesSGR(List<EtapaSGRDto> json, string usuario)
        {
            throw new NotImplementedException();
        }

        public string ObtenerFuentesNoSGR(string bpin, Guid? instanciaId)
        {
            return string.Empty;
        }

        public Task<ReponseHttp> RegistrarViabilidadFuentesNoSGR(List<EtapaNoSGRDto> json, string usuario)
        {
            throw new NotImplementedException();
        }

        public string ObtenerResumenFuentesCostos(string bpin, Guid? instanciaId)
        {
            return string.Empty;
        }

        public Task<ReponseHttp> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto json, string usuario)
        {
            throw new NotImplementedException();
        }

        public string ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, int? vigencia, int? vigenciaFuente)
        {
            return string.Empty;
        }

        public IEnumerable<ProyectoPriorizacionDetalleDto> ObtenerPriorizionProyectoDetalleSGR(Guid? instanciaId)
        {
            return new List<ProyectoPriorizacionDetalleDto>();
        }

        public ProyectoPriorizacionDetalleResultado GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario)
        {
            return new ProyectoPriorizacionDetalleResultado();
        }

        public ProyectoPriorizacionDetalleResultado GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario)
        {
            return new ProyectoPriorizacionDetalleResultado();
        }
    }
}
