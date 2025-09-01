namespace DNP.Backbone.Servicios.Interfaces.Catalogos
{
    using System.Threading.Tasks;
    using DNP.Backbone.Dominio.Dto.Catalogos;
    using DNP.Backbone.Comunes.Enums;
    using System.Collections.Generic;

    public interface ICatalogoServicio
    {
        Task<List<CatalogoDto>> consultarCatalogo(string usuarioDnp, CatalogoEnum catalogo);
        Task<string> ObtenerTablasBasicas(string jsonCondicion, string Tabla, string usuarioDnp);
    }

    
}
