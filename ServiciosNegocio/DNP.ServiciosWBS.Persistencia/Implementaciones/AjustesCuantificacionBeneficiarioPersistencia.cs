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
    using ServiciosNegocio.Dominio.Dto.Proyectos;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Utilidades;
    using Newtonsoft.Json;

    public class AjustesCuantificacionBeneficiarioPersistencia : Persistencia, IAjustesCuantificacionBeneficiarioPersistencia
    {
        public AjustesCuantificacionBeneficiarioPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiario(string bpin)
        {
            try
            {
                var cuantificacionLocalizacionDto = Contexto.uspGetCuantificacionBeneficiarios_Ajustar_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<AjustesCuantificacionBeneficiarioDto>(cuantificacionLocalizacionDto);
            }
            catch (Exception e)
            {
                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, e);
            }
        }

        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostCuantificacionBeneficiarioTemp_Ajustar(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public AjustesCuantificacionBeneficiarioDto ObtenerAjustesCuantificacionBeneficiarioPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<AjustesCuantificacionBeneficiarioDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaPreviewAjustesCuantificacionBeneficiario);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<AjustesCuantificacionBeneficiarioDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCuantificacionBeneficiario_Ajustar(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
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
