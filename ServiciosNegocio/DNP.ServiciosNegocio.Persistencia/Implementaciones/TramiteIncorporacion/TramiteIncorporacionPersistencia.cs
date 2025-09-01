using AutoMapper;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.TramiteIncorporacion;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.TramiteIncorporacion
{
    public class TramiteIncorporacionPersistencia : Persistencia, ITramiteIncorporacionPersistencia
    {
        public TramiteIncorporacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }

        /// <summary>
        ///  Funcion para Obtener los Datos Incorporacion
        /// </summary>
        /// <param name="TramiteId"></param>
        /// <returns></returns>
        public string ObtenerDatosIncorporacion(int TramiteId)
        {
            var result = ContextoOnlySP.uspGetDatosIncorporacion(TramiteId).SingleOrDefault();
            return result;
        }

        /// <summary>
        ///  Funcion para Registrar los Datos Incorporacion
        /// </summary>
        /// <param name="parametrosGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public RespuestaGeneralDto GuardarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ContextoOnlySP.uspPostDatosIncorporacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, errorValidacionNegocio);

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

        /// <summary>
        ///  Funcion para Registrar los Datos Incorporacion
        /// </summary>
        /// <param name="parametrosGuardar"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public RespuestaGeneralDto EiliminarDatosIncorporacion(ParametrosGuardarDto<ConvenioDonanteDto> parametrosGuardar, string usuario)
        {
            var respuesta = new RespuestaGeneralDto();
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ContextoOnlySP.uspPostEliminarDatosIncorporacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, errorValidacionNegocio);

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
    }
}


