using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DNP.ServiciosNegocio.Dominio.Dto.Proyectos;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Proyectos;
using DNP.ServiciosNegocio.Persistencia.Modelo;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Proyectos
{
    using System.Data.Entity.Core.Objects;
    using Comunes.Dto.Formulario;
    using Comunes.Excepciones;
    using Comunes.Utilidades;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json.Linq;

    public class DatosBasicosSGRPersistencia : Persistencia, IDatosBasicosSGRPersistencia
    {
        public DatosBasicosSGRPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            Mapper.Reset();

        }

        public DatosBasicosSGRDto ObtenerDatosBasicosSGR(string bpin)
        {
            var listadoFuentesFinanciacion = Contexto.uspGetDatosBasicosSGR(bpin).ToList();
            DatosBasicosSGRDto datosBasicosSGRDto = MapearADatosBasicosSGRDto(listadoFuentesFinanciacion);
            datosBasicosSGRDto.FuentesInterventoria.Insert(0, ObtenerFilaBienio());
            return datosBasicosSGRDto;

        }

        public DatosBasicosSGRDto ObtenerDatosBasicosSGRPreview()
        {
            DatosBasicosSGRDto a = JsonUtilidades.SerializarJsonObjeto<DatosBasicosSGRDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                       @RutasPreviewRecursos.RutaPreviewDatosBasicosSGR);

            a.FuentesInterventoria.Insert(0,ObtenerFilaBienio());
            return a;
        }

        
       
        public void GuardarDefinitivamente(ParametrosGuardarDto<DatosBasicosSGRDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    JObject json = JObject.Parse(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido));
                    (json.SelectToken("FuentesInterventoria"))[0].Remove();

                    Contexto.uspPostDatosBasicosSGR(json.ToString(),
                                                         usuario,
                                                         parametrosGuardar.FormularioId,
                                                         errorValidacionNegocio);

                    if (string.IsNullOrEmpty(errorValidacionNegocio.Value.ToString()))
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
                        var mensajeError = Convert.ToString(errorValidacionNegocio.Value);
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
       

        private FuentesInterventoriaDto ObtenerFilaBienio()
        {
            FuentesInterventoriaDto fuentesInterventoriaDto = new FuentesInterventoriaDto();
            var bienioActivo = (from u in Contexto.Bienio
                                where u.EsActual == true
                                select u).FirstOrDefault();
            var bienios = (from u in Contexto.Bienio
                           where u.EsActivo == true && u.Orden > (bienioActivo.Orden)
                           select u);
            fuentesInterventoriaDto.ValorAprobadoBienio1 = bienioActivo.Bienio1;
            fuentesInterventoriaDto.ValorAprobadoBienio2 = bienios.FirstOrDefault(item => item.Orden == (bienioActivo.Orden + 1)).Bienio1;
            fuentesInterventoriaDto.ValorAprobadoBienio3 = bienios.FirstOrDefault(item => item.Orden == (bienioActivo.Orden + 2)).Bienio1;
            fuentesInterventoriaDto.ValorAprobadoBienio4 = bienios.FirstOrDefault(item => item.Orden == (bienioActivo.Orden + 3)).Bienio1;
            return fuentesInterventoriaDto;
        }

            private DatosBasicosSGRDto MapearADatosBasicosSGRDto(List<uspGetDatosBasicosSGR_Result> listadoDesdeBd)
        {
            var datosBasicosSGRDto = new DatosBasicosSGRDto();
            datosBasicosSGRDto.FuentesInterventoria = new List<FuentesInterventoriaDto>();
            listadoDesdeBd.GroupBy(o =>
            new
            {
                o.DatosBasicosSGRId,
                o.Bpin,
                o.ProyectoId,
                o.NumeroPresentacion,
                o.FechaVerificacionRequisitos,
                o.IdObjetivoSGR,
                o.ObjetivoSGR,
                o.EjecutorPropuestoId,
                o.NitEjecutorPropuesto,
                o.EjecutorPropuesto,
                o.InterventorPropuestoId,
                o.NitInterventorPropuesto,
                o.InterventorPropuesto,
                o.TiempoEstimadoEjecucionFisicaFinanciera,
                o.EstimacionCostosFasesPosteriores
            }).ToList().ForEach(
                w =>
                {
                    var auxFuentesInterventoria = new List<FuentesInterventoriaDto>();
                    listadoDesdeBd.GroupBy(f =>

                    new
                    {
                        f.ProgramacionFuenteId,
                        f.Vigencia,
                        f.GrupoRecurso,
                        f.TipoEntidadId,
                        f.TipoEntidad,
                        f.EntidadId,
                        f.Entidad,
                        f.TipoRecursoId,
                        f.TipoRecurso,
                        f.Solicitado,
                        f.ValorAprobadoBienio1,
                        f.ValorAprobadoBienio2,
                        f.ValorAprobadoBienio3,
                        f.ValorAprobadoBienio4
                    }
                    
                    ).ToList().ForEach(
                        f1 =>
                        {
                            auxFuentesInterventoria.Add(new FuentesInterventoriaDto()
                            {
                                ProgramacionFuenteId = f1.Key.ProgramacionFuenteId,
                                Vigencia = f1.Key.Vigencia,
                                GrupoRecurso = f1.Key.GrupoRecurso,
                                TipoEntidadId = f1.Key.TipoEntidadId,
                                TipoEntidad = f1.Key.TipoEntidad,
                                EntidadId = f1.Key.EntidadId,
                                Entidad = f1.Key.Entidad,
                                TipoRecursoId = f1.Key.TipoRecursoId,
                                TipoRecurso = f1.Key.TipoRecurso,
                                Solicitado = f1.Key.Solicitado,
                                ValorAprobadoBienio1 = f1.Key.ValorAprobadoBienio1,
                                ValorAprobadoBienio2 = f1.Key.ValorAprobadoBienio2,
                                ValorAprobadoBienio3 = f1.Key.ValorAprobadoBienio3,
                                ValorAprobadoBienio4 = f1.Key.ValorAprobadoBienio4
                            });
                        }
                        );

                    datosBasicosSGRDto.DatosBasicosSGRId = w.Key.DatosBasicosSGRId;
                    datosBasicosSGRDto.ProyectoId = w.Key.ProyectoId;
                    datosBasicosSGRDto.Bpin = w.Key.Bpin;
                    datosBasicosSGRDto.NumeroPresentacion = w.Key.NumeroPresentacion;
                    datosBasicosSGRDto.FechaVerificacionRequisitos = w.Key.FechaVerificacionRequisitos;
                    datosBasicosSGRDto.ObjetivoSGRId = w.Key.IdObjetivoSGR;
                    datosBasicosSGRDto.ObjetivoSGR = w.Key.ObjetivoSGR;
                    datosBasicosSGRDto.EjecutorPropuestoId = w.Key.EjecutorPropuestoId;
                    datosBasicosSGRDto.NitEjecutorPropuesto = w.Key.NitEjecutorPropuesto;
                    datosBasicosSGRDto.EjecutorPropuesto = w.Key.EjecutorPropuesto;
                    datosBasicosSGRDto.InterventorPropuestoId = w.Key.InterventorPropuestoId;
                    datosBasicosSGRDto.NitInterventorPropuesto = w.Key.NitInterventorPropuesto;
                    datosBasicosSGRDto.InterventorPropuesto = w.Key.InterventorPropuesto;
                    datosBasicosSGRDto.TiempoEstimadoEjecucionFisicaFinanciera = w.Key.TiempoEstimadoEjecucionFisicaFinanciera;
                    datosBasicosSGRDto.EstimacionCostosFasesPosteriores = w.Key.EstimacionCostosFasesPosteriores;
                    datosBasicosSGRDto.FuentesInterventoria = auxFuentesInterventoria;
                });
           
            return datosBasicosSGRDto;

        }
    }
}


