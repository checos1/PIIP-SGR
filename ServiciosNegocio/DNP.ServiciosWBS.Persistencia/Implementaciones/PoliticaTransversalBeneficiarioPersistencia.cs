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
    using ServiciosNegocio.Dominio.Dto.Focalizacion;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;
    public class PoliticaTransversalBeneficiarioPersistencia : Persistencia, IPoliticaTransversalBeneficiarioPersistencia
    {
        public PoliticaTransversalBeneficiarioPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiario(string bpin)
        {
            try
            {
                var politicaTBeneficiarioDto = Contexto.UspGetPoliticasTransversalesBeneficiarios_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<PoliticaTBeneficiarioDto>(politicaTBeneficiarioDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostPoliticasTransversalesBeneficiarioTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public PoliticaTBeneficiarioDto ObtenerPoliticaTransversalBeneficiarioPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoliticaTBeneficiarioDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewPoliticaTBeneficiario);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoliticaTBeneficiarioDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostPoliticasTransversalesBeneficiario(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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
