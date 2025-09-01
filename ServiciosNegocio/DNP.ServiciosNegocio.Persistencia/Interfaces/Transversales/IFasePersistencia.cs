using DNP.ServiciosNegocio.Dominio.Dto.Transversales;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales
{
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using System.Collections.Generic;

    public interface IFasePersistencia
    {
        FaseDto ObtenerFaseByGuid(string guid);
    }
}
