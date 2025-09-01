using AutoMapper;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Excepciones;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.DiligenciarFuentes;
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
    public class DiligenciarFuentesPersistencia : Persistencia, IDiligenciarFuentesPersistencia
    {
        public DiligenciarFuentesPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
            //Mapper.Reset();
        }

        public DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentes(string bpin)
        {
            try
            {
                var listadodiligenciarfuentes = Contexto.uspGetFuentesFinanciacion_JSON(bpin).SingleOrDefault();
                return JsonConvert.DeserializeObject<DiligenciarFuentesProyectoDto>(listadodiligenciarfuentes);
            }
            catch (Exception ex)
            {

                throw new ServiciosNegocioException(ServiciosNegocioRecursos.ErrorMapear, ex);
            }
        }
        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostFuentesFinanciacionTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<DiligenciarFuentesProyectoDto> parametrosGuardar, string usuario)
        {
            ObjectParameter resultado = new ObjectParameter("errorValidacionNegocio", typeof(string));
            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostFuentesFinanciacion_JSON(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido), usuario, parametrosGuardar.FormularioId, resultado);
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public DiligenciarFuentesProyectoDto ObtenerDiligenciarFuentesPreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<DiligenciarFuentesProyectoDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                   @RutasPreviewRecursos.RutaDiligenciarFuentes);
        }
    }
}
