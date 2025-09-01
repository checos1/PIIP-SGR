namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using ServiciosNegocio.Comunes;
    using Interfaces;
    using Modelo;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;


    public class FocalizacionProyectoPersistencia : Persistencia, IFocalizacionProyectoPersistencia
    {
        public FocalizacionProyectoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionProyectoDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostInsertarFocalizacionVA(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);

                    if (string.IsNullOrEmpty(resultado.Value.ToString()))
                    {
                        var temporal = Contexto.AlmacenamientoTemporal.FirstOrDefault(at => at.InstanciaId == parametrosGuardar.InstanciaId && at.AccionId == parametrosGuardar.AccionId);
                        if (temporal != null)
                            Contexto.AlmacenamientoTemporal.Remove(temporal);

                        Contexto.SaveChanges();
                        dbContextTransaction.Commit();
                        return;
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

        public FocalizacionProyectoDto ObtenerFocalizacion(string bpin)
        {
            if (string.IsNullOrEmpty(bpin)) return null;
            var consultaDesdeBd = Contexto.uspGetRecursosFocalizacionVA(bpin);
            return MapearAFocalizacionProyectoDto(consultaDesdeBd.ToList());
        }

        private FocalizacionProyectoDto MapearAFocalizacionProyectoDto(List<uspGetRecursosFocalizacionVA_Result> listadoDesdeBd)
        {
            var focalizacionProyecto = new FocalizacionProyectoDto();
            focalizacionProyecto.Vigencias = new List<VigenciaFocalizacionDto>();
            listadoDesdeBd.GroupBy(o => 
            new
            { o.BPIN,
                o.Vigencia
            }).ToList().ForEach(
                w =>
                {
                    var auxFuentes = new List<FuenteDto>();
                    listadoDesdeBd.Where(v => v.Vigencia == w.Key.Vigencia).GroupBy(vigencia =>
                        new
                        {
                            vigencia.FuenteId,
                            vigencia.Fuente,
                            vigencia.FSolicitado,
                            vigencia.FInicial,
                            vigencia.FVigente
                         }).ToList()
                         .ForEach(
                          fuente =>
                          {
                             var auxPolitica = new List<PoliticaDto>();
                             listadoDesdeBd.Where(g => g.FuenteId == fuente.Key.FuenteId && g.Vigencia == w.Key.Vigencia).GroupBy(q =>
                             new
                             {
                                q.PoliticaId,
                                q.Politica
                             }).ToList().ForEach(t =>
                             {
                                var auxDimension = new List<DimensionDto>();
                                listadoDesdeBd.Where(d => d.PoliticaId == t.Key.PoliticaId && d.FuenteId == fuente.Key.FuenteId && d.Vigencia == w.Key.Vigencia).GroupBy(y =>
                                new
                                {
                                    y.DimensionId,
                                    y.Dimension,
                                    y.ValorInicial,
                                    y.ValorSolicitado,
                                    y.ValorVigente,
                                    y.FocalizacionRecursosId
                                }).ToList().ForEach(s =>
                                {
                                   auxDimension.Add(new DimensionDto()
                                   {
                                        DimensionId = s.Key.DimensionId,
                                        Dimension = s.Key.Dimension,
                                        ApropiacionInicial = s.Key.ValorInicial,
                                        Solicitado = s.Key.ValorSolicitado,
                                        ApropiacionVigente = s.Key.ValorVigente,
                                        FocalizacionRecursosId = s.Key.FocalizacionRecursosId
                                   });
                                });
                                auxPolitica.Add(new PoliticaDto()
                                {
                                    PoliticaId = t.Key.PoliticaId,
                                    Politica = t.Key.Politica,
                                    Dimensiones = auxDimension.ToList()
                                });
                             });
                             auxFuentes.Add(new FuenteDto()
                             {
                                 FuenteId = fuente.Key.FuenteId,
                                 Fuente = fuente.Key.Fuente,
                                 FInicial = fuente.Key.FInicial,
                                 FSolicitado = fuente.Key.FSolicitado,
                                 FVigente = fuente.Key.FVigente,
                                 Politicas = auxPolitica.ToList()
                             });
                         });
                         focalizacionProyecto.Vigencias.Add(new VigenciaFocalizacionDto()
                         {
                            Vigencia = w.Key.Vigencia,
                            Fuentes = auxFuentes.ToList()
                         });
                 focalizacionProyecto.Bpin = w.Key.BPIN;
                });
         return focalizacionProyecto;

        }



        public FocalizacionProyectoDto ObtenerFocalizacionPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<FocalizacionProyectoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewFocalizacion);
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostRecursosValoresFocalizacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }
    }
}
