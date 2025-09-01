
namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class RegionalizacionIndicadorAgregarPersistenciaMock : IRegionalizacionIndicadorAgregarPersistencia
    {

        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregar(string bpin)
        {
            var RegionalizacionIndicadorAgregarDto = new RegionalizacionIndicadorAgregarDto();

            if (bpin.Equals("2017761220016"))
            {
                var auxObjetivos = new List<ObjetivosRegionalizacionIndicadorDto>();
                var auxProductos = new List<ProductosRegionalizacionIndicadorDto>();
                var auxIndicadores = new List<IndicadoresRegionalizacionIndicadorDto>();
                var auxVigencias = new List<VigenciasRegionalizacionIndicadorDto>();
                var auxRegionalizacion = new List<RegionalizacionRegionalizacionIndicadorDto>();


                auxRegionalizacion.Add(new RegionalizacionRegionalizacionIndicadorDto()
                {
                    RegionId = 5,
                    NombreRegion = "Orinoquía",
                    CodigoRegion = "05",
                    DepartamentoId = 23,
                    NombreDepartamento = "Meta",
                    CodigoDepto = "0550",
                    MunicipioId = 302,
                    NombreMunicipio = "Segovia",
                    CodigoMunicipio = "05736",
                    AgrupacionId = null,
                    NombreAgrupacion = null,
                    CodigoAgrupacion = null,
                    MetaRegionalizada = null
                });

                auxVigencias.Add( new VigenciasRegionalizacionIndicadorDto()
                {
                    Vigencia = 2017,
                    MetaVigencia = 450,
                    Regionalizacion = auxRegionalizacion.OrderBy(r => r.NombreRegion).ToList()
                });

                auxVigencias.Add(new VigenciasRegionalizacionIndicadorDto()
                {
                    Vigencia = 2018,
                    MetaVigencia = 800,
                    Regionalizacion = auxRegionalizacion.OrderBy(r => r.NombreRegion).ToList()
                });


                auxIndicadores.Add(new IndicadoresRegionalizacionIndicadorDto()
                {
                    IndicadorId = 46,
                    NombreIndicador = "Plan de Intervenciones colectivas ejecutado.",
                    CodigoIndicador = "190106400",
                    IndicadorTipo = "P",
                    IndicadorAcumula = true,
                    IndicadorUnidadMedidaId = 13,
                    NombreUnidadMedida = "Número",
                    MetaTotal = 100,
                    Vigencias = auxVigencias.OrderBy(v => v.Vigencia).ToList()
                });

                auxProductos.Add(new ProductosRegionalizacionIndicadorDto()
                {
                    ProductoId = 137,
                    NombreProducto = "Vida saludable y condiciones no transmisibles",
                    CodigoProducto = null,
                    ProductoUnidadMedidaId = 13,
                    ProductoUnidadMedida = "Número",
                    Cantidad = 2,
                    Indicadores = auxIndicadores.OrderBy(ip => ip.NombreIndicador).ToList()
                });


                auxObjetivos.Add(new ObjetivosRegionalizacionIndicadorDto()
                {
                    ObjetivoId = 226,
                    ObjetivoEspecifico = "Propiciar la cultura del autocuidado y disminuir las barreras de acceso en la prestación de servicios de salud",
                    Productos = auxProductos.OrderBy(ip => ip.NombreProducto).ToList()
                });


                return new RegionalizacionIndicadorAgregarDto()
                {
                    Bpin = "2017761220016",
                    Objetivos = auxObjetivos.OrderBy(v => v.ObjetivoEspecifico).ToList()
                };
            }
            else
            {
                return new RegionalizacionIndicadorAgregarDto();
            }
        }



        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregarPreview()
        {
            var auxObjetivos = new List<ObjetivosRegionalizacionIndicadorDto>();
            var auxProductos = new List<ProductosRegionalizacionIndicadorDto>();
            var auxIndicadores = new List<IndicadoresRegionalizacionIndicadorDto>();
            var auxVigencias = new List<VigenciasRegionalizacionIndicadorDto>();
            var auxRegionalizacion = new List<RegionalizacionRegionalizacionIndicadorDto>();

            auxRegionalizacion.Add(new RegionalizacionRegionalizacionIndicadorDto()
            {
                RegionId = 5,
                NombreRegion = "Orinoquía",
                CodigoRegion = "05",
                DepartamentoId = 23,
                NombreDepartamento = "Meta",
                CodigoDepto = "0550",
                MunicipioId = 302,
                NombreMunicipio = "Segovia",
                CodigoMunicipio = "05736",
                AgrupacionId = null,
                NombreAgrupacion = null,
                CodigoAgrupacion = null,
                MetaRegionalizada = null
            });

            auxVigencias.Add(new VigenciasRegionalizacionIndicadorDto()
            {
                Vigencia = 2017,
                MetaVigencia = 450,
                Regionalizacion = auxRegionalizacion.OrderBy(r => r.NombreRegion).ToList()
            });

            auxVigencias.Add(new VigenciasRegionalizacionIndicadorDto()
            {
                Vigencia = 2018,
                MetaVigencia = 800,
                Regionalizacion = auxRegionalizacion.OrderBy(r => r.NombreRegion).ToList()
            });


            auxIndicadores.Add(new IndicadoresRegionalizacionIndicadorDto()
            {
                IndicadorId = 46,
                NombreIndicador = "Plan de Intervenciones colectivas ejecutado.",
                CodigoIndicador = "190106400",
                IndicadorTipo = "P",
                IndicadorAcumula = true,
                IndicadorUnidadMedidaId = 13,
                NombreUnidadMedida = "Número",
                MetaTotal = 100,
                Vigencias = auxVigencias.OrderBy(v => v.Vigencia).ToList()
            });

            auxProductos.Add(new ProductosRegionalizacionIndicadorDto()
            {
                ProductoId = 137,
                NombreProducto = "Vida saludable y condiciones no transmisibles",
                CodigoProducto = null,
                ProductoUnidadMedidaId = 13,
                ProductoUnidadMedida = "Número",
                Cantidad = 2,
                Indicadores = auxIndicadores.OrderBy(ip => ip.NombreIndicador).ToList()
            });


            auxObjetivos.Add(new ObjetivosRegionalizacionIndicadorDto()
            {
                ObjetivoId = 226,
                ObjetivoEspecifico = "Propiciar la cultura del autocuidado y disminuir las barreras de acceso en la prestación de servicios de salud",
                Productos = auxProductos.OrderBy(ip => ip.NombreProducto).ToList()
            });
                

            return new RegionalizacionIndicadorAgregarDto()
            {
                Bpin = "2017761220016",
                Objetivos = auxObjetivos.OrderBy(v => v.ObjetivoEspecifico).ToList()
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> parametrosGuardar, string usuario)
        {
        }


    }
}
