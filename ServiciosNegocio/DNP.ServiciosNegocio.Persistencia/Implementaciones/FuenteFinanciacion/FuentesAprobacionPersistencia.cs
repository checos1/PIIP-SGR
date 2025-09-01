using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.FuenteFinanciacion;
using DNP.ServiciosNegocio.Persistencia.Modelo;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.FuenteFinanciacion
{
    using System.Data.Entity.Core.Objects;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;

    public class FuentesAprobacionPersistencia : Persistencia, IFuentesAprobacionPersistencia
    {
        public FuentesAprobacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }

        public string ObtenerPreguntasAprobacionRol(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            if(objPreguntasSeguimientoProyectoDto == null){
                objPreguntasSeguimientoProyectoDto = new PreguntasSeguimientoProyectoDto();
                objPreguntasSeguimientoProyectoDto.tramiteId = 0;
                objPreguntasSeguimientoProyectoDto.proyectoId = 0;
                objPreguntasSeguimientoProyectoDto.tipoTramiteId = 0;
                objPreguntasSeguimientoProyectoDto.nivelId =Guid.Parse("00000000-0000-0000-0000-000000000000");
            }

            var listadoFuentesProgramarSolicitado = Contexto.uspGetSeguimientoProyectoGR(objPreguntasSeguimientoProyectoDto.tramiteId, objPreguntasSeguimientoProyectoDto.proyectoId,
                                                                                            objPreguntasSeguimientoProyectoDto.tipoTramiteId, objPreguntasSeguimientoProyectoDto.nivelId).FirstOrDefault();
            return listadoFuentesProgramarSolicitado;
        }

        public string GuardarPreguntasAprobacionRol(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                   Contexto.uspPostPreguntasAprobacionRol(JsonUtilidades.ACadenaJson(objPreguntasSeguimientoProyectoDto.Contenido), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "OK";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                        //return mensajeError;
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public string ObtenerPreguntasAprobacionJefe(PreguntasSeguimientoProyectoDto objPreguntasSeguimientoProyectoDto)
        {
            var listadoFuentesProgramarSolicitado = Contexto.uspGetPreguntasAprobacionJefePlaneacion(objPreguntasSeguimientoProyectoDto.tramiteId, objPreguntasSeguimientoProyectoDto.proyectoId,
                                                                                            objPreguntasSeguimientoProyectoDto.tipoTramiteId, objPreguntasSeguimientoProyectoDto.nivelId).FirstOrDefault();
            return listadoFuentesProgramarSolicitado;
        }

        public string GuardarPreguntasAprobacionJefe(ParametrosGuardarDto<PreguntasSeguimientoProyectoDto> objPreguntasSeguimientoProyectoDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPreguntasAprobacionJefePlanea(JsonUtilidades.ACadenaJson(objPreguntasSeguimientoProyectoDto.Contenido), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "OK";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                        //return mensajeError;
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

    }
}


