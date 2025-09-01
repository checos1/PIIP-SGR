using System;
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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    public class FocalizacionCuantificacionLocalizacionPersistencia : Persistencia, IFocalizacionCuantificacionLocalizacionPersistencia
    {

        public FocalizacionCuantificacionLocalizacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionCuantificacionLocalizacionDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFocalizacionCuantificacionLocalizacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);


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


        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacion(string bpin)
        {
            try
            {
                var focalizacionCuantificacionLocalizacionDto = new FocalizacionCuantificacionLocalizacionDto();
                IEnumerable<uspGetFocalizacionCuantificacionLocalizacion_Result> focalizacionCantificacionLocalizacionList = Contexto.uspGetFocalizacionCuantificacionLocalizacion(bpin).ToList();

                focalizacionCuantificacionLocalizacionDto.Bpin = focalizacionCantificacionLocalizacionList.FirstOrDefault()?.BPIN;
                focalizacionCuantificacionLocalizacionDto = MapearALocalizacionDto(focalizacionCantificacionLocalizacionList.ToList());

                return focalizacionCuantificacionLocalizacionDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }



        private FocalizacionCuantificacionLocalizacionDto MapearALocalizacionDto(List<uspGetFocalizacionCuantificacionLocalizacion_Result> listadoDesdeBd)
        {
            var focalizacionCuantificacionLocalizacion = new FocalizacionCuantificacionLocalizacionDto();
            focalizacionCuantificacionLocalizacion.Focalizacion = new List<FocalizacionCuantificacionDto>();
            listadoDesdeBd.GroupBy(p =>
            new
            {
                p.BPIN,
                p.PoblacionObjetivo
            }).ToList().ForEach(
                pob =>
                {             
                    var auxFocalizacionCuantificacionLocalizacion = new List<FocalizacionCuantificacionDto>();
                    listadoDesdeBd.Where(f => f.BPIN == pob.Key.BPIN).GroupBy(fo =>
                    new
                    {
                        fo.PoliticaId,
                        fo.DimensionId,
                        fo.NombreDescripcion,
                        fo.CantidadMGA
                    }).ToList().ForEach(
                        foc =>
                        {
                            var auxVigenciasFocalizacionCuantificacion = new List<VigenciasFocalizacionCuantificacionDto>();
                            listadoDesdeBd.Where(g => g.DimensionId == foc.Key.DimensionId && g.PoliticaId == foc.Key.PoliticaId).GroupBy(foV =>
                            new
                            {
                                foV.Vigencia
                            }).ToList().ForEach(
                                foVig =>
                                {                                    
                                    var localizacionFocalizacionCuantificacion = new List<LocalizacionFocalizacionCuantificacionDto>();
                                    listadoDesdeBd.Where(h => h.DimensionId == foc.Key.DimensionId && h.Vigencia == foVig.Key.Vigencia).GroupBy(foVigLoc =>
                                    new
                                    {
                                        foVigLoc.LocalizacionId,
                                        foVigLoc.RegionId,
                                        foVigLoc.NombreRegion,
                                        foVigLoc.CodigoRegion,
                                        foVigLoc.DepartamentoId,
                                        foVigLoc.NombreDepartamento,
                                        foVigLoc.CodigoDepto,
                                        foVigLoc.MunicipioId,
                                        foVigLoc.NombreMunicipio,
                                        foVigLoc.CodigoMunicipio,
                                        foVigLoc.AgrupacionId,
                                        foVigLoc.NombreAgrupacion,
                                        foVigLoc.CodigoAgrupacion,
                                        foVigLoc.CantidadCuantificada,
                                        foVigLoc.CantidadFocalizada
                                    }).ToList().ForEach(
                                        loc =>
                                        {
                                            localizacionFocalizacionCuantificacion.Add(new LocalizacionFocalizacionCuantificacionDto()
                                            {
                                                LocalizacionId = loc.Key.LocalizacionId,
                                                RegionId = loc.Key.RegionId,
                                                NombreRegion = loc.Key.NombreRegion,
                                                CodigoRegion = loc.Key.CodigoRegion,
                                                DepartamentoId = loc.Key.DepartamentoId,
                                                NombreDepartamento = loc.Key.NombreDepartamento,
                                                CodigoDepto = loc.Key.CodigoDepto,
                                                MunicipioId = loc.Key.MunicipioId,
                                                NombreMunicipio = loc.Key.NombreMunicipio,
                                                CodigoMunicipio = loc.Key.CodigoMunicipio,
                                                AgrupacionId = loc.Key.AgrupacionId,
                                                NombreAgrupacion = loc.Key.NombreAgrupacion,
                                                CodigoAgrupacion = loc.Key.CodigoAgrupacion,
                                                CantidadCuantificada = loc.Key.CantidadCuantificada,
                                                CantidadFocalizada = loc.Key.CantidadFocalizada
                                            });
                                        });

                                    auxVigenciasFocalizacionCuantificacion.Add(new VigenciasFocalizacionCuantificacionDto()
                                    {
                                        Vigencia = foVig.Key.Vigencia,
                                        Localizacion = localizacionFocalizacionCuantificacion.OrderBy(r => r.NombreRegion).ThenBy(r => r.NombreDepartamento).ThenBy(r => r.NombreMunicipio).ThenBy(r => r.NombreAgrupacion).ToList()
                                    });
                                });


                            auxFocalizacionCuantificacionLocalizacion.Add(new FocalizacionCuantificacionDto()
                            {
                                PoliticaId = foc.Key.PoliticaId,
                                DimensionId = foc.Key.DimensionId,
                                NombreDescripcion = foc.Key.NombreDescripcion,
                                CantidadMGA = foc.Key.CantidadMGA,
                                Vigencias = auxVigenciasFocalizacionCuantificacion.OrderBy(v => v.Vigencia).ToList()
                            });
                        });

                    focalizacionCuantificacionLocalizacion.Bpin = pob.Key.BPIN;
                    focalizacionCuantificacionLocalizacion.PoblacionObjetivo = pob.Key.PoblacionObjetivo;
                    focalizacionCuantificacionLocalizacion.Focalizacion = auxFocalizacionCuantificacionLocalizacion.OrderBy(v => v.NombreDescripcion).ToList();
                });

            return focalizacionCuantificacionLocalizacion;
        }


        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostFocalizacionCuantificacionLocalizacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);            
        }

        public FocalizacionCuantificacionLocalizacionDto ObtenerFocalizacionCuantificacionLocalizacionPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<FocalizacionCuantificacionLocalizacionDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewFocalizacionCuantificacionLocalizacion);
        }

    }
}
