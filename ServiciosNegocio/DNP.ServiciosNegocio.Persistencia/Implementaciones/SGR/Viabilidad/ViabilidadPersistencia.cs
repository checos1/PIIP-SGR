using AutoMapper;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using System.Linq;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using System.Data.Entity.Core.Objects;
using System;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.Viabilidad
{
    public class ViabilidadPersistencia : PersistenciaSGR, IViabilidadPersistencia
    {
        private readonly IMapper _mapper;
        #region Constructor

        public ViabilidadPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {            
            _mapper = ConfigurarMapper();
        }


        #endregion

        #region "Métodos"

        /// <summary>
        /// Leer el acuerdo, sector y clasificadores de un proyecto
        /// </summary>
        public LeerInformacionGeneralViabilidadDto SGR_Viabilidad_LeerInformacionGeneral(int proyectoId, Guid instanciaId, string tipoConceptoViabilidadCode)
        {
            return _mapper.Map<LeerInformacionGeneralViabilidadDto>(Contexto.uspGetSGR_Viabilidad_LeerInformacionGeneral(proyectoId, instanciaId, tipoConceptoViabilidadCode).FirstOrDefault());
        }

        public string SGR_Viabilidad_LeerParametricas(int proyectoId, System.Guid nivelId)
        {
            return Contexto.uspGetSGR_Viabilidad_LeerParametricas(proyectoId, nivelId).FirstOrDefault();
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_GuardarInformacionBasica(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostSGR_Viabilidad_GuardarInformacionBasica(json, usuario, errorValidacionNegocio);


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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_FirmarUsuario(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostFirmaUsuarioViabilidadSgr(json, usuario, errorValidacionNegocio);


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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_EliminarFirmaUsuario(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostEliminarFirmaUsuarioViabilidadSgp(json, usuario, errorValidacionNegocio);


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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }

            }
        }

        public string SGR_Viabilidad_ObtenerPuntajeProyecto(Guid instanciaId, int entidadId)
        {
            return Contexto.uspGetPuntajeProyecto(instanciaId, entidadId).FirstOrDefault();
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_GuardarPuntajeProyecto(string json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {

                    Contexto.uspPostPuntajeProyecto(json, usuario, errorValidacionNegocio);


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
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetSGR_Viabilidad_LeerInformacionGeneral_Result, LeerInformacionGeneralViabilidadDto>()).CreateMapper();
        }

        #endregion
    }
}
