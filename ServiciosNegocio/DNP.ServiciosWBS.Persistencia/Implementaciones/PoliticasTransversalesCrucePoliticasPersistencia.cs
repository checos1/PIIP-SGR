using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Focalizacion;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesCrucePoliticas;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Core.Objects;
using System.Linq;


namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class PoliticasTransversalesCrucePoliticasPersistencia : Persistencia, IPoliticasTransversalesCrucePoliticasPersistencia
    {
        public PoliticasTransversalesCrucePoliticasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }
        public PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticas(string Bpin, int IdFuente)
        {
            try
            {
                var PoiticaCrucePoliticasDto = Contexto.uspGetCrucePoliticas_JSON(Bpin,IdFuente).SingleOrDefault();
                return JsonConvert.DeserializeObject<PoliticasTCrucePoliticasDto>(PoiticaCrucePoliticasDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
           
        public PoliticasTCrucePoliticasDto ObtenerPoliticasTransversalesCrucePoliticasPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticasTCrucePoliticasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaPreviewPoliticaTransversalesPoliticasFuentes);
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticasTCrucePoliticasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                     Contexto.uspPostCrucePoliticas(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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
