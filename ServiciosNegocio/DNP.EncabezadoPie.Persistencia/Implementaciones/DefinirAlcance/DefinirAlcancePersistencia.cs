namespace DNP.EncabezadoPie.Persistencia.Implementaciones.DefinirAlcance
{
    using System.Collections.Generic;
    using DNP.EncabezadoPie.Dominio.Dto.DefinirAlcance;
    using DNP.EncabezadoPie.Persistencia.Interfaces.DefinirAlcance;
    using Interfaces;
    using System.Linq;
    using DNP.ServiciosNegocio.Comunes.Utilidades;
    using System;
    using DNP.ServiciosNegocio.Comunes;
    using Newtonsoft.Json;

    public class DefinirAlcancePersistencia : Persistencia, IDefinirAlcancePersistencia
    {
        public DefinirAlcancePersistencia(IContextoFactory contextoFactory) : base(contextoFactory)
        {
        }

        public AlcanceDto ObtenerAlcance(string bpin)
        {
            var listadoFuentesFinanciacion = Contexto.UspGetEncabezadoFormularioAlcance_JSON(bpin).SingleOrDefault();
            return JsonConvert.DeserializeObject<AlcanceDto>(listadoFuentesFinanciacion);
        }

        public AlcanceDto ObtenerAlcancePreview()
        {
            return JsonUtilidades.SerializarJsonObjeto<AlcanceDto>(AppDomain.CurrentDomain.RelativeSearchPath +
                                                                       @RutasPreviewRecursos.RutaPreviewDefinirAlcance);
        }
    }
}
