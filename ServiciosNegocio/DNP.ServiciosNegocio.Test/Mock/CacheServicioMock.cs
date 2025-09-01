namespace DNP.ServiciosNegocio.Test.Mock
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Comunes.Enum;
    using Dominio.Dto.Catalogos;
    using Dominio.Dto.Proyectos;
    using ServiciosNegocio.Servicios.Interfaces.Transversales;

    public class CacheServicioMock : ICacheServicio
    {
        public Task<ProyectoDto> ObtenerProyecto(string bpin, string tokenAutorizacion)
        {
            if (bpin.Equals("2017761220016"))
                return Task.FromResult(new ProyectoDto()
                {
                    Id = "427",
                    Bpin = "2017761220016",
                    Proyecto =
                                               "Apoyo a la Renovación Pedagógica y uso de las TIC's en la Educación en el Municipio de  Caicedonia",
                    Entidad = "CAICEDONIA",
                    VigenciaInicial = 2017,
                    VigenciaFinal = 2017,
                    Horizonte = "2017 - 2017",
                    ValorTotalProyecto = 16500000,
                    EstadoBanco = "",
                    Estado = "",
                });

            return Task.FromResult<ProyectoDto>(null);
        }
        
        public Task<List<CatalogoDto>> ObtenerCatalogo(string nombreCatalogo, string tokenAutorizacion)
        {
            if (nombreCatalogo == CatalogoEnum.Entidades.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new EntidadCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Entidad 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new TiposEntidadesCatalogoDto()
                                           {
                                               Id = 1,
                                               Name =
                                                   "Tipo Entidad 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Sectores.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new SectorCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Sector 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Regiones.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new RegionCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Region 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Departamentos.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new DepartamentoCatalogoDto()
                                           {
                                               Id = 1,
                                               Name =
                                                   "Departamento 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new MunicipioCatalogoDto()
                                           {
                                               Id = 1,
                                               Name =
                                                   "Municipio 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new ResguardoCatalogoDto()
                                           {
                                               Id = 1,
                                               Name =
                                                   "Resguardo 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Programas.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new ProgramaCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Programa 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Productos.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new ProductoCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Producto 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new AlternativaCatalogoDto()
                                           {
                                               Id = 1,
                                               Name =
                                                   "Alternativa 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new TipoRecursoCatalogoDto()
                                           {
                                               Id = 1,
                                               Name =
                                                   "Tipo Recurso 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new ClasificacionRecursoCatalogoDto()
                                           {
                                               Id = 1,
                                               Name =
                                                   "Clase Recurso 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Etapas.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new EtapaCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Etapa 1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new AgrupacionCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Agrupacion  1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new TipoAgrupacionCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Tipo Agrupacion  1"
                                           }
                                       });


            if (nombreCatalogo == CatalogoEnum.Politicas.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new PoliticaCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Tipo Cof  1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new TipoCofinanciadorCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Politica  1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new TipoCofinanciadorCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Politica  1"
                                           }
                                       });


            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new IndicadorPoliticaCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Ind Politica  1"
                                           }
                                       });

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString())
                return Task.FromResult(new List<CatalogoDto>()
                                       {
                                           new GrupoRecursoCatalogoDto()
                                           {
                                               Id = 1,
                                               Name = "Grupo Recurso 1"
                                           }
                                       });

            return Task.FromResult<List<CatalogoDto>>(null);
        }

        public Task<CatalogoDto> ObtenerCatalogoPorId(string nombreCatalogo, int idCatalogo, string tokenAutorizacion)
        {
            if (nombreCatalogo == CatalogoEnum.Entidades.ToString())
            {
                var listaEntidades =
                    new List<CatalogoDto>() { new EntidadCatalogoDto() { Id = 1, Name = "Entidad 1" } };

                return Task.FromResult(listaEntidades.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposEntidades.ToString())
            {
                var listaTiposEntidades =
                    new List<CatalogoDto>() { new TiposEntidadesCatalogoDto() { Id = 1, Name = "Tipo Entidad 1" } };

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
                var listaDepartamentos =
                    new List<CatalogoDto>() { new DepartamentoCatalogoDto() { Id = 1, Name = "Departamento 1" } };

                return Task.FromResult(listaDepartamentos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Municipios.ToString())
            {
                var listaMunicipios =
                    new List<CatalogoDto>() { new MunicipioCatalogoDto() { Id = 1, Name = "Municipio 1" } };

                return Task.FromResult(listaMunicipios.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Resguardos.ToString())
            {
                var listaResguardos =
                    new List<CatalogoDto>() { new ResguardoCatalogoDto() { Id = 1, Name = "Resguardo 1" } };

                return Task.FromResult(listaResguardos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Programas.ToString())
            {
                var listaProgramas =
                    new List<CatalogoDto>() { new ProgramaCatalogoDto() { Id = 1, Name = "Programa 1" } };

                return Task.FromResult(listaProgramas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Productos.ToString())
            {
                var listaProductos =
                    new List<CatalogoDto>() { new ProductoCatalogoDto() { Id = 1, Name = "Producto 1" } };

                return Task.FromResult(listaProductos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Alternativas.ToString())
            {
                var listaAlernativas =
                    new List<CatalogoDto>() { new AlternativaCatalogoDto() { Id = 1, Name = "Alternativa 1" } };

                return Task.FromResult(listaAlernativas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposRecursos.ToString())
            {
                var listaTiposRecursos =
                    new List<CatalogoDto>() { new TipoRecursoCatalogoDto() { Id = 1, Name = "Tipo Recurso 1" } };

                return Task.FromResult(listaTiposRecursos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString())
            {
                var listaTiposAgrupaciones =
                    new List<CatalogoDto>() { new TipoAgrupacionCatalogoDto () { Id = 1, Name = "TipoAgrupacion 1" } };

                return Task.FromResult(listaTiposAgrupaciones.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Politicas.ToString())
            {
                var listaPoliticas =
                    new List<CatalogoDto>() { new PoliticaCatalogoDto() { Id = 1, Name = "Politica 1" } };

                return Task.FromResult(listaPoliticas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString())
            {
                var listaTiposCofinanciaciones =
                    new List<CatalogoDto>() { new TipoCofinanciadorCatalogoDto() { Id = 1, Name = "Tipo Cof 1" } };

                return Task.FromResult(listaTiposCofinanciaciones.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Entregables.ToString())
            {
                var listaEntregables =
                    new List<CatalogoDto>() { new EntregableCatalogoDto() { Id = 1, Name = "Entregable 1" } };

                return Task.FromResult(listaEntregables.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString())
            {
                var listaIndicadoresPoliticas =
                    new List<CatalogoDto>() { new PoliticaCatalogoDto() { Id = 1, Name = "Ind Politica 1" } };

                return Task.FromResult(listaIndicadoresPoliticas.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.Agrupaciones.ToString())
            {
                var listaAgrupaciones =
                    new List<CatalogoDto>() { new AgrupacionCatalogoDto() { Id = 1, Name = "Agrupacion 1" } };

                return Task.FromResult(listaAgrupaciones.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.GruposRecursos.ToString())
            {
                var listaGruposRecursos =
                    new List<CatalogoDto>() { new GrupoRecursoCatalogoDto() { Id = 1, Name = "Grupo Recurso 1" } };

                return Task.FromResult(listaGruposRecursos.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString())
            {
                var listaClases =
                    new List<CatalogoDto>()
                    {
                        new ClasificacionRecursoCatalogoDto()
                        {
                            Id = 1,
                            Name = "Clase Recurso 1"
                        }
                    };

                return Task.FromResult(listaClases.FirstOrDefault(x => x.Id == idCatalogo));
            }

            if (nombreCatalogo != CatalogoEnum.Etapas.ToString()) return Task.FromResult<CatalogoDto>(null);

            var listaEtapas = new List<CatalogoDto>() { new EtapaCatalogoDto() { Id = 1, Name = "Etapa 1" } };

            return Task.FromResult(listaEtapas.FirstOrDefault(x => x.Id == idCatalogo));
        }
        public Task<bool> GuardarListaCatalogo(string nombreCatalogo, List<CatalogoDto> listaCatalogo, long ttl, string tokenAutorizacion)
        {
            var guardado = nombreCatalogo == CatalogoEnum.Entidades.ToString() ||
                           nombreCatalogo == CatalogoEnum.TiposEntidades.ToString() ||
                           nombreCatalogo == CatalogoEnum.Sectores.ToString() ||
                           nombreCatalogo == CatalogoEnum.Regiones.ToString() ||
                           nombreCatalogo == CatalogoEnum.Departamentos.ToString() ||
                           nombreCatalogo == CatalogoEnum.Municipios.ToString() ||
                           nombreCatalogo == CatalogoEnum.Resguardos.ToString() ||
                           nombreCatalogo == CatalogoEnum.Programas.ToString() ||
                           nombreCatalogo == CatalogoEnum.Productos.ToString() ||
                           nombreCatalogo == CatalogoEnum.Alternativas.ToString() ||
                           nombreCatalogo == CatalogoEnum.TiposRecursos.ToString() ||
                           nombreCatalogo == CatalogoEnum.ClasificacionesRecursos.ToString() ||
                           nombreCatalogo == CatalogoEnum.Etapas.ToString()||
                           nombreCatalogo == CatalogoEnum.TiposAgrupaciones.ToString() ||
                           nombreCatalogo == CatalogoEnum.Agrupaciones.ToString() ||
                           nombreCatalogo == CatalogoEnum.Politicas.ToString() ||
                           nombreCatalogo == CatalogoEnum.PoliticasNivel1.ToString() ||
                           nombreCatalogo == CatalogoEnum.PoliticasNivel2.ToString() ||
                           nombreCatalogo == CatalogoEnum.PoliticasNivel3.ToString() ||
                           nombreCatalogo == CatalogoEnum.IndicadoresPoliticas.ToString() ||
                           nombreCatalogo == CatalogoEnum.TiposCofinanciaciones.ToString() ||
                           nombreCatalogo == CatalogoEnum.Entregables.ToString() ||
                           nombreCatalogo == CatalogoEnum.GruposRecursos.ToString();

            return Task.FromResult(guardado);
        }
        public Task<CatalogoDto> ConsultarPorReferencia(string nombreCatalogo, int idCatalogo,
                                                        string nombreCatalogoReferencia, string tokenAutorizacion)
        {
            if ((nombreCatalogo != CatalogoEnum.Regiones.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Departamentos.ToString()) &&
                (nombreCatalogo != CatalogoEnum.Departamentos.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Municipios.ToString()) &&
                (nombreCatalogo != CatalogoEnum.Municipios.ToString() ||
                 nombreCatalogoReferencia != CatalogoEnum.Resguardos.ToString()))
                return Task.FromResult<CatalogoDto>(null);

            if (idCatalogo == 1)
                return Task.FromResult(new CatalogoDto()
                                       {
                                           Id = 1,
                                           Name = "Catalogo 1",
                                           CatalogosRelacionados =
                                               new List<CatalogoDto>()
                                               {
                                                   new CatalogoDto()
                                                   {
                                                       Id =
                                                           1,
                                                       Name
                                                           = "Catalogo 1"
                                                   },
                                                   new CatalogoDto()
                                                   {
                                                       Id =
                                                           2,
                                                       Name
                                                           = "Catalogo 2"
                                                   }
                                               }
                                       });

            return Task.FromResult<CatalogoDto>(null);
        }

        public Task<bool> GuardarReferencia(string nombreCatalogo, int idCatalogo, string nombreCatalogoReferencia,
                                            CatalogoDto catalogo, long ttl, string tokenAutorizacion)
        {
            var guardado =
                nombreCatalogo == CatalogoEnum.Regiones.ToString() &&
                nombreCatalogoReferencia == CatalogoEnum.Departamentos.ToString() ||
                nombreCatalogo == CatalogoEnum.Departamentos.ToString() &&
                nombreCatalogoReferencia == CatalogoEnum.Municipios.ToString() ||
                nombreCatalogo == CatalogoEnum.Municipios.ToString() &&
                nombreCatalogoReferencia == CatalogoEnum.Resguardos.ToString();

            return Task.FromResult(guardado);
        }

        public Task<List<ProyectoEntidadDto>> ConsultarProyectosEntidad(int idEntidad, string tokenAutorizacion)
        {
            return idEntidad.Equals(636) ? Task.FromResult(new List<ProyectoEntidadDto>()
                                                           {
                                                               new ProyectoEntidadDto()
                                                               {
                                                                   CodigoBpin = "1234",
                                                                   EntidadId = 636,
                                                                   EntidadNombre = "Entidad 636",
                                                                   ProyectoId = 1,
                                                                   ProyectoNombre = "Proyecto 1",
                                                                   SectorId = 1,
                                                                   SectorNombre = "Sector 1"
                                                               },
                                                               new ProyectoEntidadDto()
                                                               {
                                                                   CodigoBpin = "5678",
                                                                   EntidadId = 636,
                                                                   EntidadNombre = "Entidad 636",
                                                                   ProyectoId = 2,
                                                                   ProyectoNombre = "Proyecto 2",
                                                                   SectorId = 1,
                                                                   SectorNombre = "Sector 1"
                                                               },
                                                               new ProyectoEntidadDto()
                                                               {
                                                                   CodigoBpin = "9012",
                                                                   EntidadId = 636,
                                                                   EntidadNombre = "Entidad 636",
                                                                   ProyectoId = 3,
                                                                   ProyectoNombre = "Proyecto 3",
                                                                   SectorId = 1,
                                                                   SectorNombre = "Sector 1"
                                                               }
                                                           }) : Task.FromResult<List<ProyectoEntidadDto>>(null);
        }

        public Task<bool> GuardarProyectosEntidad(int idEntidad, List<ProyectoEntidadDto> proyectoEntidadDtos,
                                                  string tokenAutorizacion, long ttl)
        {
            return Task.FromResult(idEntidad.Equals(636)|| idEntidad.Equals(79));
        }
    
    }
}
