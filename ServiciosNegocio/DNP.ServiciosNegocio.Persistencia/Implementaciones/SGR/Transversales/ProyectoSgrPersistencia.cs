using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGR.Transversales;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.Viabilidad;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using System.Collections.Generic;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
using System.Data.Entity.Core.Objects;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTUS;
using DNP.ServiciosNegocio.Comunes.Dto.ObjetosNegocio;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.OcadPaz;
using System.CodeDom.Compiler;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.CTEI;
using DNP.ServiciosNegocio.Dominio.Dto.SGR.AvalUso;
using DNP.ServiciosNegocio.Dominio.Dto.Priorizacion;
using System.Data.SqlClient;
using System.Data;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGR.Transversales
{
    public class ProyectoSgrPersistencia : PersistenciaSGR, IProyectoSgrPersistencia
    {
        private readonly IMapper _mapper;

        #region Constructor

        public ProyectoSgrPersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
            _mapper = ConfigurarMapper();
        }

        #endregion

        #region "Métodos"

        /// <summary>
        /// Lee una lista para un proyecto según parámetro
        /// </summary>
        public string SGR_Proyectos_LeerListas(System.Guid nivelId, int proyectoId, string nombreLista)
        {
            string Json = Contexto.uspGetSGR_Proyectos_LeerListas(nivelId, proyectoId, nombreLista).SingleOrDefault();
            return Json;
        }

        /// <summary>
        /// Validar cumplimiento de un proyecto por instancia
        /// </summary>     
        /// <param name="instanciaId"></param>   
        /// <returns>int</returns> 
        public int SGR_Proyectos_CumplimentoFlujoSGR(Guid instanciaId)
        {
            var valueCump = Contexto.uspGetCumplimentoFlujoSGR(instanciaId).SingleOrDefault();

            if (valueCump is null)
                return -1;
            else if ((bool)valueCump)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Leer entidades por id del proyecto
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipoEntidad"></param>  
        /// <returns>string</returns> 
        public string SGR_Proyectos_LeerEntidadesAdscritas(int proyectoId, string tipoEntidad)
        {
            string Json = Contexto.uspGetSGR_Proyectos_LeerEntidadesAdscritas(proyectoId, tipoEntidad).SingleOrDefault();
            return Json;
        }

        /// <summary>
        /// Validar entidad delegada
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="tipo"></param>  
        /// <returns>ResultadoProcedimientoDto</returns> 
        public ResultadoProcedimientoDto SGR_Proyectos_ValidarEntidadDelegada(int proyectoId, string tipo)
        {
            ResultadoProcedimientoDto resultado = new ResultadoProcedimientoDto();

            using (var dbContextTransaction = Contexto)
            {
                string resJson = dbContextTransaction.uspGetSGR_Proyectos_ValidarEntidadDelegada(proyectoId, tipo).SingleOrDefault();

                if (resJson is null)
                    resultado.Exito = false;
                else
                {
                    resultado.Exito = true;
                    resultado.Mensaje = resJson;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Actualizar entidad adscrita
        /// </summary>     
        /// <param name="proyectoId"></param>  
        /// <param name="entityId"></param>  
        /// <param name="delegado"></param> 
        /// <param name="user"></param> 
        /// <returns>bool</returns> 
        public bool SGR_Proyectos_ActualizarEntidadAdscrita(int proyectoId, int entityId, bool delegado, string user)
        {
            ObjectParameter result = new ObjectParameter("result", typeof(int));
            bool resultado = false;

            using (var dbContextTransaction = Contexto)
            {
                dbContextTransaction.uspPostSGR_Proyectos_ActualizarEntidadAdscrita(proyectoId, entityId, user, delegado, result);

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

        /// <summary>
        /// Leer involucrados proyecto
        /// </summary>
        public IEnumerable<ProyectoViabilidadInvolucradosDto> SGR_Proyectos_LeerProyectoViabilidadInvolucrados(int proyectoId, int tipoConceptoViabilidadId)
        {
            var db = Contexto.uspGetObtenerProyectoViabilidadInvolucrados(proyectoId, tipoConceptoViabilidadId);
            IEnumerable<ProyectoViabilidadInvolucradosDto> list = _mapper.Map<IEnumerable<ProyectoViabilidadInvolucradosDto>>(db);
            return list;
        }

        /// <summary>
        /// Leer involucrados proyecto para firma
        /// </summary>
        public IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> SGR_Proyectos_LeerProyectoViabilidadInvolucradosFirma(Guid instanciaId, int tipoConceptoViabilidadId)
        {
            var db = Contexto.uspGetObtenerProyectoViabilidadInvolucradosFirma(instanciaId, tipoConceptoViabilidadId);
            IEnumerable<ProyectoViabilidadInvolucradosFirmaDto> list = _mapper.Map<IEnumerable<ProyectoViabilidadInvolucradosFirmaDto>>(db);
            return list;
        }

        public ProyectoViabilidadInvolucradosResultado EliminarProyectoViabilidadInvolucradoso(int id)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ProyectoViabilidadInvolucradosResultado();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostProyectoViabilidadInvolucradosEliminar(id, errorValidacionNegocio);

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

        public ProyectoViabilidadInvolucradosResultado GuardarProyectoViabilidadInvolucrados(ParametrosGuardarDto<ProyectoViabilidadInvolucradosDto> parametrosGuardar, string usuario)
        {
            ObjectParameter Resultado = new ObjectParameter("Resultado", typeof(string));
            var resultado = new ProyectoViabilidadInvolucradosResultado();

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostProyectoViabilidadInvolucrados(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), parametrosGuardar.Contenido.ProyectoId, usuario, Resultado);

                    if (string.IsNullOrEmpty(Resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(Resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception es)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string SGR_Proyectos_GenerarMensajeEstadoProyecto(Guid instanciaId)
        {
            var mensaje = new ObjectParameter("Mensaje", typeof(string));

            try
            {
                Contexto.uspGetMensajeEstadoProyectoSgr(instanciaId, mensaje);
            }
            catch (ServiciosNegocioException)
            {
                throw;
            }

            return mensaje.Value.ToString();
        }

        public bool SGR_Proyectos_PostAplicarFlujoSGR(AplicarFlujoSGRDto parametros)
        {
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostAplicarFlujoSgr(parametros.ObjetoNegocioId, parametros.instanciaId, parametros.Usuario);
                    Contexto.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new ServiciosNegocioException(e.Message);
                }
            }

            return true;
        }

        public bool SGR_Proyectos_PostDevolverFlujoSGR(DevolverFlujoSGRDto parametros)
        {
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostDevolverFlujoSgr(parametros.ObjetoNegocioId, parametros.instanciaId, parametros.Usuario);
                    Contexto.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new ServiciosNegocioException(e.Message);
                }
            }

            return true;
        }

        /// <summary>
        /// Leer Proyecto Ctus
        /// </summary>
        public ProyectoCtusDto SGR_Proyectos_LeerProyectoCtus(int ProyectoId, Guid instanciaId)
        {
            var db = Contexto.uspGetObtenerProyectoCtus(ProyectoId, instanciaId);
            IEnumerable<ProyectoCtusDto> list = _mapper.Map<IEnumerable<ProyectoCtusDto>>(db);
            return list.First();
        }

        public IEnumerable<EntidadesSolicitarCtusDto> SGR_Proyectos_LeerEntidadesSolicitarCtus(int ProyectoId)
        {
            var db = Contexto.uspGetEntidadesSolicitarCtus(ProyectoId);
            IEnumerable<EntidadesSolicitarCtusDto> list = _mapper.Map<IEnumerable<EntidadesSolicitarCtusDto>>(db);
            return list;
        }

        public ProyectoCtuResultado SGR_Proyectos_GuardarProyectoSolicitarCtus(ParametrosGuardarDto<ProyectoCtusDto> parametrosGuardar, string usuario)
        {
            ObjectParameter Resultado = new ObjectParameter("Resultado", typeof(string));
            var resultado = new ProyectoCtuResultado();

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostProyectoSolicitarCTUS(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), parametrosGuardar.Contenido.ProyectoId, usuario, Resultado);

                    if (string.IsNullOrEmpty(Resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        resultado.Exito = true;
                        return resultado;
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(Resultado.Value);
                        throw new ServiciosNegocioException(mensajeError);
                    }

                }
                catch (ServiciosNegocioException ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
                catch (Exception es)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public string SGR_Proyectos_validarTecnicoOcadpaz(Nullable<System.Guid> instanciaId, Nullable<System.Guid> accionId)
        {
            var mensaje = new ObjectParameter("Mensaje", typeof(string));

            try
            {
                return Contexto.uspGetSGR_ValidarAsignacionTecnicoOcadPaz(instanciaId, accionId).SingleOrDefault();
            }
            catch (ServiciosNegocioException)
            {
                throw;
            }
        }

        public List<ProyectoEntidadVerificacionOcadPazDto> ObtenerProyectosVerificacionOcadPazSgr(ParametrosProyectoVerificacionSgrDto parametros)
        {
            var resultSp = Contexto.uspGetObtenerProyectosVerificacionOcadPazSgr(parametros.IdTipoObjetoNegocio, parametros.IdUsuarioDNP, parametros.ListRol, parametros.ListNivel, parametros.ListSubPasos, parametros.ValidarActual).ToList();

            var proyectos = _mapper.Map<List<ProyectoEntidadVerificacionOcadPazDto>>(resultSp);

            return proyectos;
        }

        public IEnumerable<UsuariosVerificacionOcadPazDto> SGR_Proyectos_ObtenerUsuariosVerificacionOcadPaz(Guid rolId, int entidadId)
        {
            var db = Contexto.uspGetObtenerUsuariosVerificacionOcadPazSgr(rolId, entidadId);
            IEnumerable<UsuariosVerificacionOcadPazDto> list = _mapper.Map<IEnumerable<UsuariosVerificacionOcadPazDto>>(db);
            return list;
        }

        public ResultadoProcedimientoDto SGR_OCADPaz_GuardarAsignacionUsuarioEncargado(AsignacionUsuarioOcadPazDto json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostSGR_OCADPaz_GuardarAsignacionUsuarioEncargado(JsonUtilidades.ACadenaJson(json), usuario, errorValidacionNegocio);

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

        public ResultadoProcedimientoDto SGR_Proyectos_GuardarAsignacionUsuarioEncargado(UsuarioEncargadoDto json, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostSGR_Viabilidad_GuardarAsignacionUsuarioEncargado(JsonUtilidades.ACadenaJson(json), usuario, errorValidacionNegocio);

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

        public string SGR_Proyectos_LeerAsignacionUsuarioEncargado(int proyectoId, Guid instanciaId)
        {
            string Json = Contexto.uspGetSGR_Viabilidad_LeerProyectoUsuarioEncargado(proyectoId, instanciaId).SingleOrDefault();
            return Json;
        }

        public string SGR_Proyectos_LeerDatosAdicionalesCTEI(int proyectoId, Guid instanciaId)
        {
            string Json = Contexto.uspGetDatosAdicionalesCTEI_JSON(proyectoId, instanciaId).SingleOrDefault();
            return Json;
        }

        public ResultadoProcedimientoDto SGR_Proyectos_GuardarDatosAdicionalesCTEI(DatosAdicionalesCTEIDto datosAdicionalesCTEIDto, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostDatosAdicionalesCTEI_JSON(JsonUtilidades.ACadenaJson(datosAdicionalesCTEIDto), usuario, errorValidacionNegocio);

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

        public ResultadoProcedimientoDto SGR_Proyectos_RegistrarAvalUsoSgr(DatosAvalUsoDto datosAvalUsoDto, string usuario) {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostRegistrarAvalUso_JSON(JsonUtilidades.ACadenaJson(datosAvalUsoDto), usuario, errorValidacionNegocio);

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

        public string SGR_Proyectos_LeerAvalUsoSgr(int proyectoId, Guid instanciaId)
        {
            string Json = Contexto.uspGetDatosAdicionalesCTEI_JSON(proyectoId, instanciaId).SingleOrDefault();
            return Json;
        }

        public string SGR_Proyectos_LeeAvalUsoSgr(int proyectoId, Guid instanciaId)
        {
            string Json = Contexto.uspGetAvalUsoSgr_JSON(proyectoId, instanciaId).SingleOrDefault();
            return Json;
        }

        public bool SGR_Obtener_Proyectos_TieneInstanciaActiva(String ObjetoNegocioId)
        {
            var tieneInstanciaActiva = Contexto.Database.SqlQuery<bool>("[Proyectos].[ProyectoTieneInstanciaActiva] @ObjetoNegocioId",
                new SqlParameter("@ObjetoNegocioId", ObjetoNegocioId)
            ).FirstOrDefault();

            return tieneInstanciaActiva;
        }

        public ResultadoProcedimientoDto SGR_Viabilidad_EliminarOperacionCreditoSGR(int proyectoid, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            var resultado = new ResultadoProcedimientoDto();
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.UspDeleteOperacionesCredito(proyectoid, usuario, errorValidacionNegocio);

                    if (errorValidacionNegocio.Value == null || string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
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


        #endregion



        private static IMapper ConfigurarMapper()
        {
            Mapper.Reset();
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<uspGetSGR_Encabezado_LeerEncabezado_Result, EncabezadoSGRDto>();
                cfg.CreateMap<uspGetObtenerProyectoViabilidadInvolucrados_Result, ProyectoViabilidadInvolucradosDto>();
                cfg.CreateMap<uspGetObtenerProyectoViabilidadInvolucradosFirma_Result, ProyectoViabilidadInvolucradosFirmaDto>();
                cfg.CreateMap<uspGetObtenerProyectoCtus_Result, ProyectoCtusDto>();
                cfg.CreateMap<uspGetEntidadesSolicitarCtus_Result, EntidadesSolicitarCtusDto>();
                cfg.CreateMap<uspGetObtenerProyectosVerificacionOcadPazSgr_Result, ProyectoEntidadVerificacionOcadPazDto>();
                cfg.CreateMap<uspGetObtenerUsuariosVerificacionOcadPazSgr_Result, UsuariosVerificacionOcadPazDto>();
            }).CreateMapper();
        }

        
    }
}
