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
    using ServiciosNegocio.Dominio.Dto.CuantificacionBeneficiario;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;

    public class CuantificacionBeneficiarioPersistencia : Persistencia, ICuantificacionBeneficiarioPersistencia
    {
        public CuantificacionBeneficiarioPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public PoblacionDto ObtenerCuantificacionBeneficiario(string bpin)
        {
            try
            {
                var cuantificacionLocalizacionDto = Contexto.uspGetCuantificacionBeneficiarios_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<PoblacionDto>(cuantificacionLocalizacionDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostCuantificacionBeneficiarioTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public PoblacionDto ObtenerCuantificacionBeneficiarioPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<PoblacionDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewCuantificacionBeneficiario);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<PoblacionDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCuantificacionBeneficiario(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), 
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
