using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Tramites.TramitesDistribucionesAnteriores;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using Newtonsoft.Json;


namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class TramitesDistribucionesAnterioresPersistencia : Persistencia, ITramitesDistribucionesAnterioresPersistencia
    {
        public TramitesDistribucionesAnterioresPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }
        public TramitesDistribucionesAnterioresDto ObtenerTramitesDistribucionAnterior(Guid Instancia)
        {
            try
            {
                
                var TramitesDitribucionAnteriorDto = Contexto.uspGetResumenDistribucionesAnteriores_JSON(Instancia).SingleOrDefault();
                return JsonConvert.DeserializeObject<TramitesDistribucionesAnterioresDto>(TramitesDitribucionAnteriorDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public TramitesDistribucionesAnterioresDto ObtenertramitesDistribucionAnterioresPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<TramitesDistribucionesAnterioresDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaPreviewTramitesDistribucionAnteriores);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<TramitesDistribucionesAnterioresDto> parametrosGuardar, string usuario)
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
