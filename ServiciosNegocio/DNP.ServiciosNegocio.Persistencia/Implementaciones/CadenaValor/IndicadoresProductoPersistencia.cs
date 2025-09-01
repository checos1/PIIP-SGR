using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.CadenaValor;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.RegMetasRecursosDtoAjuste;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.CadenaValor;
using Newtonsoft.Json;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.CadenaValor
{
    public class IndicadoresProductoPersistencia : Persistencia, IIndicadoresProductoPersistencia
    {
        public IndicadoresProductoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public IndicadorProductoDto ObtenerIndicadoresProducto(string bpin)
        {
            try
            {
                var indicadorProductoDto = new IndicadorProductoDto();
                string jsonString = Contexto.uspGetIndicadorProducto_JSON(bpin).SingleOrDefault();

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

        public IndicadorResponse GuardarIndicadoresSecundarios(AgregarIndicadoresSecundariosDto parametrosGuardar,
                                                          string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new IndicadorResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    var result = Contexto.uspPostIndicadorSecundarioProductoAgregar(JsonUtilidades.ACadenaJson(parametrosGuardar),
                                                         usuario,
                                                         errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(parametrosGuardar);
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

        public IndicadorResponse EliminarIndicadorProducto(int indicadorId, string usuario)
        {
            var resultado = new IndicadorResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
                    var result = Contexto.uspPostIndicadorProductoEliminar(indicadorId, usuario, errorValidacionNegocio).SingleOrDefault();


                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = "Indicador Eliminado Exitosamente!";
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

        public IndicadorResponse ActualizarMetaAjusteIndicador(IndicadoresIndicadorProductoDto Indicador, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new IndicadorResponse();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    var IndicadorJson = JsonUtilidades.ACadenaJson(Indicador);
                    Contexto.uspPostIndicadorProductoActualizar(IndicadorJson,
                                                         usuario,
                                                         errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(Indicador);
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

        public List<IndicadorCapituloModificadoDto> IndicadoresValidarCapituloModificado(string bpin)
        {
            try
            {
                var indicadoresCapituloModificados = new List<IndicadorCapituloModificadoDto>();
                string jsonString = Contexto.uspPostIndicadoresValidarCapituloModificado(bpin).SingleOrDefault();

 
                if (jsonString != null)
                {
                    indicadoresCapituloModificados = JsonConvert.DeserializeObject<List<IndicadorCapituloModificadoDto>>(jsonString);
                }

                return indicadoresCapituloModificados;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public RegionalizacionDto RegionalizacionGeneral(string bpin)
        {
            try
            {
                var regionalizacion = new RegionalizacionDto();
                string jsonString = Contexto.UspGetDesagregarRegionalizacion_Ajustes_JSON(bpin).SingleOrDefault();

                regionalizacion = JsonConvert.DeserializeObject<RegionalizacionDto>(jsonString);

                return regionalizacion;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public RespuestaGeneralDto GuardarRegionalizacionFuentesFinanciacionAjustes(List<RegionalizacionFuenteAjusteDto> regionalizacionFuenteAjuste, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    var result = Contexto.UspPostDesagregarRegionalizacionAjustes(JsonUtilidades.ACadenaJson(regionalizacionFuenteAjuste),
                                                         usuario,
                                                         errorValidacionNegocio).SingleOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        resultado.Mensaje = JsonUtilidades.ACadenaJson(regionalizacionFuenteAjuste);
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

        public RespuestaGeneralDto GuardarFocalizacionCategoriasAjustes(List<FocalizacionCategoriasAjusteDto> focalizacionCategoriasAjuste, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new RespuestaGeneralDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostPoliticasTransversalesCategoriasAjustes(JsonUtilidades.ACadenaJson(focalizacionCategoriasAjuste), usuario, errorValidacionNegocio);

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

        public string ObtenerDetalleAjustesJustificaionRegionalizacion(string bpin)
        {
            var listadoAjustesJustificaionRegionalizacion = Contexto.upsGetJustificaionRegionalizacion_JSON(bpin).SingleOrDefault();
            return listadoAjustesJustificaionRegionalizacion;
        }

        public string ObtenerSeccionOtrasPoliticasFacalizacionPT(string bpin)
        {
            var listadoAjustesJustificaionRegionalizacion = Contexto.uspGetOtrasPoliticasJustificacion_Ajustes_JSON(bpin).SingleOrDefault();
            return listadoAjustesJustificaionRegionalizacion;
        }

        public string ObtenerSeccionPoliticaFocalizacionDT(string bpin)
        {
            var listadoAjustesJustificaionRegionalizacionDT = Contexto.uspGetPoliticasJustificacionAjustes_JSON(bpin).SingleOrDefault();
            return listadoAjustesJustificaionRegionalizacionDT;
        }
    }
}