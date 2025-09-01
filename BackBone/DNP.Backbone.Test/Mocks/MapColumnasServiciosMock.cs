namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Servicios.Interfaces.Monitoreo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MapColumnasServiciosMock : IMapColumnasServicios
    {
        public Task<List<MapColumnasDto>> ObtenerMapColumnas(MapColumnasFiltroDto mapColumnasFiltroDto)
        {
            return mapColumnasFiltroDto.ParametrosDto?.IdUsuarioDNP == "jdelgado" ? Task.FromResult(new List<MapColumnasDto>()) :
                Task.FromResult<List<MapColumnasDto>>(null); 
        }
    }
}
