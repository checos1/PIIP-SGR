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
    using System.Runtime.Remoting.Messaging;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json;

    public class FuenteFinanciacionAgregarPersistencia : Persistencia, IFuenteFinanciacionAgregarPersistencia
    {
        public FuenteFinanciacionAgregarPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();

        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregar(string bpin)
        {

            var listadoFuentesFinanciacion = Contexto.uspGetFuenteFinanciacionAgregar(bpin).ToList();
            List<FuenteFinanciacionAgregarDto> listadoRetorno = null;
            listadoRetorno = new List<FuenteFinanciacionAgregarDto>();
            ConfigurarMapper();
            foreach (var fuenteFinanciacion in listadoFuentesFinanciacion)
            {
                listadoRetorno.Add((FuenteFinanciacionAgregarDto)(MapearEntidad(fuenteFinanciacion)));
            }

            ProyectoFuenteFinanciacionAgregarDto proyectoFuenteFinanciacionAgregarDto = new ProyectoFuenteFinanciacionAgregarDto();
            if (listadoFuentesFinanciacion.Any())
            {
                proyectoFuenteFinanciacionAgregarDto.ProyectoId = listadoFuentesFinanciacion[0].ProyectoId;
                proyectoFuenteFinanciacionAgregarDto.BPIN = listadoFuentesFinanciacion[0].BPIN;
                proyectoFuenteFinanciacionAgregarDto.CR = listadoFuentesFinanciacion[0].CRTypeId;
            }
            else
            {
                proyectoFuenteFinanciacionAgregarDto.BPIN = bpin;
            }

            proyectoFuenteFinanciacionAgregarDto.FuentesFinanciacionAgregar = listadoRetorno;
            return proyectoFuenteFinanciacionAgregarDto;
        }

        public string ObtenerFuenteFinanciacionVigencia(string bpin)
        {
            var listadoFuentesFinanciacion = Contexto.UspGetAgregarFuenteVigencia(bpin).SingleOrDefault();
            return listadoFuentesFinanciacion;
        }

        public string ObtenerFuenteFinanciacionAgregarN(string bpin)
        {
            var listadoFuentesFinanciacion = Contexto.UspGetAgregarFuente(bpin).SingleOrDefault();
            return listadoFuentesFinanciacion;
        }

        public ProyectoFuenteFinanciacionAgregarDto ObtenerFuenteFinanciacionAgregarPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<ProyectoFuenteFinanciacionAgregarDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                       @RutasPreviewRecursos.RutaPreviewFuentesAgregar);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFuenteFinanciacionAgregar(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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

        public void GuardarFuenteFinanciacion(ParametrosGuardarDto<ProyectoFuenteFinanciacionAgregarDto> parametrosGuardar,
                                          string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFuenteFinanciacionAgregar(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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

        public FuenteFinanciacionResultado EliminarFuentesFinanciacionProyecto(int fuentesFinanciacionId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new FuenteFinanciacionResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFuentesFinanciacionEliminar(fuentesFinanciacionId, errorValidacionNegocio);

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

        public string ObtenerResumenCostosVsSolicitado(string bpin)
        {
            var listadoFuentesFinanciacion = Contexto.UspGetCostosMgaVsFuentesPiip(bpin).SingleOrDefault();
            return listadoFuentesFinanciacion;
        }

        //public ResumenFuenteFinanciacionDTO ConsultarResumenFteFinanciacion(string bpin)
        //{
        //    var resumenFuentesFinanciacion = Contexto.uspGetFuentesTablasResumen_JSON(bpin).SingleOrDefault();
        //    return JsonConvert.DeserializeObject<ResumenFuenteFinanciacionDTO>(resumenFuentesFinanciacion);
        //    //return resumenFuentesFinanciacion;
        //}

        public string ConsultarResumenFteFinanciacion(string bpin)
        {
            var resumenFuentesFinanciacion = Contexto.uspGetFuentesTablasResumen_JSON(bpin).SingleOrDefault();
            return resumenFuentesFinanciacion;
        }

        public string ConsultarCostosPIIPvsFuentesPIIP(string bpin)
        {
            var resumenFuentesFinanciacion = Contexto.UspGetCostosPIIPVsFuentesPiip_JSON(bpin).SingleOrDefault();
            return resumenFuentesFinanciacion;
        }

        public string FuentesFinanciacionRecursosAjustesAgregar(FuenteFinanciacionAgregarAjusteDto objFuenteFinanciacionAgregarAjusteDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    //string cadena = JsonUtilidades.ACadenaJson(objFuenteFinanciacionAgregarAjusteDto);
                    Contexto.uspPostFuentesFinanciacionRecursosAjustesAgregar(JsonUtilidades.ACadenaJson(objFuenteFinanciacionAgregarAjusteDto), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return "OK";
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                        throw new ServiciosNegocioException(mensajeError);
                        //return mensajeError;
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

        public string ObtenerDetalleAjustesFuenteFinanciacion(string bpin, string usuario)
        {
            try
            {
                return Contexto.uspGetFuentesFinanciacion_ObtenerDetalleAjuste(bpin).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ObtenerDetalleAjustesJustificaionFacalizacionPT(string bpin, string usuario)
        {
            try
            {
                return Contexto.uspGetPoliticasTransversalesCategorias_ObtenerDetalleAjuste(bpin).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OperacionCreditoDatosGeneralesDto ObtenerOperacionCreditoDatosGenerales(string bpin, Guid? instanciaId)
        {
            try
            {
                var OperacionCreditoDatosGenerales = Contexto.uspGetOperacionesCredito(bpin, instanciaId).SingleOrDefault();

                if (OperacionCreditoDatosGenerales == null)
                {
                    var objOperacionCreditoDatosGenerales = new OperacionCreditoDatosGeneralesDto()
                    {
                        BPIN = bpin,
                        Criterios = new List<CriteriosDto>()
                        {
                            new CriteriosDto()
                            {
                                Habilita = false,
                                NombreTipoValor = String.Empty,
                                Valor = 0
                            }
                        },
                        ProyectoId = Convert.ToInt32(bpin),
                        FuentesCredito = 0
                    };

                    return objOperacionCreditoDatosGenerales;
                }
                else
                {
                    return JsonConvert.DeserializeObject<OperacionCreditoDatosGeneralesDto>(OperacionCreditoDatosGenerales);

                }
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public FuenteFinanciacionResultado GuardarOperacionCreditoDatosGenerales(OperacionCreditoDatosGeneralesDto OperacionCreditoDatosGeneralesDto,
                                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new FuenteFinanciacionResultado();


            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.UspPostOperacionesCredito(JsonUtilidades.ACadenaJson(OperacionCreditoDatosGeneralesDto),
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

        public OperacionCreditoDetallesDto ObtenerOperacionCreditoDetalles(string bpin, Guid? instanciaId)
        {
            try
            {
                var OperacionCreditoDetalles = Contexto.uspGetOperacionesCreditoDetalle_JSON(bpin, instanciaId).SingleOrDefault();

                if (OperacionCreditoDetalles == null)
                {
                    var objOperacionCreditoDetalles = new OperacionCreditoDetallesDto()
                    {
                        BPIN = bpin,
                        ValoresCredito = new List<ValoresCreditoDto>()
                        {
                            new ValoresCreditoDto()
                            {
                                Etapa = String.Empty,
                                TipoEntidad = String.Empty,
                                Entidad = String.Empty,
                                TipoRecurso = String.Empty,
                                ValorSolicitado = 0,
                                ValorCredito = 0,
                                CostoFinanciero = 0,
                                CostoPatrimonio = 0
                            }
                        }
                    };

                    return objOperacionCreditoDetalles;
                }
                else
                {
                    return JsonConvert.DeserializeObject<OperacionCreditoDetallesDto>(OperacionCreditoDetalles);
                }
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public FuenteFinanciacionResultado GuardarOperacionCreditoDetalles(OperacionCreditoDetallesDto OperacionCreditoDetallesDto,
                                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new FuenteFinanciacionResultado();


            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostOperacionCreditoDetalle_JSON(JsonUtilidades.ACadenaJson(OperacionCreditoDetallesDto),
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

        #region Metodos utilitarios
        private FuenteFinanciacionAgregarDto MapearEntidad(uspGetFuenteFinanciacionAgregar_Result entidad)
        {
            return Mapper.Map<FuenteFinanciacionAgregarDto>(entidad);
        }

        //private FuenteFinanciacionAgregarDto MapearEntidadN(uspGetFuenteFinanciacionAgregar_ResultN entidad)
        //{
        //    return Mapper.Map<FuenteFinanciacionAgregarDto>(entidad);
        //}
        private static void ConfigurarMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<uspGetFuenteFinanciacionAgregar_Result, FuenteFinanciacionAgregarDto>());
        }

        #endregion
    }
}


