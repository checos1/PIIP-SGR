using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Servicios.Interfaces.Catalogos
{
    using System.Threading.Tasks;
    using Dominio.Dto.Catalogos;

    public interface ICatalogoServicio
    {
        Task<List<CatalogoDto>> ObtenerCatalogo(string nombreCatalogo, string tokenAutorizacion);
        Task<CatalogoDto> ObtenerCatalogosPorReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia, string tokenAutorizacion);
        Task<CatalogoDto> ObtenerCatalogoPorId(string nombreCatalogo, int idCatalogo, string tokenAutorizacion);
        List<CatalogoDto> ObtenerListaCatalogo(string nombreCatalogo);
        CatalogoDto ObtenerCatalogoReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia);
        List<DepartamentoCatalogoDto> ConsultarDepartamentosRegion();
        List<AgrupacionCodeDto> ConsultarAgrupacionesCompleta();
        List<CatalogoDto> ConsultarTiposRecursosEntidad(int entityTypeCatalogId);
        List<CatalogoDto> ConsultarCategoriaByPadre(int idPadre);
        List<CatalogoDto> ConsultarEjecutorPorTipoEntidadId(int idTipoEntidad);
        string ObtenerTablasBasicas(string jsonCondicion, string Tabla);
        List<CatalogoDto> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir);
    }
}
