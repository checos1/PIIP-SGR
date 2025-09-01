namespace DNP.ServiciosWBS.Test.Mocks
{
    using System.Collections.Generic;
    using System.Linq;
    using Persistencia.Interfaces;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Dominio.Dto.Indicadores;

    public class AjustesPoliticasTransversalesMetasPersistenciaMock : IAjustesPoliticasTransversalesMetasPersistencia
    {
        public AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetas(string bpin)
        {
            var politicaTransversalCategoriaDto = new AjustesPoliticaTMetasDto();

            if (bpin.Equals("202000000000005"))
            {
                var auxPoliticas = new List<PoliticaAjustesPoliticaTMetasDto>();
                var auxProductos = new List<ProductosAjustesPoliticaTMetasDto>();

                auxProductos.Add(new ProductosAjustesPoliticaTMetasDto()
                {
                    ProductoId = 1259,
                    Producto = "Servicio de información penitenciaria y carcelaria para la toma de decisiones",
                    IndicadorId = 1358,
                    Indicador = "Boletines de información penitenciaria y carcelaria elaborados",
                    UnidaddeMedidaId = 177,
                    UnidaddeMedida = "Número de boletines",
                    MetaTotalProducto = 2,
                    Vigencias = new List<VigenciaAjustesPoliticaTMetasDto>()
                });

                auxPoliticas.Add(new PoliticaAjustesPoliticaTMetasDto()
                {
                    PoliticaId = 20,
                    Politica = "VÍCTIMAS",
                    Productos = auxProductos.OrderBy(p => p.ProductoId).ToList()
                });

                return new AjustesPoliticaTMetasDto()
                {
                    ProyectoId = 97652,
                    BPIN = "202100000000008",
                    POLITICAS = auxPoliticas.OrderBy(p => p.PoliticaId).ToList()
                };
            }
            else
            {
                return new AjustesPoliticaTMetasDto();
            }
        }

        public AjustesPoliticaTMetasDto ObtenerAjustesPoliticasTransversalesMetasPreview()
        {
            var auxPoliticas = new List<PoliticaAjustesPoliticaTMetasDto>();
            var auxProductos = new List<ProductosAjustesPoliticaTMetasDto>();

            auxProductos.Add(new ProductosAjustesPoliticaTMetasDto()
            {
                ProductoId = 1259,
                Producto = "Servicio de información penitenciaria y carcelaria para la toma de decisiones",
                IndicadorId = 1358,
                Indicador = "Boletines de información penitenciaria y carcelaria elaborados",
                UnidaddeMedidaId = 177,
                UnidaddeMedida = "Número de boletines",
                MetaTotalProducto = 2,
                Vigencias = new List<VigenciaAjustesPoliticaTMetasDto>()
            });

            auxPoliticas.Add(new PoliticaAjustesPoliticaTMetasDto()
            {
                PoliticaId = 20,
                Politica = "VÍCTIMAS",
                Productos = auxProductos.OrderBy(p => p.ProductoId).ToList()
            });

            return new AjustesPoliticaTMetasDto()
            {
                ProyectoId = 97652,
                BPIN = "202100000000008",
                POLITICAS = auxPoliticas.OrderBy(p => p.PoliticaId).ToList()
            };
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesPoliticaTMetasDto> parametrosGuardar, string usuario)
        {

        }
    }
}
