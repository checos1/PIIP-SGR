using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using ServiciosNegocio.Dominio.Dto.Productos;
    using ServiciosNegocio.Dominio.Dto.Indicadores;
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using Modelo;


    public class RegionalizacionIndicadoresPersistencia : Persistencia, IRegionalizacionIndicadoresPersistencia
    {

        public RegionalizacionIndicadoresPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<RegionalizacionIndicadorDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostRegionalizacionIndicadoresProgramacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);


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


        public RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadores(string bpin)
        {
            try
            {
                var regionalizacionIndicadorDto = new RegionalizacionIndicadorDto();
                IEnumerable<uspGetRegionalizacionIndicadoresProgramacion_Result> regionalizacionIndicadorList = Contexto.uspGetRegionalizacionIndicadoresProgramacion(bpin).ToList();

                regionalizacionIndicadorDto.Bpin = regionalizacionIndicadorList.FirstOrDefault()?.BPIN;
                regionalizacionIndicadorDto = MapearARegionalizacionIndicadoresDto(regionalizacionIndicadorList.ToList());

                return regionalizacionIndicadorDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }



        private RegionalizacionIndicadorDto MapearARegionalizacionIndicadoresDto(List<uspGetRegionalizacionIndicadoresProgramacion_Result> listadoDesdeBd)
        {
            var regionalizacionIndicadores = new RegionalizacionIndicadorDto();
            regionalizacionIndicadores.Vigencias = new List<VigenciaObjetivoProductoDto>();            
            listadoDesdeBd.GroupBy(o => 
            new
            {
                o.BPIN,
                o.Vigencia                     
            }).ToList().ForEach(
                vig =>
                {
                    var auxVigenciaObjetivoProducto = new List<VigenciaObjetivoProductoDto>();
                    listadoDesdeBd.Where(v => v.Vigencia == vig.Key.Vigencia).GroupBy(op =>
                       new
                       {
                           op.ObjetivoId,
                           op.ObjetivoEspecifico,
                           op.ProductoId,
                           op.NombreProducto,
                           op.ProductoUnidadMedidaId,
                           op.ProductoUnidadMedida,
                           op.Cantidad
                       }).ToList()
                       .ForEach(
                        op1 =>
                        {
                           var auxIndicadores = new List<IndicadorDto>();
                            listadoDesdeBd.Where(ind => ind.Vigencia == vig.Key.Vigencia && ind.ObjetivoId == op1.Key.ObjetivoId && ind.ProductoId == op1.Key.ProductoId).GroupBy(i =>
                           new
                           {
                               i.IndicadorId,
                               i.NombreIndicador,
                               i.CodigoIndicador,
                               i.IndicadorTipo,
                               i.IndicadorAcumula,
                               i.IndicadorUnidadMedidaId,
                               i.NombreUnidadMedida,
                               i.MetaTotal,
                               i.MetaVigenteVigencia
                           }).ToList().ForEach(j =>
                           {
                               var auxRegionalizacion = new List<IndicadorRegionalizacionDto>();
                               listadoDesdeBd.Where(r => r.IndicadorId == j.Key.IndicadorId && r.ProductoId == op1.Key.ProductoId && r.ObjetivoId == op1.Key.ObjetivoId && r.Vigencia == vig.Key.Vigencia)
                               .GroupBy(reg =>
                                new
                                {
                                    reg.IndicadorId,
                                    reg.RegionalizacionMetasId,
                                    reg.RegionId,
                                    reg.NombreRegion,
                                    reg.CodigoRegion,
                                    reg.DepartmentId,
                                    reg.NombreDepartamento,
                                    reg.CodigoDepto,
                                    reg.MunicipalityId,
                                    reg.NombreMunicipio,
                                    reg.CodigoMunicipio,
                                    reg.AgrupacionId,
                                    reg.NombreAgrupacion,
                                    reg.CodigoAgrupacion,
                                    reg.MetaVigenteReg
                                }).ToList().ForEach(pir =>
                                {
                                    auxRegionalizacion.Add(new IndicadorRegionalizacionDto()
                                    {
                                        IndicadorId = pir.Key.IndicadorId,
                                        RegionalizacionMetasId = pir.Key.RegionalizacionMetasId,
                                        RegionId = pir.Key.RegionId,
                                        RegionNombre = pir.Key.NombreRegion,
                                        RegionCodigo = pir.Key.CodigoRegion,
                                        DepartamentoId = pir.Key.DepartmentId,
                                        DepartamentoNombre = pir.Key.NombreDepartamento,
                                        DepartamentoCodigo = pir.Key.CodigoDepto,
                                        MunicipioId = pir.Key.MunicipalityId,
                                        MunicipioNombre = pir.Key.NombreMunicipio,
                                        MunicipioCodigo = pir.Key.CodigoMunicipio,
                                        AgrupacionId = pir.Key.AgrupacionId,
                                        AgrupacionNombre = pir.Key.NombreAgrupacion,
                                        AgrupacionCodigo = pir.Key.CodigoAgrupacion,
                                        MetaVigente = pir.Key.MetaVigenteReg
                                    });
                                } );

                               auxIndicadores.Add(new IndicadorDto()
                               {
                                   IndicadorId = j.Key.IndicadorId,
                                   ProductoId = op1.Key.ProductoId,
                                   IndicadorNombre = j.Key.NombreIndicador,
                                   IndicadorCodigo = j.Key.CodigoIndicador,
                                   IndicadorTipo = j.Key.IndicadorTipo,
                                   IndicadorAcumula = j.Key.IndicadorAcumula,
                                   IndicadorUnidadMedidaId = j.Key.IndicadorUnidadMedidaId,
                                   IndicadorNombreUnidadMedida = j.Key.NombreUnidadMedida,
                                   IndicadorMetaTotal = j.Key.MetaTotal,
                                   IndicadorMetaVigente = j.Key.MetaVigenteVigencia,                                  
                                   Regionalizacion = auxRegionalizacion.OrderBy(ri => ri.RegionNombre).ToList()
                               });
                           });

                            regionalizacionIndicadores.Vigencias.Add(new VigenciaObjetivoProductoDto()
                            {
                                Vigencia = vig.Key.Vigencia,
                                ObjetivoEspecificoId = op1.Key.ObjetivoId,
                                ObjetivoEspecifico = op1.Key.ObjetivoEspecifico,
                                ProductoId = op1.Key.ProductoId,
                                Producto = op1.Key.NombreProducto,
                                Meta = op1.Key.Cantidad,
                                UnidadMedidaId = op1.Key.ProductoUnidadMedidaId,
                                NombreUnidadMedida = op1.Key.ProductoUnidadMedida,
                                Indicadores = auxIndicadores.OrderBy(ip => ip.IndicadorId).ToList()
                            });

                        });               

                    regionalizacionIndicadores.Bpin = vig.Key.BPIN;
                }); 

            return regionalizacionIndicadores;
        }


        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {            
            Contexto.uspPostRegionalizacionIndicadoresProgramacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }


        public RegionalizacionIndicadorDto ObtenerRegionalizacionIndicadoresPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<RegionalizacionIndicadorDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewRegionalizacionIndicadores);
        }

    }
}
