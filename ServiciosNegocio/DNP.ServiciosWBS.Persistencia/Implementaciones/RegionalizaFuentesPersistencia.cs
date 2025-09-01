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
    using ServiciosNegocio.Dominio.Dto.FuenteFinanciacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;

    public class RegionalizaFuentesPersistencia : Persistencia, IRegionalizaFuentesPersistencia
    {
        public RegionalizaFuentesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacion(string bpin)
        {
            try
            {
                var listadoFuentesFinanciacion = Contexto.UspGetRegionalizaicion_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<FuenteFinanciacionRegionalizacionDto>(listadoFuentesFinanciacion);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostRegionalizacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public FuenteFinanciacionRegionalizacionDto ObtenerFuenteFinanciacionRegionalizacionPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<FuenteFinanciacionRegionalizacionDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                       @RutasPreviewRecursos.RutaPreviewRegionalizaFuentes);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<FuenteFinanciacionRegionalizacionDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostRegionalizacion(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
                                                usuario,
                                                parametrosGuardar.InstanciaId,
                                                parametrosGuardar.AccionId,
                                                parametrosGuardar.FormularioId,
                                                errorValidacionNegocio);

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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
    }
}
