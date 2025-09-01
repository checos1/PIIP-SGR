using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Interfaces.Catalogos
{
    using Dominio.Dto.Catalogos;

    public interface ICatalogoPersistencia
    {
        List<CatalogoDto> ObtenerEntidades();
        List<CatalogoDto> ObtenerDireccionesTecnicas();
        List<CatalogoDto> ObtenerSubDireccionesTecnicas();
        List<CatalogoDto> ObtenerAnalistasSubDireccionesTecnicas();
        List<CatalogoDto> ConsultarEtapas();
        List<CatalogoDto> ConsultarClasificacionesRecursos();
        List<CatalogoDto> ConsultarTiposRecursos();
        List<CatalogoDto> ConsultarAlternativas();
        List<CatalogoDto> ConsultarProductos();
        List<CatalogoDto> ConsultarProgramas();
        List<CatalogoDto> ObtenerTiposEntidades();
        List<CatalogoDto> ObtenerTodosTiposEntidades();
        List<CatalogoDto> ConsultarSectores();
        List<CatalogoDto> ConsultarSectoresEntity();
        
        List<CatalogoDto> ConsultarRegiones();
        List<CatalogoDto> ConsultarMunicipios();
        List<DepartamentoCatalogoDto> ConsultarDepartamentosRegion();
        List<CatalogoDto> ConsultarDepartamentos();
        List<CatalogoDto> ConsultarResguardos();
        List<CatalogoDto> ConsultarGruposRecursos();
        List<CatalogoDto> ConsultarAgrupaciones();
        List<CatalogoDto> ConsultarFondos();
        List<CatalogoDto> ConsultarRubros();
        List<CatalogoDto> ConsultarTipoCofinanciador();
        List<CatalogoDto> ConsultarTiposAgrupaciones();
        List<CatalogoDto> ConsultarPoliticas(int? tipoAgrupacion);
        List<CatalogoDto> ConsultarCategoriaByPadre(int idPadre);
        List<CatalogoDto> ConsultarIndicadoresPoliticas();
        List<CatalogoDto> ConsultarTiposCofinanciaciones();
        List<CatalogoDto> ConsultarEntregables();
        CatalogoDto ConsultarDepartamentosPorIdRegion(int idCatalogo);
        CatalogoDto ConsultarMunicipioPorIdDepartamento(int idCatalogo);
        CatalogoDto ConsultarResguardosPorIdMunicipio(int idCatalogo);
        CatalogoDto ConsultarEntidadPorTipoEntidadId(int idCatalogo);
        CatalogoDto ConsultarTiposRecursosPorTipoEntidadId(int idCatalogo);
        CatalogoDto ConsultarAgrupacionesPorIdMunicipio(int idCatalogo);
        List<AgrupacionCodeDto> ConsultarAgrupacionesCompleta();

        List<CatalogoDto> ConsultarTiposRecursosEntidad(int entityTypeCatalogId);
        List<CatalogoDto> ConsultarEjecutorPorTipoEntidadId(int idTipoEntidad);
        string ObtenerTablasBasicas(string jsonCondicion, string Tabla);

        List<CatalogoDto> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir);
    }
}
