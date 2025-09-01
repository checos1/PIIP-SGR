using System.Collections.Generic;
using System.Linq;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;
using System;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System.Data.Entity.Core.Objects;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Comunes.Excepciones;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Formulario
{
    public class RegionalizacionProyectoPersistencia : Persistencia, IRegionalizacionProyectoPersistencia
    {
        public RegionalizacionProyectoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionProyectoDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));
            Contexto.uspPostInsertarRecursos(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, errorValidacionNegocio);

            if (errorValidacionNegocio.Value == null) return;

            var mensajeError = Convert.ToString(errorValidacionNegocio.Value);

            if (!string.IsNullOrEmpty(mensajeError))
                throw new ServiciosNegocioException(mensajeError);

        }
        public List<RegionalizacionDto> ObtenerRegionalizacion(string bpin)
        {
            if (string.IsNullOrEmpty(bpin)) return null;
            var consultaDesdeBd = Contexto.uspGetRecursos(bpin);
            return MapearARegionalizacionDto(consultaDesdeBd.ToList());
        }     

        private List<RegionalizacionDto> MapearARegionalizacionDto(IEnumerable<uspGetRecursos_Result> listadoDesdeBd)
        {
            List<RegionalizacionDto> listadoFinal = new List<RegionalizacionDto>();

            foreach (var itemDeBd in listadoDesdeBd)
            {
                RegionalizacionDto regDto = new RegionalizacionDto
                {
                    Bpin = itemDeBd.BPIN,
                    FfProgramacionId = itemDeBd.FFProgramacionId,
                    FfProgramacionValorSolicitado = itemDeBd.ValorTotalProyecto.HasValue ? Convert.ToDouble(itemDeBd.FFProgramacionValorSolicitado) : 0,
                    FfProgramacionVigencia = itemDeBd.Vigencia.HasValue ? Convert.ToInt16(itemDeBd.Vigencia) : 0,
                    FuenteDescripcion = itemDeBd.FuenteFinanciacion,

                    FuenteEtapaId = itemDeBd.FuenteFinanciacionEtapaId,
                    FuenteFinanciacionId = itemDeBd.FuenteFinanciacionId,

                    FuenteId = itemDeBd.Fuenteid,

                    ProjectId = itemDeBd.ProyectoId,

                    RegRecursosAgrupacionId = itemDeBd.RegRecursosAgrupacionId,

                    RegRecursosDepartamentoId = itemDeBd.RegRecursosDepartamentoId,
                    RegRecursosId = itemDeBd.RegRecursosId,

                    RegRecursosMunicipioId = itemDeBd.RegRecursosMunicipioId,

                    RegRecursosRegionId = itemDeBd.RegRecursosRegionId,
                    ValorTotalSolicitadoProyecto = itemDeBd.ValorTotalProyecto.HasValue ? Convert.ToDouble(itemDeBd.ValorTotalProyecto) : 0,
                    Mes = itemDeBd.Mes,
                    ValorInicial = itemDeBd.Valor_Inicial.HasValue ? Convert.ToDouble(itemDeBd.Valor_Inicial) : 0,
                    ValorSocilitado = itemDeBd.Valor_Socilitado.HasValue ? Convert.ToDouble(itemDeBd.Valor_Socilitado) : 0,
                    ValorVigente = itemDeBd.Valor_Vigente.HasValue ? Convert.ToDouble(itemDeBd.Valor_Vigente) : 0,
                    EjecucionCompromiso = itemDeBd.Ejecucion_Compromiso.HasValue ? Convert.ToDouble(itemDeBd.Ejecucion_Compromiso) : 0,
                    EjecucionObligacion = itemDeBd.Ejecucion_Obligacion.HasValue ? Convert.ToDouble(itemDeBd.Ejecucion_Obligacion) : 0,
                    EjecucionPago = itemDeBd.Ejecucion_Pago.HasValue ? Convert.ToDouble(itemDeBd.Ejecucion_Pago) : 0,
                    EjecucionValorInicial = itemDeBd.Ejecucion_Valor_Inicial.HasValue ? Convert.ToDouble(itemDeBd.Ejecucion_Valor_Inicial) : 0,
                    EjecucionValorVigente = itemDeBd.Ejecucion_Valor_Vigente.HasValue ? Convert.ToDouble(itemDeBd.Ejecucion_Valor_Vigente) : 0
                };
                listadoFinal.Add(regDto);
            }
            return listadoFinal;
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

                    RegRecursosAgrupacionId = null,

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

                    RegRecursosAgrupacionId = null,

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

                    RegRecursosAgrupacionId = null,

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

                    RegRecursosAgrupacionId = null,

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

                    RegRecursosAgrupacionId = null,

                    RegRecursosDepartamentoId = 4,
                    RegRecursosId = 68,

                    RegRecursosMunicipioId = null,

                    RegRecursosRegionId = 6
                },

            };
        }

    }
}
