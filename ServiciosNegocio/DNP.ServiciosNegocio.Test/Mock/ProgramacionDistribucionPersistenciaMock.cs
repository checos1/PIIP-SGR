using DNP.ServiciosNegocio.Comunes.Autorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Servicios.Interfaces.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Persistencia.Interfaces.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;

namespace DNP.ServiciosNegocio.Test.Mock
{
    public class ProgramacionDistribucionPersistenciaMock: IProgramacionDistribucionPersistencia
    {
        private readonly IProgramacionDistribucionServicio _programacionDistribucionServicio;
        private readonly IAutorizacionUtilidades _autorizacionUtilidades;

        public ProgramacionDistribucionPersistenciaMock(IProgramacionDistribucionServicio ProgramacionDistribucionServicioServicio, IAutorizacionUtilidades autorizacionUtilidades)
        {
            _programacionDistribucionServicio = ProgramacionDistribucionServicioServicio;
            _autorizacionUtilidades = autorizacionUtilidades;
        }

        public TramitesResultado GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            return _programacionDistribucionServicio.GuardarDatosProgramacionDistribucion(ProgramacionDistribucion, usuario);
        }

        public string ObtenerDatosProgramacionDistribucion(int EntidadDestinoId, int TramiteId)
        {
            return _programacionDistribucionServicio.ObtenerDatosProgramacionDistribucion(EntidadDestinoId, TramiteId);
        }

        public string ObtenerDatosProgramacionFuenteEncabezado(int EntidadDestinoId, int tramiteid)
        {
            return _programacionDistribucionServicio.ObtenerDatosProgramacionFuenteEncabezado(EntidadDestinoId, tramiteid);
        }

        public string ObtenerDatosProgramacionFuenteDetalle(int tramiteidProyectoId)
        {
            return _programacionDistribucionServicio.ObtenerDatosProgramacionFuenteDetalle(tramiteidProyectoId);
        }

        public TramitesResultado GuardarDatosProgramacionFuente(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            return _programacionDistribucionServicio.GuardarDatosProgramacionFuente(ProgramacionDistribucion, usuario);
        }
    }
}
