using AutoMapper;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using System.Linq;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using System.Data.Entity.Core.Objects;
using System;
using System.Data.SqlClient;
using System.Data;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Modelo_OnlySP;
using DNP.ServiciosNegocio.Dominio.Dto.SGP.Transversales;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Viabilidad
{
    public class ViabilidadSgpPersistencia : PersistenciaSGP, IViabilidadSgpPersistencia
    {
        private readonly IMapper _mapper;
        #region Constructor

        public ViabilidadSgpPersistencia(IContextoFactory contextoFactory, IContextoFactorySGR contextoFactorySGR) : base(contextoFactory, contextoFactorySGR)
        {
            _mapper = ConfigurarMapper();
        }
        #endregion

        #region "Métodos"

        public string SGPTransversalLeerParametro(string Parametro)
        {
            string Json = ContextoOnlySP.Database.SqlQuery<string>("Transversal.uspGetSGPTransversalLeerParametro @Parametro ",
                                             new SqlParameter("Parametro", Parametro)                                             
                                              ).SingleOrDefault();
            return Json;
        }
        /// <summary>
        /// Leer el acuerdo, sector y clasificadores de un proyecto
        /// </summary>
        public LeerInformacionGeneralViabilidadDto SGPViabilidadLeerInformacionGeneral(int proyectoId, Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            return _mapper.Map < LeerInformacionGeneralViabilidadDto > (ContextoOnlySP.Database.SqlQuery<string>("Proyectos.uspGetSGPViabilidadLeerInformacionGeneral @proyectoId,@instanciaId ",
                                          new SqlParameter("proyectoId", proyectoId),
                                          new SqlParameter("instanciaId", instanciaId),
                                          new SqlParameter("tipoConceptoViabilidadCode", tipoConceptoViabilidadCode)
                                           ).SingleOrDefault());            
        }

        public string SGPViabilidadLeerParametricas(int proyectoId, System.Guid nivelId)
        {
            string Json = ContextoOnlySP.Database.SqlQuery<string>("Proyectos.uspGetSGPViabilidadLeerParametricas @proyectoId,@nivelId ",
                                             new SqlParameter("proyectoId", proyectoId),
                                             new SqlParameter("nivelId", nivelId)
                                              ).SingleOrDefault();
            return Json;
        }

        public ResultadoProcedimientoDto SGPViabilidadGuardarInformacionBasica(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var respuesta = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = ContextoOnlySP.Database.BeginTransaction())
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostSGPViabilidadGuardarInformacionBasica @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
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

        public ResultadoProcedimientoDto SGPViabilidadFirmarUsuario(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var respuesta = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = ContextoOnlySP.Database.BeginTransaction())
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostFirmaUsuarioViabilidadSgp @json,@usuario,@errorValidacionNegocio output ",
                                           new SqlParameter("json", json),
                                           new SqlParameter("usuario", usuario),
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
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

        public IEnumerable<ProyectoViabilidadInvolucradosDto> SGPProyectosLeerProyectoViabilidadInvolucrados(int proyectoId, Guid instanciaId, int tipoConceptoViabilidadId)
        {
            var db = ContextoOnlySP.Database.SqlQuery<uspGetObtenerProyectoViabilidadInvolucrados_Result>("Proyectos.uspGetObtenerProyectoViabilidadInvolucradosSgp @proyectoId, @instanciaId, @tipoConceptoViabilidadId ",
                                             new SqlParameter("proyectoId", proyectoId),
                                             new SqlParameter("instanciaId", instanciaId),
                                             new SqlParameter("tipoConceptoViabilidadId", tipoConceptoViabilidadId)
                                              ).ToList();
            IEnumerable<ProyectoViabilidadInvolucradosDto> list = _mapper.Map<IEnumerable<ProyectoViabilidadInvolucradosDto>>(db);
            return list;
        }

        public EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujo(Guid rolId, int crTypeId, int entidadResponsableId, int proyectoId)
        {
            var db = ContextoOnlySP.Database.SqlQuery<uspGetObtenerEntidadDestinoResponsableFlujoSgp_Result>("Transversal.uspGetObtenerEntidadDestinoResponsableFlujoSgp @RolId, @CrTypeId, @EntidadResponsableId, @ProyectoId ",
                                             new SqlParameter("RolId", rolId),
                                             new SqlParameter("CrTypeId", crTypeId),
                                             new SqlParameter("EntidadResponsableId", entidadResponsableId),
                                             new SqlParameter("ProyectoId", proyectoId)).SingleOrDefault();
            EntidadDestinoResponsableFlujoSgpDto data = _mapper.Map<EntidadDestinoResponsableFlujoSgpDto>(db);
            return data;
        }

        public EntidadDestinoResponsableFlujoSgpDto SGPProyectosObtenerEntidadDestinoResponsableFlujoTramite(Guid rolId, int entidadResponsableId, int tramiteId)
        {
            var db = ContextoOnlySP.Database.SqlQuery<uspGetObtenerEntidadDestinoResponsableFlujoTramiteSgp_Result>("Transversal.uspGetObtenerEntidadDestinoResponsableFlujoTramiteSgp @RolId, @EntidadResponsableId, @TramiteId ",
                                             new SqlParameter("RolId", rolId),
                                             new SqlParameter("EntidadResponsableId", entidadResponsableId),
                                             new SqlParameter("TramiteId", tramiteId)).SingleOrDefault();
            EntidadDestinoResponsableFlujoSgpDto data = _mapper.Map<EntidadDestinoResponsableFlujoSgpDto>(db);
            return data;
        }

        /// <summary>
        /// Leer involucrados proyecto para firma
        /// </summary>
        public IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> SGPProyectosLeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            var db = ContextoOnlySP.Database.SqlQuery<uspGetObtenerProyectoViabilidadInvolucradosFirma_Result>("Proyectos.uspGetObtenerProyectoViabilidadInvolucradosFirmaSgp @instanciaId,@tipoConceptoViabilidadId ",
                                             new SqlParameter("instanciaId", instanciaId),
                                             new SqlParameter("tipoConceptoViabilidadId", tipoConceptoViabilidadId)
                                              ).ToList();
            IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> list = _mapper.Map<IEnumerable<ProyectoViabilidadInvolucradosFirmaDto>>(db);
            return list;
        }

        public ProyectoViabilidadInvolucradosResultado EliminarProyectoViabilidadInvolucradosSGP(int id)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var respuesta = new ProyectoViabilidadInvolucradosResultado();
            using (var dbContextTransaction = ContextoOnlySP.Database.BeginTransaction())
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


                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostProyectoViabilidadInvolucradosEliminarSgp @id,@errorValidacionNegocio output ",
                                           new SqlParameter("id", id),                                           
                                           outParam
                                           );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
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

        public ProyectoViabilidadInvolucradosResultado GuardarProyectoViabilidadInvolucradosSGP(ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto> parametrosGuardar, string usuario)
        {
            ObjectParameter Resultado = new ObjectParameter("Resultado", typeof(string));
            var respuesta = new ProyectoViabilidadInvolucradosResultado();
            using (var dbContextTransaction = ContextoOnlySP.Database.BeginTransaction())
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

                    var resultado = ContextoOnlySP.Database.ExecuteSqlCommand("Exec Proyectos.uspPostProyectoViabilidadInvolucradosSgp @JsonData,@ProyectoId,@Usuario,@errorValidacionNegocio output ",
                                                new SqlParameter("JsonData", JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido)),
                                                new SqlParameter("ProyectoId", parametrosGuardar.Contenido.ProyectoId),
                                                new SqlParameter("Usuario", usuario),
                                                outParam                                           
                                           );

                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        respuesta.Exito = true;
                        return respuesta;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
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

        private static IMapper ConfigurarMapper()
        {
            Mapper.Reset();
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<uspGetSGR_Viabilidad_LeerInformacionGeneral_Result, LeerInformacionGeneralViabilidadDto>();                
                cfg.CreateMap<uspGetObtenerProyectoViabilidadInvolucrados_Result, ProyectoViabilidadInvolucradosDto>();
                cfg.CreateMap<uspGetObtenerProyectoViabilidadInvolucradosFirma_Result, ProyectoViabilidadInvolucradosFirmaDto>();
                cfg.CreateMap<uspGetObtenerEntidadDestinoResponsableFlujoSgp_Result, EntidadDestinoResponsableFlujoSgpDto>();
                cfg.CreateMap<uspGetObtenerEntidadDestinoResponsableFlujoTramiteSgp_Result, EntidadDestinoResponsableFlujoSgpDto>();
            }).CreateMapper();
        }
        #endregion
    }
}
