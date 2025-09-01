
namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Indicadores;

    public class RegionalizacionIndicadoresPersistenciaMock : IRegionalizacionIndicadoresPersistencia
    {

        public RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadores(string bpin)
        {
            var regionalizacionIndicadorDto = new RegionalizacionIndicadorDto();

            //return regionalizacionIndicadorDto;

            if (bpin.Equals("2017761220016"))
            {              
                var auxObjetivos = new List<VigenciaObjetivoProductoDto>();
                var auxIndicadores = new List<IndicadorDto>();
                var auxRegionalizacionIndicadores = new List<IndicadorRegionalizacionDto>();


                auxRegionalizacionIndicadores.Add(new IndicadorRegionalizacionDto()
                {
                    IndicadorId = 1,
                    RegionalizacionMetasId = 2,
                    RegionId = 3,
                    RegionNombre = "Occidente",
                    RegionCodigo = "03",
                    DepartamentoId = 16,
                    DepartamentoNombre = "Valle del Cauca",
                    DepartamentoCodigo = "0376",
                    MunicipioId = 521,
                    MunicipioNombre = "Caicedonia",
                    MunicipioCodigo = "76122",
                    AgrupacionId = null,
                    AgrupacionNombre = null,
                    AgrupacionCodigo = null,
                    MetaVigente = 10
                });
                auxIndicadores.Add(new IndicadorDto()
                {
                    IndicadorId = 1,
                    ProductoId = 19,
                    IndicadorNombre = "Procesos para el mejoramiento de la calidad de la educación para el trabajo y el desarrollo humano adelantados",
                    IndicadorCodigo = "220201100",
                    IndicadorTipo = "P",
                    IndicadorAcumula = false,
                    IndicadorUnidadMedidaId = 13,
                    IndicadorNombreUnidadMedida = "Número",
                    IndicadorMetaTotal = 3,
                    IndicadorMetaVigente = 10,
                    Regionalizacion = auxRegionalizacionIndicadores.OrderBy(ri => ri.RegionNombre).ToList()
                });

                auxObjetivos.Add(new VigenciaObjetivoProductoDto()
                {
                    Vigencia = 2018,
                    ObjetivoEspecificoId = 26,
                    ObjetivoEspecifico = "Realizar Jornadas de formación en competencias a Docentes y estudiantes, de las 6 Instituciones Educativas oficiales del Municipio",
                    ProductoId = 19,
                    Producto = "Servicio de mejoramiento de la calidad de la educación para el trabajo y el desarrollo humano",
                    Meta = 3,
                    UnidadMedidaId = 259,
                    NombreUnidadMedida = "Número de procesos",
                    Indicadores = auxIndicadores.OrderBy(ip => ip.IndicadorId).ToList()
                });

                auxObjetivos.Add(new VigenciaObjetivoProductoDto()
                {
                    Vigencia = 2019,
                    ObjetivoEspecificoId = 26,
                    ObjetivoEspecifico = "Realizar Jornadas de formación en competencias a Docentes y estudiantes, de las 6 Instituciones Educativas oficiales del Municipio",
                    ProductoId = 19,
                    Producto = "Servicio de mejoramiento de la calidad de la educación para el trabajo y el desarrollo humano",
                    Meta = 3,
                    UnidadMedidaId = 259,
                    NombreUnidadMedida = "Número de procesos",
                    Indicadores = auxIndicadores.OrderBy(ip => ip.IndicadorId).ToList()
                });

                return new RegionalizacionIndicadorDto()
                {
                    Bpin = "2017761220016",
                    Vigencias = auxObjetivos.OrderBy(v => v.Vigencia).ToList()
                };
            }
            else
            {
                return new RegionalizacionIndicadorDto();
            }

        }

        public RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadoresPreview()
        {
            var regionalizacionIndicadorDto = new RegionalizacionIndicadorDto();        
            var auxObjetivos = new List<VigenciaObjetivoProductoDto>();
            var auxIndicadores = new List<IndicadorDto>();
            var auxRegionalizacionIndicadores = new List<IndicadorRegionalizacionDto>();


            auxRegionalizacionIndicadores.Add(new IndicadorRegionalizacionDto()
            {
                IndicadorId = 1,
                RegionalizacionMetasId = 2,
                RegionId = 3,
                RegionNombre = "Oriente",
                RegionCodigo = "04",
                DepartamentoId = 16,
                DepartamentoNombre = "Meta",
                DepartamentoCodigo = "0376",
                MunicipioId = 521,
                MunicipioNombre = "Acacias",
                MunicipioCodigo = "76122",
                AgrupacionId = null,
                AgrupacionNombre = null,
                AgrupacionCodigo = null,
                MetaVigente = 10
            });
            auxRegionalizacionIndicadores.Add(new IndicadorRegionalizacionDto()
            {
                IndicadorId = 1,
                RegionalizacionMetasId = 2,
                RegionId = 3,
                RegionNombre = "Occidente",
                RegionCodigo = "03",
                DepartamentoId = 16,
                DepartamentoNombre = "Valle del Cauca",
                DepartamentoCodigo = "0376",
                MunicipioId = 521,
                MunicipioNombre = "Caicedonia",
                MunicipioCodigo = "76122",
                AgrupacionId = null,
                AgrupacionNombre = null,
                AgrupacionCodigo = null,
                MetaVigente = 10
            });
            auxIndicadores.Add(new IndicadorDto()
            {
                IndicadorId = 1,
                ProductoId = 19,
                IndicadorNombre = "Procesos para el mejoramiento de la calidad de la educación para el trabajo y el desarrollo humano adelantados",
                IndicadorCodigo = "220201100",
                IndicadorTipo = "P",
                IndicadorAcumula = false,
                IndicadorUnidadMedidaId = 13,
                IndicadorNombreUnidadMedida = "Número",
                IndicadorMetaTotal = 3,
                IndicadorMetaVigente = 10,
                Regionalizacion = auxRegionalizacionIndicadores.OrderBy(ri => ri.RegionNombre).ToList()
            });
            auxIndicadores.Add(new IndicadorDto()
            {
                IndicadorId = 2,
                ProductoId = 19,
                IndicadorNombre = "Procesos para el mejoramiento de la calidad de la educación para el trabajo y el desarrollo humano adelantados",
                IndicadorCodigo = "220201100",
                IndicadorTipo = "S",
                IndicadorAcumula = false,
                IndicadorUnidadMedidaId = 13,
                IndicadorNombreUnidadMedida = "Número",
                IndicadorMetaTotal = 3,
                IndicadorMetaVigente = 10,
                Regionalizacion = auxRegionalizacionIndicadores.OrderBy(ri => ri.RegionNombre).ToList()
            });

            auxObjetivos.Add(new VigenciaObjetivoProductoDto()
            {
                Vigencia = 2018,
                ObjetivoEspecificoId = 26,
                ObjetivoEspecifico = "Realizar Jornadas de formación en competencias a Docentes y estudiantes, de las 6 Instituciones Educativas oficiales del Municipio",
                ProductoId = 19,
                Producto = "Servicio de mejoramiento de la calidad de la educación para el trabajo y el desarrollo humano",
                Meta = 3,
                UnidadMedidaId = 259,
                NombreUnidadMedida = "Número de procesos",
                Indicadores = auxIndicadores.OrderBy(ip => ip.IndicadorId).ToList()
            });

            auxObjetivos.Add(new VigenciaObjetivoProductoDto()
            {
                Vigencia = 2019,
                ObjetivoEspecificoId = 26,
                ObjetivoEspecifico = "Realizar Jornadas de formación en competencias a Docentes y estudiantes, de las 6 Instituciones Educativas oficiales del Municipio",
                ProductoId = 19,
                Producto = "Servicio de mejoramiento de la calidad de la educación para el trabajo y el desarrollo humano",
                Meta = 3,
                UnidadMedidaId = 259,
                NombreUnidadMedida = "Número de procesos",
                Indicadores = auxIndicadores.OrderBy(ip => ip.IndicadorId).ToList()
            });

            return new RegionalizacionIndicadorDto()
            {
                Bpin = "2017761220016",
                Vigencias = auxObjetivos.OrderBy(v => v.Vigencia).ToList()
            };   

        }


        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionIndicadorDto> parametrosGuardar, string usuario)
        {

        }


    }


}