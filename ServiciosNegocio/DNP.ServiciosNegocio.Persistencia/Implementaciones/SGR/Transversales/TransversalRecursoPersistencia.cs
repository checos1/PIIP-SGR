using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Transversales;
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

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.Transversales
{
    public class TransversalRecursoPersistencia : PersistenciaSGR, ITransversalRecursoPersistencia
    {
        #region Constructor

        public TransversalRecursoPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
        }
        #endregion
        #region "Métodos"                                   
        //Regionalizacion
        public DesagregarRegionalizacionDto ObtenerDesagregarRegionalizacionSgr(string bpin)
        {
            try
            {
                var desagregarRegionalizacionDto = new DesagregarRegionalizacionDto();
                string jsonString = Contexto.UspGetDesagregarRegionalizacion_JSONSgr(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<DesagregarRegionalizacionDto>(jsonString);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        //public DatosGeneralesProyectosDto ObtenerDatosGeneralesProyectoSgr(int? pProyectoId, Guid pNivelId)
        //{
        //    //var result = Contexto.DatosGeneralesProyectoSGR(pProyectoId, pNivelId);
        //    //DatosGeneralesProyectosDto datosGenerales = result.Select(j => new DatosGeneralesProyectosDto
        //    //{
        //    //    ProyectoId = j.ProyectoId,
        //    //    NombreProyecto = j.NombreProyecto,
        //    //    BPIN = j.BPIN,
        //    //    EntidadId = j.EntidadId,
        //    //    Entidad = j.Entidad,
        //    //    SectorId = j.SectorId,
        //    //    Sector = j.Sector,
        //    //    EstadoId = j.EstadoId,
        //    //    Estado = j.Estado,
        //    //    Horizonte = j.Horizonte,
        //    //    Valor = j.Valor
        //    //}).FirstOrDefault();

        //    //return datosGenerales;
        //}
        //Focalizacion
        public FocalizacionPoliticaSgrDto ObtenerFocalizacionPoliticasTransversalesFuentesSgr(string bpin)
        {
            try
            {
                var focalizacionPoliticaSgrDto = new FocalizacionPoliticaSgrDto();
                string jsonString = Contexto.uspGetPoliticasTransversalesFuentes_JSONSgr(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<FocalizacionPoliticaSgrDto>(jsonString);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public RespuestaGeneralDto GuardarFocalizacionCategoriasAjustesSgr(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var result = Contexto.uspPostPoliticasTransversalesCategoriasAjustesSgr(JsonUtilidades.ACadenaJson(focalizacionCategoriasAjuste), usuario, errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(focalizacionCategoriasAjuste);
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        //public PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticasSgr(string bpin, int IdFuente)
        //{
        //    try
        //    {
        //        var PoiticaCrucePoliticasDto = Contexto.uspGetCrucePoliticas_JSONSgr(bpin, IdFuente).SingleOrDefault();
        //        return JsonConvert.DeserializeObject<PoliticasTCrucePoliticasDto>(PoiticaCrucePoliticasDto);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
        //    }
        //}
        //public IndicadoresPoliticaDto ObtenerDatosIndicadoresPoliticaSgr(string bpin)
        //{
        //    var indicadorProductoDto = new IndicadoresPoliticaDto();
        //    string jsonString = Contexto.uspGetPoliticasTransversalesFuentesIndicadores_JSONSgr(bpin).FirstOrDefault();
        //    return JsonConvert.DeserializeObject<IndicadoresPoliticaDto>(jsonString);
        //}
        //public IndicadoresPoliticaDto ObtenerDatosCategoriaProductosPoliticaSgr(string bpin, int fuenteId, int politicaId)
        //{
        //    var indicadorProductoDto = new IndicadoresPoliticaDto();
        //    string jsonString = Contexto.uspGetPoliticasTransversalesCategorias_JSONSgr(bpin, fuenteId, politicaId).FirstOrDefault();
        //    return JsonConvert.DeserializeObject<IndicadoresPoliticaDto>(jsonString);
        //}

        public string ObtenerPoliticasTransversalesProyectoSgr(string Bpin)
        {
            var listaPoliticas  = Contexto.uspGetPoliticasTransversales_Ajustes_JSONSgr(Bpin).FirstOrDefault();
            return listaPoliticas;
        }
        public RespuestaGeneralDto EliminarPoliticasProyectoSgr(int proyectoId, int politicaId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesProyectoEliminarSgr(proyectoId, politicaId, errorValidacionNegocio);

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
        public string AgregarPoliticasTransversalesAjustesSgr(ParametrosGuardarDto<IncluirPoliticasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAgregarPoliticas_ajustesSgr(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        //var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        //if (temporal != null)
                        //    Contexto.AlmacenamientoTemporal.Remove(temporal);
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
        public string ConsultarPoliticasCategoriasIndicadoresSgr(Guid instanciaId)
        {
            try
            {
                var politicasCategoriasIndi = Contexto.uspGetPoliticasCategoriasIndicadores_JSONSgr(instanciaId).FirstOrDefault();
                return politicasCategoriasIndi;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public string ObtenerPoliticasTransversalesCategoriasSgr(Guid instanciaId)
        {
            var listaPoliticas = Contexto.uspGetPoliticasTransversalesCategorias_Ajustes_JSONSgr(instanciaId).FirstOrDefault();
            return listaPoliticas;
        }
        public RespuestaGeneralDto EliminarCategoriaPoliticasProyectoSgr(int proyectoId, int politicaId, int categoriaId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostEliminarCategoriasProyectoSgr(proyectoId, politicaId, categoriaId, errorValidacionNegocio);

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
        public ResultadoProcedimientoDto ModificarPoliticasCategoriasIndicadoresSgr(CategoriasIndicadoresDto parametrosGuardar, string usuario)
        {

            var respuesta = new ResultadoProcedimientoDto();
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCategoriasIndicadoresSgr(JsonUtilidades.ACadenaJson(parametrosGuardar), usuario, resultado);
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
        public string ObtenerCrucePoliticasAjustesSgr(Guid instanciaId)
        {
            var listaPoliticas = Contexto.uspGetCrucePoliticasAjustes_JSONSgr(instanciaId).FirstOrDefault();
            return listaPoliticas;
        }
        public string GuardarCrucePoliticasAjustesSgr(ParametrosGuardarDto<List<CrucePoliticasAjustesDto>> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAgregarPoliticas_ajustesSgr(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        //var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        //if (temporal != null)
                        //    Contexto.AlmacenamientoTemporal.Remove(temporal);

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
        public string ObtenerPoliticasTransversalesResumenSgr(Guid instanciaId)
        {
            var listaPoliticas = Contexto.uspGetPoliticasTransversalesCategorias_Ajustes_Resumen_JSONSgr(instanciaId.ToString()).FirstOrDefault();
            return listaPoliticas;
        }
        #endregion
    }
}

