using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Comunes.Dto.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.ProgramacionDistribucion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;


namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.ProgramacionDistribucion
{
    public class ProgramacionDistribucionPersistencia : Persistencia, IProgramacionDistribucionPersistencia
    {
        #region Constructor

        public ProgramacionDistribucionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        public string ObtenerDatosProgramacionDistribucion(int EntidadDestinoId, int tramiteid)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetProgramaciondistribucion @EntidadDestinoId, @tramiteid ",
                                                new SqlParameter("EntidadDestinoId", EntidadDestinoId),
                                                new SqlParameter("tramiteid", tramiteid)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarDatosProgramacionDistribucion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            var respuesta = new TramitesResultado();
            var json = JsonUtilidades.ACadenaJson(ProgramacionDistribucion);

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "errorValidacionNegocio",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProgramaciondistribucion @json,@Usuario,@errorValidacionNegocio output ", 
                                            new SqlParameter("json", json),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );


                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
                }
                catch (Exception )
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ObtenerDatosProgramacionFuenteEncabezado(int EntidadDestinoId, int tramiteid)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetProgramacionFuentesEncabezado @EntidadDestinoId, @tramiteid ",
                                                new SqlParameter("EntidadDestinoId", EntidadDestinoId),
                                                new SqlParameter("tramiteid", tramiteid)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public string ObtenerDatosProgramacionFuenteDetalle(int tramiteidProyectoId)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetProgramacionFuentesDetalle @tramiteidProyectoId ",
                                                new SqlParameter("tramiteidProyectoId", tramiteidProyectoId)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarDatosProgramacionFuente(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
        {
            var respuesta = new TramitesResultado();
            var json = JsonUtilidades.ACadenaJson(ProgramacionDistribucion);

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "errorValidacionNegocio",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProgramaciondistribucion @json,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("json", json),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );


                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        //var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        //if (temporal != null)
                        //    Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
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
