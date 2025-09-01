using AutoMapper;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.SGP.Viabilidad;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.SGP.Viabilidad
{
    public class PreguntasPersonalizadasSGPPersistencia : PersistenciaSGP, IPreguntasPersonalizadasSGPPersistencia
    {
        public PreguntasPersonalizadasSGPPersistencia(IContextoFactory contextoFactory, IContextoFactorySGR contextoFactorySGR) : base(contextoFactory, contextoFactorySGR)
        {
        }

        public List<TematicaDto> ObtenerPreguntasEspecificasComponenteSGPCustom(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = ContextoOnlySP.Database.SqlQuery<ObjectResult<uspGetObtenerPreguntasEspecificasComponenteCustom_Result>>("Preguntas.uspGetObtenerPreguntasEspecificasComponenteSGPCustom @BPIN,@NivelId,@InstanciaId,@NombreComponentePregunta,@ListaRoles_Json ",
                                              new SqlParameter("BPIN", bPin),
                                              new SqlParameter("NivelId", nivelId),
                                              new SqlParameter("InstanciaId", instanciaId),
                                              new SqlParameter("NombreComponentePregunta", nombreComponente),
                                              new SqlParameter("ListaRoles_Json", listaRoles)
                                               ).SingleOrDefault();            
            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd.ToList();
            var mapper = ConfigurarMapperEspecificas();
            foreach (var tematica in listaResultadoSp)
            {
                if (listadoRetorno.Where(x => x.Tematica == tematica.Tematica).Count() == 0)
                {
                    infoTematicaDto infotematica = MapearTematicaEspecificas(tematica);
                    TematicaDto tamaticaDto = new TematicaDto();
                    tamaticaDto.Tematica = infotematica.Tematica;
                    tamaticaDto.OrdenTematica = infotematica.OrdenTematica;
                    listadoRetorno.Add(tamaticaDto);
                }
            }

            foreach (var tematica in listadoRetorno)
            {
                tematica.Preguntas = new List<PreguntasPersonalizadasDto>();
                foreach (var pregunta in listaResultadoSp.Where(x => x.Tematica == tematica.Tematica))
                {
                    tematica.Preguntas.Add(MapearPregunta(pregunta, mapper));
                }
            }

            //Mapeo Cuestionario
            if (listadoRetorno.Count > 0)
                infoCuestionario = MapearCuestionario(listaResultadoSp.First());

            return listadoRetorno;
        }

        public List<TematicaDto> ObtenerPreguntasEspecificasComponenteSGPCustomSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = ContextoOnlySP.Database.SqlQuery<uspGetObtenerPreguntasEspecificasComponenteCustomSGR_Result>("Preguntas.uspGetObtenerPreguntasEspecificasComponenteSGPCustomSGP @BPIN,@NivelId,@InstanciaId,@NombreComponentePregunta,@ListaRoles_Json ",
                                               new SqlParameter("BPIN", bPin),
                                               new SqlParameter("NivelId", nivelId),
                                               new SqlParameter("InstanciaId", instanciaId),
                                               new SqlParameter("NombreComponentePregunta", nombreComponente),
                                               new SqlParameter("ListaRoles_Json", listaRoles)
                                                ).ToList();
            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd;
            var mapper = ConfigurarMapperEspecificasSGR();
            foreach (var tematica in listaResultadoSp)
            {
                if (listadoRetorno.Where(x => x.Tematica == tematica.Tematica).Count() == 0)
                {
                    infoTematicaDto infotematica = MapearTematicaEspecificasSGR(tematica);
                    TematicaDto tamaticaDto = new TematicaDto();
                    tamaticaDto.Tematica = infotematica.Tematica;
                    tamaticaDto.OrdenTematica = infotematica.OrdenTematica;
                    listadoRetorno.Add(tamaticaDto);
                }
            }

            foreach (var tematica in listadoRetorno)
            {
                tematica.Preguntas = new List<PreguntasPersonalizadasDto>();
                foreach (var pregunta in listaResultadoSp.Where(x => x.Tematica == tematica.Tematica))
                {
                    tematica.Preguntas.Add(MapearPregunta(pregunta, mapper));
                }
            }

            //Mapeo Cuestionario
            if (listadoRetorno.Count > 0)
                infoCuestionario = MapearCuestionario(listaResultadoSp.First());

            return listadoRetorno;
        }

        public List<TematicaDto> ObtenerPreguntasGeneralesComponenteSGPCustom(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = ContextoOnlySP.Database.SqlQuery<ObjectResult<uspGetObtenerPreguntasGeneralesComponenteCustom_Result>>("Preguntas.uspGetObtenerPreguntasGeneralesComponenteSGPCustom @BPIN,@NivelId,@InstanciaId,@NombreComponentePregunta,@ListaRoles_Json ",
                                             new SqlParameter("BPIN", bPin),
                                             new SqlParameter("NivelId", nivelId),
                                             new SqlParameter("InstanciaId", instanciaId),
                                             new SqlParameter("NombreComponentePregunta", nombreComponente),
                                             new SqlParameter("ListaRoles_Json", listaRoles)
                                              ).SingleOrDefault();
            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd.ToList();
            var mapper = ConfigurarMapperGenerales();
            foreach (var tematica in listaResultadoSp.OrderBy(x => x.OrdenTematica))
            {
                if (listadoRetorno.Where(x => x.Tematica == tematica.Tematica).Count() == 0)
                {
                    infoTematicaDto infotematica = MapearTematicaGenerales(tematica);
                    TematicaDto tamaticaDto = new TematicaDto();
                    tamaticaDto.Tematica = infotematica.Tematica;
                    tamaticaDto.OrdenTematica = infotematica.OrdenTematica;
                    listadoRetorno.Add(tamaticaDto);
                }
            }

            foreach (var tematica in listadoRetorno)
            {
                tematica.Preguntas = new List<PreguntasPersonalizadasDto>();
                foreach (var pregunta in listaResultadoSp.Where(x => x.Tematica == tematica.Tematica).OrderBy(x => x.OrdenPregunta))
                {
                    tematica.Preguntas.Add(MapearPregunta(pregunta, mapper));
                }
            }

            //Mapeo Cuestionario
            if (listadoRetorno.Count > 0)
                infoCuestionario = MapearCuestionario(listaResultadoSp.First());

            return listadoRetorno;
        }

        public List<TematicaDto> ObtenerPreguntasGeneralesComponenteSGPCustomSGP(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = ContextoOnlySP.Database.SqlQuery<uspGetObtenerPreguntasGeneralesComponenteCustomSGR_Result>("Preguntas.uspGetObtenerPreguntasGeneralesComponenteSGPCustomSGP @BPIN,@NivelId,@InstanciaId,@NombreComponentePregunta,@ListaRoles ",
                                            new SqlParameter("BPIN", bPin),
                                           new SqlParameter("NivelId", nivelId),
                                           new SqlParameter("InstanciaId", instanciaId),
                                           new SqlParameter("NombreComponentePregunta", nombreComponente),
                                           new SqlParameter("ListaRoles", listaRoles)
                                                ).ToList();
            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd;
            var mapper = ConfigurarMapperGeneralesSGR();
            foreach (var tematica in listaResultadoSp.OrderBy(x => x.OrdenTematica))
            {
                if (listadoRetorno.Where(x => x.Tematica == tematica.Tematica).Count() == 0)
                {
                    infoTematicaDto infotematica = MapearTematicaGeneralesSGR(tematica);
                    TematicaDto tamaticaDto = new TematicaDto();
                    tamaticaDto.Tematica = infotematica.Tematica;
                    tamaticaDto.OrdenTematica = infotematica.OrdenTematica;
                    listadoRetorno.Add(tamaticaDto);
                }
            }

            foreach (var tematica in listadoRetorno)
            {
                tematica.Preguntas = new List<PreguntasPersonalizadasDto>();
                foreach (var pregunta in listaResultadoSp.Where(x => x.Tematica == tematica.Tematica).OrderBy(x => x.OrdenPregunta))
                {
                    tematica.Preguntas.Add(MapearPregunta(pregunta, mapper));
                }
            }

            //Mapeo Cuestionario
            if (listadoRetorno.Count > 0)
                infoCuestionario = MapearCuestionario(listaResultadoSp.First());

            return listadoRetorno;
        }

        public List<AgregarPreguntasDto> ObtenerAgregarPreguntas()
        {
            var listadoDesdeBd = ContextoSGR.SqlAgregarPreguntasDto();

            return listadoDesdeBd.ToList();
        }

        public void GuardarPreguntasPersonalizadasCustomSGP(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(string));
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

                    ContextoOnlySP.Database.ExecuteSqlCommand("Exec Preguntas.uspPostGeneracionCuestionarioPreguntasPersonalizadasCustomSGP @JsonData,@InstanciaId,@SeccionCapituloId,@Usuario,@errorValidacionNegocio output ",
                                                new SqlParameter("JsonData", JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido)),
                                                new SqlParameter("InstanciaId", parametrosGuardar.Contenido.InstanciaId),
                                                new SqlParameter("SeccionCapituloId", parametrosGuardar.Contenido.SeccionCapituloId),
                                                new SqlParameter("Usuario", usuario),
                                                outParam
                                           );
                    if (string.IsNullOrEmpty(outParam.Value.ToString()))
                    {
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(outParam.Value);
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

       

        #region Metodos utilitarios

        private infoTematicaDto MapearTematicaEspecificas(object tematica)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasEspecificasComponenteCustom_Result, infoTematicaDto>()).CreateMapper();
            return mapper.Map<infoTematicaDto>(tematica);
        }

        private infoTematicaDto MapearTematicaEspecificasSGR(object tematica)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasEspecificasComponenteCustomSGR_Result, infoTematicaDto>()).CreateMapper();
            return mapper.Map<infoTematicaDto>(tematica);
        }

        private infoTematicaDto MapearTematicaGenerales(object tematica)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasGeneralesComponenteCustom_Result, infoTematicaDto>()).CreateMapper();
            return mapper.Map<infoTematicaDto>(tematica);
        }

        private infoTematicaDto MapearTematicaGeneralesSGR(object tematica)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasGeneralesComponenteCustomSGR_Result, infoTematicaDto>()).CreateMapper();
            return mapper.Map<infoTematicaDto>(tematica);
        }

        private PreguntasPersonalizadasDto MapearPregunta(object pregunta, IMapper mapper)
        {
            return mapper.Map<PreguntasPersonalizadasDto>(pregunta);
        }

        private CuestionarioDto MapearCuestionario(object cuestionario)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ObjectResult, CuestionarioDto>()).CreateMapper();
            return mapper.Map<CuestionarioDto>(cuestionario);
        }

        private static IMapper ConfigurarMapperGenerales()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasGeneralesComponenteCustom_Result, PreguntasPersonalizadasDto>()
                                        .ForMember(dto => dto.OpcionesRespuesta, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.Respuesta, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }
        private static IMapper ConfigurarMapperGeneralesSGR()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasGeneralesComponenteCustomSGR_Result, PreguntasPersonalizadasDto>()
                                        .ForMember(dto => dto.OpcionesRespuesta, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.Respuesta, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }

        private static IMapper ConfigurarMapperEspecificas()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasEspecificasComponenteCustom_Result, PreguntasPersonalizadasDto>()
                                        .ForMember(dto => dto.OpcionesRespuesta, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.Respuesta, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }

        private static IMapper ConfigurarMapperEspecificasSGR()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasEspecificasComponenteCustomSGR_Result, PreguntasPersonalizadasDto>()
                                        .ForMember(dto => dto.OpcionesRespuesta, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.Respuesta, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }

        #endregion
    }
}
