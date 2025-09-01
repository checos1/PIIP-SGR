using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.GestionRecursos;
using System;
using System.Linq;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;
using DNP.ServiciosNegocio.Dominio.Dto.Productos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR;
using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using System.Data.SqlClient;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System.Data;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.GestionRecursos
{
   public class GestionRecursosSgpPersistencia: Persistencia, IGestionRecursosSgpPersistencia
    {
        public GestionRecursosSgpPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }


        public string ObtenerlocalizacionSgp(string bpin)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.UspGetLocalizacionSgp_JSON @BPIN ",
                                               new SqlParameter("BPIN", bpin)
                                                ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public string ObtenerFocalizacionPoliticasTransversalesFuentesSgp(string bpin)
        {
            try
            {
                
                   var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasTransversalesFuentes_JSONSgp @BPIN ",
                                               new SqlParameter("BPIN", bpin)
                                                ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public string ObtenerPoliticasTransversalesProyectoSgp(string Bpin)
        {
            try
            {

                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasTransversalesFuentes_JSONSgp @BPIN ",
                                            new SqlParameter("BPIN", Bpin)
                                             ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesResultado EliminarPoliticasProyectoSgp(int tramiteidProyectoId, int politicaId)
        {
            var respuesta = new TramitesResultado();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Tramites.uspPostEliminarPoliticasProyectoSgp @tramiteidProyectoId,@politicaId,@errorValidacionNegocio output ",
                                           new SqlParameter("tramiteidProyectoId", tramiteidProyectoId),
                                           new SqlParameter("politicaId", politicaId),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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
        public TramitesResultado AgregarPoliticasTransversalesSgp(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido);
            var respuesta = new TramitesResultado();
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

                    
                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Focalizacion.uspPostAgregarPoliticas_ajustesSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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
        
        public string ConsultarPoliticasCategoriasIndicadoresSgp(Guid instanciaId)
        {
            try
            {
                string insta = instanciaId.ToString();

                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasCategoriasIndicadores_JSONSgp @InstanciaId ",
                                new SqlParameter("InstanciaId", insta)
                                  ).SingleOrDefault();
                return jsonConsulta;


            
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }


        public TramitesResultado ModificarPoliticasCategoriasIndicadoresSgp(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {

            var json = JsonUtilidades.ACadenaJson(parametrosGuardar);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Focalizacion.uspPostCategoriasIndicadoresSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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

        public string ObtenerPoliticasTransversalesCategoriasSgp(string instanciaId)
        {
            try
            {
                var instanciaIdParameter = new SqlParameter("InstanciaId", instanciaId);


                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasTransversalesCategorias_Ajustes_JSONSgp @InstanciaId ",
                               instanciaIdParameter
                                  ).SingleOrDefault();
                return jsonConsulta;



            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }


        public TramitesResultado EliminarCategoriaPoliticasProyectoSgp(int proyectoId, int politicaId, int categoriaId)
        {

            var respuesta = new TramitesResultado();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Focalizacion.uspPostEliminarCategoriasProyectoSgp @ProyectoId,@PoliticaId,@DimensionId,@errorValidacionNegocio output ",
                                           new SqlParameter("ProyectoId", proyectoId),
                                           new SqlParameter("PoliticaId", politicaId),
                                            new SqlParameter("DimensionId", categoriaId),   
                                           outParam
                                           );
                    if(((System.Data.SqlTypes.SqlString)outParam.SqlValue).IsNull)
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(((System.Data.SqlTypes.SqlString)outParam.SqlValue).Value);
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


        public TramitesResultado GuardarFocalizacionCategoriasAjustesSgp(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {

            var json = JsonUtilidades.ACadenaJson(focalizacionCategoriasAjuste);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Focalizacion.uspPostPoliticasTransversalesCategoriasAjustesSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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

        public string GetCategoriasSubcategoriasSgp(int padreId, int? entidadId, int esCategoria, int esGruposEtnicos)
        {

            try
            {

                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetCategoriaSubcategoria_JSONSgp @padreId,@entidadId,@esCategoria,@esGruposEtnicos ",
                                              new SqlParameter("padreId", padreId),
                                           new SqlParameter("entidadId", entidadId),
                                                new SqlParameter("esCategoria", esCategoria),
                                           new SqlParameter("esGruposEtnicos", esGruposEtnicos)
                                  ).SingleOrDefault();
                return jsonConsulta;



            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesResultado GuardarCategoriasPoliticaTransversalesAjustesSgp(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario)
        {
  
            var json = JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Focalizacion.uspPostAgregarCategoriaPolitica_ajustesSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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
        public string ObtenerCrucePoliticasAjustesSgp(Guid instanciaId)
        {
            try
            {
                var instanciaIdParameter = new SqlParameter("InstanciaId", instanciaId);


                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetCrucePoliticasAjustes_JSONSgp @InstanciaId ",
                               instanciaIdParameter
                                  ).SingleOrDefault();
                return jsonConsulta;



            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string ObtenerPoliticasTransversalesResumenSgp(Guid instanciaId)
        {
            try
            {
                var instanciaIdParameter = new SqlParameter("InstanciaId", instanciaId);


                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasTransversalesCategorias_Ajustes_Resumen_JSONSgp @InstanciaId ",
                               instanciaIdParameter
                                  ).SingleOrDefault();
                return jsonConsulta;



            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesResultado GuardarCrucePoliticasAjustesSgp(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario)
        {
 
            var json = JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Focalizacion.uspPostAgregarPoliticas_ajustesSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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

        public string ObtenerDesagregarRegionalizacionSgp(string bpin)
        {
            
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Productos.UspGetDesagregarRegionalizacion_JSONSgp @BPIN ",
                                            new SqlParameter("BPIN", bpin)
                                             ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }


        }

        public string ObtenerFuenteFinanciacionVigenciaSgp(string bpin)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("FuentesFinanciacion.UspGetAgregarFuenteVigenciaSgp @BPIN ",
                                            new SqlParameter("BPIN", bpin)
                                             ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string ObtenerFuentesProgramarSolicitadoSgp(string bpin)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("FuentesFinanciacion.uspGetFuentesProgramarSolicitado_JSONSgp @BPIN ",
                                            new SqlParameter("BPIN", bpin)
                                             ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public TramitesResultado EliminarFuentesFinanciacionProyectoSgp(int fuentesFinanciacionId)
        {
            var respuesta = new TramitesResultado();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec FuentesFinanciacion.uspPostFuentesFinanciacionEliminarSgp @FuentesFinanciacionId,@errorValidacionNegocio output ",
                                           new SqlParameter("FuentesFinanciacionId", fuentesFinanciacionId),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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

        public TramitesResultado GuardarFuentesProgramarSolicitadoSgp(ProgramacionValorFuenteDto objProgramacionValorFuenteDto, string usuario)
        {

            var json = JsonUtilidades.ACadenaJson(objProgramacionValorFuenteDto);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec FuentesFinanciacion.uspPostProgramacionValorFuenteSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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
        public string ObtenerDatosAdicionalesFuenteFinanciacionSgp(int fuenteId)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("FuentesFinanciacion.UspGetDatosAdicionalesFinanciacion_JSONSgp @fuenteId ",
                                            new SqlParameter("fuenteId", fuenteId)
                                             ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public TramitesResultado GuardarDatosAdicionalesSgp(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario)
        {

            var json = JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec FuentesFinanciacion.uspPostDatosAdicionalesSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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

        public TramitesResultado EliminarDatosAdicionalesSgp(int coFinanciacionId)
        {

            var respuesta = new TramitesResultado();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec FuentesFinanciacion.uspPostDatosAdicionalesFinanciacionEliminarSgp @CofinanciadorId,@errorValidacionNegocio output ",
                                           new SqlParameter("CofinanciadorId", coFinanciacionId),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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

        public TramitesResultado GuardarFuenteFinanciacionSgp(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar,
                                        string usuario)
        {

            var json = JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec FuentesFinanciacion.uspPostFuenteFinanciacionAgregarSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        ContextoOnlySP.SaveChanges();
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

        public string ObtenerDatosIndicadoresPoliticaSgp(string Bpin)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasTransversalesFuentesIndicadoresSgp_JSON @BPIN ", 
                                              new SqlParameter("BPIN", Bpin)).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string ObtenerDatosCategoriaProductosPoliticaSgp(string Bpin, int fuenteId, int politicaId)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasTransversalesCategoriasSgp_JSON @BPIN,@FuenteId,@PoliticaId ",
                                              new SqlParameter("BPIN", Bpin),
                                              new SqlParameter("FuenteId", fuenteId),
                                              new SqlParameter("PoliticaId", politicaId)).SingleOrDefault();
                return jsonConsulta;

            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public string GuardarDatosSolicitudRecursosSgp(ParametrosGuardarDto<CategoriaProductoPoliticaDto> categoriaProductoPoliticaDto, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(categoriaProductoPoliticaDto.Contenido);
            var respuesta = new TramitesResultado();
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Focalizacion.uspPostPoliticasTransversalesCategoriasAjustesSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        ContextoOnlySP.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
                        return mensajeError;
                    }
                }
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    return ex.Message;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return ex.Message;
                }
            }            
        }
    }
}
