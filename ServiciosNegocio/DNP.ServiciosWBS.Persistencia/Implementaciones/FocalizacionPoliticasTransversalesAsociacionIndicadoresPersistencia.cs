using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasTransversalesAsociacionIndicadores;
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
    public class FocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia : Persistencia, IFocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia
    {
        public FocalizacionPoliticasTransversalesAsociacionIndicadoresPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesIndicadoresTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTIndicadoresDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesIndicadores(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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

        public PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadores(string bpin)
        {
            try
            {
                var politicasTransversalesMetas = Contexto.uspGetPoliticasTransversalesIndicadores_JSON(bpin).SingleOrDefault();
                if (politicasTransversalesMetas == null)
                {
                    PoliticaTIndicadoresDto pltm = new PoliticaTIndicadoresDto();
                    pltm.BPIN = bpin;
                    return pltm;
                }
                return JsonConvert.DeserializeObject<PoliticaTIndicadoresDto>(politicasTransversalesMetas);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public PoliticaTIndicadoresDto ObtenerFocalizacionPoliticasTransversalesAsociacionIndicadoresPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTIndicadoresDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaFocalizacionPoliticasTransversalesAsociacionIndicadores);
        }
    }
}
