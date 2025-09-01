namespace DNP.Backbone.Servicios.Interfaces.Nivel
{
    using DNP.Backbone.Dominio.Dto.Nivel;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INivelServicios
    {
        Task<List<NivelDto>> ObtenerPorIdPadreIdNivelTipo(Guid? idPadre, string claveNivelTipo ,string idUsuarioDnp);
    }
}
