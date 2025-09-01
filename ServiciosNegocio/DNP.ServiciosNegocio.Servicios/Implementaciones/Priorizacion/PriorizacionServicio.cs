using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.SeguimientoControl;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Priorizacion;
using DNP.ServiciosNegocio.Servicios.Interfaces.Priorizacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Priorizacion
{
    public class PriorizacionServicio: IPriorizacionServicio
    {
        private readonly IPriorizacionPersistencia _priorizacionPersistencia;

        public PriorizacionServicio(IPriorizacionPersistencia priorizacionPersistencia)
        {
            _priorizacionPersistencia = priorizacionPersistencia;
        }

        public Task<List<PriorizacionDatosBasicosDto>> ConsultarProyectosPorBPINs(BPINsProyectosDto bpins)
        {
            var priorizacionDatosBasicos = new List<PriorizacionDatosBasicosDto>();

            if (bpins != null && bpins.BPINs.Count > 0)
                priorizacionDatosBasicos = _priorizacionPersistencia.ObtenerProyectosPorBPINs(bpins);

            if (priorizacionDatosBasicos.Count == 0) return Task.FromResult<List<PriorizacionDatosBasicosDto>>(null);

            return Task.FromResult(priorizacionDatosBasicos);
        }

        public Task<InstanciaPriorizacionDto> ObtenerRegistroPriorizacion(ObjetoNegocio objetoNegocio)
        {
            var priorizacionDatosBasicos = new InstanciaPriorizacionDto();

            if (objetoNegocio != null)
                priorizacionDatosBasicos = _priorizacionPersistencia.ObtenerRegistroPriorizacion(objetoNegocio);


            return Task.FromResult(priorizacionDatosBasicos);
        }

        public string ObtenerFuentesSGR(string bpin, Nullable<Guid> instanciaId)
        {
            return _priorizacionPersistencia.ObtenerFuentesSGR(bpin, instanciaId);
        }

        public Task<ReponseHttp> RegistrarViabilidadFuentesSGR(List<EtapaSGRDto> json, string usuario)
        {
            try
            {                
                _priorizacionPersistencia.RegistrarViabilidadFuentesSGR(json, usuario);

                return Task.FromResult(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        //public Task<IEnumerable<PriorizacionProyectoDto>> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId)
        //{
        //    var priorizacionProyecto = _priorizacionPersistencia.ObtenerPriorizacionProyecto(instanciaId);

        //    return Task.FromResult(priorizacionProyecto);
        //}


        //public Task<IEnumerable<PriorizacionProyectoDto>> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId)
        //{
        //    var aprobacionProyecto = _priorizacionPersistencia.ObtenerAprobacionProyecto(instanciaId);

        //    return Task.FromResult(aprobacionProyecto);
        //}

        public string ObtenerFuentesNoSGR(string bpin, Nullable<Guid> instanciaId)
        {
            return _priorizacionPersistencia.ObtenerFuentesNoSGR(bpin, instanciaId);
        }

        public Task<ReponseHttp> RegistrarViabilidadFuentesNoSGR(List<EtapaNoSGRDto> json, string usuario)
        {
            try
            {
                _priorizacionPersistencia.RegistrarViabilidadFuentesNoSGR(json, usuario);

                return Task.FromResult(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        public string ObtenerResumenFuentesCostos(string bpin, Nullable<Guid> instanciaId)
        {
            return _priorizacionPersistencia.ObtenerResumenFuentesCostos(bpin, instanciaId);
        }

        public Task<ReponseHttp> RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(DatosAdicionalesCofinanciadorDto json, string usuario)
        {
            try
            {
                _priorizacionPersistencia.RegistrarDatosAdicionalesCofinanciadorFuentesNoSGR(json, usuario);

                return Task.FromResult(new ReponseHttp() { Status = true });
            }
            catch (ServiciosNegocioException e)
            {
                return Task.FromResult(new ReponseHttp()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        public string ObtenerDatosAdicionalesCofinanciadorNoSGR(string bpin, Nullable<int> vigencia, Nullable<int> vigenciaFuente)
        {
            return _priorizacionPersistencia.ObtenerDatosAdicionalesCofinanciadorNoSGR(bpin, vigencia, vigenciaFuente);
        }
      
        public ProyectoPriorizacionDetalleResultado GuardarPermisosPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario)
        {
            return _priorizacionPersistencia.GuardarPermisosPriorizionProyectoDetalleSGR(proyectoPriorizacionDetalleDto, usuario);
        }
    }
}
