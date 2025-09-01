using System;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System.Collections.Generic;
using AutoMapper;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Preguntas
{
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Dominio.Dto.Preguntas;
    using Interfaces.Preguntas;
    using Newtonsoft.Json;
    using Comunes.Utilidades;
    using Comunes;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;

    public class PreguntasPersistencia : Persistencia, IPreguntasPersistencia
    {
        #region Incializacion

        public PreguntasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }
        #endregion

        #region Consulta
        public List<PreguntasDto> ObtenerPreguntasEspecificas(string bPin, Guid nivelId, Guid instanciaId, Guid formularioId, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<PreguntasDto> listadoRetorno = new List<PreguntasDto>();

            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasEspecificas(bPin, nivelId, instanciaId, formularioId);
            if (listadoDesdeBd == null)
                return listadoRetorno;
            var listaResultadoSp = listadoDesdeBd.ToList();
            var mapper = ConfigurarMapperEspecificas();
            foreach (var pregunta in listaResultadoSp)
            {
                listadoRetorno.Add(MapearPregunta(pregunta, mapper));
            }
            //Mapeo Cuestionario
            if (listadoRetorno.Count > 0)
                infoCuestionario = MapearCuestionario(listaResultadoSp.First());

            return listadoRetorno;
        }

        public List<PreguntasDto> ObtenerPreguntasGenerales(string bPin, Guid nivelId, Guid instanciaId, Guid formularioId, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<PreguntasDto> listadoRetorno = new List<PreguntasDto>();
            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasGenerales(bPin, nivelId, instanciaId, formularioId);

            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd.ToList();
            var mapper = ConfigurarMapperGenerales();
            foreach (var pregunta in listaResultadoSp)
            {
                listadoRetorno.Add(MapearPregunta(pregunta, mapper));
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

        public ServicioPreguntasDto ObtenerPreguntasPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<ServicioPreguntasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.PreviewPreguntas);
        }

        #endregion

        #region Guardar

        public void GuardarDefinitivamente(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostGeneracionCuestionarioPreguntas(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, false,  parametrosGuardar.InstanciaId, parametrosGuardar.FormularioId, resultado );

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

        public void GuardarTemporalmente(ParametrosGuardarDto<ServicioPreguntasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostGeneracionCuestionarioPreguntas(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, true,  parametrosGuardar.InstanciaId, parametrosGuardar.FormularioId, resultado);

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

        #endregion

        #region Metodos utilitarios
        private PreguntasDto MapearPregunta(object pregunta, IMapper mapper)
        {
            return mapper.Map<PreguntasDto>(pregunta);
        }

        private CuestionarioDto MapearCuestionario(object cuestionario)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ObjectResult, CuestionarioDto>()).CreateMapper();
            return mapper.Map<CuestionarioDto>(cuestionario);
        }

        private static IMapper ConfigurarMapperGenerales()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasGenerales_Result, PreguntasDto>()
                                        .ForMember(dto => dto.OpcionesRespuestas, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.OpcionesRespuestasSeleccionado, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }


        private static IMapper ConfigurarMapperEspecificas()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasEspecificas_Result, PreguntasDto>()
                                        .ForMember(dto => dto.OpcionesRespuestas, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.OpcionesRespuestasSeleccionado, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }

        #endregion
    }
}
