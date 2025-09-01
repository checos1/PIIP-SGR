using AutoMapper;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Viabilidad 
{
    public class AcuerdoSgpPersistencia : PersistenciaSGP, IAcuerdoSgpPersistencia
    {
        #region Constructor

        public AcuerdoSgpPersistencia(IContextoFactory contextoFactory, IContextoFactorySGR contextoFactorySGR) : base(contextoFactory, contextoFactorySGR)
        {            
           
        }
        #endregion

        #region "Métodos"

        /// <summary>
        /// Leer el acuerdo, sector y clasificadores de un proyecto
        /// </summary>
        public string SGPAcuerdoLeerProyecto(int proyectoId, System.Guid nivelId)
        {
            string Json = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.uspGetSGPAcuerdoLeerProyecto @proyectoId,@nivelId ",
                                              new SqlParameter("proyectoId", proyectoId),
                                              new SqlParameter("nivelId", nivelId)
                                               ).SingleOrDefault();
            return Json;
        }

        public ResultadoProcedimientoDto SGPAcuerdoGuardarProyecto(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var respuesta = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = ContextoOnlySP.Database.BeginTransaction())
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostSGPAcuerdoGuardarProyecto @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public string SGPProyectosLeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            string Json = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.uspGetSGPProyectosLeerListas @nivelId,@proyectoId,@nombreLista ",
                                            new SqlParameter("nivelId", nivelId),
                                              new SqlParameter("proyectoId", proyectoId),
                                              new SqlParameter("nombreLista", nombreLista)
                                               ).SingleOrDefault();
            return Json;
        }
        
        #endregion
    }
}
