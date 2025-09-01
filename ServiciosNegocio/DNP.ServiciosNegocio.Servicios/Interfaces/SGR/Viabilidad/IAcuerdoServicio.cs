using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.SGR.Viabilidad
{
    public interface IAcuerdoServicio
    {
        string Usuario { get; set; }
        string Ip { get; set; }
        string SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId);
        ResultadoProcedimientoDto SGR_Acuerdo_GuardarProyecto(string Json, string usuario);

    }
}
