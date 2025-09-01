namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Catalogos
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Dominio.Dto.Catalogos;
    using Interfaces;
    using Interfaces.Catalogos;

    public class CatalogoPersistencia : Persistencia, ICatalogoPersistencia
    {
        public CatalogoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }
        public List<CatalogoDto> ObtenerEntidades()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.EntityTypeCatalogOption
                                               where e.EntityTypeId != 1 && e.EntityTypeId != null
                                               select new EntidadCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   EntityTypeId = e.EntityTypeId,
                                                   ParentId = e.ParentId,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).Union
                                               (from e1 in Contexto.EntityTypeCatalogOption
                                                join e2 in Contexto.EntityTypeCatalogOption on e1.ParentId equals e2.Id
                                                where e1.ParentId != null && e2.EntityTypeId == 1
                                                select new EntidadCatalogoDto()
                                                {
                                                    Id = e1.Id,
                                                    Name = e1.Name,
                                                    EntityTypeId = e2.EntityTypeId,
                                                    ParentId = e1.ParentId,
                                                    Code = e1.Code,
                                                    IsActive = e1.IsActive
                                                }).
                                               ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }
        public List<CatalogoDto> ObtenerDireccionesTecnicas()
        {
            try
            {
                var lista = new List<CatalogoDto>((from et in Contexto.EntityTypeCatalogOption
                                                   join c in Contexto.SystemConfigurations on et.Id.ToString() equals c.Value
                                                   where et.EntityTypeId != 1  //&& et.ParentId == 105  
                                                   && c.Key == "SubdirecciondeCredito"
                                                   //se seleciona solo la direccion tecnica DIFP - Dirección de Inversiones y Finanzas Públicas
                                                   select new EntidadCatalogoDTDto()
                                                   {
                                                       EntityTypeCatalogOptionId = et.Id,
                                                       Name = et.Name,
                                                       EntityTypeId = et.EntityTypeId,
                                                       ParentId = et.ParentId,
                                                       Code = et.Code,
                                                       IsActive = et.IsActive
                                                   }).
                                               ToList().
                                                         OrderBy(x => x.Name));

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<CatalogoDto> ObtenerSubDireccionesTecnicas()
        {
            var lista = new List<CatalogoDto>((from s in Contexto.EntityTypeCatalogOption
                                               join c in Contexto.SystemConfigurations on s.Id.ToString() equals c.Value
                                               join d in Contexto.EntityTypeCatalogOption on s.ParentId equals d.Id
                                               where s.EntityTypeId != 1  //&& et.ParentId == 105  
                                                && c.Key == "SubdirecciondeCredito"
                                               select new EntidadCatalogoSTDto()
                                               {
                                                   EntityTypeCatalogOptionId = s.Id,
                                                   Name = s.Name,
                                                   DireccionTecnicaId = d.Id,
                                                   EntityTypeId = s.EntityTypeId,
                                                   ParentId = s.ParentId,
                                                   Code = s.Code,
                                                   IsActive = s.IsActive
                                               }).
                                               ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public List<CatalogoDto> ObtenerAnalistasSubDireccionesTecnicas()
        {
            var lista = new List<CatalogoDto>();
            return lista;
        }

        public List<CatalogoDto> ConsultarEtapas()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Stage
                                               select new EtapaCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Description
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }
        public List<CatalogoDto> ConsultarClasificacionesRecursos()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.ClassificationType
                                               select new ClasificacionRecursoCatalogoDto()
                                               {
                                                   Id = (int)e.Id,
                                                   Name = e.Description,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   IsActive = e.IsActive
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }
        public List<CatalogoDto> ConsultarTiposRecursos()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.ResourceType
                                               select new TipoRecursoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Description,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   EntityTypeId = e.EntityType.Id,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive,
                                                   IdEscritorio = e.IdEscritorio,
                                                   IdClassificationType = e.ClassificationType.Id,
                                                   ResourceGroupId = e.ResourceGroup.Id
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public List<CatalogoDto> ConsultarTiposRecursosEntidad(int entityTypeCatalogId)
        {
            var lista = new List<CatalogoDto>((from e in Contexto.ResourceType
                                               join re in Contexto.ResourceTypeByEntity on e.Id equals re.ResourceType.Id
                                               where re.EntityTypeCatalogOption.Id == entityTypeCatalogId
                                               select new TipoRecursoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Description,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   EntityTypeId = e.EntityType.Id,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive,
                                                   IdEscritorio = e.IdEscritorio,
                                                   IdClassificationType = e.ClassificationType.Id,
                                                   ResourceGroupId = e.ResourceGroup.Id
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public List<CatalogoDto> ConsultarAlternativas()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Alternative
                                               select new AlternativaCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                               }).Take(12000).Distinct().ToList().OrderBy(x => x.Name));

            return lista;
        }
        public List<CatalogoDto> ConsultarProductos()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.ProductCatalog.AsNoTracking()
                                               select new ProductoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   MeasureTypeId = e.MeasureTypeId,
                                                   ProgramId = e.ProgramId,
                                                   Description = e.Description,
                                                   Codigo = e.codigo,
                                                   IsTerritorial = e.IsTerritorial,
                                                   IsActive = e.IsEnabled

                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }
        public List<CatalogoDto> ConsultarProgramas()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Program
                                               select new ProgramaCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   IdSector = e.IdSector,
                                                   BankCode = e.BankCode,
                                                   IsTerritorial = e.IsTerritorial,
                                                   IsActive = e.IsActive
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }
        public List<CatalogoDto> ObtenerTiposEntidades()
        {
            var lista = new List<CatalogoDto>((from rg in Contexto.ResourceGroupEntityType 
                                               join  e in Contexto.EntityType  on rg.EntityTypeId equals e.Id
                                               select new TiposEntidadesCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.EntityType1,
                                                   IsBankEntity = e.IsBankEntity,
                                                   ResourceGroupId = rg.ResourceGroupId,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).Distinct().ToList().
                                                         OrderBy(x => x.Name));

           
            return lista;
           
        }
        public List<CatalogoDto> ObtenerTodosTiposEntidades()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.EntityType
                                               select new TiposEntidadesCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.EntityType1,
                                                   IsBankEntity = e.IsBankEntity,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).Distinct().ToList().
                                                         OrderBy(x => x.Name));
            return lista;

        }

        public CatalogoDto ConsultarEntidadPorTipoEntidadId(int idTipoEntidad)
        {
            var tipoEntidad = new List<CatalogoDto>(from e in Contexto.EntityType
                                                    select new TiposEntidadesCatalogoDto()
                                                    {
                                                        Id = e.Id,
                                                        Name = e.EntityType1,
                                                        IsBankEntity = e.IsBankEntity,
                                                        Code = e.Code,
                                                        IsActive = e.IsActive
                                                    }).FirstOrDefault();

            if (tipoEntidad == null) return null;

            var entidades = (from e in Contexto.EntityTypeCatalogOption
                             where e.EntityTypeId == idTipoEntidad
                             select new EntidadCatalogoDto()
                             {
                                 Id = e.Id,
                                 Name = e.Name,
                                 EntityTypeId = e.EntityTypeId,
                                 ParentId = e.ParentId,
                                 Code = e.Code,
                                 IsActive = e.IsActive
                             }).ToList().
                                        OrderBy(x => x.Name);

            tipoEntidad.CatalogosRelacionados = new List<CatalogoDto>(entidades);
            return tipoEntidad;
        }

        public List<CatalogoDto> ConsultarEjecutorPorTipoEntidadId(int idTipoEntidad)
        {
            var tipoEntidad = new List<CatalogoDto>(from e in Contexto.Ejecutores
                                                    where e.EntityTypeId == idTipoEntidad
                                                    select new EjecutorCatalogoDto()
                                                    {
                                                        IdEjecutor = e.Id,
                                                        Name = e.NombreEjecutor
    
                                                    }).ToList();

            return tipoEntidad;
        }

        public List<CatalogoDto> ConsultarSectores()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Sector
                                               select new SectorCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Description,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   IsTerritorial = e.IsTerritorial,
                                                   TerritorialName = e.TerritorialName,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public List<CatalogoDto> ConsultarSectoresEntity()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.EntityTypeCatalogOption
                                               where e.EntityTypeId == 1 && e.ParentId == null && e.IsActive == null
                                               select new EntidadCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   EntityTypeId = e.EntityTypeId,
                                                   ParentId = e.ParentId,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).
                                              ToList().
                                                        OrderBy(x => x.Name));

            return lista;
        }

        public List<CatalogoDto> ConsultarRegiones()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Region
                                               select new RegionCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public List<CatalogoDto> ConsultarMunicipios()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Municipality
                                               join d in Contexto.Department on e.DepartmentId equals d.Id
                                               select new MunicipioCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name+" ("+ d.Name+")",
                                                   DepartmentId = e.DepartmentId,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).ToList().
                                                         OrderBy(x => x.Name)) ;

            return lista;
        }

        public List<CatalogoDto> ConsultarDepartamentos()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Department
                                               select new DepartamentoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   RegionId = e.RegionId,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public List<DepartamentoCatalogoDto> ConsultarDepartamentosRegion()
        {
            var lista = new List<DepartamentoCatalogoDto>((from e in Contexto.Department
                                                           select new DepartamentoCatalogoDto()
                                                           {
                                                               Id = e.Id,
                                                               Name = e.Name,
                                                               RegionId = e.RegionId,
                                                               Created = e.Created,
                                                               Modified = e.Modified,
                                                               CreatedBy = e.CreatedBy,
                                                               ModifiedBy = e.ModifiedBy,
                                                               Code = e.Code,
                                                               IsActive = e.IsActive
                                                           }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public List<CatalogoDto> ConsultarResguardos()
        {
            var lista = new List<CatalogoDto>((from e in Contexto.Shelter
                                               select new ResguardoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   MunicipalityId = e.MunicipalityId,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

        public CatalogoDto ConsultarDepartamentosPorIdRegion(int idCatalogo)
        {
            var region = (from e in Contexto.Region
                          where e.Id == idCatalogo
                          select new RegionCatalogoDto()
                          {
                              Id = e.Id,
                              Name = e.Name,
                              Created = e.Created,
                              Modified = e.Modified,
                              CreatedBy = e.CreatedBy,
                              ModifiedBy = e.ModifiedBy,
                              Code = e.Code,
                              IsActive = e.IsActive
                          }).FirstOrDefault();

            if (region == null) return null;

            var departamentos = (from s in Contexto.Department
                                 where s.RegionId == idCatalogo
                                 select new DepartamentoCatalogoDto()
                                 {
                                     Id = s.Id,
                                     Name = s.Name,
                                     RegionId = s.RegionId,
                                     Created = s.Created,
                                     Modified = s.Modified,
                                     CreatedBy = s.CreatedBy,
                                     ModifiedBy = s.ModifiedBy,
                                     Code = s.Code,
                                     IsActive = s.IsActive
                                 }).ToList().
                                           OrderBy(x => x.Name);

            region.CatalogosRelacionados = new List<CatalogoDto>(departamentos);

            return region;
        }

        public CatalogoDto ConsultarMunicipioPorIdDepartamento(int idCatalogo)
        {

            var departamento = (from e in Contexto.Department
                                where e.Id == idCatalogo
                                select new DepartamentoCatalogoDto()
                                {
                                    Id = e.Id,
                                    Name = e.Name,
                                    RegionId = e.RegionId,
                                    Created = e.Created,
                                    Modified = e.Modified,
                                    CreatedBy = e.CreatedBy,
                                    ModifiedBy = e.ModifiedBy,
                                    Code = e.Code,
                                    IsActive = e.IsActive
                                }).FirstOrDefault();

            if (departamento == null) return null;

            var municipios = (from s in Contexto.Municipality
                              where s.DepartmentId == departamento.Id
                              select new MunicipioCatalogoDto()
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  DepartmentId = s.DepartmentId,
                                  Created = s.Created,
                                  Modified = s.Modified,
                                  CreatedBy = s.CreatedBy,
                                  ModifiedBy = s.ModifiedBy,
                                  Code = s.Code,
                                  IsActive = s.IsActive
                              }).ToList().
                                        OrderBy(x => x.Name);


            departamento.CatalogosRelacionados = new List<CatalogoDto>(municipios);

            return departamento;
        }

        public CatalogoDto ConsultarResguardosPorIdMunicipio(int idCatalogo)
        {
            var municipio = (from e in Contexto.Municipality
                             where e.Id == idCatalogo
                             select new MunicipioCatalogoDto()
                             {
                                 Id = e.Id,
                                 Name = e.Name,
                                 DepartmentId = e.DepartmentId,
                                 Created = e.Created,
                                 Modified = e.Modified,
                                 CreatedBy = e.CreatedBy,
                                 ModifiedBy = e.ModifiedBy,
                                 Code = e.Code,
                                 IsActive = e.IsActive
                             }).FirstOrDefault();

            if (municipio == null) return null;

            var resguardos = (from s in Contexto.Shelter
                              where s.MunicipalityId == idCatalogo
                              select new ResguardoCatalogoDto()
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  MunicipalityId = s.MunicipalityId,
                                  Created = s.Created,
                                  Modified = s.Modified,
                                  CreatedBy = s.CreatedBy,
                                  ModifiedBy = s.ModifiedBy,
                                  Code = s.Code,
                                  IsActive = s.IsActive
                              }).ToList().
                                        OrderBy(x => x.Name);


            municipio.CatalogosRelacionados = new List<CatalogoDto>(resguardos);

            return municipio;
        }

        public CatalogoDto ConsultarTiposRecursosPorTipoEntidadId(int idCatalogo)
        {

            var tipoEntidad = new List<CatalogoDto>(from e in Contexto.EntityType
                                                    select new TiposEntidadesCatalogoDto()
                                                    {
                                                        Id = e.Id,
                                                        Name = e.EntityType1,
                                                        IsBankEntity = e.IsBankEntity,
                                                        Code = e.Code,
                                                        IsActive = e.IsActive
                                                    }).FirstOrDefault();

            if (tipoEntidad == null) return null;

            var tipoRecursos = (from e in Contexto.ResourceType
                                where e.EntityType.Id == idCatalogo
                                select new TipoRecursoCatalogoDto()
                                {
                                    Id = e.Id,
                                    Name = e.Description,
                                    Created = e.Created,
                                    Modified = e.Modified,
                                    EntityTypeId = e.EntityType.Id,
                                    CreatedBy = e.CreatedBy,
                                    ModifiedBy = e.ModifiedBy,
                                    Code = e.Code,
                                    IsActive = e.IsActive,
                                    IdEscritorio = e.IdEscritorio,
                                    IdClassificationType = e.ClassificationType.Id,
                                    ResourceGroupId = e.ResourceGroup.Id
                                }).ToList().
                                                         OrderBy(x => x.Name);

            tipoEntidad.CatalogosRelacionados = new List<CatalogoDto>(tipoRecursos);

            return tipoEntidad;
        }

        public CatalogoDto ConsultarAgrupacionesPorIdMunicipio(int idCatalogo)
        {

            var municipio = (from e in Contexto.Municipality
                             where e.Id == idCatalogo
                             select new MunicipioCatalogoDto()
                             {
                                 Id = e.Id,
                                 Name = e.Name,
                                 DepartmentId = e.DepartmentId,
                                 Created = e.Created,
                                 Modified = e.Modified,
                                 CreatedBy = e.CreatedBy,
                                 ModifiedBy = e.ModifiedBy,
                                 Code = e.Code,
                                 IsActive = e.IsActive
                             }).FirstOrDefault();

            if (municipio == null) return null;

            var agrupaciones = (from s in Contexto.Agrupaciones
                                where s.Municipality.Id == idCatalogo
                                select new AgrupacionCatalogoDto()
                                {
                                    Id = s.Id,
                                    Name = s.Descripcion,
                                    MunicipalityId = s.Municipality.Id,
                                    Created = s.FechaCreacion,
                                    Modified = s.FechaModificacion,
                                    CreatedBy = s.CreadoPor,
                                    ModifiedBy = s.ModificadoPor,
                                    Code = s.Codigo,
                                    IsActive = s.EsActivo,
                                    TipoAgrupacionId = s.TipoAgrupacion.Id
                                }).ToList().
                                        OrderBy(x => x.Name);


            municipio.CatalogosRelacionados = new List<CatalogoDto>(agrupaciones);

            return municipio;
        }

        public List<CatalogoDto> ConsultarGruposRecursos()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.ResourceGroup
                                               select new GrupoRecursoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Description
                                               }).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarAgrupaciones()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.Agrupaciones
                                               select new AgrupacionCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Descripcion,
                                                   MunicipalityId = e.Municipality.Id,
                                                   Created = e.FechaCreacion,
                                                   Modified = e.FechaModificacion,
                                                   CreatedBy = e.CreadoPor,
                                                   ModifiedBy = e.ModificadoPor,
                                                   Code = e.Codigo,
                                                   IsActive = e.EsActivo,
                                                   TipoAgrupacionId = e.TipoAgrupacion.Id

                                               }).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        //cambios para compilar
        public List<AgrupacionCodeDto> ConsultarAgrupacionesCompleta()
        {

            var lista = new List<AgrupacionCodeDto>();
            var listadoDesdeBd = Contexto.upsGetShelterAgrupacionCode();
            if (listadoDesdeBd == null)
                return lista;
            var resultSp = listadoDesdeBd.ToList();
            try
            {

                lista = resultSp.Select(e => new AgrupacionCodeDto()
                {
                    Id = e.Id,
                    Name = e.Descripcion,
                    MunicipalityId = e.MunicipalityId,
                    Created = e.Created,
                    Modified = e.Modified,
                    CreatedBy = e.CreatedBy,
                    ModifiedBy = e.ModifiedBy,
                    Code = e.Code,
                    IsActive = e.IsActive,
                    TipoAgrupacionId = e.TipoAgrupacionId
                }).ToList();


                return lista;
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }
        public List<CatalogoDto> ConsultarTiposAgrupaciones()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.TipoAgrupacion
                                               select new GrupoRecursoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.TipoAgrupacion1,
                                                   Created = e.FechaCreacion,
                                                   Modified = e.FechaModificacion,
                                                   CreatedBy = e.CreadoPor,
                                                   ModifiedBy = e.ModificadoPor,


                                               }).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarPoliticas(int? tipoAgrupacion)
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.PolicyTargeting
                                               where e.PolicyTargetingTypeId == tipoAgrupacion
                                               select new PoliticaCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   ParentId = e.ParentId,
                                                   PolicyTargetingTypeId = e.PolicyTargetingTypeId
                                               }).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarCategoriaByPadre(int idPadre)
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.PolicyTargeting
                                               where e.ParentId == idPadre
                                               select new PoliticaCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Name,
                                                   ParentId = e.ParentId,
                                                   PolicyTargetingTypeId = e.PolicyTargetingTypeId
                                               }).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarIndicadoresPoliticas()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from pi in Contexto.PolicyIndicator
                                               join pt in Contexto.PolicyTargetingIndicator on pi.Id equals pt.PolicyIndicator.Id
                                               join di in Contexto.PolicyTargeting on pt.PolicyTargeting.Id equals di.Id
                                               join po in Contexto.PolicyTargeting on di.ParentId equals po.Id
                                               select new IndicadorPoliticaCatalogoDto()
                                               {
                                                   Id = pi.Id,
                                                   Code = pi.Code,
                                                   Name = po.Name +" - "+ di.Name + " - " + pi.Name ,
                                                   PolicyTargetingId = pt.PolicyTargeting.Id
                                               }).GroupBy(p => p.Code)
                                                    .Select(g => g.FirstOrDefault()).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarTiposCofinanciaciones()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from pi in Contexto.TipoCofinanciador
                                              
                                               select new TipoCofinanciadorCatalogoDto()
                                               {
                                                   Id = pi.Id,
                                                   Name = pi.Descripcion
                                               }).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarEntregables()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from dc in Contexto.DeliverableCatalog
                                               where dc.ProductCatalog.IsDeliverable == true
                                               select new EntregableCatalogoDto()
                                               {
                                                   Id = dc.Id,
                                                   Name = dc.Name,
                                                   MeasureTypeId = dc.MeasureTypeId,
                                                   MeasuredThrough = dc.MeasuredThrough,
                                                   ProductCId = dc.ProductCatalog.Id,
                                                   IsActive = dc.IsEnabled
                                               }).ToList().
                                                            OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarFondos()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.Fondo
                                               select new EntidadCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Descripcion,
                                                   Code = e.Codigo
                                               }).Distinct().ToList().
                                                         OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarRubros()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.DNH_SIIF
                                               where e.Rubro != null
                                               select new RubroCatalogoDto()
                                               {
                                                   Name = e.Nombre,
                                                   Bpin = e.Bpin,
                                                   Rubro = e.Rubro

                                               }).Distinct().ToList().
                                                         OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }

        public List<CatalogoDto> ConsultarTipoCofinanciador()
        {
            var lista = new List<CatalogoDto>();
            try
            {
                lista = new List<CatalogoDto>((from e in Contexto.TipoCofinanciador
                                               select new EntidadCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Descripcion
                                               }).Distinct().ToList().
                                                         OrderBy(x => x.Name));
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lista;
        }
        public string ObtenerTablasBasicas(string jsonCondicion, string Tabla)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Transversal.uspGetTablaBasica @json,@Tabla ",
                                                new SqlParameter("json", jsonCondicion),
                                                new SqlParameter("Tabla", Tabla)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public List<CatalogoDto> ConsultarTiposRecursosEntidadPorGrupoRecursos(int entityTypeCatalogId, int resourceGroupId, bool incluir)
        {
            var lista = new List<CatalogoDto>((from e in Contexto.ResourceType
                                               join re in Contexto.ResourceTypeByEntity on e.Id equals re.ResourceType.Id
                                               where incluir ? (re.EntityTypeCatalogOption.Id == entityTypeCatalogId) && (e.ResourceGroup.Id == resourceGroupId) : (re.EntityTypeCatalogOption.Id == entityTypeCatalogId) && !(e.ResourceGroup.Id == resourceGroupId)
                                               select new TipoRecursoCatalogoDto()
                                               {
                                                   Id = e.Id,
                                                   Name = e.Description,
                                                   Created = e.Created,
                                                   Modified = e.Modified,
                                                   EntityTypeId = e.EntityType.Id,
                                                   CreatedBy = e.CreatedBy,
                                                   ModifiedBy = e.ModifiedBy,
                                                   Code = e.Code,
                                                   IsActive = e.IsActive,
                                                   IdEscritorio = e.IdEscritorio,
                                                   IdClassificationType = e.ClassificationType.Id,
                                                   ResourceGroupId = e.ResourceGroup.Id
                                               }).ToList().
                                                         OrderBy(x => x.Name));

            return lista;
        }

    }
}
