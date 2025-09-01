using System.Collections.Generic;
using System.Linq;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Genericos;
using DNP.ServiciosNegocio.Servicios.Interfaces.Formulario;
using DNP.ServiciosNegocio.Servicios.Interfaces.Transversales;

namespace DNP.ServiciosNegocio.Servicios.Implementaciones.Formulario
{
    public class RegionalizacionProyectoServicios : ServicioBase<RegionalizacionProyectoDto>, IRegionalizacionProyectoServicios
    {
        private readonly IRegionalizacionProyectoPersistencia _regionalizacionPersistencia;

        public RegionalizacionProyectoServicios(IRegionalizacionProyectoPersistencia regionalizacionPersistencia, IPersistenciaTemporal persistenciaTemporal, IAuditoriaServicios auditoriaServicios) : base(persistenciaTemporal, auditoriaServicios)
        {
            _regionalizacionPersistencia = regionalizacionPersistencia;
        }

        public RegionalizacionProyectoDto ObtenerRegionalizacion(ParametrosConsultaDto parametrosConsulta)
        {
            return Obtener(parametrosConsulta);
        }

        private RegionalizacionProyectoDto MapearInformacion(List<RegionalizacionDto> infoPersistencia)
        {
            var auxProyecto = new RegionalizacionProyectoDto();
            infoPersistencia.GroupBy(o => new { o.ProjectId, o.Bpin, o.ValorTotalSolicitadoProyecto }).ToList().ForEach(
                w =>
                {
                    var auxVigencias = new List<VigenciaDto>();
                    infoPersistencia.Where(v => v.ProjectId == w.Key.ProjectId && v.Bpin == w.Key.Bpin).GroupBy(vig =>
                         new
                         {
                             vig.FfProgramacionVigencia
                         }).ToList().ForEach(ff =>
                         {
                             var auxFuente = new List<FuenteDto>();
                             infoPersistencia.Where(g => g.FfProgramacionVigencia == ff.Key.FfProgramacionVigencia)
                                 .GroupBy(
                                     q =>
                                         new
                                         {
                                             q.FuenteId,
                                             q.FuenteDescripcion,
                                             q.FuenteEtapaId
                                         }).ToList().ForEach(e =>
                                         {
                                             var auxProgramacionFuente = new List<ProgramacionFuenteDto>();

                                             infoPersistencia
                         .Where(d => d.FuenteId == e.Key.FuenteId && d.FfProgramacionVigencia == ff.Key.FfProgramacionVigencia)
                         .GroupBy(t => new
                         {
                             t.FfProgramacionId,
                             t.FfProgramacionVigencia,
                             t.FfProgramacionValorSolicitado
                         })
                         .ToList()
                         .ForEach(t =>
                         {
                             var auxRegionalizacionRecursos = new List<RegionalizacionRecursosDto>();

                             infoPersistencia.Where(d => d.FfProgramacionId == t.Key.FfProgramacionId && d.FfProgramacionVigencia == t.Key.FfProgramacionVigencia && d.RegRecursosId != null).GroupBy(y =>
                                                                      new
                                                      {
                                                          Id = y.RegRecursosId,
                                                          RegionId = y.RegRecursosRegionId,
                                                          AgrupacionId = y.RegRecursosAgrupacionId,
                                                          DepartamentoId = y.RegRecursosDepartamentoId,
                                                          MunicipioId = y.RegRecursosMunicipioId,
                                                          ValorInicial = y.ValorInicial,
                                                          ValorVigente = y.ValorVigente,
                                                          ValorSolicitado = y.FfProgramacionValorSolicitado
                                                      }).ToList().ForEach(reg =>
                                                      {
                                                          var auxDetalleEjecucion = new List<DetalleEjecucionDto>();
                                                          infoPersistencia.Where(det => det.RegRecursosId == reg.Key.Id).ToList().ForEach(det2 =>
                                                                                                                                                                                auxDetalleEjecucion.Add(
                                                                                                                                                                                    new DetalleEjecucionDto()
                                                                                                                                                                        {
                                                                                                                                                                            Mes = det2.Mes ?? 0,
                                                                                                                                                                            ValorInicial = det2.EjecucionValorInicial,
                                                                                                                                                                            ValorVigente = det2.EjecucionValorVigente,
                                                                                                                                                                            Compromiso = det2.EjecucionCompromiso,
                                                                                                                                                                            Obligacion = det2.EjecucionObligacion,
                                                                                                                                                                            Pago = det2.EjecucionPago
                                                                                                                                                                        }
                                                                                                                                                                                                        )
                                                                                                                                                                       );
                                                          auxRegionalizacionRecursos.Add(new RegionalizacionRecursosDto()
                                                          {
                                                              Id = reg.Key.Id,
                                                              ValorVigente = reg.Key.ValorVigente,
                                                              ValorInicial = reg.Key.ValorInicial,
                                                              RegionId = reg.Key.RegionId,
                                                              AgrupacionId = reg.Key.AgrupacionId,
                                                              DepartamentoId = reg.Key.DepartamentoId,
                                                              DetalleEjecucion = auxDetalleEjecucion,
                                                              MunicipioId = reg.Key.MunicipioId,
                                                              ValorSolicitado = reg.Key.ValorSolicitado

                                                          });
                                                      });


                             auxProgramacionFuente.Add(new ProgramacionFuenteDto()
                             {
                                 Id = t.Key.FfProgramacionId,
                                 ValorSolicititado = t.Key.FfProgramacionValorSolicitado,
                                 RegionalizacionRecursos = auxRegionalizacionRecursos.OrderBy(s => s.Id).ToList()

                             });
                         });
                                             auxFuente.Add(new FuenteDto()
                                             {
                                                 Id = e.Key.FuenteId,
                                                 Nombre = e.Key.FuenteDescripcion,
                                                 EtapaId = e.Key.FuenteEtapaId,
                                                 ProgramacionFuente = auxProgramacionFuente.OrderBy(s => s.Id).ToList()
                                             });
                                         });

                             auxVigencias.Add(new VigenciaDto()
                             {
                                 Vigencia = ff.Key.FfProgramacionVigencia,
                                 Fuente = auxFuente
                             });
                         }
                    );

                    auxProyecto.Id = w.Key.ProjectId;
                    auxProyecto.Bpin = w.Key.Bpin;
                    auxProyecto.ValorTotal = w.Key.ValorTotalSolicitadoProyecto;
                    auxProyecto.Vigencias = auxVigencias;
                });
            return auxProyecto;
        }

        public RegionalizacionProyectoDto ObtenerRegionalizacionPreview()
        {
            return MapearInformacion(_regionalizacionPersistencia.ObtenerRegionalizacionPreview());
        }

        protected override RegionalizacionProyectoDto ObtenerDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            List<RegionalizacionDto> infoPersistencia = _regionalizacionPersistencia.ObtenerRegionalizacion(parametrosConsultaDto.Bpin);
            return MapearInformacion(infoPersistencia);
        }

        protected override void GuardadoDefinitivo(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar,
                                                   string usuario)
        {
            _regionalizacionPersistencia.GuardarDefinitivamente(parametrosGuardar, usuario);
        }
    }
}
