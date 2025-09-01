using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    using ServiciosNegocio.Dominio.Dto.Poblacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;

    public class CuantificacionLocalizacionPersistencia: Persistencia, ICuantificacionLocalizacionPersistencia
    {

        public CuantificacionLocalizacionPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoblacionDto> parametrosGuardar,
                                           string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCuantificacionLocalizacionPoblacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.InstanciaId, parametrosGuardar.AccionId, parametrosGuardar.FormularioId, resultado);


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


        public PoblacionDto ObtenerCuantificacionLocalizacion(string bpin)
        {
            try
            {
                var cuantificacionLocalizacionDto = new PoblacionDto();
                IEnumerable<uspGetCuantificacionLocalizacionPoblacion_Result> cuantificacionLocalizacionList = Contexto.uspGetCuantificacionLocalizacionPoblacion(bpin).ToList();

                cuantificacionLocalizacionDto.Bpin = cuantificacionLocalizacionList.FirstOrDefault()?.BPIN;
                cuantificacionLocalizacionDto = MapearALocalizacionDto(cuantificacionLocalizacionList.ToList());

                return cuantificacionLocalizacionDto;
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }


        private PoblacionDto MapearALocalizacionDto(List<uspGetCuantificacionLocalizacionPoblacion_Result> listadoDesdeBd)
        {
            var cuantificacionLocalizacion = new PoblacionDto();
            cuantificacionLocalizacion.Vigencias = new List<PoblacionVigenciasDto>();
            listadoDesdeBd.GroupBy(o =>
            new
            {   o.BPIN,
                o.Cantidad,
                o.Vigencia
            }).ToList().ForEach(
                vig =>
                {
                    var auxVigenciacuantificacionLocalizacion = new List<PoblacionVigenciaLocalizacion>();
                    listadoDesdeBd.Where(v => v.Vigencia == vig.Key.Vigencia).GroupBy(op =>
                       new
                       {
                           op.LocalizacionId,
                           op.RegionId,
                           op.NombreRegion,
                           op.CodigoRegion,
                           op.DepartamentoId,
                           op.NombreDepartamento,                           
                           op.CodigoDepto,
                           op.MunicipioId,
                           op.NombreMunicipio,
                           op.CodigoMunicipio,
                           op.AgrupacionId,
                           op.NombreAgrupacion,
                           op.CodigoAgrupacion,
                           op.CantidadLocalizada
                       }).ToList().ForEach(j =>
                       {
                        auxVigenciacuantificacionLocalizacion.Add(new PoblacionVigenciaLocalizacion()
                        {
                            LocalizacionId = j.Key.LocalizacionId,
                            RegionId = j.Key.RegionId,
                            NombreRegion = j.Key.NombreRegion,
                            CodigoRegion = j.Key.CodigoRegion,
                            DepartamentoId = j.Key.DepartamentoId,
                            NombreDepartamento = j.Key.NombreDepartamento,
                            CodigoDepto = j.Key.CodigoDepto,
                            MunicipioId = j.Key.MunicipioId,
                            NombreMunicipio = j.Key.NombreMunicipio,
                            CodigoMunicipio = j.Key.CodigoMunicipio,
                            AgrupacionId = j.Key.AgrupacionId,
                            NombreAgrupacion = j.Key.NombreAgrupacion,
                            CodigoAgrupacion = j.Key.CodigoAgrupacion,
                            CantidadLocalizada = j.Key.CantidadLocalizada
                        });
                       });

                    cuantificacionLocalizacion.Vigencias.Add(new PoblacionVigenciasDto()
                    {
                        Vigencia = vig.Key.Vigencia,                   
                        Localizacion = auxVigenciacuantificacionLocalizacion.OrderBy(r => r.NombreRegion).ThenBy(r => r.NombreDepartamento).ThenBy(r => r.NombreMunicipio).ThenBy(r => r.NombreAgrupacion).ToList()
                    });

                    cuantificacionLocalizacion.Bpin = vig.Key.BPIN;
                    cuantificacionLocalizacion.CantidadPoblacion = vig.Key.Cantidad;
                });

            return cuantificacionLocalizacion;
        }


        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostCuantificacionLocalizacionPoblacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }


        public PoblacionDto ObtenerCuantificacionLocalizacionPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoblacionDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewCuantificacionLocalizacion);
        }

    }
}
