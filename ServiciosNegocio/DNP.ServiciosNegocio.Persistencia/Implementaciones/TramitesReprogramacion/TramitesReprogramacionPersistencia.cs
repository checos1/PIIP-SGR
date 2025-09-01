using AutoMapper;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Genericos;
using DNP.ServiciosNegocio.Dominio.Dto.TramiteIncorporacion;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesReprogramacion;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.TramitesReprogramacion;
using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.TramitesReprogramacion
{
    public class TramitesReprogramacionPersistencia : Persistencia, ITramitesReprogramacionPersistencia
    {
        public TramitesReprogramacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();
        }


        ///<summary>
        ///  Funcion para Obtener los Datos Reprogramacion
        ///</summary>
        ///<param name="InstanciaId"></param>
        ///<param name="ProyectoId"></param>
        ///<param name="TramiteId"></param>
        ///<returns></returns>
        public string ObtenerResumenReprogramacionPorVigencia(Guid? InstanciaId, int ProyectoId, int TramiteId)
        {
            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetResumenReprogramacionPorVigencia_JSON @InstanciaId, @ProyectoId, @TramiteId ",
            new SqlParameter("InstanciaId", InstanciaId),
            new SqlParameter("ProyectoId", ProyectoId),
            new SqlParameter("TramiteId", TramiteId)
           ).SingleOrDefault();
            return jsonConsulta;

        }


        ///<summary>
        /// Funcion para Registrar los Datos Reprogramacion
        ///</summary>
        ///<param name="parametrosGuardar"></param>
        ///<param name="usuario"></param>
        ///<returns></returns>
        public TramitesResultado GuardarDatosReprogramacion(ParametrosGuardarDto<DatosReprogramacionDto> parametrosGuardar, string usuario)
        {
            var respuesta = new TramitesResultado();
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    ContextoOnlySP.uspPostReprogramacionPorVigencia(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, errorValidacionNegocio);

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




        ///<summary>
        /// Funcion para obtener los datos reprogramacion por producto vigencia
        ///</summary>
        ///<param name="InstanciaId"></param>
        ///<param name="ProyectoId"></param>
        ///<param name="TramiteId"></param>
        ///<returns></returns>
        public string ObtenerResumenReprogramacionPorProductoVigencia(Guid? InstanciaId, int? ProyectoId, int TramiteId)
        {
            SqlParameter pInstanciaId = new SqlParameter("InstanciaId", SqlDbType.UniqueIdentifier);
            pInstanciaId.Value = InstanciaId;
            SqlParameter pProyectoId = new SqlParameter("ProyectoId", SqlDbType.Int);
            if (ProyectoId == null)
            {
                pProyectoId.Value = DBNull.Value;
            }
            else
            {
                pProyectoId.Value = ProyectoId;
            }
            SqlParameter pTramiteId = new SqlParameter("TramiteId", SqlDbType.Int);
            pTramiteId.Value = TramiteId;

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetResumenReprogramacionPorProductoVigencia_JSON @InstanciaId, @ProyectoId, @TramiteId ",
           pInstanciaId,
           pProyectoId,
           pTramiteId
           ).SingleOrDefault();
            return jsonConsulta;
        }


    }
}


