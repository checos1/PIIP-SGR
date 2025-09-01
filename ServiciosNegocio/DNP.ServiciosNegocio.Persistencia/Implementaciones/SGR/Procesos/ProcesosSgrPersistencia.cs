using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.DesignacionEjecutor;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.GestionRecursos.Aprobacion;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Procesos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Procesos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.Procesos
{
    public class ProcesosSgrPersistencia : PersistenciaSGR, IProcesosSgrPersistencia
    {
        public ProcesosSgrPersistencia(Interfaces.IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
        }

        public IEnumerable<PriorizacionProyectoDto> ObtenerPriorizacionProyecto(Nullable<Guid> instanciaId)
        {
            var priorizacionProyecto = Contexto.Database.SqlQuery<PriorizacionProyectoDto>("[Proyectos].[uspGetSGR_Priorizacion_Proyecto] @InstanciaId",
                new SqlParameter("@InstanciaId", instanciaId)
            ).ToList();

            return priorizacionProyecto;
        }

        public IEnumerable<AprobacionProyectoDto> ObtenerAprobacionProyecto(Nullable<Guid> instanciaId)
        {
            var aprobacionProyecto = Contexto.Database.SqlQuery<AprobacionProyectoDto>("[Proyectos].[uspGetSGR_Aprobacion_Proyecto] @InstanciaId",
                new SqlParameter("@InstanciaId", instanciaId)
            ).ToList();

            return aprobacionProyecto;
        }

        public IEnumerable<ProyectoAprobacionInstanciasDto> ObtenerProyectoAprobacionInstanciasSGR(Nullable<Guid> instanciaId)
        {
            var aprobacionProyectoDetalle = Contexto.Database.SqlQuery<ProyectoAprobacionInstanciasDto>("[Proyectos].[uspGetObtenerProyectoAprobacionInstancias] @InstanciaId",
                new SqlParameter("@InstanciaId", instanciaId)
            ).ToList();

            return aprobacionProyectoDetalle;
        }

        public ProyectoAprobacionInstanciasResultado GuardarProyectoAprobacionInstanciasSGR(ProyectoAprobacionInstanciasDto proyectoAprobacionInstanciasDto, string usuario)
        {
            var respuesta = new ProyectoAprobacionInstanciasResultado();
            var json = JsonUtilidades.ACadenaJson(proyectoAprobacionInstanciasDto);

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "Resultado",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };

                    var jsonConsulta = Contexto.Database.SqlQuery<string>("[Proyectos].[uspPostGuardarProyectoAprobacionInstanciasSGR] @JsonData, @Usuario, @Resultado output  ",
                                                      new SqlParameter("JsonData", json),
                                                      new SqlParameter("Usuario", usuario),
                                                      outParam
                                                      ).SingleOrDefault();

                    if (outParam.SqlValue.ToString() == "Null")
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = jsonConsulta;
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
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public AprobacionProyectoCreditoDto ObtenerAprobacionProyectoCredito(Guid instancia, int entidad)
        {
            try
            {
                var aprobacionProyectoCredito = Contexto.uspGetAprobacionCredito(instancia, entidad).SingleOrDefault();
                if (aprobacionProyectoCredito == null)
                {
                    throw new ServiciosNegocioException(ServiciosNegocioRecursos.SinResultados);
                }
                else
                {
                    return JsonConvert.DeserializeObject<AprobacionProyectoCreditoDto>(aprobacionProyectoCredito);
                }
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public ResultadoProcedimientoDto GuardarAprobacionProyectoCredito(AprobacionProyectoCreditoDto aprobacionProyectoCreditoDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostAprobacionCredito(JsonUtilidades.ACadenaJson(aprobacionProyectoCreditoDto),
                                                         usuario,
                                                         errorValidacionNegocio);


                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
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

                //return resultado;

            }
        }

        public ProyectoProcesoResultado GuardarProyectoPermisosProcesoSGR(ProyectoProcesoDto proyectoProcesoDto, string usuario)
        {
            var respuesta = new ProyectoProcesoResultado();

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "Resultado",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };

                    var jsonConsulta = Contexto.Database.SqlQuery<string>("[Proyectos].[uspPostProyectoPermisosProcesosSGR] @BPIN,@InstanciaId, @ProcesoId, @FlujoId, @Usuario, @Resultado output",
                                                       new SqlParameter("BPIN", proyectoProcesoDto.BPIN),
                                                       new SqlParameter("InstanciaId", proyectoProcesoDto.InstanciaId),
                                                       new SqlParameter("ProcesoId", proyectoProcesoDto.ProcesoId),
                                                       new SqlParameter("FlujoId", proyectoProcesoDto.FlujoId),
                                                       new SqlParameter("Usuario", usuario),
                                                       outParam
                                                       ).SingleOrDefault();

                    if (outParam.SqlValue.ToString() == "Null")
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = jsonConsulta;
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
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public IEnumerable<ProyectoPriorizacionDetalleDto> ObtenerPriorizionProyectoDetalleSGR(Nullable<Guid> instanciaId)
        {
            var priorizacionProyectoDetalle = Contexto.Database.SqlQuery<ProyectoPriorizacionDetalleDto>("[Proyectos].[uspGetObtenerProyectoPriorizacionDetalle]  @InstanciaId",
                new SqlParameter("@InstanciaId", instanciaId)
            ).ToList();

            return priorizacionProyectoDetalle;
        }

        public ProyectoPriorizacionDetalleResultado GuardarPriorizionProyectoDetalleSGR(ProyectoPriorizacionDetalleDto proyectoPriorizacionDetalleDto, string usuario)
        {
            var respuesta = new ProyectoPriorizacionDetalleResultado();
            var json = JsonUtilidades.ACadenaJson(proyectoPriorizacionDetalleDto);

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "Resultado",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };

                    var jsonConsulta = Contexto.Database.SqlQuery<string>("[Proyectos].[uspPostProyectoPriorizacionDetalleSGR] @JsonData, @Usuario, @Resultado output  ",
                                                       new SqlParameter("JsonData", json),
                                                       new SqlParameter("Usuario", usuario),
                                                       outParam
                                                       ).SingleOrDefault();

                    if (outParam.SqlValue.ToString() == "Null")
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = jsonConsulta;
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
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    return respuesta;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public string SGR_Proyectos_MostrarEstadosPriorizacion(int proyectoId)
        {
            string Json = Contexto.uspGetSGR_Proyectos_MostrarEstadosPriorizacion(proyectoId).SingleOrDefault();
            return Json;
        }

        public string SGR_Proyectos_MostrarEstadosAprobacion(int proyectoId)
        {

            string Json = Contexto.uspGetSGR_Proyectos_MostrarEstadosAprobacionCreditoParcial(proyectoId).SingleOrDefault();
            return Json;
        }
        public List<ProyectoResumenEstadoAprobacionCreditoDto> SGR_Proyectos_ResumenEstadoAprobacionCredito(int proyectoId)
        {
            var sql = "EXEC Proyectos.uspGetResumenEstadoAprobacionCredito @ProyectoId";
            var param = new SqlParameter("@ProyectoId", proyectoId);


            var partesJson = Contexto.Database
                .SqlQuery<string>(sql, param)
                .ToList();


            var jsonCompleto = string.Concat(partesJson);


            var lista = JsonConvert
                .DeserializeObject<List<ProyectoResumenEstadoAprobacionCreditoDto>>(jsonCompleto);

            return lista;
        }

        public string SGR_Proyectos_LeerValoresAprobacion(int proyectoId)
        {
            string Json = Contexto.uspGetProyectoLeerValorAprobacionSGR(proyectoId).SingleOrDefault();
            return Json;
        }

        public string SGR_Proyectos_MostrarEstadosAprobacionCreditoParcial(int proyectoId)
        {
            string Json = Contexto.uspGetSGR_Proyectos_MostrarEstadosAprobacionCreditoParcial(proyectoId).SingleOrDefault();
            return Json;
        }

        public List<EjecutorEntidadAsociado> SGR_Procesos_ConsultarEjecutorbyTipo(int proyectoId, int tipoEjecuutorId)
        {
            var resultSp = Contexto.uspGetSGR_Procesos_ConsultarEjecutorbyTipo(proyectoId, tipoEjecuutorId).ToList();

            var ejecutorList = new List<EjecutorEntidadAsociado>();

            ejecutorList = resultSp.Select(est => new EjecutorEntidadAsociado()
            {
                EjecutorId = est.EjecutorId,
                Id = est.id,
                NitEjecutor = est.NitEjecutor,
                NombreEntidad = est.nombreEntidad,
                TipoEntidad = est.TipoEntidad
            }).ToList();

            return ejecutorList;
        }


        #region Designación Ejecutor

        /// <summary>
        /// Registrar valor de una columna dinamica del ejecutor por proyectoId.
        /// </summary>     
        /// <param name="valores"></param> 
        /// <param name="usuario"></param>
        /// <returns>bool</returns>        
        public bool RegistrarRespuestaEjecutorSGR(RespuestaDesignacionEjecutorDto valores, string usuario)
        {
            using (var connection = Contexto.Database.Connection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Proyectos.uspPostProyectoRegistrarRespuestaEjecutorSGR";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@campo", valores.campo));
                    command.Parameters.Add(new SqlParameter("@respuesta", valores.respuesta));
                    command.Parameters.Add(new SqlParameter("@proyectoId", valores.proyectoId));
                    command.Parameters.Add(new SqlParameter("@usuario", usuario));

                    var resultadoParam = new SqlParameter("@resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(resultadoParam);
                    command.ExecuteNonQuery();

                    return Convert.ToBoolean(resultadoParam.Value);
                }
            }
        }

        /// <summary>
        /// Obtener el valor de una columna dinámica del ejecutor por proyectoId.
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="proyectoId"></param>
        /// <returns>string</returns>
        public string ObtenerRespuestaEjecutorSGR(string campo, int proyectoId)
        {
            using (var connection = Contexto.Database.Connection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Proyectos.uspGetProyectoObtenerRespuestaEjecutorSGR";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@campo", campo));
                    command.Parameters.Add(new SqlParameter("@proyectoId", proyectoId));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader[0]?.ToString();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Registrar valor de dinamico aprobación valores.
        /// </summary>     
        /// <param name="valores"></param>         
        /// <param name="usuario"></param>   
        /// <returns>bool</returns>
        public bool ActualizarValorEjecutorSGR(CampoItemValorDto valores, string usuario)
        {
            string json = JsonConvert.SerializeObject(valores.ListaValores);

            using (var connection = Contexto.Database.Connection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Proyectos.uspPostActEjecutorInstanciasValoresSGR";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@json", json));
                    command.Parameters.Add(new SqlParameter("@campo", valores.Campo));
                    command.Parameters.Add(new SqlParameter("@usuario", usuario));

                    var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(resultadoParam);
                    command.ExecuteNonQuery();

                    return Convert.ToBoolean(resultadoParam.Value);
                }
            }
        }

        /// <summary>
        /// Obtener valor de costos de estructuracion viabilidad.
        /// </summary>
        /// <param name="instanciaId"></param>     
        /// <returns>string</returns>
        public string ObtenerValorCostosEstructuracionViabilidadSGR(Guid instanciaId)
        {
            using (var connection = Contexto.Database.Connection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Proyectos.uspGetProyectoObtenerCostosEstrucViabilidadSGR";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@InstanciaId", instanciaId));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader[0]?.ToString();
                    }
                }
            }

            return null;
        }

        #endregion Designación Ejecutor
    }
}
