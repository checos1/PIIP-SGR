using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Programacion;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionDistribucion;
using DNP.ServiciosNegocio.Dominio.Dto.Programacion;
using DNP.ServiciosNegocio.Dominio.Dto.ProgramacionFuente;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using System.Net.Http.Headers;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Programacion
{
    public class ProgramacionPersistencia : Persistencia, IProgramacionPersistencia
    {
        #region Constructor

        public ProgramacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        public bool ValidarCalendarioProgramacion(Nullable<int> entityTypeCatalogOptionId, Nullable<System.Guid> nivelId, Nullable<int> seccionCapituloId)
        {
            ObjectParameter resultado = new ObjectParameter("Result", typeof(bool));
            try
            {
                ContextoOnlySP.UspGetValidarCalendario(entityTypeCatalogOptionId, nivelId, seccionCapituloId, resultado);

                return Convert.ToBoolean(resultado.Value);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string ObtenerCargaMasivaCreditos()
        {
            var listaCreditos = ContextoOnlySP.UspGetCargaMasivaCreditos().FirstOrDefault();
            return listaCreditos;
        }

        public string ObtenerProgramacionProyectosSinPresupuestal(int? sectorId, int? entidadId, string proyectoId)
        {
        
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Transversal.uspGetProgramacionProyectosSinPresupuestal @sectorId,@entidadId,@proyectoId ",
                                               new SqlParameter("sectorId", sectorId),
                                               new SqlParameter("entidadId", entidadId),
                                               new SqlParameter("proyectoId", proyectoId)
                                                ).SingleOrDefault();
            return jsonConsulta;
        }

        public string ObtenerCargaMasivaCuotas(int? Vigencia, int? EntityTypeCatalogOptionId)
        {
            var listaPresupuestal = ContextoOnlySP.UspGetCargaMasivaCuotas(Vigencia, EntityTypeCatalogOptionId).FirstOrDefault();
            return listaPresupuestal;
        }

        public string ObtenerProgramacionSectores(int? sectorId)
        {
            if(sectorId == 0)
            {
                sectorId = null;
            }
            var listaSectores = ContextoOnlySP.uspGetProgramacionSectores(sectorId).FirstOrDefault();
            return listaSectores;
        }
        public string ObtenerProgramacionEntidadesSector(int? sectorId) 
        {
            if (sectorId == 0)
            {
                sectorId = null;
            }

            var listaSectoresEntidad = ContextoOnlySP.uspGetProgramacionEntidadesSector(sectorId).FirstOrDefault();
            return listaSectoresEntidad;
        }

        public string ObtenerCalendarioProgramacion(Guid FlujoId)
        {
            var listaCalendario = ContextoOnlySP.UspGetCalendarioProgramacion(FlujoId).FirstOrDefault();
            return listaCalendario;
        }

        public TramitesResultado RegistrarCargaMasivaCreditos(List<CargueCreditoDto> json, string usuario)
        {
            var respuesta = new TramitesResultado();
            var jsonModel = JsonUtilidades.ACadenaJson(json);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Programacion.UspPostCargaMasivaCreditos @json,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("json", jsonModel),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );


                    if (outParam.SqlValue.ToString() == "Null")
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

        public string ObtenerDatosProgramacionEncabezado(int EntidadDestinoId, int tramiteid, string origen)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetProgramacionEncabezado @EntidadDestinoId, @tramiteid,@origen ",
                                                new SqlParameter("EntidadDestinoId", EntidadDestinoId),
                                                new SqlParameter("tramiteid", tramiteid),
                                                new SqlParameter("origen", origen)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public string ObtenerDatosProgramacionDetalle(int tramiteidProyectoId, string origen)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetProgramacionDetalle @tramiteidProyectoId , @origen ",
                                                new SqlParameter("tramiteidProyectoId", tramiteidProyectoId),
                                                new SqlParameter("origen", origen)
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ValidarCargaMasivaCreditos(List<CargueCreditoDto> json)
        {
            var jsonModel = JsonUtilidades.ACadenaJson(json);
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Programacion.UspGetValidarCargaMasivaCreditos @json",
                                                new SqlParameter("json", jsonModel)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado RegistrarCargaMasivaCuota(List<CargueCuotaDto> json, string usuario)
        {
            var respuesta = new TramitesResultado();
            var jsonModel = JsonUtilidades.ACadenaJson(json);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Programacion.UspPostCargaMasivaCuota @json,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("json", jsonModel),
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado RegistrarProyectosSinPresupuestal(List<ProyectoSinPresupuestalDto> json, string usuario)
        {
            var respuesta = new TramitesResultado();
            var jsonModel = JsonUtilidades.ACadenaJson(json);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Programacion.uspPostProgramacionProyectosSinPresupuestal @json,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("json", jsonModel),
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado RegistrarCalendarioProgramacion(List<CalendarioProgramacionDto> json, string usuario)
        {
            var respuesta = new TramitesResultado();
            var jsonModel = JsonUtilidades.ACadenaJson(json);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Programacion.UspPostCalendarioProgramacion @json,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("json", jsonModel),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );

                    if (outParam.SqlValue.ToString() == "Null")
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

        public TramitesResultado GuardarDatosProgramacionFuente(ProgramacionFuenteDto ProgramacionFuente, string usuario)
        {
            var respuesta = new TramitesResultado();
            var json = JsonUtilidades.ACadenaJson(ProgramacionFuente);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProgramacionFuente @json,@Usuario,@errorValidacionNegocio output ",
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ValidarConsecutivoPresupuestal(List<ProyectoSinPresupuestalDto> json)
        {
            var jsonModel = JsonUtilidades.ACadenaJson(json);
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Programacion.UspGetValidarConsecutivoPresupuestal @json",
                                                new SqlParameter("json", jsonModel)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public string ValidarCargaMasivaCuotas(List<CargueCuotaDto> json)
        {
            var jsonModel = JsonUtilidades.ACadenaJson(json);
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Programacion.UspGetValidarCargaMasivaCuotas @json",
                                                new SqlParameter("json", jsonModel)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }
        public string ObtenerDatostProgramacionProducto(int tramiteId)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetProgramacionProducto @TramiteID ",
                                                new SqlParameter("TramiteID", tramiteId)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarDatosProgramacionProducto(ProgramacionProductoDto ProgramacionProducto, string usuario)
        {
            var respuesta = new TramitesResultado();
            var json = JsonUtilidades.ACadenaJson(ProgramacionProducto);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProgramacionProducto @json,@Usuario,@errorValidacionNegocio output ",
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

       

        public TramitesResultado GuardarProgramacionRegionalizacion(ProgramacionRegionalizacionDto ProgramacionRegionalizacion, string usuario)
        {
            var respuesta = new TramitesResultado();
            var json = JsonUtilidades.ACadenaJson(ProgramacionRegionalizacion);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProgramacionRegionalizacion @json,@Usuario,@errorValidacionNegocio output ",
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ConsultarPoliticasTransversalesProgramacion(string Bpin)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetPoliticasTransversalesProgramacion @Bpin",
                                             new SqlParameter("Bpin", Bpin)).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado AgregarPoliticasTransversalesProgramacion(IncluirPoliticasDto parametrosGuardar, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(parametrosGuardar);
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostAgregarPoliticasTransversalesProgramacion @json,@Usuario,@errorValidacionNegocio output ",
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
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ConsultarPoliticasTransversalesCategoriasProgramacion(string Bpin)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetPoliticasTransversalesCategoriasProgramacion @Bpin",
                                              new SqlParameter("Bpin", Bpin)).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado EliminarPoliticasProyectoProgramacion(int tramiteidProyectoId, int politicaId)
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostEliminarPoliticasProyectoProgramacion @tramiteidProyectoId,@politicaId,@errorValidacionNegocio output ",
                                           new SqlParameter("tramiteidProyectoId", tramiteidProyectoId),
                                           new SqlParameter("politicaId", politicaId),
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
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado AgregarCategoriasPoliticaTransversalesProgramacion(FocalizacionCategoriasDto objIncluirPoliticasDto, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(objIncluirPoliticasDto);
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostAgregarCategoriasPoliticaTransversalesProgramacion @json,@Usuario,@errorValidacionNegocio output ",
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
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado GuardarPoliticasTransversalesCategoriasProgramacion(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(objIncluirPoliticasDto);
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostPoliticasTransversalesCategoriasProgramacion @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("Usuario", usuario),
                                           outParam
                                           );

                    if (outParam.SqlValue.ToString() == "Null" || string.IsNullOrEmpty(outParam.SqlValue.ToString()))
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
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado EliminarCategoriasProyectoProgramacion(EliminarCategoriasProyectoProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(objIncluirPoliticasDto);
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostEliminarCategoriasProyectoProgramacion @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("Usuario", usuario),
                                           outParam
                                           );

                    if (outParam.SqlValue.ToString() == "Null" || string.IsNullOrEmpty(outParam.SqlValue.ToString()))
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
                        dbContextTransaction.Rollback();

                    }
                }
                catch (ServiciosNegocioException)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Error al procesar la petición";
                    dbContextTransaction.Rollback();
                }
                catch (Exception ex)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Error al procesar la petición";
                    dbContextTransaction.Rollback();
                }
            }

            return respuesta;
        }

        public TramitesResultado EliminarCategoriaPoliticasProyectoProgramacion(int proyectoId, int politicaId, int categoriaId)
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostEliminarCategoriaPoliticasProyectoProgramacion @proyectoId,@politicaId,@categoriaId,@errorValidacionNegocio output ",
                                           new SqlParameter("proyectoId", proyectoId),
                                           new SqlParameter("politicaId", politicaId),
                                           new SqlParameter("categoriaId", categoriaId),
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
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ObtenerCrucePoliticasProgramacion(string Bpin)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.ObtenerCrucePoliticasProgramacion @Bpin",
                                              new SqlParameter("Bpin", Bpin)).SingleOrDefault();
            return jsonConsulta;
        }

        public string PoliticasSolicitudConceptoProgramacion(string Bpin)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetPoliticasSolicitudConceptoProgramacion @Bpin",
                                              new SqlParameter("Bpin", Bpin)).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarCrucePoliticasProgramacion(List<CrucePoliticasAjustesDto> parametrosGuardar, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(parametrosGuardar);
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostCrucePoliticasProgramacion @json,@Usuario,@errorValidacionNegocio output ",
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado SolicitarConceptoDTProgramacion(List<FocalizacionSolicitarConceptoDto> parametrosGuardar, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(parametrosGuardar);
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostSolicitarConceptoDTProgramacion @json,@Usuario,@errorValidacionNegocio output ",
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
                    throw;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ObtenerResumenSolicitudConceptoProgramacion(string Bpin)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetResumenSolicitudConceptoProgramacion @Bpin",
                                              new SqlParameter("Bpin", Bpin)).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarDatosProgramacionIniciativa(ProgramacionIniciativaDto ProgramacionIniciativa, string usuario)
        {
            var respuesta = new TramitesResultado();
            var json = JsonUtilidades.ACadenaJson(ProgramacionIniciativa);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProgramacionIniciativa @json,@Usuario,@errorValidacionNegocio output ",
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ObtenerProgramacionBuscarProyecto(int EntidadDestinoId,int tramiteid, string bpin, string NombreProyecto)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetProgramacionBuscarProyecto @EntidadDestinoId,@tramiteid,@bpin,@NombreProyecto ",
                                            new SqlParameter("EntidadDestinoId", EntidadDestinoId),
                                            new SqlParameter("tramiteid", tramiteid),
                                            new SqlParameter("bpin", bpin),
                                            new SqlParameter("NombreProyecto", NombreProyecto)
                                              ).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado BorrarTramiteProyecto(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostBorrarTramiteProyecto @json,@Usuario,@errorValidacionNegocio output ",
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public TramitesResultado GuardarDatosInclusion(ProgramacionDistribucionDto ProgramacionDistribucion, string usuario)
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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostInclusionproyecto @json,@Usuario,@errorValidacionNegocio output ",
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ConsultarPoliticasTransversalesCategoriasModificaciones(string Bpin)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetPoliticasTransversalesCategoriasModificaciones @Bpin",
                                              new SqlParameter("Bpin", Bpin)).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarPoliticasTransversalesCategoriasModificaciones(PoliticasTransversalesCategoriasProgramacionDto objIncluirPoliticasDto, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(objIncluirPoliticasDto);
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostPoliticasTransversalesCategoriasModificaciones @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("Usuario", usuario),
                                           outParam
                                           );

                    if (outParam.SqlValue.ToString() == "Null" || string.IsNullOrEmpty(outParam.SqlValue.ToString()))
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string ConsultarPoliticasTransversalesAprobacionesModificaciones(string Bpin)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetPoliticasTransversalesAprobacionesModificaciones @Bpin",
                                              new SqlParameter("Bpin", Bpin)).SingleOrDefault();
            return jsonConsulta;
        }

        #region cargue masivo saldos

        public TramitesResultado RegistrarCargaMasivaSaldos(int TipoCargueId, string usuario)
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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostProcesarCargueMasivo @TipoCargueId,@Usuario,@errorValidacionNegocio output ",
                                            new SqlParameter("TipoCargueId", TipoCargueId),
                                            new SqlParameter("Usuario", usuario),
                                            outParam
                                            );


                    if (outParam.SqlValue.ToString() == "Null")
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

        public string ObtenerLogErrorCargaMasivaSaldos(int? TipoCargueDetalleId, int? CarguesIntegracionId)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetLogCargues @TipoCargueDetalleId, @CarguesIntegracionId",
                                               new SqlParameter("TipoCargueDetalleId", TipoCargueDetalleId),
                                               new SqlParameter("CarguesIntegracionId", CarguesIntegracionId)
                                                ).SingleOrDefault();
            return jsonConsulta;
        }

        public string ObtenerCargaMasivaSaldos(string TipoCargue)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Programacion.UspGetCargaMasivaSaldos @TipoCargue ",
                                               new SqlParameter("TipoCargue", TipoCargue)
                                               ).SingleOrDefault();
            return jsonConsulta;
        }

        public string ObtenerTipoCargaMasiva(string TipoCargue)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Programacion.UspGetTipoCargaMasiva @TipoCargue ",
                                               new SqlParameter("TipoCargue", TipoCargue)
                                               ).SingleOrDefault();
            return jsonConsulta;
         
        }

        public TramitesResultado ValidarCargaMasiva(dynamic jsonListaRegistros, string usuario)
        {
            var respuesta = new TramitesResultado(); 
            var json = JsonUtilidades.ACadenaJson(jsonListaRegistros);

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

                    var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspPostValidaCargueMasivo @json, @usuario, @errorValidacionNegocio output  ",
                                                       new SqlParameter("json", json),
                                                       new SqlParameter("usuario", usuario),
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

        public string ObtenerDetalleCargaMasivaSaldos(int? CargueId)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Programacion.UspGetDetalleCargaMasivaSaldos @CargueId ",
                                               new SqlParameter("CargueId", CargueId)
                                               ).SingleOrDefault();
            return jsonConsulta;
        }

        #endregion cargue masivo saldos

      



        public string ConsultarCatalogoIndicadoresPolitica(string PoliticaId, string Criterio)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetCatalogoIndicadoresPolitica @PoliticaId,@Criterio",
                                              new SqlParameter("PoliticaId", PoliticaId),
                                               new SqlParameter("Criterio", Criterio) ).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarModificacionesAsociarIndicadorPolitica(int proyectoId, int politicaId, int categoriaId, int indicadorId, string accion, string usuario)
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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.UspPostModificacionesAsociarIndicadorPolitica @proyectoId,@politicaId,@categoriaId,@indicadorId,@accion,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("proyectoId", proyectoId),
                                           new SqlParameter("politicaId", politicaId),
                                           new SqlParameter("categoriaId", categoriaId),
                                           new SqlParameter("indicadorId", indicadorId),
                                           new SqlParameter("accion", accion),
                                           new SqlParameter("Usuario", usuario),
                                           outParam
                                           );

                    if (outParam.SqlValue.ToString() == "Null" || string.IsNullOrEmpty(outParam.SqlValue.ToString()))
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

    }
}
