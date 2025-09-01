using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.Administracion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Administracion;
using DNP.ServiciosNegocio.Persistencia.Modelo;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Administracion
{
    using System.Data.Entity.Core.Objects;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json.Linq;

    public class EjecutorPersistencia : Persistencia, IEjecutorPersistencia
    {
        public EjecutorPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();

        }

        public EjecutorDto ConsultarEjecutor(string nit)
        {
            return Contexto.Ejecutores
                    .Where(p => (p.Nit == nit && p.Activo == true))
                        .Select(x => new EjecutorDto 
                        {
                            Id = x.Id,
                            Nit = x.Nit,
                            Digito = x.Digito,
                            NombreEjecutor  = x.NombreEjecutor,
                            EntityTypeId = x.EntityType.Id,
                            CreadoPor = x.CreadoPor,
                            FechaCreacion = x.FechaCreacion,
                            ModificadoPor = x.ModificadoPor,
                            FechaModificacion = x.FechaModificacion,
                            NombreEntityType = x.EntityType.EntityType1,
                            
                        }).FirstOrDefault();
        }


        public bool GuardarEjecutor(EjecutorDto ObjDto)
        {
           if (ObjDto.Id > 0)
            {
                return ActualizarEjecutor(ObjDto);
            }
           else
            {
                EjecutorDto Dato = ConsultarEjecutor(ObjDto.Nit);
                if (Dato == null)
                {
                    return InsertarEjecutor(ObjDto); 
                }
                else
                {
                    return ActualizarEjecutor(ObjDto);
                }
            }

        }


        private bool InsertarEjecutor(EjecutorDto ObjDto)
        {
            var dbContextTransaction = Contexto.Database.BeginTransaction();
            try
            {
                
                Ejecutor ObjNuevo = new Ejecutor()
                {
                    Nit = ObjDto.Nit,
                    Digito = ObjDto.Digito,
                    NombreEjecutor = ObjDto.NombreEjecutor,
                    EntityTypeId = ObjDto.EntityTypeId,
                    CreadoPor = ObjDto.CreadoPor,
                    FechaCreacion = DateTime.Now,
                    ModificadoPor = ObjDto.ModificadoPor,
                    FechaModificacion = DateTime.Now,
                    Activo = ObjDto.Activo
                };

                Contexto.Ejecutores.Add(ObjNuevo);
                GuardarCambios();
                dbContextTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                return false;
            }
            
        }

        private bool ActualizarEjecutor(EjecutorDto ObjDto)
        {
            try
            {
                var idActualizacion = Contexto.uspPostActualizaEjecutor(
                ObjDto.Nit,
                ObjDto.Digito,
                ObjDto.NombreEjecutor,
                ObjDto.EntityTypeId,
                ObjDto.CreadoPor,
                ObjDto.ModificadoPor,
                ObjDto.FechaModificacion,
                ObjDto.Activo);

                GuardarCambios();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}


