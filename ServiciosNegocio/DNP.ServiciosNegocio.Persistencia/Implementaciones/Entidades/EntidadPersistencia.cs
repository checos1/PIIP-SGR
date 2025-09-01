using System;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Entidades;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System.Data.Entity.Validation;
using DNP.ServiciosNegocio.Dominio.Dto.Entidades;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Entidades
{
    public class EntidadPersistencia : Persistencia, IEntidadPersistencia
    {
        #region Incializacion


        public EntidadPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<EntityTypeCatalogOption, OpcionCatalogoTipoEntidadDto>()
                .ForMember(dto => dto.Nombre, opt => opt.MapFrom(ent => ent.Name))
                .ForMember(dto => dto.IdTipo, opt => opt.MapFrom(ent => ent.EntityTypeId))
                .ForMember(dto => dto.IdPadre, opt => opt.MapFrom(ent => ent.ParentId))
                .ForMember(dto => dto.CodigoEntidad, opt => opt.MapFrom(ent => ent.Code))
                .ForMember(dto => dto.EsActiva, opt => opt.MapFrom(ent => ent.IsActive)));
                //.ForMember(dto => dto.Atributos, opt => opt.MapFrom(ent => ent.AtributosEntidad)));
        }
       
        #endregion

        #region EntidadBase

        public void InsertarOpcionCatalogoTipoEntidad(OpcionCatalogoTipoEntidadDto entidadDto)
        {
            EntityTypeCatalogOption entidadNueva = new EntityTypeCatalogOption()
            {
                Name = entidadDto.Nombre,
                Id = entidadDto.Id,
                EntityTypeId = entidadDto.IdTipo,
                ParentId = entidadDto.IdPadre,
                Code = entidadDto.CodigoEntidad,
                IsActive = entidadDto.EsActiva,
                AtributosEntidad = entidadDto.Atributos == null ? null : new AtributosEntidad()
                {
                    Id = entidadDto.Id,
                    CabeceraSector = entidadDto.Atributos.CabeceraSector,
                    Orden = entidadDto.Atributos.Orden,
                    //SectorId = entidadDto.Atributos.SectorId,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    CreadoPor = entidadDto.Atributos.CreadoPor,
                    ModificadoPor = entidadDto.Atributos.ModificadoPor
                }
            };

            Contexto.EntityTypeCatalogOption.Add(entidadNueva);
            GuardarCambios();
        }

        public OpcionCatalogoTipoEntidadDto ConsultarOpcionCatalogoTipoEntidadPorId(int id)
        {
            var entidadEncontrada = Contexto.EntityTypeCatalogOption.FirstOrDefault(p => p.Id == id);
            if (entidadEncontrada != null)
            {
                return MapearEntidad(entidadEncontrada);
            }
            return null;
        }


        public void ActualizarOpcionCatalogoTipoEntidad(OpcionCatalogoTipoEntidadDto entidadDto)
        {
            EntityTypeCatalogOption entidadAActualizar = Contexto.EntityTypeCatalogOption.FirstOrDefault(p => p.Id == entidadDto.Id);
            if (entidadAActualizar != null)
            {
                entidadAActualizar.Name = entidadDto.Nombre;
                entidadAActualizar.EntityTypeId = entidadDto.IdTipo;
                entidadAActualizar.ParentId = entidadDto.IdPadre;
                entidadAActualizar.Code = entidadDto.CodigoEntidad;
                entidadAActualizar.IsActive = entidadDto.EsActiva;
                entidadAActualizar.AtributosEntidad.CabeceraSector = entidadDto.Atributos.CabeceraSector;
                entidadAActualizar.AtributosEntidad.Orden = entidadDto.Atributos.Orden;
                //entidadAActualizar.AtributosEntidad.SectorId = entidadDto.Atributos.SectorId;
                entidadAActualizar.AtributosEntidad.FechaModificacion = DateTime.Now;
                entidadAActualizar.AtributosEntidad.ModificadoPor = entidadDto.Atributos.ModificadoPor;

                try
                {
                    GuardarCambios();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }
        }



        public List<OpcionCatalogoTipoEntidadDto> ConsultarOpcionCatalogoTipoEntidadTodos()
        {
            var listadoEntidades = Contexto.EntityTypeCatalogOption.ToList();
            List<OpcionCatalogoTipoEntidadDto> listadoRetorno = new List<OpcionCatalogoTipoEntidadDto>();
            foreach (var entidad in listadoEntidades)
            {
                listadoRetorno.Add(MapearEntidad(entidad));
            }

            return listadoRetorno;
        }

        public void BorrarOpcionCatalogoTipoEntidad(OpcionCatalogoTipoEntidadDto entidadDto)
        {
            EntityTypeCatalogOption entidadABorrar = Contexto.EntityTypeCatalogOption.FirstOrDefault(p => p.Id == entidadDto.Id);
            if (entidadABorrar != null) Contexto.EntityTypeCatalogOption.Remove(entidadABorrar);
            GuardarCambios();
        }

        #endregion

        #region Metodos utilitarios
        private OpcionCatalogoTipoEntidadDto MapearEntidad(EntityTypeCatalogOption entidad)
        {
            return Mapper.Map<OpcionCatalogoTipoEntidadDto>(entidad);
        }

        #endregion
    }
}
