using DNP.ServiciosNegocio.Comunes.Dto.Formulario;
using DNP.ServiciosNegocio.Comunes.Utilidades;
using DNP.ServiciosNegocio.Dominio.Dto.Formulario;
using DNP.ServiciosNegocio.Persistencia.Interfaces;
using DNP.ServiciosNegocio.Persistencia.Interfaces.Formulario;
using DNP.ServiciosNegocio.Persistencia.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using DNP.ServiciosNegocio.Comunes;
using DNP.ServiciosNegocio.Comunes.Excepciones;

namespace DNP.ServiciosNegocio.Persistencia.Implementaciones.Formulario
{
    public class CadenaValorPersistencia : Persistencia, ICadenaValorPersistencia
    {

        public CadenaValorPersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {

        }

        public void GuardarDefinitivamente(ParametrosGuardarDto<CadenaValorDto> parametrosGuardar, string usuario)
        {
            ObjectParameter errorValidacionNegocio = new ObjectParameter("errorValidacionNegocio", typeof(string));

            Contexto.uspPostCadenaValor(JsonUtilidades.ACadenaJson(parametrosGuardar.Contenido),
                                        usuario,
                                        errorValidacionNegocio);

            if (errorValidacionNegocio.Value == null) return;

            var mensajeError = Convert.ToString(errorValidacionNegocio.Value);

            if (!string.IsNullOrEmpty(mensajeError))
                throw new ServiciosNegocioException(mensajeError);
        }

        public IEnumerable<uspGetCadenaValor_Result> ObtenerCadenaValor(string bpin)
        {
            return Contexto.uspGetCadenaValor(bpin).ToList();
        }

        public object ObtenerCadenaValorPreview()
        {
            CadenaValorDto cadenaValor = SerializadorJson.SerializarJsonObjeto<CadenaValorDto>(AppDomain.CurrentDomain.RelativeSearchPath + RutasPreviewRecursos.PreviewCadenaValor);
            return cadenaValor;
        }
    }
}
