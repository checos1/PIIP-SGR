using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionada;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionadaAjustes;
using DNP.ServiciosWBS.Persistencia.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNP.ServiciosNegocio.Dominio.Dto.PoliticasIndicadoresCategorias;

namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    public class FocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia : Persistencia, IFocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia
    {
        public FocalizacionPoliticasTransversalesRelacionadasAjustesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
                
        }

        public PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustes(string bpin)
        {          
            try
            {
                var politicasTransversales = Contexto.uspGetPoliticasTransversalesRelacionadas_Ajustes_JSON(bpin);
                if (politicasTransversales == null)
                {
                    PoliticaTransversalRelacionadaAjustesDto plt = new PoliticaTransversalRelacionadaAjustesDto();
                    plt.BPIN = bpin;
                    return plt;
                }
                return JsonConvert.DeserializeObject<PoliticaTransversalRelacionadaAjustesDto>(politicasTransversales.SingleOrDefault());
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public PoliticaTransversalRelacionadaAjustesDto ObtenerFocalizacionPoliticasTransversalesRelacionadasAjustesPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTransversalRelacionadaAjustesDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaFocalizacionPoliticasTransversalesRelacionadasAjustes);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTransversalRelacionadaAjustesDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesRelacionadasAjustes(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesRelacionadasAjustesTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

    }
}
