using DNP.Backbone.Comunes.Dto;
using DNP.Backbone.Dominio.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.Backbone.Servicios.Interfaces.Catalogo
{
    public interface ICatalogoServicio
    {
        Task<List<CatalogoDto>> ObtenerCatalogo(string catalogo, string tokenAutorizacion, string uriMetodo);
    }
}
