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
    using ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;


    public class FocalizacionPersistencia : Persistencia, IFocalizacionPersistencia
    {
        public FocalizacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
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
                    Contexto.uspPostInsertarProyectoFocalizacionVA(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);

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

        public FocalizacionProyectoDto ObtenerFocalizacionProyecto(string bpin)
        {
            if (string.IsNullOrEmpty(bpin)) return null;
            var consultaDesdeBd = Contexto.uspGetProyectoFocalizacionVA(bpin);
            return MapearAFocalizacionDto(consultaDesdeBd.ToList());
        }

        private FocalizacionProyectoDto MapearAFocalizacionDto(List<uspGetProyectoFocalizacionVA_Result> listadoDesdeBd)
        {
            var focalizacionProyecto = new FocalizacionProyectoDto();
            focalizacionProyecto.Politicas = new List<PoliticaDto>();
            listadoDesdeBd.GroupBy(o =>
            new
            {
                o.BPIN,
                o.ProyectoId,
                o.Politica,
                o.PoliticaId
            }).ToList().ForEach(
                w =>
                {
                    var auxPolitica = new List<PoliticaDto>();
                    listadoDesdeBd.Where(v => v.Politica == w.Key.Politica).GroupBy(politica =>
                        new
                        {
                            politica.PoliticaId,
                            politica.Politica
                        }).ToList()
                         .ForEach(
                          s =>
                          {
                              var auxDimension = new List<DimensionDto>();
                              listadoDesdeBd.Where(d => d.PoliticaId == s.Key.PoliticaId && d.PoliticaId == w.Key.PoliticaId).GroupBy(y =>
                              new
                              {
                                  y.DimensionId,
                                  y.Dimension,
                                  y.FocalizacionProyectoId
                              }).ToList().ForEach(x =>
                              {
                                  auxDimension.Add(new DimensionDto()
                                  {
                                      DimensionId = x.Key.DimensionId,
                                      Dimension = x.Key.Dimension,
                                      FocalizacionProyectoId = x.Key.FocalizacionProyectoId
                                  });
                              });
                          
                             focalizacionProyecto.Politicas.Add(new PoliticaDto()
                             {
                               Politica = w.Key.Politica,
                                 PoliticaId = w.Key.PoliticaId,
                                 Dimensiones = auxDimension.ToList()
                             });
                     focalizacionProyecto.Bpin = w.Key.BPIN;
                     focalizacionProyecto.ProyectoId = w.Key.ProyectoId;
                          });
                });
         return focalizacionProyecto;

        }



        public FocalizacionProyectoDto ObtenerFocalizacionProyectoPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<FocalizacionProyectoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewProyectoFocalizacion);
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostProyectosValoresFocalizacionTemp (parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }
    }
}
