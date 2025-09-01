using DNP.ServiciosNegocio.Dominio.Dto.Transversales;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Transversales
{
    using System.Collections.Generic;

    public interface IEstadoPersistencia
    {
        List<EstadoDto> ObtenerListaEstado();
        
    }
}
