namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Productos;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Modelo;

    public class ProductoPersistencia : Persistencia, IProductoPersistencia
    {
        public ProductoPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<ProyectoDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostProductosPorProyecto(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, errorValidacionNegocio);

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

        public ProyectoDto ObtenerProductoDefinitivo(ParametrosConsultaDto parametrosConsultaDto)
        {
            try
            {
                var proyectoDto = new ProyectoDto();
                IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList = Contexto.uspGetProductosRegionalizacionPorProyecto(parametrosConsultaDto.Bpin).ToList();

                proyectoDto.Bpin = productoList.FirstOrDefault()?.ProyectoBPIN;
                proyectoDto.Id = productoList.FirstOrDefault()?.ProyectoId.ToString();

                CrearVigencias(proyectoDto, productoList);
                CrearProblemaCentral(proyectoDto, productoList);
                CrearCausa(proyectoDto, productoList);
                CrearObjetivosEspecifico(proyectoDto, productoList);
                CrearProductos(proyectoDto, productoList);
                CrearProductoIndicador(proyectoDto, productoList);
                CrearMeta(proyectoDto, productoList);
                CrearRegionalizacion(proyectoDto, productoList);

                return proyectoDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);

            }

        }

        public ProyectoDto ObtenerProductosPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<ProyectoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaProducto);
        }

        private static void CrearVigencias(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            proyectoDto.Vigencia = new List<VigenciaDto>();

            foreach (var productoItem in productoList.Where(productoItem => proyectoDto.Vigencia.Find(x => x.Vigencia == productoItem.Vigencia) == null))
                proyectoDto.Vigencia.Add(new VigenciaDto()
                {
                    Vigencia = productoItem.Vigencia,
                    ProblemaCentral = new List<ProblemaCentralDto>()
                });
        }

        private static void CrearProblemaCentral(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            var uspGetProductosPorProyectoResults = productoList as uspGetProductosRegionalizacionPorProyecto_Result[] ?? productoList.ToArray();

            foreach (var vigenciaItem in proyectoDto.Vigencia)
            {
                var problemaCentralDto = (from productoItem in uspGetProductosPorProyectoResults where productoItem.Vigencia == vigenciaItem.Vigencia select new ProblemaCentralDto() { ProblemaCentral = productoItem.ProblemaCentral, ProblemaCentralId = productoItem.ProblemaCentralId, ProyectoId = productoItem.ProyectoId, Causa = new List<CausaDto>() }).ToList();

                vigenciaItem.ProblemaCentral.AddRange(problemaCentralDto.GroupBy(x => x.ProblemaCentralId).Select(y => y.First()));

            }
        }


        private static void CrearCausa(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            var uspGetProductosPorProyectoResults = productoList as uspGetProductosRegionalizacionPorProyecto_Result[] ?? productoList.ToArray();

            foreach (var vigenciaItem in proyectoDto.Vigencia)
            {

                foreach (var problemaCentralItem in vigenciaItem.ProblemaCentral)
                {
                    var causaDto = (from productoItem in uspGetProductosPorProyectoResults where productoItem.Vigencia == vigenciaItem.Vigencia select new CausaDto() { CausaId = productoItem.CausaId, Causa = productoItem.Causa, ObjectivoEspecifico = new List<ObjetivoEspecificoDto>(), ProblemaCentralId = productoItem.ProblemaCentralId }).ToList();

                    problemaCentralItem.Causa.AddRange(causaDto.GroupBy(x => x.CausaId).Select(y => y.First()));

                }
            }
        }

        private static void CrearObjetivosEspecifico(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            var uspGetProductosPorProyectoResults = productoList as uspGetProductosRegionalizacionPorProyecto_Result[] ?? productoList.ToArray();

            foreach (var vigenciaItem in proyectoDto.Vigencia)
            {

                foreach (var problemaCentralItem in vigenciaItem.ProblemaCentral)
                {


                    foreach (var causaItem in problemaCentralItem.Causa)
                    {
                        var objectivoEspecificoDto = (from productoItem in uspGetProductosPorProyectoResults where productoItem.CausaId == causaItem.CausaId select new ObjetivoEspecificoDto() { ObjetivoEspecifico = productoItem.ObjetivoEspecifico, CausaId = productoItem.CausaId, ObjetivoEspecificoId = productoItem.ObjetivoEspecificoId, Producto = new List<ProductoDto>() }).ToList();

                        causaItem.ObjectivoEspecifico.AddRange(objectivoEspecificoDto.GroupBy(x => x.ObjetivoEspecificoId).Select(y => y.First()));

                    }
                }
            }
        }




        private static void CrearProductoIndicador(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            var uspGetProductosPorProyectoResults = productoList as uspGetProductosRegionalizacionPorProyecto_Result[] ?? productoList.ToArray();

            foreach (var vigenciaItem in proyectoDto.Vigencia)
            {

                foreach (var problemaCentralItem in vigenciaItem.ProblemaCentral)
                {


                    foreach (var causaItem in problemaCentralItem.Causa)
                    {

                        foreach (var objectivoEspecificoItem in causaItem.ObjectivoEspecifico)
                        {
                            foreach (var productoIndicadorItem in objectivoEspecificoItem.Producto)
                            {
                                var productoIndicadorDetalle = (from productoItem in uspGetProductosPorProyectoResults where productoItem.ProductoId == productoIndicadorItem.ProductoId select new ProductoIndicadorDetalleDto() { IndicadorProyectoBase = productoItem.IndicadorProyectoBase, IndicadorMetaTotal = productoItem.IndicadorMetaTotal, IndicadorId = productoItem.IndicadorId, IndicadorDelPrograma = productoItem.IndicadorDelPrograma, IndicadorUnidadMedidaId = productoItem.IndicadorUnidadMedidaId, IndicadorProducto = productoItem.IndicadorProducto, IndicadorFuenteVerificacionId = productoItem.IndicadorFuenteVerificacionId, IndicadorAsociado = productoItem.IndicadorAsociado, IndicadorAcumula = productoItem.IndicadorAcumula, IndicadorProgramaId = productoItem.IndicadorProgramaId, IndicadorBisId = productoItem.IndicadorBISId, IndicadorTipo = productoItem.IndicadorTipo, IndicadorTipoOrigenId = productoItem.IndicadorTipoOrigenId, Metas = new List<MetaDto>() }).ToList();

                                productoIndicadorItem.ProductoIndicadorDetalle.AddRange(productoIndicadorDetalle.GroupBy(x => x.IndicadorId).Select(y => y.First()));

                            }
                        }
                    }

                }
            }
        }
        private static void CrearProductos(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            var uspGetProductosPorProyectoResults = productoList as uspGetProductosRegionalizacionPorProyecto_Result[] ?? productoList.ToArray();

            foreach (var vigenciaItem in proyectoDto.Vigencia)
            {

                foreach (var problemaCentralItem in vigenciaItem.ProblemaCentral)
                {


                    foreach (var causaItem in problemaCentralItem.Causa)
                    {

                        foreach (var objectivoEspecificoItem in causaItem.ObjectivoEspecifico)
                        {
                            var productoDto = (from productoItem in uspGetProductosPorProyectoResults where productoItem.ObjetivoEspecificoId == objectivoEspecificoItem.ObjetivoEspecificoId select new ProductoDto() { Producto = productoItem.Producto, ProductoId = productoItem.ProductoId, ProductoTipoMedidaId = productoItem.ProductoTipoMedidaId, ProductoAlternativaId = productoItem.ProductoAlternativaId, ProductoCantidad = productoItem.ProductoCantidad, ProductoCatalogoId = productoItem.ProductoCatalogoId, ProductoComplemento = productoItem.ProductoComplemento, ObjetivoEspecificoId = productoItem.ObjetivoEspecificoId, ProductoIndicadorDetalle = new List<ProductoIndicadorDetalleDto>() }).ToList();

                            objectivoEspecificoItem.Producto.AddRange(productoDto.GroupBy(x => x.ProductoId).Select(y => y.First()));



                        }


                    }
                }
            }
        }
        private static void CrearMeta(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            var uspGetProductosPorProyectoResults = productoList as uspGetProductosRegionalizacionPorProyecto_Result[] ?? productoList.ToArray();

            foreach (var vigenciaItem in proyectoDto.Vigencia)
            {

                foreach (var problemaCentralItem in vigenciaItem.ProblemaCentral)
                {


                    foreach (var causaItem in problemaCentralItem.Causa)
                    {

                        foreach (var objectivoEspecificoItem in causaItem.ObjectivoEspecifico)
                        {
                            foreach (var productoIndicadorItem in objectivoEspecificoItem.Producto)
                            {

                                foreach (var productoIndicadorDetalleItem in productoIndicadorItem.ProductoIndicadorDetalle)
                                {
                                    var metaDto = (from productoItem in uspGetProductosPorProyectoResults where productoItem.IndicadorId == productoIndicadorDetalleItem.IndicadorId && vigenciaItem.Vigencia == productoItem.Vigencia select new MetaDto() { Meta = productoItem.Meta, MetaId = productoItem.MetaId, IndicadorId = productoItem.IndicadorId, RegionalizacionMetas = new List<RegionalizacionMetasDto>() }).ToList();

                                    productoIndicadorDetalleItem.Metas.AddRange(metaDto.GroupBy(x => x.MetaId).Select(y => y.First()));

                                }

                            }
                        }
                    }


                }


            }
        }
        private static void CrearRegionalizacion(ProyectoDto proyectoDto, IEnumerable<uspGetProductosRegionalizacionPorProyecto_Result> productoList)
        {
            var uspGetProductosPorProyectoResults = productoList as uspGetProductosRegionalizacionPorProyecto_Result[] ?? productoList.ToArray();

            foreach (var vigenciaItem in proyectoDto.Vigencia)
            {
                foreach (var problemaCentralItem in vigenciaItem.ProblemaCentral)
                {
                    foreach (var causaItem in problemaCentralItem.Causa)
                    {
                        foreach (var objectivoEspecificoItem in causaItem.ObjectivoEspecifico)
                        {
                            foreach (var productoIndicadorItem in objectivoEspecificoItem.Producto)
                            {
                                foreach (var productoIndicadorDetalleItem in productoIndicadorItem.ProductoIndicadorDetalle)
                                {
                                    foreach (var metasItem in productoIndicadorDetalleItem.Metas)
                                    {
                                        var regionalizacionMetasDto = (from productoItem in uspGetProductosPorProyectoResults where productoItem.MetaId == metasItem.MetaId select new RegionalizacionMetasDto() { MetaId = productoItem.MetaId, RegionalizacionMetaAgrupacionId = productoItem.RegionalizacionMetaAgrupacionId, Agrupacion = productoItem.Agrupacion, RegionalizacionMetaDepartamentoId = productoItem.RegionalizacionMetaDepartamentoId, Departamento = productoItem.Departamento, RegionalizacionMetaId = productoItem.RegionalizacionMetaId, RegionalizacionMetaMunicipioId = productoItem.RegionalizacionMetaMunicipioId, Municipio = productoItem.Municipio,  RegionalizacionMetaRegionId = productoItem.RegionalizacionMetaRegionId, Region = productoItem.Region, RegionalizacionMetaTotalRegionalizado = productoItem.RegionalizacionMetaTotalRegionalizado, RegionalizacionMetaVigente = productoItem.RegionalizacionMetaVigente, RegionalizacionMetaRezagada = productoItem.RegionalizacionMetaRezagada, RegionalizacionMetaTotalRezagada = productoItem.RegionalizacionMetaTotalRezagada, RegionalizacionMetaAvance = productoItem.RegionalizacionMetaAvance, RegionalizacionMetaTotalAvance = productoItem.RegionalizacionMetaTotalAvance, RegionalizacionMetaAvanceRezago = productoItem.RegionalizacionMetaAvanceRezago, RegionalizacionMetaTotalAvanceRezago = productoItem.RegionalizacionMetaTotalAvanceRezago, RegionalizacionMetaAvanceAcumulado = productoItem.RegionalizacionMetaAvanceAcumulado, RegionalizacionMetaTotalAvanceAcumulado = productoItem.RegionalizacionMetaTotalAvanceAcumulado}).ToList();
                                        metasItem.RegionalizacionMetas.AddRange(regionalizacionMetasDto.GroupBy(x => x.RegionalizacionMetaId).Select(y => y.First()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
