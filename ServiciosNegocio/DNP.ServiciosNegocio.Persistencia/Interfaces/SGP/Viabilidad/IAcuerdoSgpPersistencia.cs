using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad
{        
    public interface IAcuerdoSgpPersistencia
    {
        string SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId);
        ResultadoProcedimientoDto SGPAcuerdoGuardarProyecto(string Json, string usuario);
        string SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista);
    }
}
