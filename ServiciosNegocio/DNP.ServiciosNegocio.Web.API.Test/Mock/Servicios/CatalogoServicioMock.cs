namespace DNP.ServiciosNegocio.Web.API.Test.Mock.Servicios
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes.Enum;
    using Dominio.Dto.Catalogos;
    using ServiciosNegocio.Servicios.Interfaces.Catalogos;

    internal class CatalogoServicioMock : ICatalogoServicio
    {
        public Task<List<CatalogoDto>> ObtenerCatalogo(string nombreCatalogo, string tokenAutorizacion)
        {
            if (nombreCatalogo == CatalogoEnum.Entidades.ToString()) return Task.FromResult(new List<CatalogoDto>() { new EntidadCatalogoDto() { Id = 1, Name = "Entidad 1" } });

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString()) return Task.FromResult(new List<CatalogoDto>() { new TiposEntidadesCatalogoDto() { Id = 1, Name = "Tipo Entidad 1" } });

            if (nombreCatalogo == CatalogoEnum.Sectores.ToString()) return Task.FromResult(new List<CatalogoDto>() { new SectorCatalogoDto() { Id = 1, Name = "Sector 1" } });

            if (nombreCatalogo == CatalogoEnum.Regiones.ToString()) return Task.FromResult(new List<CatalogoDto>() { new RegionCatalogoDto() { Id = 1, Name = "Region 1" } });

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString()) return Task.FromResult(new List<CatalogoDto>() { new DepartamentoCatalogoDto() { Id = 1, Name = "Departamento 1" } });

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString()) return Task.FromResult(new List<CatalogoDto>() { new MunicipioCatalogoDto() { Id = 1, Name = "Municipio 1" } });

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString()) return Task.FromResult(new List<CatalogoDto>() { new ResguardoCatalogoDto() { Id = 1, Name = "Resguardo 1" } });

            if (nombreCatalogo == CatalogoEnum.Programas.ToString()) return Task.FromResult(new List<CatalogoDto>() { new ProgramaCatalogoDto() { Id = 1, Name = "Programa 1" } });

            if (nombreCatalogo == CatalogoEnum.Productos.ToString()) return Task.FromResult(new List<CatalogoDto>() { new ProductoCatalogoDto() { Id = 1, Name = "Producto 1" } });

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString()) return Task.FromResult(new List<CatalogoDto>() { new AlternativaCatalogoDto() { Id = 1, Name = "Alternativa 1" } });

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString()) return Task.FromResult(new List<CatalogoDto>() { new TipoRecursoCatalogoDto() { Id = 1, Name = "Tipo Recurso 1" } });

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString()) return Task.FromResult(new List<CatalogoDto>() { new ClasificacionRecursoCatalogoDto() { Id = 1, Name = "Clase Recurso 1" } });

            if (nombreCatalogo == CatalogoEnum.Etapas.ToString()) return Task.FromResult(new List<CatalogoDto>() { new EtapaCatalogoDto() { Id = 1, Name = "Etapa 1" } });

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString()) return Task.FromResult(new List<CatalogoDto>() { new TipoAgrupacionCatalogoDto() { Id = 1, Name = "TipoAgrupacion 1" } });

            if (nombreCatalogo == CatalogoEnum.Politicas.ToString()) return Task.FromResult(new List<CatalogoDto>() { new PoliticaCatalogoDto() { Id = 1, Name = "Politica 1" } });

            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString()) return Task.FromResult(new List<CatalogoDto>() { new IndicadorPoliticaCatalogoDto() { Id = 1, Name = "Ind Politica 1" } });

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString()) return Task.FromResult(new List<CatalogoDto>() { new TipoCofinanciadorCatalogoDto() { Id = 1, Name = "Tipo Cof 1" } });

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString()) return Task.FromResult(new List<CatalogoDto>() { new EntregableCatalogoDto() { Id = 1, Name = "Entregable 1" } });

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString()) return Task.FromResult(new List<CatalogoDto>() { new AgrupacionCatalogoDto() { Id = 1, Name = "Agrupacion 1" } });

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString()) return Task.FromResult(new List<CatalogoDto>() { new GrupoRecursoCatalogoDto() { Id = 1, Name = "Grupo Recurso 1" } });

            return Task.FromResult<List<CatalogoDto>>(null);
        }
        public Task<CatalogoDto> ObtenerCatalogosPorReferencia(string nombreCatalogo, int idCatalogo,
                                                        string nombreCatalogoReferencia, string tokenAutorizacion)
        {
            if ((nombreCatalogo == CatalogoEnum.Regiones.ToString() &&
                 nombreCatalogoReferencia == CatalogoEnum.Departamentos.ToString()) ||
                (nombreCatalogo == CatalogoEnum.Departamentos.ToString() &&
                 nombreCatalogoReferencia == CatalogoEnum.Municipios.ToString()) ||
                (nombreCatalogo == CatalogoEnum.Municipios.ToString() &&
                 nombreCatalogoReferencia == CatalogoEnum.Resguardos.ToString()))
            {
                return Task.FromResult(new CatalogoDto()
                {
                    Id = 1,
                    Name = "Catalogo 1",
                    CatalogosRelacionados = new List<CatalogoDto>()
                                                                   {
                                                                       new CatalogoDto()
                                                                       {
                                                                           Id = 1,
                                                                           Name = "Catalogo 1"

                                                                       },
                                                                       new CatalogoDto()
                                                                       {
                                                                           Id = 2,
                                                                           Name = "Catalogo 2"
                                                                       }
                                                                   }
                });
            }

            return Task.FromResult<CatalogoDto>(null);
        }

        public Task<CatalogoDto> ObtenerCatalogoPorId(string nombreCatalogo, int idCatalogo, string tokenAutorizacion)
        {
            if (nombreCatalogo == CatalogoEnum.Entidades.ToString())
            {
                var listaEntidades = new List<CatalogoDto>() { new EntidadCatalogoDto() { Id = 1, Name = "Entidad 1" } };

                return Task.FromResult(listaEntidades.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString())
            {
                var listaTiposEntidades = new List<CatalogoDto>() { new TiposEntidadesCatalogoDto() { Id = 1, Name = "Tipo Entidad 1" } };
                return Task.FromResult(listaTiposEntidades.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Sectores.ToString())
            {
                var listaSectores = new List<CatalogoDto>() { new SectorCatalogoDto() { Id = 1, Name = "Sector 1" } };
                return Task.FromResult(listaSectores.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Regiones.ToString())
            {
                var listaRegiones = new List<CatalogoDto>() { new RegionCatalogoDto() { Id = 1, Name = "Region 1" } };
                return Task.FromResult(listaRegiones.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString())
            {
                var listaDepartamentos = new List<CatalogoDto>() { new DepartamentoCatalogoDto() { Id = 1, Name = "Departamento 1" } };
                return Task.FromResult(listaDepartamentos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString())
            {
                var listaMunicipios = new List<CatalogoDto>() { new MunicipioCatalogoDto() { Id = 1, Name = "Municipio 1" } };
                return Task.FromResult(listaMunicipios.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString())
            {
                var listaResguardos = new List<CatalogoDto>() { new ResguardoCatalogoDto() { Id = 1, Name = "Resguardo 1" } };
                return Task.FromResult(listaResguardos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Programas.ToString())
            {
                var listaProgramas = new List<CatalogoDto>() { new ProgramaCatalogoDto() { Id = 1, Name = "Programa 1" } };
                return Task.FromResult(listaProgramas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Productos.ToString())
            {
                var listaProductos = new List<CatalogoDto>() { new ProductoCatalogoDto() { Id = 1, Name = "Producto 1" } };
                return Task.FromResult(listaProductos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString())
            {
                var listaAlernativas = new List<CatalogoDto>() { new AlternativaCatalogoDto() { Id = 1, Name = "Alternativa 1" } };
                return Task.FromResult(listaAlernativas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString())
            {
                var listaTiposRecursos = new List<CatalogoDto>() { new TipoRecursoCatalogoDto() { Id = 1, Name = "Tipo Recurso 1" } };
                return Task.FromResult(listaTiposRecursos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString())
            {
                var listaClases = new List<CatalogoDto>() { new ClasificacionRecursoCatalogoDto() { Id = 1, Name = "Clase Recurso 1" } };
                return Task.FromResult(listaClases.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString())
            {
                var listaTiposAgrupaciones = new List<CatalogoDto>() { new AgrupacionCatalogoDto() { Id = 1, Name = "TipoAgrupacion 1" } };
                return Task.FromResult(listaTiposAgrupaciones.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Politicas.ToString())
            {
                var listaPoliticas = new List<CatalogoDto>() { new PoliticaCatalogoDto() { Id = 1, Name = "Politica 1" } };
                return Task.FromResult(listaPoliticas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString())
            {
                var listaTiposCofinanciaciones = new List<CatalogoDto>() { new TipoCofinanciadorCatalogoDto() { Id = 1, Name = "Tipo Cof 1" } };
                return Task.FromResult(listaTiposCofinanciaciones.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString())
            {
                var listaEntregables = new List<CatalogoDto>() { new EntregableCatalogoDto() { Id = 1, Name = "Entregable 1" } };
                return Task.FromResult(listaEntregables.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString())
            {
                var listaIndicadoresPoliticas = new List<CatalogoDto>() { new IndicadorPoliticaCatalogoDto() { Id = 1, Name = "Ind politica 1" } };
                return Task.FromResult(listaIndicadoresPoliticas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString())
            {
                var listaAgrupaciones = new List<CatalogoDto>() { new AgrupacionCatalogoDto() { Id = 1, Name = "Agrupacion 1" } };
                return Task.FromResult(listaAgrupaciones.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString())
            {
                var listaGrupos = new List<CatalogoDto>() { new GrupoRecursoCatalogoDto() { Id = 1, Name = "Grupo Recurso 1" } };
                return Task.FromResult(listaGrupos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo != CatalogoEnum.Etapas.ToString()) return null;

            var listaEtapas = new List<CatalogoDto>() { new EtapaCatalogoDto() { Id = 1, Name = "Etapa 1" } };
            return Task.FromResult(listaEtapas.FirstOrDefault(x => x.Id == idCatalogo));
        }

        public List<CatalogoDto> ObtenerListaCatalogo(string nombreCatalogo)
        {
            var listaAlernativas = new List<CatalogoDto>() { new AlternativaCatalogoDto() { Id = 1, Name = nombreCatalogo } };
            return listaAlernativas.Where(x => x.Name == nombreCatalogo).ToList();
        }

        public CatalogoDto ObtenerCatalogoReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia)
        {
            var listaAlernativas = new List<CatalogoDto>() { new AlternativaCatalogoDto() { Id = 1, Name = nombreCatalogo, CatalogosRelacionados = new List<CatalogoDto>() { new CatalogoDto() { Name = nombreCatalogoReferencia } } } };
            return listaAlernativas.FirstOrDefault(x => x.Id == idCatalogo && x.Name == nombreCatalogo);
        }

		public List<DepartamentoCatalogoDto> ConsultarDepartamentosRegion()
		{
			throw new System.NotImplementedException();
		}

		public List<AgrupacionCodeDto> ConsultarAgrupacionesCompleta()
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
