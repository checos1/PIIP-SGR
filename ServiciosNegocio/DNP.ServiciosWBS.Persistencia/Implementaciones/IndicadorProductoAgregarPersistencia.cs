namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Modelo;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Dominio.Dto.IndicadorProductoAgregar;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using System.Threading.Tasks;

    public class IndicadorProductoAgregarPersistencia: Persistencia, IIndicadorProductoAgregarPersistencia
    {

        public IndicadorProductoAgregarPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }


        public void GuardarDefinitivamente(ParametrosGuardarDto<IndicadorProductoAgregarDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostIndicadorProductoAgregar(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);


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

        public IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregar(string bpin)
        {
            try
            {
                var indicadorProductoAgregarDto = new IndicadorProductoAgregarDto();
                IEnumerable<uspGetIndicadorProductoAgregar_Result> indicadorProductoAgregarList = Contexto.uspGetIndicadorProductoAgregar(bpin).ToList();

                indicadorProductoAgregarDto.Bpin = indicadorProductoAgregarList.FirstOrDefault()?.BPIN;
                indicadorProductoAgregarDto = MapearIndicadorProductoAgregarDto(indicadorProductoAgregarList.ToList());

                return indicadorProductoAgregarDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
                

        private IndicadorProductoAgregarDto MapearIndicadorProductoAgregarDto(List<uspGetIndicadorProductoAgregar_Result> listadoDesdeBd)
        {
            var indicadorProductoAgregar = new IndicadorProductoAgregarDto();
            indicadorProductoAgregar.Objetivos = new List<ObjetivosIndicadorProductoAgregarDto>();
            
            listadoDesdeBd.GroupBy(o =>
            new
            {
                o.BPIN,
                o.ObjetivoId,
                o.ObjetivoEspecifico
            }).ToList().ForEach(
                obj =>
                {
                    var auxProductosIndicadorProductoAgregar = new List<ProductosIndicadorProductoAgregarDto>();
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
                        var auxIndicadoresIndicadorProductoAgregar = new List<IndicadoresIndicadorProductoAgregarDto>();
                        listadoDesdeBd.Where(pr => pr.ObjetivoId == obj.Key.ObjetivoId && pr.ProductoId  == p.Key.ProductoId).GroupBy(ind =>
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
                        }).ToList().ForEach(i =>
                        {
                            auxIndicadoresIndicadorProductoAgregar.Add(new IndicadoresIndicadorProductoAgregarDto()
                            {
                                IndicadorId =i.Key.IndicadorId,
                                NombreIndicador  = i.Key.NombreIndicador,
                                CodigoIndicador  = i.Key.CodigoIndicador,
                                IndicadorTipo  = i.Key.IndicadorTipo,
                                IndicadorAcumula  = i.Key.IndicadorAcumula,
                                IndicadorUnidadMedidaId  = i.Key.IndicadorUnidadMedidaId,
                                NombreUnidadMedida  = i.Key.NombreUnidadMedida,
                                MetaTotal  = i.Key.MetaTotal
                            });
                        });

                        auxProductosIndicadorProductoAgregar.Add(new ProductosIndicadorProductoAgregarDto()
                        {
                            ProductoId = p.Key.ProductoId,
                            NombreProducto  = p.Key.NombreProducto,
                            CodigoProducto = p.Key.CodigoProducto,
                            ProductoUnidadMedidaId = p.Key.ProductoUnidadMedidaId,
                            ProductoUnidadMedida = p.Key.ProductoUnidadMedida,
                            Cantidad = p.Key.Cantidad,
                            Indicadores = auxIndicadoresIndicadorProductoAgregar.OrderBy(lp => lp.NombreIndicador).ToList()
                        });
                    });

                    indicadorProductoAgregar.Objetivos.Add(new ObjetivosIndicadorProductoAgregarDto()
                    {
                        ObjetivoId = obj.Key.ObjetivoId,
                        ObjetivoEspecifico = obj.Key.ObjetivoEspecifico,                      
                        Productos = auxProductosIndicadorProductoAgregar.OrderBy(lp => lp.NombreProducto).ToList()
                    });

                    indicadorProductoAgregar.Bpin = obj.Key.BPIN;
                    indicadorProductoAgregar.Objetivos.OrderBy(o => o.ObjetivoEspecifico).ToList();
                });          

            return indicadorProductoAgregar;
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostIndicadorProductoAgregarTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public IndicadorProductoAgregarDto ObtenerIndicadorProductoAgregarPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<IndicadorProductoAgregarDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewIndicadorProductoAgregar);
        }




    }
}
