using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Ajustes;
using System.Linq;
using System.Data.SqlClient;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using System;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using System.Data;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using Newtonsoft.Json;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Ajustes;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Data.Entity.Core.Objects;
using RegionalizacionDto = DNP.ServiciosNegocio.Dominio.Dto.CadenaValor.RegionalizacionDto;
using DNP.ServiciosEnrutamiento.Persistencia.Modelo;
using DNP.ServiciosNegocio.Dominio.Dto.Catalogos;
using System.Collections.Generic;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Ajustes
{
    public class AjustesSgpPersistencia : PersistenciaSGP, IAjustesSgpPersistencia
    {
        public AjustesSgpPersistencia(IContextoFactory contextoFactory, IContextoFactorySGR contextoFactorySGR) : base(contextoFactory, contextoFactorySGR)
        {
        }

        #region Horizonte

        public EncabezadoSGPDto ObtenerHorizonteSgp(ParametrosEncabezadoSGP parametros)
        {
            if (parametros.Tramite == null)
            {
                parametros.Tramite = "";
            }
            EncabezadoSGPDto resultado = ContextoOnlySP.Database.SqlQuery<EncabezadoSGPDto>("Proyectos.uspGetObtenerHorizonteProyectoSgp @idInstancia,@idFlujo,@idNivel,@idProyecto,@tramite ",
                              new SqlParameter("idInstancia", parametros.IdInstancia),
                              new SqlParameter("idFlujo", parametros.IdFlujo),
                              new SqlParameter("idNivel", parametros.IdNivel),
                              new SqlParameter("idProyecto", parametros.IdProyecto),
                              new SqlParameter("tramite", parametros.Tramite)
                               ).FirstOrDefault();

            return resultado;
        }

        public RespuestaGeneralDto ActualizarHorizonteSgp(HorizonteProyectoDto datosHorizonteProyecto, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostActualizaHorizonteSgp @idProyecto,@mantiene,@vigenciaInicio,@vigenciaHasta,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("idProyecto", datosHorizonteProyecto.IdProyecto),
                                           new SqlParameter("mantiene", datosHorizonteProyecto.Mantiene),
                                           new SqlParameter("vigenciaInicio", datosHorizonteProyecto.VigenciaInicio),
                                           new SqlParameter("vigenciaHasta", datosHorizonteProyecto.VigenciaFinal),
                                           new SqlParameter("Usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || outParam.SqlValue.ToString() == "Null")
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
                        return respuesta;
                       
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

        public string ObtenerCambiosJustificacionHorizonteSgp(int IdProyecto)
        {
            var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.upsGetEstadoProyectoHorizonteSgp @ProyectoId ",
                                new SqlParameter("ProyectoId", IdProyecto)                                
                                 ).SingleOrDefault();

            return jsonConsulta;
        }

        #endregion

        #region Indicadores

        public IndicadorProductoDto ObtenerIndicadoresProductoSgp(string bpin)
        {
            try
            {
                var indicadorProductoDto = new IndicadorProductoDto();
                var jsonString = ContextoOnlySP.Database.SqlQuery<string>("Productos.uspGetIndicadorProductoSgp_JSON @BPIN ",
                                new SqlParameter("bpin", bpin)
                                 ).SingleOrDefault();

                indicadorProductoDto = JsonConvert.DeserializeObject<IndicadorProductoDto>(jsonString);
                indicadorProductoDto.Bpin = bpin;

                //Calcula el campo metatotalfirmeajustado para los indicadores.

                foreach (var objs in indicadorProductoDto.ObjetivosEspecificos)
                {
                    objs.LabelBotonObjetivo = "+";
                    foreach (var prods in objs.Productos)
                    {
                        prods.LabelBotonProducto = "+";
                        foreach (var indi in prods.Indicadores)
                        {
                            indi.LabelBotonIndicador = "+";
                            double? metaTotalFirmeAjustado = 0;

                            if (indi.IndicadorAcumula == true)
                            {
                                foreach (var vige in indi.Vigencias)
                                {
                                    metaTotalFirmeAjustado = metaTotalFirmeAjustado + vige.MetaVigencialIndicadorAjuste;
                                }
                            }
                            else
                            {
                                metaTotalFirmeAjustado = indi.Vigencias.Max(x => x.MetaVigencialIndicadorAjuste);
                                indi.MetaTotalActual = indi.Vigencias.Max(x => x.MetaVigenciaIndicadorMga);
                                indi.MetaTotalFirme = indi.Vigencias.Max(x => x.MetaVigenciaIndicadorFirme);
                            }

                            indi.MetaTotalFirmeAjustado = metaTotalFirmeAjustado;
                            indi.IndicadorAcumulaAjustado = (bool)(indi?.IndicadorAcumula) ? "SI" : "NO";
                            indi.IndicadorAcumulaOriginal = indi.IndicadorAcumula;
                            indi.IndicadorAcumulaAjustadoOriginal = (bool)(indi?.IndicadorAcumula) ? "SI" : "NO";
                            indi.MetaTotalFirmeAjustadoOriginal = metaTotalFirmeAjustado;

                            foreach (var vige in indi.Vigencias)
                            {
                                vige.MetaVigencialIndicadorAjuste = Double.Parse(String.Format("{0:0.0000}", vige.MetaVigencialIndicadorAjuste));
                                vige.MetaVigencialIndicadorAjusteOriginal = Double.Parse(String.Format("{0:0.0000}", vige.MetaVigencialIndicadorAjuste));
                            }
                        }
                    }
                }

                return indicadorProductoDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public IndicadorResponse GuardarIndicadoresSecundariosSgp(AgregarIndicadoresSecundariosDto parametrosGuardar,string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(parametrosGuardar);
            var respuesta = new IndicadorResponse();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Productos.uspPostIndicadorSecundarioProductoAgregarSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = JsonUtilidades.ACadenaJson(parametrosGuardar);
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public IndicadorResponse EliminarIndicadorProductoSgp(int indicadorId, string usuario)
        {
            var respuesta = new IndicadorResponse();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Productos.uspPostIndicadorProductoEliminarSgp @IdIndicador,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("IdIndicador", indicadorId),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = "Indicador Eliminado Exitosamente!";
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public IndicadorResponse ActualizarMetaAjusteIndicadorSgp(IndicadoresIndicadorProductoDto Indicador, string usuario)
        {
            var IndicadorJson = JsonUtilidades.ACadenaJson(Indicador);
            var respuesta = new IndicadorResponse();
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Productos.uspPostIndicadorProductoActualizarSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", IndicadorJson),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = JsonUtilidades.ACadenaJson(Indicador);
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region Beneficiarios

        public string ObtenerProyectosBeneficiariosSgp(string Bpin)
        {
            var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.upsGetProyectosBeneficiariosSgp_JSON @BPIN ",
                                new SqlParameter("BPIN", Bpin)
                                 ).SingleOrDefault();

            return jsonConsulta;
        }

        public string ObtenerProyectosBeneficiariosDetalleSgp(string Json)
        {
            var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.upsGetProyectosBeneficiariosDetalleSgp_JSON @Json ",
                                new SqlParameter("Json", Json)
                                 ).SingleOrDefault();

            return jsonConsulta;
        }

        public void GuardarBeneficiarioTotalesSgp(BeneficiarioTotalesDto beneficiario, string usuario)
        {
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostBeneficiarioTotalesSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(beneficiario)),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || outParam.SqlValue.ToString() == "Null")
                    {                        
                        ContextoOnlySP.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
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
            }
        }

        public void GuardarBeneficiarioProductoSgp(BeneficiarioProductoSgpDto beneficiario, string usuario)
        {
            using (var dbContextTransaction = ContextoOnlySP.Database.BeginTransaction())
            {
                var prueba = JsonUtilidades.ACadenaJson(beneficiario);
                try
                {
                    var outParam = new SqlParameter
                    {
                        ParameterName = "errorValidacionNegocio",
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Output,
                        Size = 500
                    };

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostBeneficiarioProductoSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(beneficiario)),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || outParam.SqlValue.ToString() == "Null")
                    {
                        ContextoOnlySP.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (ServiciosNegocioException)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception ee)
                {
                  
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void GuardarBeneficiarioProductoLocalizacionSgp(BeneficiarioProductoLocalizacionDto beneficiario, string usuario)
        {
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostBeneficiarioProductoLocalizacionSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(beneficiario)),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || outParam.SqlValue.ToString() == "Null")
                    {
                        ContextoOnlySP.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
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
            }
        }

        public void GuardarBeneficiarioProductoLocalizacionCaracterizacionSgp(BeneficiarioProductoLocalizacionCaracterizacionDto beneficiario, string usuario)
        {
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostBeneficiarioProductoLocalizacionCaracterizacionSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(beneficiario)),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || outParam.SqlValue.ToString() == "Null")
                    {
                        ContextoOnlySP.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
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
            }
        }

        #endregion

        #region Localizaciones

        public ResultadoProcedimientoDto GuardarLocalizacionSgp(LocalizacionProyectoAjusteDto localizacionProyecto, string usuario)
        {
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostLocalizacion_AjustesSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(localizacionProyecto)),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()) || outParam.SqlValue.ToString() == "Null")
                    {
                        respuesta.Exito = true;
                        //var capituloModificado = new CapituloModificado()
                        //{
                        //    InstanciaId = localizacionProyecto.InstanciaId,
                        //    Justificacion = string.IsNullOrEmpty(localizacionProyecto.Justificacion) ? null : localizacionProyecto.Justificacion,
                        //    ProyectoId = localizacionProyecto.ProyectoId,
                        //    SeccionCapituloId = localizacionProyecto.SeccionCapituloId,
                        //    Usuario = usuario,
                        //    AplicaJustificacion = 1
                        //};
                        //_seccionCapituloPersistencia.GuardarJustificacionCambios(capituloModificado);
                        ContextoOnlySP.SaveChanges();
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Exito = false;
                        var mensajeError = Convert.ToString(outParam.SqlValue.ToString());
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
            }
        }

        #endregion
        #region fuentefinanciacion
        public string FuentesFinanciacionRecursosAjustesAgregarSgp(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var respuesta = new ResultadoProcedimientoDto();
            var Json = JsonUtilidades.ACadenaJson(objFuenteFinanciacionAgregarAjusteDto);
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
                    var recursos = ContextoOnlySP.Database.ExecuteSqlCommand("Exec FuentesFinanciacion.uspPostFuentesFinanciacionRecursosAjustesAgregarSgp @JSON, @USUARIO,@errorValidacionNegocio output",
                                                       new SqlParameter("JSON", Json),
                                                        new SqlParameter("USUARIO", usuario),
                                                        outParam
                                                        );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return "OK";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (Exception e)
                {

                    throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
                }
            }
        }
        #endregion
        #region costos
        public ObjectivosAjusteDto ObtenerResumenObjetivosProductosActividadesSgp(string bpin)
        {
            try
            {
                var Ajuste = ContextoOnlySP.Database.SqlQuery<string>("FuentesFinanciacion.UspGetResumenObjetivosProductosActividadesSgp @BPIN ",
                                                   new SqlParameter("BPIN", bpin)
                                                    ).SingleOrDefault();

                return JsonConvert.DeserializeObject<ObjectivosAjusteDto>(Ajuste);
            }
            catch (Exception e)
            {

                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void GuardarAjusteCostoActividadesSgp(ProductoAjusteDto producto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var respuesta = new ResultadoProcedimientoDto();
            var Json = JsonUtilidades.ACadenaJson(producto);
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
                    var Ajuste = ContextoOnlySP.Database.ExecuteSqlCommand("Exec CadenaValor.uspPostCadenaValor_AjusteCostoActividadesSgp @JSON,@USUARIO,@errorValidacionNegocio output",
                                                       new SqlParameter("JSON", Json),
                                                       new SqlParameter("USUARIO", usuario),
                                                       outParam
                                                    );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (Exception e)
                {

                    throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
                }
            }
        }

        public void AgregarEntregableSgp(AgregarEntregable[] entregables, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var respuesta = new ResultadoProcedimientoDto();
            var Json = JsonUtilidades.ACadenaJson(entregables);
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
                    var entregrable = ContextoOnlySP.Database.ExecuteSqlCommand("Exec CadenaValor.uspPostCadenaValor_AgregarEntregableSgp @JSON,@USUARIO,@errorValidacionNegocio output ",
                                                           new SqlParameter("JSON", Json),
                                                           new SqlParameter("USUARIO", usuario),
                                                           outParam
                                                        );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (Exception e)
                {

                    throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
                }
            }
        }

        public void EliminarEntregableSgp(EntregablesActividadesDto entregable)
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
                    var entregrable = ContextoOnlySP.Database.ExecuteSqlCommand("Exec CadenaValor.uspPostCadenaValor_EliminarEntregableSgp @ACTIVIDADID,@errorValidacionNegocio output",
                                                       new SqlParameter("@ACTIVIDADID", entregable.EntregableActividadId),
                                                       outParam
                                                    );
                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        throw new ServiciosNegocioException(mensajeError);
                    }
                }
                catch (Exception e)
                {

                    throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
                }
            }
        }
        #endregion
        #region regionalizacion
        public RegionalizacionDto RegionalizacionGeneralSgp(string bpin)
        {

            try
            {
                var jsonString = ContextoOnlySP.Database.SqlQuery<string>("Exec Productos.UspGetDesagregarRegionalizacion_Ajustes_JSON_Sgp @BPIN",
                new SqlParameter("BPIN", bpin)
                 ).SingleOrDefault();

                return JsonConvert.DeserializeObject<RegionalizacionDto>(jsonString);
            }
            catch (Exception e)
            {

                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        #endregion
        #region recursos
        public List<CatalogoDto> ConsultarTiposRecursosEntidadSgp(int entityTypeCatalogId, int entityType) {
            try
            {
                var jsonString = ContextoOnlySP.Database.SqlQuery<string>("Exec FuentesFinanciacion.UspGetConsultarTiposRecursosEntidad_Ajustes_JSON_Sgp @ENTIDADID, @TIPOENTIDADID",
                new SqlParameter("ENTIDADID", entityTypeCatalogId),
                new SqlParameter("TIPOENTIDADID", entityType)
                 ).SingleOrDefault();

                return JsonConvert.DeserializeObject<List<CatalogoDto>>(jsonString);
            }
            catch (Exception e)
            {

                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }

        }
        #endregion
        public string ObtenerCategoriasFocalizacionJustificacionSgp(string Bpin)
        {
            var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasJustificacion_Ajustes_JSONSgp @BPIN",
                                new SqlParameter("BPIN", Bpin)
                                 ).SingleOrDefault();

            return jsonConsulta;
        }
        public string ObtenerDetalleCategoriasFocalizacionJustificacionSgp(string Bpin)
        {
            var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Focalizacion.uspGetPoliticasTransversalesCategorias_ObtenerDetalleAjusteSgp @BPIN",
                                new SqlParameter("BPIN", Bpin)
                                 ).SingleOrDefault();

            return jsonConsulta;
        }
    }
}
