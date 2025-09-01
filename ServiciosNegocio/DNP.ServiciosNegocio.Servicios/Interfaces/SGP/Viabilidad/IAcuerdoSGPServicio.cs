using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGP.Viabilidad
{
    public interface IAcuerdoSGPServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        string SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId);
        ResultadoProcedimientoDto SGPAcuerdoGuardarProyecto(string Json, string usuario);
        string SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista);
    }
}
