namespace DNP.ServiciosWBS.Persistencia.Implementaciones
{
    using Interfaces;
    using Modelo;
    using ServiciosNegocio.Comunes;
    using ServiciosNegocio.Comunes.Dto.Formulario;
    using ServiciosNegocio.Comunes.Excepciones;
    using ServiciosNegocio.Comunes.Utilidades;
    using ServiciosNegocio.Dominio.Dto.Formulario;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;

    public class CadenaValorPersistencia : Persistencia, ICadenaValorPersistencia
    {

        public CadenaValorPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            using (var dbContextTransaction = Contexto.Database.BeginTransaction())
            {
                try
                {
                    Contexto.uspPostCadenaValor(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
                                                usuario,
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
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public IEnumerable<uspGetCadenaValor_Result> ObtenerCadenaValor(string bpin)
        {
            return Contexto.uspGetCadenaValor(bpin).ToList();
        }

        public CadenaValorDto ObtenerCadenaValorPreview()
        {
            CadenaValorDto cadenaValor = SerializadorJson.SerializarJsonObjeto<CadenaValorDto>(AppDomain.CurrentDomain.RelativeSearchPath + RutasPreviewRecursos.PreviewCadenaValor);
            return cadenaValor;
        }


        public void ActualizarTemporal(ParametrosConsultaDto parametrosConsultaDto)
        {
            Contexto.uspPostCadenaValorTemp(parametrosConsultaDto.InstanciaId, parametrosConsultaDto.AccionId);
        }

}
}
