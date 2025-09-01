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
    using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
    using DNP.ServiciosNegocio.Dominio.Dto.IndicadoresPolitica;
    using DNP.ServiciosNegocio.Dominio.Dto.PoliticasIndicadoresCategorias;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using Newtonsoft.Json;

    public class FuenteFinanciacionPersistencia : Persistencia, IFuenteFinanciacionPersistencia
    {
        public FuenteFinanciacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();

        }

        public FuenteFinanciacionProyectoDto ObtenerFuenteFinanciacionProyecto(string bpin)
        {
            var listadoFuentesFinanciacion = Contexto.uspGetFuentesFinanciacion(bpin).ToList();
            FuenteFinanciacionProyectoDto listadoRetorno = null;
            ConfigurarMapper();
            foreach (var fuenteFinanciacion in listadoFuentesFinanciacion)
            {
                if (listadoRetorno == null)
                {
                    listadoRetorno = MapearEntidad(fuenteFinanciacion);
                    if (listadoRetorno.ListadoFuenteFinanciacion == null)
                        listadoRetorno.ListadoFuenteFinanciacion = new List<FuenteFinanciacionProyectoDto>();
                }
                listadoRetorno.ListadoFuenteFinanciacion.Add(MapearEntidad(fuenteFinanciacion));
            }

            return listadoRetorno;
        }

        public ProyectoFuenteFinanciacionDto ObtenerFuenteFinanciacionProyectoPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<ProyectoFuenteFinanciacionDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                       @RutasPreviewRecursos.RutaPreviewFuentes);
        }

        public List<FuenteFinanciacionProyectoDto> ObtenerFuentesFinanciacionProyecto(string bpin)
        {
            if (string.IsNullOrEmpty(bpin))
                return null;

            var consultaDesdeBd = Contexto.uspGetFuentesFinanciacion(bpin);

            return MapearAFuenteFinanciacionProyectoDto(consultaDesdeBd);
        }

        private List<FuenteFinanciacionProyectoDto> MapearAFuenteFinanciacionProyectoDto(IEnumerable<uspGetFuentesFinanciacion_Result> consultaDesdeBd)
        {
            List<FuenteFinanciacionProyectoDto> listadoFinal = new List<FuenteFinanciacionProyectoDto>();


            foreach (var registro in consultaDesdeBd)
            {
                FuenteFinanciacionProyectoDto fuente = new FuenteFinanciacionProyectoDto()
                {
                    ApropiacionInicial = registro.ApropiacionInicial,
                    ApropiacionVigente = registro.ApropiacionVigente,
                    CodigoBpin = registro.BPIN,
                    CrProyecto = registro.CR,
                    ValorTotalProyecto = registro.ValorTotalProyecto,
                    Vigencia = registro.Vigencia,
                    Mes = registro.Mes,
                    EtapaId = registro.EtapaId,
                    GrupoRecurso = registro.GrupoRecurso,
                    TipoEntidadId = registro.TipoEntidadId,
                    TipoEntidad = registro.TipoEntidad,
                    EntidadId = registro.EntidadId,
                    Entidad = registro.Entidad,
                    OtraEntidad = registro.OtraEntidad,
                    TipoRecursoId = registro.TipoRecursoId,
                    TipoRecurso = registro.TipoRecurso,
                    NombreCompleto = registro.NombreCompleto,
                    Solicitado = registro.Solicitado,
                    Compromiso = registro.Compromiso,
                    Obligacion = registro.Obligacion,
                    Pago = registro.Pago,
                    FuenteId = registro.FuenteId,
                    EjecucionId = registro.EjecucionId,
                    ProgramacionId = registro.ProgramacionId
                };
                listadoFinal.Add(fuente);
            }

            return listadoFinal;
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoFuenteFinanciacionDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFuentesFinanciacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
                                                         usuario,
                                                          parametrosGuardar.FormularioId,
                                                         errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
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

        //public FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId)
        //{
        //    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
        //    var resultado = new FuenteFinanciacionResultado();
        //    using (var dbContextTransaction = Contexto.Database.BeginTransaction())
        //    {
        //        try
        //        {

        //            Contexto.uspPostFuentesFinanciacionEliminar(fuentesFinanciacionId, errorValidacionNegocio);


        //            if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
        //            {
        //                dbContextTransaction.Commit();
        //                resultado.Exito = true;
        //                return resultado;
        //            }
        //            else
        //            {
        //                var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
        //                resultado.Exito = false;
        //                resultado.Mensaje = mensajeError;
        //                throw new ServiciosNegocioException(mensajeError);
        //            }

        //        }
        //        catch (ServiciosNegocioException)
        //        {
        //            dbContextTransaction.Rollback();
        //            return resultado;
        //            //throw;
        //        }
        //        catch (Exception)
        //        {
        //            dbContextTransaction.Rollback();
        //            throw;
        //        }

        //        //return resultado;

        //    }
        //}

        #region Metodos utilitarios
        private FuenteFinanciacionProyectoDto MapearEntidad(uspGetFuentesFinanciacion_Result entidad)
        {
            return Mapper.Map<FuenteFinanciacionProyectoDto>(entidad);
        }
        private static void ConfigurarMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<uspGetFuentesFinanciacion_Result, FuenteFinanciacionProyectoDto>());
        }

        public string ObtenerPoliticasTransversalesAjustes(string Bpin)
        {
            var listaPoliticas = Contexto.uspGetPoliticasTransversales_Ajustes_JSON(Bpin).FirstOrDefault();
            return listaPoliticas;
        }

        public string GuardarPoliticasTransversalesAjustes(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAgregarPoliticas_ajustes(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
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

        public string ObtenerPoliticasTransversalesCategorias(string Bpin)
        {
            var listaPoliticas = Contexto.uspGetPoliticasTransversalesCategorias_Ajustes_JSON(Bpin).FirstOrDefault();
            return listaPoliticas;
        }

        public RespuestaGeneralDto EliminarPoliticasProyecto(int proyectoId, int politicaId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesProyectoEliminar(proyectoId, politicaId, errorValidacionNegocio);

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

        public string GuardarCategoriasPoliticaTransversalesAjustes(ParametrosGuardarDto<FocalizacionCategoriasAjusteDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAgregarCategoriaPolitica_ajustes(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
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

        public string ObtenerPoliticasTransversalesResumen(string Bpin)
        {
            var listaPoliticas = Contexto.uspGetPoliticasTransversalesCategorias_Ajustes_Resumen_JSON(Bpin).FirstOrDefault();
            return listaPoliticas;
        }

        public string ObtenerPoliticasCategoriasIndicadores(string Bpin)
        {
            try
            {
                var politicasCategoriasIndi = Contexto.uspGetPoliticasCategoriasIndicadores_JSON(Bpin).FirstOrDefault();
                return politicasCategoriasIndi;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }


        public ResultadoProcedimientoDto ModificarCategoriasIndicadores(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {

            var respuesta = new ResultadoProcedimientoDto();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCategoriasIndicadores(JsonUtilidades.ACadenaJson(parametrosGuardar), usuario, resultado);
                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        respuesta.Exito = true;
                        dbContextTransaction.Commit();
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
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

        public RespuestaGeneralDto EliminarCategoriaPoliticasProyecto(int proyectoId, int politicaId, int categoriaId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostEliminarCategoriasProyecto(proyectoId, politicaId, categoriaId, errorValidacionNegocio);

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

        public string ObtenerCrucePoliticasAjustes(string bpin)
        {
            var listaPoliticas = Contexto.uspGetCrucePoliticasAjustes_JSON(bpin).FirstOrDefault();
            return listaPoliticas;
        }

        public RespuestaGeneralDto GuardarCrucePoliticasAjustes(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCrucePoliticasAjustes(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        resultado.Exito = true;
                        dbContextTransaction.Commit();
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        resultado.Exito = false;
                        resultado.Mensaje = mensajeError;
                        dbContextTransaction.Rollback();
                        return resultado;
                        //throw new ServiciosNegocioException(mensajeError);
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

        public string ObtenerPoliticasSolicitudConcepto(string Bpin)
        {
            var listaPoliticasSolicitudConcepto = Contexto.uspGetPoliticasSolicitudConcepto_JSON(Bpin).FirstOrDefault();
            return listaPoliticasSolicitudConcepto;
        }

        public string FocalizacionSolicitarConceptoDT(ParametrosGuardarDto<List<FocalizacionSolicitarConceptoDto>> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFocalizacionSolicitarConceptoDT(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "ok";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
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

        public string ObtenerDireccionesTecnicasPoliticasFocalizacion()
        {
            var rDireccionesTecnicasPoliticasFocalizacion = Contexto.UspGetDireccionesTecnicasPoliticas_JSON().FirstOrDefault();
            return rDireccionesTecnicasPoliticasFocalizacion;
        }

        public string ObtenerResumenSolicitudConcepto(string Bpin)
        {
            var listaPoliticasSolicitudConcepto = Contexto.uspGetResumenSolicitudConcepto_JSON(Bpin).FirstOrDefault();
            return listaPoliticasSolicitudConcepto;
        }

        public string ObtenerPreguntasEnvioPoliticaSubDireccion(Guid instanciaid, int proyectoid, string usuarioDNP, Guid nivelid)
        {
            string json = Contexto.uspGetPreguntasEnvioPoliticaSubDireccion(instanciaid, proyectoid, usuarioDNP, nivelid).FirstOrDefault();
            return json;
        }

        public string GuardarPreguntasEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<PreguntasEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFocalizacionGuardarPreguntasEnvioPoliticaSubDireccionAjustes(parametrosGuardar.Contenido.IdInstancia, parametrosGuardar.Contenido.IdProyecto,
                        parametrosGuardar.Contenido.IdUsuarioDNP, parametrosGuardar.Contenido.IdNivel, parametrosGuardar.Contenido.PoliticaId, parametrosGuardar.Contenido.Respuesta.ToString(),
                        parametrosGuardar.Contenido.ObservacionPregunta, parametrosGuardar.Contenido.EnvioPoliticaSubDireccionIdAgrupa, parametrosGuardar.Contenido.PreguntaId, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "OK";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
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

        public string GuardarRespuestaEnvioPoliticaSubDireccionAjustes(ParametrosGuardarDto<RespuestaEnvioPoliticaSubDireccionAjustes> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFocalizacionGuardarRespuestaEnvioPoliticaSubDireccionAjustes(parametrosGuardar.Contenido.Id, parametrosGuardar.Contenido.ProyectoId,
                        parametrosGuardar.Contenido.PoliticaId, parametrosGuardar.Contenido.IdUsuarioDNP, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "OK";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
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

        #endregion
    }
}


