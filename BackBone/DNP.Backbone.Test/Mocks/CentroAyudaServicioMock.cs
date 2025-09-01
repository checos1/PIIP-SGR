using DNP.Backbone.Dominio.Dto.CentroAyuda;
using DNP.Backbone.Servicios.Interfaces.CentroAyuda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNP.Backbone.Test.Mocks
{
    public class CentroAyudaServicioMock : ICentroAyudaServicio
    {
        public Task<bool> CrearActualizarTema(AyudaTemaListaItemDto dto, string usuarioDnp)
            => !string.IsNullOrEmpty(usuarioDnp)
                ? Task.FromResult(true)
                : Task.FromResult(false);

        public Task<bool> EliminarTema(int id, string usuarioDnp)
            => !string.IsNullOrEmpty(usuarioDnp)
                ? Task.FromResult(true)
                : Task.FromResult(false);

        public Task<IEnumerable<AyudaTemaListaItemDto>> ObtenerListaTemas(AyudaTemaFiltroDto dto, string usuarioDnp)
            => !string.IsNullOrEmpty(usuarioDnp) 
                ? Task.FromResult((IEnumerable<AyudaTemaListaItemDto>) new List<AyudaTemaListaItemDto>())
                : Task.FromResult<IEnumerable<AyudaTemaListaItemDto>>(null);

        Task<AyudaTemaListaItemDto> ICentroAyudaServicio.CrearActualizarTema(AyudaTemaListaItemDto dto, string usuarioDnp)
          => Task.FromResult(new AyudaTemaListaItemDto());

    }
}
