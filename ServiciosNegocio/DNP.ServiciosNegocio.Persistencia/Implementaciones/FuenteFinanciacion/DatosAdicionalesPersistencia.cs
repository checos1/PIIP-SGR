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
    using DNP.ServiciosNegocio.Dominio.Dto.Genericos;

    public class DatosAdicionalesPersistencia : Persistencia, IDatosAdicionalesPersistencia
    {
        public DatosAdicionalesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();

        }

        public string ObtenerDatosAdicionalesFuenteFinanciacion(int fuenteId)
        {
            var listadoDatosAdicionales = Contexto.UspGetDatosAdicionalesFinanciacion_JSON(fuenteId).FirstOrDefault();
            return listadoDatosAdicionales;
        }

        public RespuestaGeneralDto GuardarDatosAdicionales(ParametrosGuardarDto<DatosAdicionalesDto> parametrosGuardar, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string)); 

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostDatosAdicionales(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
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

        public DatosAdicionalesResultado EliminarDatosAdicionales(int coFinanciacionId)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new DatosAdicionalesResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostDatosAdicionalesFinanciacionEliminar(coFinanciacionId, errorValidacionNegocio);

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
                    return resultado;
                    //throw;
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
        private DatosAdicionalesDto MapearEntidad(UspGetDatosAdicionalesFinanciacion_JSON_Result entidad)
        {
            return Mapper.Map<DatosAdicionalesDto>(entidad);
        }
        private static void ConfigurarMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<UspGetDatosAdicionalesFinanciacion_JSON_Result, DatosAdicionalesDto>());
        }
        #endregion
    }
}


