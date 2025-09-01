
using DNP.ServiciosTransaccional.Persistencia.Interfaces.Programacion;
using DNP.ServiciosTransaccional.Persistencia.Interfaces;
using System.Data.SqlClient;
using System.Linq;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using System.Data;
using System;
using DNP.ServiciosNegocio.Comunes.Excepciones;

namespace DNP.ServiciosTransaccional.Persistencia.Implementaciones.Programacion
{
    public class ProgramacionPersistencia : Persistencia, IProgramacionPersistencia
    {
        #region Constructor

        public ProgramacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion
        public TramitesResultado GuardarDatosProgramacionDistribucion(string NumeroTramite, string usuario)
        {
            var respuesta = new TramitesResultado();

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProgramacionCargueMasivoAprobacion @NumeroTramite,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("NumeroTramite", NumeroTramite),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );


                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || resultado >= 0)
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado InclusionFuentesProgramacion(string NumeroTramite, string usuario)
        {
            var respuesta = new TramitesResultado();

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostInclusionFuentesProgramacion @NumeroTramite,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("NumeroTramite", NumeroTramite),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );


                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || resultado >= 0)
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
    }
}
