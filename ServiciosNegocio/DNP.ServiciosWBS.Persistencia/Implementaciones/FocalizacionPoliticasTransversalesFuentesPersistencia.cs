using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class FocalizacionPoliticasTransversalesFuentesPersistencia : Persistencia, IFocalizacionPoliticasTransversalesFuentesPersistencia
    {
        public FocalizacionPoliticasTransversalesFuentesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }
        public FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentes(string bpin)
        {
            try
            {
                var PoiticaFuentesDto = Contexto.uspGetPoliticasTransversalesFuentes_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<FocalizacionPoliticaTFuentesDto>(PoiticaFuentesDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
           
        public FocalizacionPoliticaTFuentesDto ObtenerFocalizacionPoliticasTransversalesFuentesPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<FocalizacionPoliticaTFuentesDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaPreviewAjustesPoliticaTranversalFuentes);
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<FocalizacionPoliticaTFuentesDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    // Contexto.uspPostPoliticasTransversales(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }


    }
}
