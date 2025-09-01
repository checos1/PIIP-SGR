namespace DNP.Backbone.Web.API.Test.Mocks
{
    using DNP.Backbone.Comunes.Dto;
    using DNP.Backbone.Dominio.Dto.CadenaValor;
    using DNP.Backbone.Dominio.Dto.CostoActividades;
    using DNP.Backbone.Dominio.Dto.Focalizacion;
    using DNP.Backbone.Dominio.Dto.Monitoreo;
    using DNP.Backbone.Dominio.Dto.Proyecto;
    using DNP.Backbone.Servicios.Interfaces.Focalizacion;
    using DNP.Backbone.Servicios.Interfaces.Proyectos;
    using System.Collections.Generic;
    using System.Linq;
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
