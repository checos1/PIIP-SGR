using AutoMapper;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Preguntas;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Preguntas;
using DNP.ServiciosNegocio.Persistencia.ModeloSGR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Preguntas
{
    public class PreguntasPersonalizadasComponentePersistencia : PersistenciaSGR, IPreguntasPersonalizadasComponentePersistencia
    {
        public PreguntasPersonalizadasComponentePersistencia(IContextoFactorySGR contextoFactory) : base(contextoFactory)
        {
        }

        public List<TematicaDto> ObtenerPreguntasEspecificasComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasEspecificasComponenteCustom(bPin, nivelId, instanciaId, nombreComponente, listaRoles);
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

        public List<TematicaDto> ObtenerPreguntasEspecificasComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasEspecificasComponenteCustomSGR(bPin, nivelId, instanciaId, nombreComponente, listaRoles);
            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd.ToList();
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

        public List<TematicaDto> ObtenerPreguntasGeneralesComponente(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasGeneralesComponenteCustom(bPin, nivelId, instanciaId, nombreComponente, listaRoles);
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

        public List<TematicaDto> ObtenerPreguntasGeneralesComponenteSGR(string bPin, Guid nivelId, Guid instanciaId, string nombreComponente, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasGeneralesComponenteCustomSGR(bPin, nivelId, instanciaId, nombreComponente, listaRoles);
            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd.ToList();
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
            var listadoDesdeBd = Contexto.SqlAgregarPreguntasDto();

            return listadoDesdeBd.ToList();
        }

        public void GuardarDefinitivamenteCustomSGR(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostGeneracionCuestionarioPreguntasPersonalizadasCustomSGR(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), parametrosGuardar.Contenido.InstanciaId, parametrosGuardar.Contenido.SeccionCapituloId, usuario, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        var mensajeError = Convert.ToString(resultado.Value);
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
            }
        }

        public ConceptosPreviosEmitidosDto ObtenerConceptosPreviosEmitidos(string bPin, int? tipoConcepto)
        {
            try
            {
                var ConceptosPreviosEmitidos = Contexto.uspGetConceptosEmitidos_JSON(bPin, tipoConcepto).SingleOrDefault();

                if (ConceptosPreviosEmitidos == null)
                {
                    var objOperacionCreditoDatosGenerales = new ConceptosPreviosEmitidosDto()
                    {
                        TotalConceptosEmitidos = 0,
                        FechaSolicitudUltimoConcepto = "",
                        FechaEmisionUltimoConcepto = "",
                        ConceptosEmitidos = new List<ConceptoEmitido>()
                            {
                                new ConceptoEmitido()
                                {
                                    Id = 0,
                                    FechaEmision = ""
                                }
                            },
                    };

                    return objOperacionCreditoDatosGenerales;
                }
                else
                {
                    return JsonConvert.DeserializeObject<ConceptosPreviosEmitidosDto>(ConceptosPreviosEmitidos);
                }
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
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
