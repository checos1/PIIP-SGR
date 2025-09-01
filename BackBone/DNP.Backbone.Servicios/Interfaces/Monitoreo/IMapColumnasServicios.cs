namespace DNP.Backbone.Servicios.Interfaces.Monitoreo
{
    using Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMapColumnasServicios
    {
        Task<List<MapColumnasDto>> ObtenerMapColumnas(MapColumnasFiltroDto mapColumnasFiltroDto);
    }
}
