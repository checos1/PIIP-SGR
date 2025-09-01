namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    public class IndicadorProductoAgregarPersistenciaMock: IIndicadorProductoAgregarPersistencia
    {

        public IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregar(string bpin)
        {
            var IndicadorProductoAgregarDto = new IndicadorProductoAgregarDto();

            if (bpin.Equals("2017761220016"))
            {
                var auxObjetivos = new List<ObjetivosIndicadorProductoAgregarDto>();
                var auxProductos = new List<ProductosIndicadorProductoAgregarDto>();
                var auxIndicadores = new List<IndicadoresIndicadorProductoAgregarDto>();

                auxIndicadores.Add(new IndicadoresIndicadorProductoAgregarDto()
                {
                  IndicadorId =  46,
                  NombreIndicador = "Plan de Intervenciones colectivas ejecutado.",
                  CodigoIndicador = "190106400",
                  IndicadorTipo = "P",
                  IndicadorAcumula = true,
                  IndicadorUnidadMedidaId = 13,
                  NombreUnidadMedida = "Número",
                  MetaTotal = 100
                });

                auxProductos.Add(new ProductosIndicadorProductoAgregarDto()
                {
                  ProductoId = 137,
                  NombreProducto = "Vida saludable y condiciones no transmisibles",
                  CodigoProducto = null,
                  ProductoUnidadMedidaId = 13,
                  ProductoUnidadMedida = "Número",
                  Cantidad = 2,
                  Indicadores = auxIndicadores.OrderBy(ip => ip.NombreIndicador).ToList()
                });


                auxObjetivos.Add(new ObjetivosIndicadorProductoAgregarDto()
                {
                  ObjetivoId = 226,
                  ObjetivoEspecifico = "Propiciar la cultura del autocuidado y disminuir las barreras de acceso en la prestación de servicios de salud",
                  Productos = auxProductos.OrderBy(ip => ip.NombreProducto).ToList()                    
                });


                return new IndicadorProductoAgregarDto()
                {
                    Bpin = "2017761220016",                    
                    Objetivos = auxObjetivos.OrderBy(v => v.ObjetivoEspecifico).ToList()
                };
            }
            else
            {
                return new IndicadorProductoAgregarDto();
            }

        }



        public IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregarPreview()
        {
            var IndicadorProductoAgregarDto = new IndicadorProductoAgregarDto();
            var auxObjetivos = new List<ObjetivosIndicadorProductoAgregarDto>();
            var auxProductos = new List<ProductosIndicadorProductoAgregarDto>();
            var auxIndicadores = new List<IndicadoresIndicadorProductoAgregarDto>();

            auxIndicadores.Add(new IndicadoresIndicadorProductoAgregarDto()
            {
                IndicadorId = 46,
                NombreIndicador = "Plan de Intervenciones colectivas ejecutado.",
                CodigoIndicador = "190106400",
                IndicadorTipo = "P",
                IndicadorAcumula = true,
                IndicadorUnidadMedidaId = 13,
                NombreUnidadMedida = "Número",
                MetaTotal = 100
            });

            auxProductos.Add(new ProductosIndicadorProductoAgregarDto()
            {
                ProductoId = 137,
                NombreProducto = "Vida saludable y condiciones no transmisibles",
                CodigoProducto = null,
                ProductoUnidadMedidaId = 13,
                ProductoUnidadMedida = "Número",
                Cantidad = 2,
                Indicadores = auxIndicadores.OrderBy(ip => ip.NombreIndicador).ToList()
            });


            auxObjetivos.Add(new ObjetivosIndicadorProductoAgregarDto()
            {
                ObjetivoId = 226,
                ObjetivoEspecifico = "Propiciar la cultura del autocuidado y disminuir las barreras de acceso en la prestación de servicios de salud",
                Productos = auxProductos.OrderBy(ip => ip.NombreProducto).ToList()
            });
            
            return new IndicadorProductoAgregarDto()
            {
                Bpin = "2017761220016",
                Objetivos = auxObjetivos.OrderBy(v => v.ObjetivoEspecifico).ToList()
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<IndicadorProductoAgregarDto> parametrosGuardar, string usuario)
        {
        }

    }
}
