using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.ModificacionLey;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DNP.ServiciosNegocio.Dominio.Dto.ModificacionLey;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.ModificacionLey
{
    public class ModificacionLeyPersistencia : Persistencia, IModificacionLeyPersistencia
    {
        #region Constructor

        public ModificacionLeyPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        #endregion

        public string ObtenerInformacionPresupuestalMLEncabezado(int EntidadDestinoId, int tramiteid, string origen)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetModificacionLeyEncabezadoInformacionPresupuestal @EntidadDestinoId, @tramiteid,@origen ",
                                                new SqlParameter("EntidadDestinoId", EntidadDestinoId),
                                                new SqlParameter("tramiteid", tramiteid),
                                                new SqlParameter("origen", origen)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public string ObtenerInformacionPresupuestalMLDetalle(int tramiteidProyectoId, string origen)
        {

            var jsonConsulta = Contexto.Database.SqlQuery<string>("Tramites.uspGetModificacionLeyDetalleFuentes @tramiteidProyectoId , @origen ",
                                                new SqlParameter("tramiteidProyectoId", tramiteidProyectoId),
                                                new SqlParameter("origen", origen)
                                                 ).SingleOrDefault();
            return jsonConsulta;
        }

        public TramitesResultado GuardarInformacionPresupuestalML(InformacionPresupuestalMLDto InformacionPresupuestal, string usuario, string origen)
        {
            var respuesta = new TramitesResultado();
            var json = JsonUtilidades.ACadenaJson(InformacionPresupuestal);

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
                    var resultado = Contexto.Database.ExecuteSqlCommand("Exec Tramites.uspPostModificacionLeyFuentes @json,@Usuario,@errorValidacionNegocio output, @origen ",
                                            new SqlParameter("json", json),
                                            new SqlParameter("Usuario", usuario),
                                            outParam,
                                            new SqlParameter("origen", origen)
                                            );


                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
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
    }
}
