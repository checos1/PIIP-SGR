namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Modelo;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;


    public class RegionalizacionIndicadorAgregarPersistencia: Persistencia, IRegionalizacionIndicadorAgregarPersistencia
    {

        public RegionalizacionIndicadorAgregarPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }


        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionIndicadorAgregarDto> parametrosGuardar,
                                                   string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostRegionalizacionIndicadorAgregar(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);
                    
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


        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregar(string bpin)
        {
            try
            {
                var regionalizacionIndicadorAgregarDto = new RegionalizacionIndicadorAgregarDto();
                IEnumerable<uspGetRegionalizacionIndicadorAgregar_Result> regionalizacionIndicadorAgregarList = Contexto.uspGetRegionalizacionIndicadorAgregar(bpin).ToList();

                regionalizacionIndicadorAgregarDto.Bpin = regionalizacionIndicadorAgregarList.FirstOrDefault()?.BPIN;
                regionalizacionIndicadorAgregarDto = MapearRegionalizacionIndicadorAgregarDto(regionalizacionIndicadorAgregarList.ToList());

                return regionalizacionIndicadorAgregarDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }



        private RegionalizacionIndicadorAgregarDto MapearRegionalizacionIndicadorAgregarDto(List<uspGetRegionalizacionIndicadorAgregar_Result> listadoDesdeBd)
        {
            var regionalizacionIndicadorAgregar = new RegionalizacionIndicadorAgregarDto();
            regionalizacionIndicadorAgregar.Objetivos = new List<ObjetivosRegionalizacionIndicadorDto>();

            listadoDesdeBd.GroupBy(o =>
            new
            {
                o.BPIN,
                o.ObjetivoId,
                o.ObjetivoEspecifico
            }).ToList().ForEach(
                obj =>
                {
                    var auxProductosIndicadorProductoAgregar = new List<ProductosRegionalizacionIndicadorDto>();
                    listadoDesdeBd.Where(v => v.ObjetivoId == obj.Key.ObjetivoId).GroupBy(obp =>
                    new
                    {
                        obp.ProductoId,
                        obp.NombreProducto,
                        obp.CodigoProducto,
                        obp.ProductoUnidadMedidaId,
                        obp.ProductoUnidadMedida,
                        obp.Cantidad
                    }).ToList().ForEach(
                    p =>
                    {
                        var auxIndicadoresIndicadorProductoAgregar = new List<IndicadoresRegionalizacionIndicadorDto>();
                        listadoDesdeBd.Where(pr => pr.ObjetivoId == obj.Key.ObjetivoId && pr.ProductoId == p.Key.ProductoId).GroupBy(ind =>
                       new
                       {
                           ind.IndicadorId,
                           ind.NombreIndicador,
                           ind.CodigoIndicador,
                           ind.IndicadorTipo,
                           ind.IndicadorAcumula,
                           ind.IndicadorUnidadMedidaId,
                           ind.NombreUnidadMedida,
                           ind.MetaTotal
                       }).ToList().ForEach(
                       i =>
                       {
                           var auxVigenciasRegionalizacionIndicadorAgregar = new List<VigenciasRegionalizacionIndicadorDto>();
                           listadoDesdeBd.Where(indi => indi.ObjetivoId == obj.Key.ObjetivoId && indi.ProductoId == p.Key.ProductoId && indi.IndicadorId == i.Key.IndicadorId).GroupBy(vig =>
                           new
                           {
                               vig.Vigencia,
                               vig.MetaVigencia
                           }).ToList().ForEach(
                           vi =>
                           {
                               var auxRegionalizacionRegionalizacionIndicadorAgregar = new List<RegionalizacionRegionalizacionIndicadorDto>();
                               listadoDesdeBd.Where(vn => vn.ObjetivoId == obj.Key.ObjetivoId && vn.ProductoId == p.Key.ProductoId && vn.IndicadorId == i.Key.IndicadorId && vn.Vigencia == vi.Key.Vigencia).GroupBy(reg =>
                               new
                               {
                                   reg.RegionId,
                                   reg.NombreRegion,
                                   reg.CodigoRegion,
                                   reg.DepartamentoId,
                                   reg.NombreDepartamento,
                                   reg.CodigoDepto,
                                   reg.MunicipioId,
                                   reg.NombreMunicipio,
                                   reg.CodigoMunicipio,
                                   reg.AgrupacionId,
                                   reg.NombreAgrupacion,
                                   reg.CodigoAgrupacion,
                                   reg.MetaRegionalizada
                               }).ToList().ForEach(
                               re =>
                               {
                                   auxRegionalizacionRegionalizacionIndicadorAgregar.Add(new RegionalizacionRegionalizacionIndicadorDto()
                                   {
                                       RegionId = re.Key.RegionId,
                                       NombreRegion = re.Key.NombreRegion,
                                       CodigoRegion = re.Key.CodigoRegion,
                                       DepartamentoId = re.Key.DepartamentoId,
                                       NombreDepartamento = re.Key.NombreDepartamento,
                                       CodigoDepto = re.Key.CodigoDepto,
                                       MunicipioId = re.Key.MunicipioId,
                                       NombreMunicipio = re.Key.NombreMunicipio,
                                       CodigoMunicipio = re.Key.CodigoMunicipio,
                                       AgrupacionId = re.Key.AgrupacionId,
                                       NombreAgrupacion = re.Key.NombreAgrupacion,
                                       CodigoAgrupacion = re.Key.CodigoAgrupacion,
                                       MetaRegionalizada = re.Key.MetaRegionalizada
                                   });
                               });

                               auxVigenciasRegionalizacionIndicadorAgregar.Add(new VigenciasRegionalizacionIndicadorDto()
                               {
                                   Vigencia = vi.Key.Vigencia,
                                   MetaVigencia  = vi.Key.MetaVigencia,
                                   Regionalizacion = auxRegionalizacionRegionalizacionIndicadorAgregar.OrderBy(r => r.NombreRegion).ThenBy(r => r.NombreDepartamento).ThenBy(r => r.NombreMunicipio).ThenBy(r => r.NombreAgrupacion).ToList()

                               });
                           });


                           auxIndicadoresIndicadorProductoAgregar.Add(new IndicadoresRegionalizacionIndicadorDto()
                           {
                               IndicadorId = i.Key.IndicadorId,
                               NombreIndicador = i.Key.NombreIndicador,
                               CodigoIndicador = i.Key.CodigoIndicador,
                               IndicadorTipo = i.Key.IndicadorTipo,
                               IndicadorAcumula = i.Key.IndicadorAcumula,
                               IndicadorUnidadMedidaId = i.Key.IndicadorUnidadMedidaId,
                               NombreUnidadMedida = i.Key.NombreUnidadMedida,
                               MetaTotal = i.Key.MetaTotal,
                               Vigencias = auxVigenciasRegionalizacionIndicadorAgregar.OrderBy(vg => vg.Vigencia).ToList()
                           });
                       });

                        auxProductosIndicadorProductoAgregar.Add(new ProductosRegionalizacionIndicadorDto()
                        {
                            ProductoId = p.Key.ProductoId,
                            NombreProducto = p.Key.NombreProducto,
                            CodigoProducto = p.Key.CodigoProducto,
                            ProductoUnidadMedidaId = p.Key.ProductoUnidadMedidaId,
                            ProductoUnidadMedida = p.Key.ProductoUnidadMedida,
                            Cantidad = p.Key.Cantidad,
                            Indicadores = auxIndicadoresIndicadorProductoAgregar.OrderBy(lp => lp.NombreIndicador).ToList()
                        });
                    });

                    regionalizacionIndicadorAgregar.Objetivos.Add(new ObjetivosRegionalizacionIndicadorDto()
                    {
                        ObjetivoId = obj.Key.ObjetivoId,
                        ObjetivoEspecifico = obj.Key.ObjetivoEspecifico,
                        Productos = auxProductosIndicadorProductoAgregar.OrderBy(lp => lp.NombreProducto).ToList()
                    });
                    regionalizacionIndicadorAgregar.Bpin = obj.Key.BPIN;
                    regionalizacionIndicadorAgregar.Objetivos.OrderBy(o => o.ObjetivoEspecifico).ToList();
                });

            return regionalizacionIndicadorAgregar;
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {            
            Contexto.uspPostRegionalizacionIndicadorAgregarTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public RegionalizacionIndicadorAgregarDto ObtenerRegionalizacionIndicadorAgregarPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<RegionalizacionIndicadorAgregarDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewRegionalizacionIndicadorAgregar);
        }

    }
}
