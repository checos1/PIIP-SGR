using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.FocalizacionProyecto;
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
    public class FocalizacionPoliticasTransversalesPersistencia : Persistencia, IFocalizacionPoliticasTransversalesPersistencia
    {
        public FocalizacionPoliticasTransversalesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }
        public PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversales(string bpin)
        {
            try
            {
                var politicasTransversales = Contexto.uspGetPoliticasTransversales_JSON(bpin).SingleOrDefault();
                if (politicasTransversales == null)
                {
                    PoliticaTRelacionadasDto plt = new PoliticaTRelacionadasDto(); 
                    plt.BPIN = bpin;
                    return plt;
                }
                return JsonConvert.DeserializeObject<PoliticaTRelacionadasDto>(politicasTransversales);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }
        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }
        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTRelacionadasDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversales(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);

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
        public PoliticaTRelacionadasDto ObtenerFocalizacionPoliticasTransversalesPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTRelacionadasDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                    @RutasPreviewRecursos.RutaPoliticaRelacionadas);
        }
    }
}
