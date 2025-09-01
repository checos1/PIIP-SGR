namespace DNP.Backbone.Test.Mocks
{
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using System.Threading.Tasks;

    public class FocalizacionCategoriaServiciosMock : ICategoriaProductosPoliticaServicios
    {
        public Task<string> ObtenerCategoriaProductosPolitica(string bpin, int fuenteId, int politicaId, string usuarioDnp, string tokenAutorizacion)
        {
            return Task.FromResult<string>(string.Empty);
        }

        public Task<string> GuardarDatosSolicitudRecursos(CategoriaProductoPoliticaDto categoriaProductoPoliticaDto, string usuarioDnp, string tokenAutorizacion)
        {
            return Task.FromResult<string>(string.Empty);
        }

    }
}
