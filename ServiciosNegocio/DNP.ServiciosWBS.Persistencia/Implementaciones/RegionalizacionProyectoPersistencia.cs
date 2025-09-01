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
    using ServiciosNegocio.Dominio.Dto.Formulario;


    public class RegionalizacionProyectoPersistencia : Persistencia, IRegionalizacionProyectoPersistencia
    {
        public RegionalizacionProyectoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostInsertarRecursos(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, resultado);

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

        public RegionalizacionProyectoDto ObtenerRegionalizacion(string bpin)
        {
            if (string.IsNullOrEmpty(bpin)) return null;
            var consultaDesdeBd = Contexto.uspGetRecursosRegionalizacion(bpin);
            return MapearARegionalizacionProyectoDto(consultaDesdeBd.ToList());
        }

        private RegionalizacionProyectoDto MapearARegionalizacionProyectoDto(List<uspGetRecursosRegionalizacion_Result> listadoDesdeBd)
        {
            var regionalizacionProyecto = new RegionalizacionProyectoDto();
            regionalizacionProyecto.Vigencias = new List<VigenciaRegionalizacionDto>();
            listadoDesdeBd.GroupBy(o => new { o.BPIN, o.Vigencia }).ToList().ForEach(
                w =>
                {
                    var auxmeses = new List<MesDto>();
                    listadoDesdeBd.Where(v => v.Vigencia == w.Key.Vigencia).GroupBy(mes =>
                       new
                       {
                           mes.Mes,
                           mes.NomMes
                       }).ToList()
                        .ForEach(ff =>
                        {
                            var auxFuente = new List<FuenteDto>();
                            listadoDesdeBd.Where(g => g.Mes == ff.Key.Mes && g.Vigencia == w.Key.Vigencia)
                                .GroupBy(
                                    q =>
                                        new
                                        {
                                            q.FuenteId,
                                            q.Fuente,
                                            q.GrupoRecurso,
                                            q.ValorSolicitado,
                                            q.ValorInicial,
                                            q.ValorVigente,
                                            q.Compromiso,
                                            q.Obligacion,
                                            q.Pago
                                        }).ToList()
                                    .ForEach(t =>
                                    {
                                        var auxRegionalizacionRecursos = new List<RegionalizacionRecursosDto>();

                                        listadoDesdeBd.Where(d => d.FuenteId == t.Key.FuenteId
                                                               && d.GrupoRecurso == t.Key.GrupoRecurso
                                                               && d.ValorSolicitado == t.Key.ValorSolicitado
                                                               && d.ValorInicial == t.Key.ValorInicial
                                                               && d.ValorVigente == t.Key.ValorVigente
                                                               && d.Compromiso == t.Key.Compromiso
                                                               && d.Obligacion == t.Key.Obligacion
                                                               && d.Pago == t.Key.Pago
                                                               && d.Vigencia == w.Key.Vigencia
                                                               && d.Mes == ff.Key.Mes
                                                             //&& d.RegionalizacionRecursosId != null
                                                             ).GroupBy(y =>
                                                                                 new
                                                                                 {
                                                                                     Id = y.RegionalizacionRecursosId,
                                                                                     RegionId = y.RegRecursosRegionId,
                                                                                     Region = y.RegRecursosRegion,
                                                                                     AgrupacionId = y.RegAgrupacionId,
                                                                                     Agrupacion = y.RegRecursosAgrupacion,
                                                                                     DepartamentoId = y.RegRecursosDepartamentoId,
                                                                                     Departamento = y.RegRecursosDepartamento,
                                                                                     MunicipioId = y.RegRecursosMunicipioId,
                                                                                     Municipio = y.RegRecursosMunicipio,
                                                                                     NombreRegionalizacion = y.RegRecursosRegionalizacionNombre,
                                                                                     ValorInicial = y.RegValorInicial,
                                                                                     ValorVigente = y.RegValorVigente,
                                                                                     ValorSolicitado = y.RegValorSolicitado,
                                                                                     Compromiso = y.RegCompromiso,
                                                                                     Obligacion = y.RegObligacion,
                                                                                     Pago = y.RegPago,
                                                                                     EjecucionRecursosId = y.EjecucionRecursosId
                                                                                 }).ToList().ForEach(reg =>
                                                                                 {

                                                                                     auxRegionalizacionRecursos.Add(new RegionalizacionRecursosDto()
                                                                                     {
                                                                                         RegionalizacionRecursosId = reg.Key.Id,
                                                                                         RegValorVigente = reg.Key.ValorVigente,
                                                                                         RegValorInicial = reg.Key.ValorInicial,
                                                                                         RegRecursoRegionID = reg.Key.RegionId,
                                                                                         RegRecursoRegion = reg.Key.Region,
                                                                                         RegRecursosAgrupacionId = reg.Key.AgrupacionId,
                                                                                         RegRecursosAgrupacion = reg.Key.Agrupacion,
                                                                                         RegRecursosDepartamentoId = reg.Key.DepartamentoId,
                                                                                         RegRecursosDepartamento = reg.Key.Departamento,
                                                                                         RegRecursosMunicipioId = reg.Key.MunicipioId,
                                                                                         RegRecursosMunicipio = reg.Key.Municipio,
                                                                                         RegRecursosRegionalizacionNombre = reg.Key.NombreRegionalizacion,
                                                                                         RegValorSolicitado = reg.Key.ValorSolicitado,
                                                                                         RegCompromiso = reg.Key.Compromiso,
                                                                                         RegObligacion = reg.Key.Obligacion,
                                                                                         RegPago = reg.Key.Pago,
                                                                                         EjecucionRecursosId = reg.Key.EjecucionRecursosId
                                                                                     });
                                                                                 });

                                        auxFuente.Add(new FuenteDto()
                                        {
                                            FuenteId = t.Key.FuenteId,
                                            Nombre = t.Key.Fuente,
                                            GrupoRecurso = t.Key.GrupoRecurso,
                                            ValorSolicitado = t.Key.ValorSolicitado,
                                            ValorVigente = t.Key.ValorVigente,
                                            Compromiso = t.Key.Compromiso,
                                            Obligacion = t.Key.Obligacion,
                                            Pago = t.Key.Pago,
                                            ValorInicial = t.Key.ValorInicial,
                                            Regionalizacion = auxRegionalizacionRecursos.OrderBy(s => s.RegRecursoRegionID).ToList()
                                        });

                                    });

                            auxmeses.Add(new MesDto()
                            {
                                Mes = ff.Key.Mes,
                                NombreMes = ff.Key.NomMes,
                                Fuente = auxFuente.OrderBy(s => s.FuenteId).ToList()
                            });
                        });

                    regionalizacionProyecto.Vigencias.Add(new VigenciaRegionalizacionDto()
                    {
                        Vigencia = w.Key.Vigencia,
                        Mes = auxmeses
                    });

                    regionalizacionProyecto.Bpin = w.Key.BPIN;
                });

            return regionalizacionProyecto;
        }



        public RegionalizacionProyectoDto ObtenerRegionalizacionPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<RegionalizacionProyectoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewRegionalizacion);
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)

        {
            Contexto.uspPostRecursosValoresFuentesTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }
    }
}