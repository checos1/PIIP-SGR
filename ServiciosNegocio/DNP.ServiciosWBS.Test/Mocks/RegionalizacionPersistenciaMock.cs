namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;

    public class RegionalizacionPersistenciaMock : IRegionalizacionProyectoPersistencia
    {
        public RegionalizacionProyectoDto ObtenerRegionalizacion(string bpin)
        {
            if (bpin.Equals("2017011000236"))
            {
                var auxVigencias = new List<VigenciaRegionalizacionDto>();
                var auxMeses = new List<MesDto>();
                var auxFuente = new List<FuenteDto>();
                var auxRegionalizacionRecursos = new List<RegionalizacionRecursosDto>();


                auxRegionalizacionRecursos.Add(new RegionalizacionRecursosDto()
                {
                    RegionalizacionRecursosId = 10,
                    RegRecursoRegionID = 2,
                    RegRecursosDepartamentoId = 2,
                    RegRecursosMunicipioId = 32,
                    RegRecursosAgrupacionId = 4,
                    RegValorSolicitado = 1545454,
                    RegValorInicial = 1111152,
                    RegValorVigente = 11112,
                    RegCompromiso = 32323,
                    RegObligacion = 342366,
                    RegPago = 122,
                    EjecucionRecursosId = null
                });
                auxRegionalizacionRecursos.Add(new RegionalizacionRecursosDto()
                {
                    RegionalizacionRecursosId = 12,
                    RegRecursoRegionID = 2,
                    RegRecursosDepartamentoId = 2,
                    RegRecursosMunicipioId = 32,
                    RegRecursosAgrupacionId = 4,
                    RegValorSolicitado = 35432,
                    RegValorInicial = 32423,
                    RegValorVigente = 4234,
                    RegCompromiso = 2342,
                    RegObligacion = 423,
                    RegPago = 122,
                    EjecucionRecursosId = null
                });

                auxFuente.Add(new FuenteDto()
                {
                    FuenteId = 2,
                    GrupoRecurso = "Territorial",
                    Nombre = "Municipios - CAICEDONIA - Propios",
                    ValorSolicitado = 0,
                    ValorInicial = 0,
                    ValorVigente = 0,
                    Compromiso = 0,
                    Obligacion = 0,
                    Pago = 0,
                    Regionalizacion = auxRegionalizacionRecursos.OrderBy(r => r.RegionalizacionRecursosId).ToList()
                });
                auxFuente.Add(new FuenteDto()
                {
                    FuenteId = 2,
                    GrupoRecurso = "Territorial",
                    Nombre = "Municipios -  Propios",
                    ValorSolicitado = 5155554,
                    ValorInicial = 51564,
                    ValorVigente = 51515,
                    Compromiso = 5515,
                    Obligacion = 5166,
                    Pago = 0,
                    Regionalizacion = auxRegionalizacionRecursos.OrderBy(r => r.RegionalizacionRecursosId).ToList()
                });
                auxMeses.Add(new MesDto()
                {
                    Mes = 0,
                    NombreMes = "Programación",
                    Fuente = auxFuente.OrderBy(f => f.FuenteId).ToList()
                });
                auxMeses.Add(new MesDto()
                {
                    Mes = 12,
                    NombreMes = "Diciembre",
                    Fuente = auxFuente.OrderBy(f => f.FuenteId).ToList()
                });
                auxVigencias.Add(new VigenciaRegionalizacionDto()
                {
                    Vigencia = 2018,
                    Mes = auxMeses.OrderBy(s => s.Mes).ToList()
                });
                return new RegionalizacionProyectoDto()
                {
                    Bpin = "2017011000236",
                    Vigencias = auxVigencias.OrderBy(v => v.Vigencia).ToList()
                };
            }
            else
            {
                return new RegionalizacionProyectoDto();
            }
        }
        public RegionalizacionProyectoDto ObtenerRegionalizacionPreview()
        {
            var auxVigencias = new List<VigenciaRegionalizacionDto>();
            var auxMeses = new List<MesDto>();
            var auxFuente = new List<FuenteDto>();
            var auxRegionalizacionRecursos = new List<RegionalizacionRecursosDto>();


            auxRegionalizacionRecursos.Add(new RegionalizacionRecursosDto()
            {
                RegionalizacionRecursosId = 10,
                RegRecursoRegionID = 2,
                RegRecursosDepartamentoId = 2,
                RegRecursosMunicipioId = 32,
                RegRecursosAgrupacionId = 4,
                RegValorSolicitado = 1545454,
                RegValorInicial = 1111152,
                RegValorVigente = 11112,
                RegCompromiso = 32323,
                RegObligacion = 342366,
                RegPago = 122,
                EjecucionRecursosId = null
            });
            auxRegionalizacionRecursos.Add(new RegionalizacionRecursosDto()
            {
                RegionalizacionRecursosId = 12,
                RegRecursoRegionID = 2,
                RegRecursosDepartamentoId = 2,
                RegRecursosMunicipioId = 32,
                RegRecursosAgrupacionId = 4,
                RegValorSolicitado = 35432,
                RegValorInicial = 32423,
                RegValorVigente = 4234,
                RegCompromiso = 2342,
                RegObligacion = 423,
                RegPago = 122,
                EjecucionRecursosId = null
            });

            auxFuente.Add(new FuenteDto()
            {
                FuenteId = 2,
                GrupoRecurso = "Territorial",
                Nombre = "Municipios - CAICEDONIA - Propios",
                ValorSolicitado = 0,
                ValorInicial = 0,
                ValorVigente = 0,
                Compromiso = 0,
                Obligacion = 0,
                Pago = 0,
                Regionalizacion = auxRegionalizacionRecursos.OrderBy(r => r.RegionalizacionRecursosId).ToList()
            });
            auxFuente.Add(new FuenteDto()
            {
                FuenteId = 2,
                GrupoRecurso = "Territorial",
                Nombre = "Municipios -  Propios",
                ValorSolicitado = 5155554,
                ValorInicial = 51564,
                ValorVigente = 51515,
                Compromiso = 5515,
                Obligacion = 5166,
                Pago = 0,
                Regionalizacion = auxRegionalizacionRecursos.OrderBy(r => r.RegionalizacionRecursosId).ToList()
            });
            auxMeses.Add(new MesDto()
            {
                Mes = 0,
                NombreMes = "Programación",
                Fuente = auxFuente.OrderBy(f => f.FuenteId).ToList()
            });
            auxMeses.Add(new MesDto()
            {
                Mes = 12,
                NombreMes = "Diciembre",
                Fuente = auxFuente.OrderBy(f => f.FuenteId).ToList()
            });
            auxVigencias.Add(new VigenciaRegionalizacionDto()
            {
                Vigencia = 2018,
                Mes = auxMeses.OrderBy(s => s.Mes).ToList()
            });
            return new RegionalizacionProyectoDto()
            {
                Bpin = "2017761220016",
                Vigencias = auxVigencias.OrderBy(v => v.Vigencia).ToList()
            };
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar,
                                           string usuario)
        {
            
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }
    }
}
