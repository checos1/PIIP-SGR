using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadoresAjuste;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia : Persistencia, IAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia
    {
        public AjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesIndicadores_AjustesTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTIndicadoresAjusteDto> parametrosGuardar, string usuario)
        {
            
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesIndicadores_Ajustes(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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

        public PoliticaTIndicadoresAjusteDto ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadores(string bpin)
        {            
            try
            {
                var politicasTransversalesMetas = Contexto.uspGetPoliticasTransversalesIndicadores_Ajustes_JSON(bpin).SingleOrDefault();
                if (politicasTransversalesMetas == null)
                {
                    PoliticaTIndicadoresAjusteDto pltm = new PoliticaTIndicadoresAjusteDto();
                    pltm.BPIN = bpin;
                    return pltm;
                }
                return JsonConvert.DeserializeObject<PoliticaTIndicadoresAjusteDto>(politicasTransversalesMetas);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public PoliticaTIndicadoresAjusteDto ObtenerAjusteFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTIndicadoresAjusteDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaAjusteFocalizacionPoliticasTransversalesAsociacionIndicadores);
        }
    }
}
