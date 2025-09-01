namespace DNP.ServiciosNegocio.Test.Mock
{
    using System.Collections.Generic;
    using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
    using Dominio.Dto.Catalogos;
    using Persistencia.Interfaces.Catalogos;

    public class CatalogoPersistenciaMock : ICatalogoPersistencia
    {
        public List<CatalogoDto> ObtenerEntidades() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarEtapas() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarClasificacionesRecursos() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarTiposRecursos() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarAlternativas() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarProductos() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarProgramas() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ObtenerTiposEntidades() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ObtenerTodosTiposEntidades() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarSectores() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarSectoresEntity() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarRegiones() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarMunicipios() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarDepartamentos() { return new List<CatalogoDto>(); }
        public List<DepartamentoCatalogoDto> ConsultarDepartamentosRegion() { return new List<DepartamentoCatalogoDto>(); }

        public List<AgrupacionCodeDto> ConsultarAgrupacionesCompleta() { return new List<AgrupacionCodeDto>(); }

        public List<CatalogoDto> ConsultarResguardos() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarTiposAgrupaciones() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarAgrupaciones() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarPoliticas(int? tipoAgrupacion) { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarIndicadoresPoliticas() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarTiposCofinanciaciones() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarEntregables() { return new List<CatalogoDto>(); }
        public List<CatalogoDto> ConsultarGruposRecursos() { return new List<CatalogoDto>(); }
        public CatalogoDto ConsultarDepartamentosPorIdRegion(int idCatalogo)
        {
            if ((idCatalogo == 1) || (idCatalogo == -1))
                return new RegionCatalogoDto()
                {
                    Id = 1,
                    Name = "Región 1",
                    CatalogosRelacionados = new List<CatalogoDto>()
                                               {
                                                   new DepartamentoCatalogoDto()
                                                   {
                                                       Id = 1,
                                                       Name = "Departamento 1"

                                                   },
                                                   new DepartamentoCatalogoDto()
                                                   {
                                                       Id = 2,
                                                       Name = "Departamento 2"
                                                   }
                                               }
                };
            return null;
        }
        public CatalogoDto ConsultarMunicipioPorIdDepartamento(int idCatalogo)
        {
            if ((idCatalogo == 1) || (idCatalogo == -1))
                return new DepartamentoCatalogoDto()
                {
                    Id = 1,
                    Name = "Departamento 1",
                    CatalogosRelacionados = new List<CatalogoDto>()
                                               {
                                                   new MunicipioCatalogoDto()
                                                   {
                                                       Id = 1,
                                                       Name = "Municipio 1"

                                                   },
                                                   new MunicipioCatalogoDto()
                                                   {
                                                       Id = 2,
                                                       Name = "Municipion 2"
                                                   }
                                               }
                };

            return null;
        }
        public CatalogoDto ConsultarResguardosPorIdMunicipio(int idCatalogo)
        {
            if ((idCatalogo == 1) || (idCatalogo == -1))
                return new MunicipioCatalogoDto()
                {
                    Id = 1,
                    Name = "Municipio 1",
                    CatalogosRelacionados = new List<CatalogoDto>()
                                               {
                                                   new ResguardoCatalogoDto()
                                                   {
                                                       Id = 1,
                                                       Name = "Resguardo 1"

                                                   },
                                                   new ResguardoCatalogoDto()
                                                   {
                                                       Id = 2,
                                                       Name = "Resguardo 2"
                                                   }
                                               }
                };

            return null;
        }

        public CatalogoDto ConsultarAgrupacionesPorIdMunicipio(int idCatalogo)
        {
            if ((idCatalogo == 1) || (idCatalogo == -1))
                return new MunicipioCatalogoDto()
                {
                    Id = 1,
                    Name = "Municipio 1",
                    CatalogosRelacionados = new List<CatalogoDto>()
                                               {
                                                   new ResguardoCatalogoDto()
                                                   {
                                                       Id = 1,
                                                       Name = "Agrupacion 1"

                                                   },
                                                   new ResguardoCatalogoDto()
                                                   {
                                                       Id = 2,
                                                       Name = "Agrupacion 2"
                                                   }
                                               }
                };

            return null;
        }

        public CatalogoDto ConsultarEntidadPorTipoEntidadId(int idCatalogo)
        {
            if ((idCatalogo == 1) || (idCatalogo == -1))
                return new TiposEntidadesCatalogoDto()
                {
                    Id = 1,
                    Name = "TipoEntidad 1",
                    CatalogosRelacionados = new List<CatalogoDto>()
                                               {
                                                   new EntidadCatalogoDto()
                                                   {
                                                       Id = 1,
                                                       Name = "Entidad 1"

                                                   },
                                                   new EntidadCatalogoDto()
                                                   {
                                                       Id = 2,
                                                       Name = "Entidad 2"
                                                   }
                                               }
                };

            return null;
        }

        public CatalogoDto ConsultarTiposRecursosPorTipoEntidadId(int idCatalogo)
        {
            if ((idCatalogo == 1) || (idCatalogo == -1))
                return new TiposEntidadesCatalogoDto()
                {
                    Id = 1,
                    Name = "TipoEntidad 1",
                    CatalogosRelacionados = new List<CatalogoDto>()
                                               {
                                                   new TipoRecursoCatalogoDto()
                                                   {
                                                       Id = 1,
                                                       Name = "Propios"

                                                   },
                                                   new TipoRecursoCatalogoDto()
                                                   {
                                                       Id = 2,
                                                       Name = "SGP"
                                                   }
                                               }
                };

            return null;
        }

        public List<CatalogoDto> ObtenerDireccionesTecnicas()
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ObtenerSubDireccionesTecnicas()
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ObtenerAnalistasSubDireccionesTecnicas()
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ConsultarFondos()
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ConsultarRubros()
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ConsultarTipoCofinanciador()
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ConsultarTiposRecursosEntidad(int entityTypeCatalogId)
        { return new List<CatalogoDto>(); }

        public List<CatalogoDto> ConsultarCategoriaByPadre(int idPadre)
        {
            throw new System.NotImplementedException();
        }

        public List<CatalogoDto> ConsultarEjecutorPorTipoEntidadId(int idTipoEntidad)
        {
            throw new System.NotImplementedException();
        }

        public string ObtenerTablasBasicas(string jsonCondicion, string Tabla)
        {


            return ("{ 'registros':[{ 'Id':1,'Name':'Atlántico'},{ 'Id':2,'Name':'Bolívar'},{ 'Id':4,'Name':'Córdoba'},{ 'Id':7,'Name':'Sucre'},{ 'Id':9,'Name':'Antioquia'},{ 'Id':10,'Name':'Caldas'},{ 'Id':11,'Name':'Cauca'},{ 'Id':13,'Name':'Nariño'},{ 'Id':15,'Name':'Risaralda'},{ 'Id':16,'Name':'Valle del Cauca'},{ 'Id':18,'Name':'Cundinamarca'}]})");

        }



        public List<CatalogoDto> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir)
        {
            throw new System.NotImplementedException();
        }
    }
}
