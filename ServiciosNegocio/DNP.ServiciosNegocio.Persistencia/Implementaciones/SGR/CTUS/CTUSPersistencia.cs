using AutoMapper;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Viabilidad;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.CTUS
{
    public class CTUSPersistencia : PersistenciaSGR, ICTUSPersistencia
    {
        private readonly IMapper _mapper;
        #region Constructor

        public CTUSPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
            _mapper = ConfigurarMapper();
        }
        #endregion

        #region "Métodos"

        public ConceptoCTUSDto SGR_CTUS_LeerProyectoCtusConcepto(int proyectoCtusId)
        {
            return _mapper.Map<ConceptoCTUSDto>(Contexto.uspGetSGR_CTUS_LeerProyectoCtusConcepto(proyectoCtusId).FirstOrDefault());
        }

        public ResultadoProcedimientoDto SGR_CTUS_GuardarProyectoCtusConcepto(ConceptoCTUSDto json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostSGR_CTUS_GuardarProyectoCtusConcepto(JsonUtilidades.ACadenaJson(json), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        return resultado;
                    }

                    var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                    resultado.Exito = false;
                    resultado.Mensaje = mensajeError;
                    throw new ServiciosNegocioException(mensajeError);
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

        public ResultadoProcedimientoDto SGR_CTUS_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioCTUSDto json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostSGR_CTUS_GuardarAsignacionUsuarioEncargado(JsonUtilidades.ACadenaJson(json), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        return resultado;
                    }

                    var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                    resultado.Exito = false;
                    resultado.Mensaje = mensajeError;
                    throw new ServiciosNegocioException(mensajeError);
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

        public UsuarioEncargadoCTUSDto SGR_CTUS_LeerProyectoCtusUsuarioEncargado(int proyectoCtusId, Guid instanciaId)
        {
            return _mapper.Map<UsuarioEncargadoCTUSDto>(Contexto.uspGetSGR_CTUS_LeerProyectoCtusUsuarioEncargado(proyectoCtusId, instanciaId).FirstOrDefault());
        }

        public ResultadoProcedimientoDto SGR_CTUS_GuardarResultadoConceptoCtus(ResultadoConceptoCTUSDto json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostSGR_CTUS_GuardarResultadoConceptoCtus(JsonUtilidades.ACadenaJson(json), usuario, errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        return resultado;
                    }

                    var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
                    resultado.Exito = false;
                    resultado.Mensaje = mensajeError;
                    throw new ServiciosNegocioException(mensajeError);
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

        /// <summary>
        /// Actualizar entidad adscrita CTUS
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="user"></param> 
        /// <returns>bool</returns> 
        public bool SGR_Proyectos_ActualizarEntidadAdscritaCTUS(int proyectoId, int entityId, string tipo, string user)
        {
            ObjectParameter result = new ObjectParameter("result", typeof(int));
            bool resultado = false;

            using (var dbContextTransaction = Contexto)
            {
                dbContextTransaction.uspPostSGR_Proyectos_ActualizarEntidadAdscritaCTUS(proyectoId, entityId, tipo, user, result);

                if (result.Value != null && int.TryParse(result.Value.ToString(), out int valor))
                {
                    if (valor == 1)
                        resultado = true;
                    else
                        resultado = false;
                }
                else
                    resultado = false;

                return resultado;
            }
        }

        public string ValidarInstanciaCTUSNoFinalizada(int idProyecto)
        {
            string result = _mapper.Map<string>(Contexto.uspGetValidarInstanciaCTUSNoFinalizada_JSON(idProyecto).FirstOrDefault());
            return result;
        }


        public RolApruebaCTUSDto SGR_CTUS_LeerRolDirectorProyectoCtus(int proyectoId, Guid instanciaId)
        {
            return _mapper.Map<RolApruebaCTUSDto>(Contexto.uspGetObtenerRolDirectorProyectoCtus(proyectoId, instanciaId).FirstOrDefault());
        }
        #endregion

        #region Métodos privados
        private static IMapper ConfigurarMapper()
        {
            Mapper.Reset();
            return new MapperConfiguration(cfg => { 
                cfg.CreateMap<uspGetSGR_CTUS_LeerProyectoCtusConcepto_Result, ConceptoCTUSDto>();
                cfg.CreateMap<uspGetSGR_CTUS_LeerProyectoCtusUsuarioEncargado_Result, UsuarioEncargadoCTUSDto>();
                cfg.CreateMap<uspGetObtenerRolDirectorProyectoCtus_Result, RolApruebaCTUSDto>();
            }).CreateMapper();
        }
        #endregion
    }
}
