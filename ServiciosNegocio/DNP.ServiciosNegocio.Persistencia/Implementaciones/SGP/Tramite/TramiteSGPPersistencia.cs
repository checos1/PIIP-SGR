namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Tramite
{
    using DNP.ServiciosNegocio.Comunes;
    using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Dominio.Dto.Transversales;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Tramite;
    using DNP.ServiciosNegocio.Persistencia.Modelo;
    using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
    using Interfaces;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Core.Objects;
    using System.Data.SqlClient;
    using System.Linq;

    public class TramiteSGPPersistencia: Persistencia, ITramiteSGPPersistencia
    {
        #region Constructor

        public TramiteSGPPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion

        public TramitesResultado ActualizaEstadoAjusteProyecto(string tipoDevolucion, string objetoNegocioId, int tramiteId, string observacion, string usuario)
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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostActualizaEstadoAjusteProyectoSGP @ObjetoNegocioId,@TramiteId,@Observacion,@tipoDevolucion,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("ObjetoNegocioId", objetoNegocioId),
                                           new SqlParameter("TramiteId", tramiteId),
                                           new SqlParameter("Observacion", observacion),
                                           new SqlParameter("tipoDevolucion", tipoDevolucion),
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

        public TramitesResultado EliminarProyectoTramiteNegocio(int TramiteId, int ProyectoId)
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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostEliminarProyectosSGP @TramiteId,@ProyectoId,@errorValidacionNegocio output ",
                                           new SqlParameter("TramiteId", TramiteId),
                                           new SqlParameter("ProyectoId", ProyectoId),
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

        public TramitesResultado GuardarTramiteInformacionPresupuestal(List<TramiteFuentePresupuestalDto> parametrosGuardar, string usuario)
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostActualizaValoresInfomracionPresupuestalSGP @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(parametrosGuardar)),
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

        /// <summary>
        ///  Funcion para Registrar los Datos Incorporacion
        /// </summary>
        /// <param name="parametrosGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public RespuestaGeneralDto GuardarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));



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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostDatosAdicionSgp @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido)),
                                           new SqlParameter("Usuario", usuario),
                                           outParam
                                           );


                    if (outParam.SqlValue.ToString() == "Null" || string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
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
                    return respuesta;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
        public RespuestaGeneralDto eliminarDatosAdicionSgp(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostEliminarDatosAdicionSgp @json,@Usuario,@errorValidacionNegocio output ",
                                          new SqlParameter("json", JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido)),
                                          new SqlParameter("Usuario", usuario),
                                          outParam
                                          );

                    if (outParam.SqlValue.ToString() == "Null" || string.IsNullOrEmpty(outParam.SqlValue.ToString()))
                    {
                        respuesta.Exito = true;
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        respuesta.Exito = false;
                        respuesta.Mensaje = mensajeError;
                        return respuesta;
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

        public string ObtenerDatosAdicionSgp(int TramiteId)
        {
            try
            {

                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Tramites.uspGetDatosAdicionSgp @TramiteId ",
                                            new SqlParameter("TramiteId", TramiteId)
                                             ).SingleOrDefault();
                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }


        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentes(int tramiteId)
        {
            List<ProyectoTramiteFuenteDto> lista = new List<ProyectoTramiteFuenteDto>();

            try
            {
                var jsonString = ContextoOnlySP.Database.SqlQuery<string>("Tramites.uspGetProyectoFuentePresupuestalPorTramite_TrasladoSGP @TramiteId",
                            new SqlParameter("TramiteId", tramiteId)
                            ).SingleOrDefault();

                var listatmp = JsonConvert.DeserializeObject<List<uspGetProyectoFuentePresupuestalPorTramite_Traslado_Result>>(jsonString);

                foreach (var item in listatmp)
                {
                    if (lista.Where(x => x.ProyectoId == item.proyectoId).FirstOrDefault() == null)
                    {
                        ProyectoTramiteFuenteDto prop = new ProyectoTramiteFuenteDto();
                        prop.ProyectoId = item.proyectoId.Value;
                        prop.BPIN = item.Bpin;
                        prop.NombreProyecto = item.NombreProyecto;
                        prop.Operacion = item.tipoProyecto;
                        prop.TramiteProyectoId = item.TramiteProyectoId.HasValue ? item.TramiteProyectoId.Value : 0;
                        prop.ValorTotalNacion = item.ValorTotalNacion.HasValue ? item.ValorTotalNacion.Value : 0;
                        prop.ValorTotalPropios = item.ValorTotalPropios.HasValue ? item.ValorTotalPropios.Value : 0;
                        lista.Add(prop);
                    }
                }

                foreach (var item in lista)
                {
                    item.ListaFuentes = new List<FuenteFinanciacionDto>();
                    var listafuenteTmp = listatmp.ToList().Where(x => x.proyectoId == item.ProyectoId);
                    foreach (var itemFuente in listafuenteTmp)
                    {
                        FuenteFinanciacionDto fte = new FuenteFinanciacionDto
                        {
                            FuenteId = itemFuente.FuenteId,
                            NombreCompleto = itemFuente.Nombre,
                            GrupoRecurso = itemFuente.Origen,
                            TipoValorContracreditoCSF = itemFuente.idTipoValorContracreditoCSF.HasValue ? itemFuente.idTipoValorContracreditoCSF.Value : 0,
                            TipoValorContracreditoSSF = itemFuente.idTipoValorContracreditoSSF.HasValue ? itemFuente.idTipoValorContracreditoSSF.Value : 0,
                            ValorIncialCSF = itemFuente.ValorInicialCSF,
                            ValorIncialSSF = itemFuente.ValorInicialSSF,
                            ValorVigenteSSF = itemFuente.ValorVigenteSSF,
                            ValorVigenteCSF = itemFuente.ValorVigenteCSF,
                            ValorContracreditoCSF = itemFuente.ValorContracreditoCSF,
                            ValorContracreditoSSF = itemFuente.ValorContracreditoSSF
                        };
                        item.ListaFuentes.Add(fte);
                    }
                }

                return lista;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesResultado GuardarFuentesTramiteProyectoAprobacion(List<FuentesTramiteProyectoAprobacionDto> fuentesTramiteProyectoAprobacion, string usuario)
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

                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostGuardarFuentesTramiteProyectoAprobacionSGP @json,@Usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", JsonUtilidades.ACadenaJson(fuentesTramiteProyectoAprobacion)),
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

        public List<ProyectoTramiteFuenteDto> ObtenerListaProyectosFuentesAprobado(int tramiteId)
        {
            List<ProyectoTramiteFuenteDto> lista = new List<ProyectoTramiteFuenteDto>();

            try
            {
                var jsonString = ContextoOnlySP.Database.SqlQuery<string>("Tramites.uspGetProyectoFuentePresupuestalAprobadoPorTramite_TrasladoSGP @TramiteId",
                        new SqlParameter("TramiteId", tramiteId)
                        ).SingleOrDefault();

                var listatmp = JsonConvert.DeserializeObject<List<uspGetProyectoFuentePresupuestalAprobadoPorTramite_Traslado_Result>>(jsonString);

                foreach (var item in listatmp)
                {
                    if (lista.Where(x => x.ProyectoId == item.proyectoId).FirstOrDefault() == null)
                    {
                        ProyectoTramiteFuenteDto prop = new ProyectoTramiteFuenteDto();
                        prop.ProyectoId = item.proyectoId.Value;
                        prop.BPIN = item.Bpin;
                        prop.NombreProyecto = item.NombreProyecto;
                        prop.Operacion = item.tipoProyecto;
                        prop.TramiteProyectoId = item.TramiteProyectoId.HasValue ? item.TramiteProyectoId.Value : 0;
                        prop.ValorTotalNacion = item.ValorTotalNacion.HasValue ? item.ValorTotalNacion.Value : 0;
                        prop.ValorTotalPropios = item.ValorTotalPropios.HasValue ? item.ValorTotalPropios.Value : 0;
                        lista.Add(prop);
                    }
                }

                foreach (var item in lista)
                {
                    item.ListaFuentes = new List<FuenteFinanciacionDto>();
                    var listafuenteTmp = listatmp.ToList().Where(x => x.proyectoId == item.ProyectoId);
                    foreach (var itemFuente in listafuenteTmp)
                    {
                        FuenteFinanciacionDto fte = new FuenteFinanciacionDto
                        {
                            FuenteId = itemFuente.FuenteId,
                            NombreCompleto = itemFuente.Nombre,
                            GrupoRecurso = itemFuente.Origen,
                            TipoValorAprobadoCSF = itemFuente.idTipoValorAprobadoCSF.HasValue ? itemFuente.idTipoValorAprobadoCSF.Value : 0,
                            TipoValorAprobadoSSF = itemFuente.idTipoValorAprobadoSSF.HasValue ? itemFuente.idTipoValorAprobadoSSF.Value : 0,
                            ValorIncialCSF = itemFuente.ValorInicialCSF,
                            ValorIncialSSF = itemFuente.ValorInicialSSF,
                            ValorVigenteSSF = itemFuente.ValorVigenteSSF,
                            ValorVigenteCSF = itemFuente.ValorVigenteCSF,
                            ValorContracreditoCSF = itemFuente.ValorSolicitadoCSF,
                            ValorContracreditoSSF = itemFuente.ValorSolicitadoSSF,
                            ValorAprobadoCSF = itemFuente.ValorAprobadoCSF,
                            ValorAprobadoSSF = itemFuente.ValorAprobadoSSF
                        };
                        item.ListaFuentes.Add(fte);
                    }
                }

                return lista;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesResultado GuardarTramiteTipoRequisito(List<TramiteRequitoDto> parametrosGuardar, string usuario)
        {
            var respuesta = new TramitesResultado();
            bool? isActa = null;

            if (parametrosGuardar != null)
            {
                if (parametrosGuardar.Count > 0)
                {
                    var tipoRequisito = parametrosGuardar.First().IdTipoRequisito;
                    if (tipoRequisito == 3) isActa = true;
                    else if (tipoRequisito == 4) isActa = false;
                }
            }

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    if (parametrosGuardar.Count > 1 || (parametrosGuardar.Count == 1 && parametrosGuardar.First().Descripcion != "BorrarTodo"))
                    {
                        var sinIdProyectoTramite = parametrosGuardar.Where(a => a.IdProyectoTramite == 0);
                        if (sinIdProyectoTramite.Count() != 0)
                        {
                            var tramiteId = parametrosGuardar.First().IdTramite;
                            var proyectoId = parametrosGuardar.First().IdProyecto;
                            var tramiteProyectoCollection = Contexto.Proyectos.Where(tramiteProyecto => tramiteProyecto.TramiteId == tramiteId && tramiteProyecto.ProyectoId == proyectoId).ToArray();
                            if (tramiteProyectoCollection.Length == 1)
                            {
                                var corregida = sinIdProyectoTramite.Select(tramiteProyecto =>
                                {
                                    tramiteProyecto.IdProyectoTramite = tramiteProyectoCollection.First().Id;
                                    return tramiteProyecto;
                                });
                                parametrosGuardar = parametrosGuardar.Except(sinIdProyectoTramite).ToList();
                                parametrosGuardar.AddRange(corregida);
                            }
                        }

                        var outParam = new SqlParameter
                        {
                            ParameterName = "errorValidacionNegocio",
                            SqlDbType = SqlDbType.VarChar,
                            Direction = ParameterDirection.Output,
                            Size = 500
                        };

                        if (isActa.HasValue)
                        {
                            if (isActa.Value)
                            {
                                var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostActualizaValoresTipoRequisitoSGP @json,@Usuario,@errorValidacionNegocio output ",
                                               new SqlParameter("json", JsonUtilidades.ACadenaJson(parametrosGuardar)),
                                               new SqlParameter("Usuario", usuario),                              
                                               outParam
                                               );
                            }                          
                        }
                        else throw new Exception("tipo de requisito no soportado");

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
                    else if (parametrosGuardar.Count == 1 && parametrosGuardar.First().Descripcion == "BorrarTodo")
                    {
                        var tramiteId = parametrosGuardar.First().IdTramite;
                        var proyectoId = parametrosGuardar.First().IdProyecto;
                        var tipoRequisitoId = isActa.HasValue && isActa.Value ? 3 : 4;
                        var tramiteProyectoId = Contexto.Proyectos.Where(tramiteProyecto => tramiteProyecto.TramiteId == tramiteId && tramiteProyecto.ProyectoId == proyectoId).FirstOrDefault()?.Id ?? 0;
                        var tramiteProyectoRequisito = Contexto.ProyectosRequisitos.Where(proyectoRequisitos => proyectoRequisitos.Proyectos.Id == tramiteProyectoId && proyectoRequisitos.TipoRequisitoId == tipoRequisitoId).ToArray();
                        if (tramiteProyectoRequisito.Length == 1) Contexto.ProyectosRequisitos.Remove(tramiteProyectoRequisito[0]);
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                    }
                    return respuesta;
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

        public IEnumerable<ProyectoRequisitoDto> ObtenerProyectoRequisitosPorTramite(int pProyectoId, int? pTramiteId, bool isCDP)
        {
            List<ProyectoRequisitoDto> listatmp = new List<ProyectoRequisitoDto>();

            if (isCDP)
            {
                var jsonString = ContextoOnlySP.Database.SqlQuery<string>("Tramites.uspGetProyectoRequisitosPorTramiteSGP @ProyectoId, @TramiteId",
                                    new SqlParameter("ProyectoId", pProyectoId),
                                    new SqlParameter("TramiteId", pTramiteId)
                                    ).SingleOrDefault();

                var result = JsonConvert.DeserializeObject<List<uspGetProyectoRequisitosPorTramite_Result>>(jsonString);

                if (result.Count != 0)
                    listatmp = result
                        .Select(x => new { x.Id, x.TramiteProyectoId, x.Descripcion, x.Numero, x.NumeroContrato, x.Fecha, x.UnidadEjecutora, x.TipoRequisitoId })
                        .Distinct()
                        .ToList().ConvertAll(item => new ProyectoRequisitoDto
                        {
                            Id = item.Id,
                            Descripcion = item.Descripcion,
                            TramiteProyectoId = item.TramiteProyectoId,
                            Numero = item.Numero,
                            NumeroContrato = item.NumeroContrato,
                            Fecha = item.Fecha,
                            UnidadEjecutora = item.UnidadEjecutora,
                            TipoRequisitoId = item.TipoRequisitoId
                        }).ToList();

                foreach (var item in listatmp)
                {
                    item.ListaTiposRequisito = result
                         .Where(x => x.Id == item.Id)
                    .Select(x => new { x.TipoRequisitoId, x.TipoRequisitoDescripcion, x.TipoRequisito })
                    .Distinct()
                    .ToList().ConvertAll(f => new TipoRequisitoDto
                    {
                        Id = f.TipoRequisitoId,
                        TipoRequisito = f.TipoRequisito,
                        Descripcion = f.TipoRequisitoDescripcion,
                    }).ToList();

                }

                foreach (var item in listatmp)
                {
                    foreach (var itemfuente in item.ListaTiposRequisito)
                    {
                        itemfuente.ListaValores = result
                             .Where(x => x.TipoRequisitoId == itemfuente.Id && x.Id == item.Id)
                        .Select(x => new { x.proyectorequisitovalorid, x.TipoRequisitoId, x.TipoValorId, x.TipoValor, x.Valor })
                        .Distinct()
                        .ToList().ConvertAll(v => new ProyectoRequisitoValoresDto
                        {
                            Id = v.proyectorequisitovalorid,
                            ProyectosRequisitoId = v.TipoRequisitoId,
                            TipoValor = new TipoValorDto { Id = v.TipoValorId, TipoValorFuente = v.TipoValor },
                            Valor = v.Valor
                        }).ToList();
                    }
                }
            }
            else
            {
                var jsonString = ContextoOnlySP.Database.SqlQuery<string>("Tramites.uspGetProyectoRequisitosPorTramiteCRPSGP @ProyectoId, @TramiteId",
                                    new SqlParameter("ProyectoId", pProyectoId),
                                    new SqlParameter("TramiteId", pTramiteId)
                                    ).SingleOrDefault();

                var result = JsonConvert.DeserializeObject<List<uspGetProyectoRequisitosPorTramiteCRP_Result>>(jsonString);

                if (result.Count != 0)
                    listatmp = result
                        .Select(x => new { x.Id, x.TramiteProyectoId, x.Descripcion, x.Numero, x.NumeroContrato, x.Fecha, x.UnidadEjecutora, x.TipoRequisitoId })
                        .Distinct()
                        .ToList().ConvertAll(item => new ProyectoRequisitoDto
                        {
                            Id = item.Id,
                            Descripcion = item.Descripcion,
                            TramiteProyectoId = item.TramiteProyectoId,
                            TipoRequisitoId = item.TipoRequisitoId,
                            Numero = item.Numero,
                            NumeroContrato = item.NumeroContrato,
                            Fecha = item.Fecha,
                            UnidadEjecutora = item.UnidadEjecutora
                        }).ToList();

                foreach (var item in listatmp)
                {
                    item.ListaTiposRequisito = result
                         .Where(x => x.Id == item.Id)
                    .Select(x => new { x.TipoRequisitoId, x.TipoRequisitoDescripcion, x.TipoRequisito })
                    .Distinct()
                    .ToList().ConvertAll(f => new TipoRequisitoDto
                    {
                        Id = f.TipoRequisitoId,
                        TipoRequisito = f.TipoRequisito,
                        Descripcion = f.TipoRequisitoDescripcion,
                    }).ToList();
                }

                foreach (var item in listatmp)
                {
                    foreach (var itemfuente in item.ListaTiposRequisito)
                    {
                        itemfuente.ListaValores = result
                             .Where(x => x.TipoRequisitoId == itemfuente.Id && x.Id == item.Id)
                        .Select(x => new { x.proyectorequisitovalorid, x.TipoRequisitoId, x.TipoValorId, x.TipoValor, x.Valor })
                        .Distinct()
                        .ToList().ConvertAll(v => new ProyectoRequisitoValoresDto
                        {
                            Id = v.proyectorequisitovalorid,
                            ProyectosRequisitoId = v.TipoRequisitoId,
                            TipoValor = new TipoValorDto { Id = v.TipoValorId, TipoValorFuente = v.TipoValor },
                            Valor = v.Valor
                        }).ToList();
                    }
                }

            }

            return listatmp.AsEnumerable<ProyectoRequisitoDto>();
        }        

        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosContracredito(string tipoEntidad, int? idEntidad, Guid idFLujo)
        {
            var result = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.uspGetProyectosContracreditoSgp @tipoEntidad, @entidadId, @idFlujoParametro",
                        new SqlParameter("tipoEntidad", tipoEntidad),
                        new SqlParameter("entidadId", idEntidad),
                        new SqlParameter("idFlujoParametro", idFLujo)
                        ).SingleOrDefault();

            var proyectos = JsonConvert.DeserializeObject<List<ProyectoCreditoDto>>(result);

            return proyectos;
        }


        public IEnumerable<ProyectoCreditoDto> ObtenerProyectosCredito(string tipoEntidad, int idEntidad, Guid idFLujo)
        {
            var result = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.uspGetProyectosCreditoSgp @tipoEntidad, @entidadId, @idFlujoParametro",
                        new SqlParameter("tipoEntidad", tipoEntidad),
                        new SqlParameter("entidadId", idEntidad),
                        new SqlParameter("idFlujoParametro", idFLujo)
                        ).SingleOrDefault();

            var proyectos = JsonConvert.DeserializeObject<List<ProyectoCreditoDto>>(result);

            return proyectos;
        }

        public string ObtenerTiposValorPorEntidad(int IdEntidad, int IdTipoEntidad)
        {
            var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.uspGetTipoValorPorEntidadSgp @IdEntidad,@IdTipoEntidad ",
                                new SqlParameter("IdEntidad", IdEntidad),
                                new SqlParameter("IdTipoEntidad", IdTipoEntidad)
                                ).SingleOrDefault();

            return jsonConsulta;
        }
    }
}
