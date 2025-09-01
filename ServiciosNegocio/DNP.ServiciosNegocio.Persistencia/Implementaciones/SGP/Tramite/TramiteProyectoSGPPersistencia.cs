namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Tramite
{
    using DNP.ServiciosNegocio.Comunes;
    using DNP.ServiciosNegocio.Comunes.Excepciones;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
    using DNP.ServiciosNegocio.Dominio.Dto.Tramites;
    using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Tramite;
    using Interfaces;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public class TramiteProyectoSGPPersistencia : Persistencia, ITramiteProyectoSGPPersistencia
    {
        #region Constructor

        public TramiteProyectoSGPPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion

        public string ObtenerProyectosTramiteNegocio(int TramiteId)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Tramites.uspGetProyectosTramiteNegocioSGP @tramiteId ",
                            new SqlParameter("tramiteId", TramiteId)
                             ).SingleOrDefault();

                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesResultado GuardarProyectosTramiteNegocio(DatosTramiteProyectosDto datosTramiteProyectosDto, string usuario)
        {
            var json = JsonUtilidades.ACadenaJson(datosTramiteProyectosDto);
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Tramites.uspPostAgregarProyectosSGP @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.SqlValue.ToString()))
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

        public string ValidacionProyectosTramiteNegocio(int TramiteId)
        {
            try
            {
                var jsonConsulta = ContextoOnlySP.Database.SqlQuery<string>("Tramites.UspValidacionTramiteTrasladoSgp @tramiteId ",
                            new SqlParameter("tramiteId", TramiteId)
                             ).SingleOrDefault();

                return jsonConsulta;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
    }
}
