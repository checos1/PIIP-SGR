using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.ProgramacionDistribucion
{
    public class ProgramacionDistribucionServicio : ServicioBase<ConvenioDto>, IProgramacionDistribucionServicio
    {
        private readonly IProgramacionDistribucionPersistencia _programacionDistribucionPersistencia;

        public ProgramacionDistribucionServicio(IProgramacionDistribucionPersistencia objProgramacionDistribucionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _programacionDistribucionPersistencia = objProgramacionDistribucionPersistencia;
        }

        public string ObtenerDatosProgramacionDistribucion(int EntidadDestinoId, int TramiteId)
        {
            return _programacionDistribucionPersistencia.ObtenerDatosProgramacionDistribucion(EntidadDestinoId, TramiteId);
        }

        public TramitesResultado GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            return _programacionDistribucionPersistencia.GuardarDatosProgramacionDistribucion(ProgramacionDistribucion, usuario);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<ConvenioDto> parametrosGuardar, string usuario)
        {
            throw new System.NotImplementedException();
        }

        protected override ConvenioDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            throw new System.NotImplementedException();
        }

        public string ObtenerDatosProgramacionFuenteEncabezado(int EntidadDestinoId, int tramiteid)
        {
            return _programacionDistribucionPersistencia.ObtenerDatosProgramacionFuenteEncabezado(EntidadDestinoId, tramiteid);
        }

        public string ObtenerDatosProgramacionFuenteDetalle(int tramiteidProyectoId)
        {
            return _programacionDistribucionPersistencia.ObtenerDatosProgramacionFuenteDetalle(tramiteidProyectoId);
        }

        public TramitesResultado GuardarDatosProgramacionFuente(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            return _programacionDistribucionPersistencia.GuardarDatosProgramacionFuente(ProgramacionDistribucion, usuario);
        }
    }
}
