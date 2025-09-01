namespace DNP.Backbone.Servicios.Interfaces.Cache
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dominio.Dto.AutorizacionNegocio;

    public interface ICacheEntidadesNegocioServicios
    {
        Task<List<EntidadNegocioDto>> ConsultarEntidadesPorTipoEntidad(string usuarioDnp, string tipoEntidad);
        Task<List<RolNegocioDto>> ConsultarRoles(string usuarioDnp);
        Task<List<SectorNegocioDto>> ConsultarSectores(string usuarioDnp);
    }
}
