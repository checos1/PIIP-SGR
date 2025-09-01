using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad
{
        

    public interface IAcuerdoPersistencia
    {
        string SGR_Acuerdo_LeerProyecto(int proyectoId, System.Guid nivelId);
        ResultadoProcedimientoDto SGR_Acuerdo_GuardarProyecto(string Json, string usuario);

    }
}
