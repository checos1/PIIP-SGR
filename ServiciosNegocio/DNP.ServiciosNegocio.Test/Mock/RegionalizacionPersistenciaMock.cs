using System.Collections.Generic;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;

namespace DNP.ServiciosNegocio.Test.Mock
{
    using Comunes.Dto.Formulario;

    public class RegionalizacionPersistenciaMock : IRegionalizacionProyectoPersistencia
    {
        public List<RegionalizacionDto> ObtenerRegionalizacion(string bpin)
        {
            if (bpin.Equals("2017011000236"))
            {
                return new List<RegionalizacionDto>()
                {
                    new RegionalizacionDto()
                {
                    ProjectId = 22465,
                    Bpin = "2017011000236",
                    ValorTotalSolicitadoProyecto = 2900629556000.00,
                    FuenteFinanciacionId = 16013                    ,
                    FuenteId = 25022,
                    FuenteDescripcion = "Entidades Presupuesto Nacional - PGN - AGENCIA DE RENOVACIÓN DEL TERRITORIO - Nación",

                    FuenteEtapaId = 2,

                    FfProgramacionValorSolicitado = 887526796412.00,
                    FfProgramacionVigencia = 2018,
                    FfProgramacionId =88802,



                    RegRecursosDepartamentoId = 3,
                    RegRecursosId = 1,

                    RegRecursosMunicipioId = null,

                    RegRecursosRegionId = 2
                },
                new RegionalizacionDto()
                {
                    ProjectId = 22465,
                    Bpin = "2017011000236",
                    ValorTotalSolicitadoProyecto = 2900629556000.00,
                    FuenteFinanciacionId = 16013                    ,

                    FuenteId = 25022,
                    FuenteDescripcion = "Entidades Presupuesto Nacional - PGN - AGENCIA DE RENOVACIÓN DEL TERRITORIO - Nación",

                    FuenteEtapaId = 2,

                    FfProgramacionValorSolicitado = 887526796412.00,
                    FfProgramacionVigencia = 2018,
                    FfProgramacionId =88802,



                    RegRecursosDepartamentoId = 6,
                    RegRecursosId = 2,

                    RegRecursosMunicipioId = null,

                    RegRecursosRegionId = 2
                },
                new RegionalizacionDto()
                {
                    ProjectId = 22465,
                    Bpin = "2017011000236",
                    ValorTotalSolicitadoProyecto = 2900629556000.00,
                    FuenteFinanciacionId = 16013                    ,
                    
                    
                    FuenteId = 25022,
                    FuenteDescripcion = "Entidades Presupuesto Nacional - PGN - AGENCIA DE RENOVACIÓN DEL TERRITORIO - Nación",
                    
                    FuenteEtapaId = 2,
                    
                    FfProgramacionValorSolicitado =1692502887812.00,
                    FfProgramacionVigencia = 2019,
                    FfProgramacionId =65952,


                    
                    RegRecursosDepartamentoId = 21,
                    RegRecursosId = 7,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 4
                },
                new RegionalizacionDto()
                {
                    ProjectId = 22465,
                    Bpin = "2017011000236",
                    ValorTotalSolicitadoProyecto = 2900629556000.00,
                    FuenteFinanciacionId = 16013                    ,
                    
                    FuenteId = 25022,
                    FuenteDescripcion = "Entidades Presupuesto Nacional - PGN - AGENCIA DE RENOVACIÓN DEL TERRITORIO - Nación",
                    
                    FuenteEtapaId = 2,
                    
                    FfProgramacionValorSolicitado =1692502887812.00,
                    FfProgramacionVigencia = 2019,
                    FfProgramacionId =65952,


                    
                    RegRecursosDepartamentoId = 23,
                    RegRecursosId = 8,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 5
                },
                new RegionalizacionDto()
                {
                    ProjectId = 22465,
                    Bpin = "2017011000236",
                    ValorTotalSolicitadoProyecto = 2900629556000.00,
                    FuenteFinanciacionId = 16013                    ,
                    
                    FuenteId = 25022,
                    FuenteDescripcion = "Entidades Presupuesto Nacional - PGN - AGENCIA DE RENOVACIÓN DEL TERRITORIO - Nación",
                    
                    FuenteEtapaId = 2,
                    
                    FfProgramacionValorSolicitado =320599871776.00,
                    FfProgramacionVigencia = 2020,
                    FfProgramacionId =65953,


                    
                    RegRecursosDepartamentoId = 2,
                    RegRecursosId = 11,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 5
                },

                };
            }
            else
            {
                return new List<RegionalizacionDto>();
            }
        }
        public List<RegionalizacionDto> ObtenerRegionalizacionPreview()
        {
            return new List<RegionalizacionDto>()
            {
                new RegionalizacionDto()
                {
                    ProjectId = 2288,
                    Bpin = "20170000100008",
                    ValorTotalSolicitadoProyecto = 6427480016,
                    FuenteFinanciacionId = 1027                    ,
                    
                    FuenteId = 1626,
                    FuenteDescripcion = "Empresas públicas - Colciencias - Propios",
                    
                    FuenteEtapaId = 2,
                    
                    FfProgramacionValorSolicitado = 2280000.00,
                    FfProgramacionVigencia = 2017,
                    FfProgramacionId =4343,


                    
                    RegRecursosDepartamentoId = 4,
                    RegRecursosId = 57,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 4
                },
                new RegionalizacionDto()
                {
                    ProjectId = 2288,
                    Bpin = "20170000100008",
                    ValorTotalSolicitadoProyecto = 6427480016,
                    FuenteFinanciacionId = 1027                    ,
                    
                    FuenteDescripcion = "Empresas públicas - Colciencias - Propios",
                    
                    FuenteEtapaId = 2,
                    FuenteId = 1626                    ,
                    
                    FfProgramacionValorSolicitado = 96422426.00,
                    FfProgramacionVigencia = 2018,
                    FfProgramacionId =4344,


                    
                    RegRecursosDepartamentoId = 4,
                    RegRecursosId = 58,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 4
                },
                new RegionalizacionDto()
                {
                    ProjectId = 2288,
                    Bpin = "20170000100008",
                    ValorTotalSolicitadoProyecto = 6427480016,
                    FuenteFinanciacionId = 1027                    ,
                    
                    FuenteDescripcion = "Departamentos - Huila - Fondo de ciencia, tecnología e innovación",
                    
                    FuenteEtapaId = 2,
                    FuenteId = 1625                    ,
                    
                    FfProgramacionValorSolicitado = 3500000.00,
                    FfProgramacionVigencia = 2017,
                    FfProgramacionId =4342,
                    
                    RegRecursosDepartamentoId = 4,
                    RegRecursosId = 66,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 6
                },
                new RegionalizacionDto()
                {
                    ProjectId = 2288,
                    Bpin = "20170000100008",
                    ValorTotalSolicitadoProyecto = 6427480016,
                    FuenteFinanciacionId = 1027                    ,
                    
                    FuenteDescripcion = "Departamentos - Huila - Fondo de ciencia, tecnología e innovación",
                    
                    FuenteEtapaId = 2,
                    FuenteId = 1625                    ,
                    
                    FfProgramacionValorSolicitado = 1668311170.00,
                    FfProgramacionVigencia = 2018,
                    FfProgramacionId =4348,
                    
                    RegRecursosDepartamentoId = 4,
                    RegRecursosId = 67,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 6
                },
                new RegionalizacionDto()
                {
                    ProjectId = 2288,
                    Bpin = "20170000100008",
                    ValorTotalSolicitadoProyecto = 6427480016,
                    FuenteFinanciacionId = 1027                    ,
                    
                    FuenteDescripcion = "Departamentos - Huila - Fondo de ciencia, tecnología e innovación",
                    
                    FuenteEtapaId = 2,
                    FuenteId = 1625                    ,
                    
                    FfProgramacionValorSolicitado = 2390656986.00,
                    FfProgramacionVigencia = 2019,
                    FfProgramacionId =4349,
                    
                    RegRecursosDepartamentoId = 4,
                    RegRecursosId = 68,

                    RegRecursosMunicipioId = null,
                    
                    RegRecursosRegionId = 6
                },

            };
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar,
                                           string usuario)
        {
            
        }
    }
}
