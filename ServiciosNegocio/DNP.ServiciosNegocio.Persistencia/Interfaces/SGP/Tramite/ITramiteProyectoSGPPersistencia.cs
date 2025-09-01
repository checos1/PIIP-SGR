using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Tramite
{
    public interface ITramiteProyectoSGPPersistencia
    {
        string ObtenerProyectosTramiteNegocio(int TramiteId);
        TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario);
        string ValidacionProyectosTramiteNegocio(int TramiteId);
    }
}
