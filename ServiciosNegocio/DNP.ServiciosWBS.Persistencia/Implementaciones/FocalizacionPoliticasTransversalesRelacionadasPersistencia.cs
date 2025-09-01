using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionPoliticaTransversaleRelacionada;
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
    public class FocalizacionPoliticasTransversalesRelacionadasPersistencia : Persistencia, IFocalizacionPoliticasTransversalesRelacionadasPersistencia
    {
        public FocalizacionPoliticasTransversalesRelacionadasPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
                
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesRelacionadasTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTransversalRelacionadaDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesRelacionadas(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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

        public PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadas(string bpin)
        {          
            try
            {
                var politicasTransversales =  Contexto.uspGetPoliticasTransversalesRelacionadas_JSON(bpin).SingleOrDefault();
                if (politicasTransversales == null)
                {
                    PoliticaTransversalRelacionadaDto plt = new PoliticaTransversalRelacionadaDto();
                    plt.BPIN = bpin;
                    return plt;
                }
                return JsonConvert.DeserializeObject<PoliticaTransversalRelacionadaDto>(politicasTransversales);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
            
        }

        public PoliticaTransversalRelacionadaDto ObtenerFocalizacionPoliticasTransversalesRelacionadasPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTransversalRelacionadaDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaFocalizacionPoliticasTransversalesRelacionadas);
        }
    }
}
