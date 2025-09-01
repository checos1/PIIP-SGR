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

    public class PreguntasPersonalizadasPersistencia : Persistencia, IPreguntasPersonalizadasPersistencia
    {
        #region Incializacion

        public PreguntasPersonalizadasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        #endregion

        #region Consulta

        public List<TematicaDto> ObtenerPreguntasEspecificas(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasEspecificasCustom(bPin, nivelId, instanciaId, listaRoles);
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

        public List<TematicaDto> ObtenerPreguntasGenerales(string bPin, Guid nivelId, Guid instanciaId, string listaRoles, out CuestionarioDto infoCuestionario)
        {
            infoCuestionario = null;
            List<TematicaDto> listadoRetorno = new List<TematicaDto>();

            var listadoDesdeBd = Contexto.uspGetObtenerPreguntasGeneralesCustom(bPin, nivelId, instanciaId, listaRoles);
            if (listadoDesdeBd == null)
                return listadoRetorno;

            var listaResultadoSp = listadoDesdeBd.ToList();
            var mapper = ConfigurarMapperGenerales();
            foreach (var tematica in listaResultadoSp.OrderBy(x=>x.OrdenTematica))
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

        public List<AgregarPreguntasDto> ObtenerAgregarPreguntas()
        {
            var listadoDesdeBd = Contexto.SqlAgregarPreguntasDto();

            return listadoDesdeBd.ToList();
        }

        public DatosGeneralesProyectosDto ObtenerDatosGeneralesProyecto(int? pProyectoId, Guid pNivelId)
        {
            var result = Contexto.DatosGeneralesProyecto(pProyectoId, pNivelId);
            DatosGeneralesProyectosDto datosGenerales = result.Select(j => new DatosGeneralesProyectosDto
            {
                ProyectoId = j.ProyectoId,
                NombreProyecto = j.NombreProyecto,
                BPIN = j.BPIN,
                EntidadId = j.EntidadId,
                Entidad = j.Entidad,
                SectorId = j.SectorId,
                Sector = j.Sector,
                EstadoId = j.EstadoId,
                Estado = j.Estado,
                Horizonte = j.Horizonte,
                Valor = j.Valor
            }).FirstOrDefault();

            return datosGenerales;
        }

        public ConfiguracionEntidadDto ObtenerConfiguracionEntidades(int? pProyectoId, Guid pNivelId)
        {
            var result = Contexto.uspGetConfiguracionEntidades_AplicaTecnico(pProyectoId, pNivelId);
            ConfiguracionEntidadDto configuracion = result.Select(j => new ConfiguracionEntidadDto
            {
                ProyectoId = j.ProyectoId,
                FaseId = j.FaseId,
                Fase = j.Fase,
                AplicaTecnico = j.AplicaTecnico
            }).FirstOrDefault();

            return configuracion;
        }

        #endregion

        #region Guardar

        public void GuardarDefinitivamente(ParametrosGuardarDto<ServicioPreguntasPersonalizadasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostGeneracionCuestionarioPreguntasPersonalizadas(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), parametrosGuardar.Contenido.InstanciaId, usuario, resultado);

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

        public void DevolverCuestionarioProyecto(Guid nivelId, Guid instanciaId, int estadoAccionesPorInstancia)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostDevolverCuestionarioProyecto(nivelId, instanciaId, estadoAccionesPorInstancia, resultado);

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

        private infoTematicaDto MapearTematicaEspecificas(object tematica)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasEspecificasCustom_Result, infoTematicaDto>()).CreateMapper();
            return mapper.Map<infoTematicaDto>(tematica);
        }

        private infoTematicaDto MapearTematicaGenerales(object tematica)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasGeneralesCustom_Result, infoTematicaDto>()).CreateMapper();
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
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasGeneralesCustom_Result, PreguntasPersonalizadasDto>()
                                        .ForMember(dto => dto.OpcionesRespuesta, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.Respuesta, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }

        private static IMapper ConfigurarMapperEspecificas()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<uspGetObtenerPreguntasEspecificasCustom_Result, PreguntasPersonalizadasDto>()
                                        .ForMember(dto => dto.OpcionesRespuesta, opt => opt.MapFrom(ent => JsonConvert.DeserializeObject<object>(ent.OpcionesRespuesta)))
                                        .ForMember(dto => dto.Respuesta, opt => opt.MapFrom(ent => ent.Respuesta))
                                        .ForMember(dto => dto.ObligaObservacion, opt => opt.MapFrom(ent => (ent.ObligaObservacion != null) ? JsonConvert.DeserializeObject<object>(ent.ObligaObservacion) : null)))
                                        .CreateMapper();
        }

        #endregion
    }
}
