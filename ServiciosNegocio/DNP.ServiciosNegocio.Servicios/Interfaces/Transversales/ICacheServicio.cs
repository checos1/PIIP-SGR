using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Transversales
{
    using System.Collections.Generic;
    using Dominio.Dto.Catalogos;

    public interface ICacheServicio
    {
        #region PROYECTOS

        Task<ProyectoDto> ObtenerProyecto(string bpin, string tokenAutorizacion);
        Task<List<ProyectoEntidadDto>> ConsultarProyectosEntidad(int idEntidad, string tokenAutorizacion);
        Task<bool> GuardarProyectosEntidad(int idEntidad, List<ProyectoEntidadDto> proyectoEntidadDtos, string tokenAutorizacion, long ttl);

        #endregion

        #region CATALOGOS

        Task<List<CatalogoDto>> ObtenerCatalogo(string nombreCatalogo, string tokenAutorizacion);
        Task<CatalogoDto> ObtenerCatalogoPorId(string nombreCatalogo, int idCatalogo, string tokenAutorizacion);
        Task<bool> GuardarListaCatalogo(string nombreCatalogo, List<CatalogoDto> listaCatalogo, long ttl, string tokenAutorizacion);
        Task<CatalogoDto> ConsultarPorReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoRelacion, string tokenAutorizacion);
        Task<bool> GuardarReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia, CatalogoDto catalogo, long ttl, string tokenAutorizacion);

        #endregion
    }
}
